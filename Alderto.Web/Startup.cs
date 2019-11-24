using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Alderto.Bot;
using Alderto.Bot.Services;
using Alderto.Data;
using Alderto.Services.Exceptions;
using Alderto.Web.Helpers;
using Alderto.Web.Services;
using Discord;
using Discord.Commands;
using Discord.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Alderto.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices([NotNull] IServiceCollection services)
        {
            // === <General> ===
            // Add database.
            services.AddDbContext<AldertoDbContext>(options =>
            {
                options.UseNpgsql(
                    $"Server={Configuration["Database:Host"]};" +
                    $"Port={Configuration["Database:Port"]};" +
                    $"Database={Configuration["Database:Database"]};" +
                    $"UserId={Configuration["Database:Username"]};" +
                    $"Password={Configuration["Database:Password"]};" +
                    Configuration["Database:Extras"],
                    builder => builder.MigrationsAssembly("Alderto.Data"));
            });

            // Add database accessors.
            services.AddBotManagers();

            _ = ulong.TryParse(Configuration["Discord:NewsChannelId"], out var newsChannelId);
            services.AddNewsProvider(o => o.NewsChannelId = newsChannelId);
            services.AddMessagesManager();

            // === <Web> ===
            // Use discord as authentication service.
            services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddDiscord(options =>
                {
                    options.ClientId = Configuration["DiscordAPI:ClientId"];
                    options.ClientSecret = Configuration["DiscordAPI:ClientSecret"];
                    options.SaveTokens = true;

                    options.Scope.Add("guilds");

                    options.SignInScheme = "RefreshToken";
                })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Convert.FromBase64String(Configuration["JWTPrivateKey"]))
                    };
                })
                .AddCookie("RefreshToken", options =>
                {
                    options.Cookie.Name = ".RefreshToken";
                    options.Cookie.Path = "/api/account/login";
                    options.ExpireTimeSpan = TimeSpan.FromDays(14);
                    //options.Cookie.SameSite = SameSiteMode.Strict;
                    //options.Cookie.HttpOnly = true;
                });

            services.AddAuthorization();

            services.AddHttpClient<DiscordHttpClient>(o =>
            {
                o.BaseAddress = new Uri("https://discordapp.com/api/v6");
                o.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddScoped<AuthService>();

            // Add Mvc
            services
                .AddMvcCore()
                .AddDataAnnotations()
                .AddApiExplorer()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var (key, value) = context.ModelState.First(s => s.Value.Errors.Count > 0);
                        var errorMsg = (string.IsNullOrWhiteSpace(key) ? "" : $"{key}: ") +
                                       value.Errors[0].ErrorMessage;
                        return new BadRequestObjectResult(new ErrorMessage(400, 0, errorMsg));
                    };
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                    options.JsonSerializerOptions.Converters.Add(new SnowflakeConverter());
                    options.JsonSerializerOptions.Converters.Add(new NullableSnowflakeConverter());
                });

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration => configuration.RootPath = "ClientApp/dist");

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Alderto Documentation",
                    Version = "v1"
                });
            });

            // === <Bot> ===
            // Add discord socket client
            services.AddDiscordSocketClient(Configuration["DiscordAPI:BotToken"],
                socketConfig => { socketConfig.LogLevel = LogSeverity.Debug; });

            // Add command handling services
            services.AddCommandService(serviceConfig =>
            {
                serviceConfig.DefaultRunMode = RunMode.Sync;
                serviceConfig.IgnoreExtraArgs = true;
            });
            services.AddCommandHandler();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, CommandHandler cmdHandler)
        {
            // Make sure the database is up to date
            Task.Run(() => UpdateDatabase(app));

            // Start the bot.
            Task.Run(cmdHandler.StartAsync);

            // Check if https is configured
            if (Configuration["Kestrel:Endpoints:Https:Url"] != null)
            {
                if (env.IsProduction())
                    app.UseHsts();

                app.UseHttpsRedirection();
            }

            // Configure api routing.
            app.Map("/api", api =>
            {
                if (env.IsDevelopment())
                {
                    api.UseStaticFiles();
                    api.UseSwagger();
                    api.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Alderto API v1"));
                }

                api.UseRouting();

                api.UseAuthentication();
                api.UseAuthorization();

                // Handle Service errors
                api.Use(async (context, next) =>
                {
                    try
                    {
                        await next.Invoke();
                    }
                    catch (Exception e)
                    {
                        // Handle known API Exceptions.
                        if (e is ApiException apiException)
                        {
                            context.Response.OnStarting(() =>
                    {
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = (apiException.Error.Code / 1000) switch
                        {
                            1 => StatusCodes.Status403Forbidden,
                            2 => StatusCodes.Status404NotFound,
                            3 => StatusCodes.Status400BadRequest,
                            _ => throw e
                        };

                        return Task.CompletedTask;
                    });

                            await context.Response.WriteAsync(
                                JsonSerializer.Serialize(apiException.Error, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
                        }
                        else if (e is HttpException discordException)
                        {
                            context.Response.OnStarting(() =>
                            {
                                context.Response.ContentType = "application/json";
                                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                                return Task.CompletedTask;
                            });

                            await context.Response.WriteAsync(JsonSerializer.Serialize(
                                discordException.DiscordCode switch
                                {
                                    50001 => ErrorMessages.MissingChannelAccess,
                                    50013 => ErrorMessages.MissingWritePermissions,
                                    _ => throw e
                                }, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
                        }
                        else
                            throw;
                    }
                });

                api.UseEndpoints(p => { p.MapControllers(); });
            });

            // Use SPA Application
            app.UseSpaStaticFiles();
            app.UseSpa(spa =>
            {
                if (env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }
            });
        }

        private static async Task UpdateDatabase(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();

            await using var context = serviceScope.ServiceProvider.GetService<AldertoDbContext>();
            var logger = serviceScope.ServiceProvider.GetService<ILogger<DbContext>>();

            logger.LogInformation("Initializing Database...");
            await context.Database.MigrateAsync();
            logger.LogInformation("Database Ready!");
        }
    }
}
