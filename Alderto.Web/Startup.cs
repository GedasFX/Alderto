using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Alderto.Application;
using Alderto.Bot;
using Alderto.Bot.Services;
using Alderto.Data;
using Alderto.Domain.Exceptions;
using Alderto.Web.Helpers;
using Alderto.Web.Services;
using Discord;
using Discord.Commands;
using Discord.Net;
using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ErrorMessage = Alderto.Domain.Exceptions.ErrorMessage;

namespace Alderto.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }
        public IHostEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices([NotNull] IServiceCollection services)
        {
            var dbConnectionString = $"Server={Configuration["Database:Host"]};" +
                                     $"Port={Configuration["Database:Port"]};" +
                                     $"Database={Configuration["Database:Database"]};" +
                                     $"UserId={Configuration["Database:Username"]};" +
                                     $"Password={Configuration["Database:Password"]};" +
                                     Configuration["Database:Extras"];

            // === <General> ===
            services.AddDbContext<AldertoDbContext>(options =>
            {
                options.UseNpgsql(dbConnectionString,
                    builder => builder.MigrationsAssembly("Alderto.Data"));

                if (Env.IsDevelopment())
                    options.EnableSensitiveDataLogging();
            });
            services.AddMemoryCache();

            services.AddAlderto();

            // === <Web> ===
            // For use behind reverse proxy
            services.Configure<ForwardedHeadersOptions>(o =>
            {
                o.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                o.KnownNetworks.Clear();
                o.KnownProxies.Clear();
            });

            // Use discord as authentication service.
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddDiscord(options =>
                {
                    options.ClientId = Configuration["DiscordAPI:ClientId"];
                    options.ClientSecret = Configuration["DiscordAPI:ClientSecret"];
                    options.SaveTokens = true;

                    options.ClaimActions.MapJsonKey(JwtClaimTypes.Subject, "id");

                    options.Scope.Add("guilds");
                    options.Events.OnCreatingTicket = c =>
                    {
                        c.Identity.AddClaim(new Claim("discord", c.AccessToken));
                        return Task.CompletedTask;
                    };

                    options.SignInScheme = "DiscordExt";
                })
                .AddJwtBearer(options =>
                {
                    options.Authority = Configuration["OAuth:ApiAuthority"];
                    options.Audience = "api";
                })
                .AddCookie("DiscordExt", o => // For storing discord persistent tokens
                {
                    o.Cookie.Name = ".Discord";
                    o.ExpireTimeSpan = TimeSpan.FromDays(30);
                })
                .AddCookie(); // IdentityServer4

            services.AddAuthorization(o =>
            {
                o.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
            });

            services.AddIdentityServer()
                .AddDeveloperSigningCredential(filename: "token.rsa")
                .AddOperationalStore(o => o.ConfigureDbContext = c =>
                    c.UseNpgsql(dbConnectionString,
                        b => b.MigrationsAssembly("Alderto.Web")))
                .AddProfileService<AuthProfileService>()
                .AddInMemoryIdentityResources(new IdentityResource[]
                {
                    new IdentityResources.OpenId(),
                    new IdentityResources.Profile()
                })
                .AddInMemoryApiScopes(new[] { new ApiScope("api") })
                .AddInMemoryApiResources(new[]
                {
                    new ApiResource("api")
                    {
                        Scopes = new[] { "api" }
                    }
                })
                .AddInMemoryClients(new[]
                {
                    new Client
                    {
                        ClientId = "js",
                        ClientName = "Alderto Single Page Application",
                        AllowedGrantTypes = GrantTypes.Implicit,
                        RequireClientSecret = false,
                        RequireConsent = false,

                        RedirectUris = Configuration["OAuth:RedirectUris"].Split(';'),
                        PostLogoutRedirectUris = Configuration["OAuth:PostLogoutRedirectUris"].Split(';'),
                        AllowedCorsOrigins = Configuration["OAuth:AllowedCorsOrigins"].Split(';'),

                        AllowOfflineAccess = true,
                        RefreshTokenUsage = TokenUsage.OneTimeOnly,

                        AllowAccessTokensViaBrowser = true,

                        AllowedScopes = { "api", "openid" }
                    }
                });

            services.AddHttpClient<DiscordHttpClient>(o =>
            {
                o.BaseAddress = new Uri("https://discordapp.com/api/v6");
                o.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddScoped<AuthService>();

            // Add Mvc
            services
                .AddMvcCore()
                .AddApiExplorer()
                .ConfigureApiBehaviorOptions(options =>
                {
                    // options.InvalidModelStateResponseFactory = context =>
                    // {
                    //     var (key, value) = context.ModelState.First(s => s.Value.Errors.Count > 0);
                    //     var errorMsg = (string.IsNullOrWhiteSpace(key) ? "" : $"{key}: ") +
                    //                    value.Errors[0].ErrorMessage;
                    //     return new BadRequestObjectResult(new ValidationDomainException(errorMsg));
                    // };
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                    options.JsonSerializerOptions.Converters.Add(new SnowflakeConverter());
                    options.JsonSerializerOptions.Converters.Add(new NullableSnowflakeConverter());
                });

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration => configuration.RootPath = "ClientApp/dist");

            // === <Bot> ===
            // Add discord socket client
            services.AddDiscordBot(Configuration["DiscordAPI:BotToken"],
                cfg => { cfg.LogLevel = LogSeverity.Debug; },
                cfg =>
                {
                    cfg.DefaultRunMode = RunMode.Sync;
                    cfg.IgnoreExtraArgs = true;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, CommandHandler cmdHandler)
        {
            // Make sure the database is up to date
            Task.Run(() => UpdateDatabase(app));

            // Start the bot.
            Task.Run(cmdHandler.StartAsync);

            app.UseForwardedHeaders();

            if (env.IsProduction())
                app.UseHsts();

            app.UseHttpsRedirection();

            // Configure api routing.
            app.Map("/api", api =>
            {
                api.UseRouting();

                api.UseIdentityServer();
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
                        switch (e)
                        {
                            // Handle known API Exceptions.
                            case DomainException domainException:
                                context.Response.OnStarting(() =>
                                {
                                    context.Response.ContentType = "application/json";
                                    context.Response.StatusCode = (int) domainException.ErrorState.StatusCode;

                                    return Task.CompletedTask;
                                });

                                await context.Response.WriteAsync(
                                    JsonSerializer.Serialize(
                                        new { domainException.ErrorState, domainException.Message },
                                        new JsonSerializerOptions
                                            { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
                                break;

                            case HttpException discordException:
                                context.Response.OnStarting(() =>
                                {
                                    context.Response.ContentType = "application/json";
                                    context.Response.StatusCode = StatusCodes.Status400BadRequest;

                                    return Task.CompletedTask;
                                });

                                await context.Response.WriteAsync(JsonSerializer.Serialize(new
                                {
                                    ErrorState = new ErrorState(ErrorStatusCode.BadRequest),
                                    Message = discordException.DiscordCode switch
                                    {
                                        50001 => ErrorMessage.DISCORD_MISSING_PERMISSION_CHANNEL_READ,
                                        50013 => ErrorMessage.DISCORD_MISSING_PERMISSION_CHANNEL_WRITE,
                                        _ => throw e
                                    }
                                }, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
                                break;

                            default:
                                throw;
                        }
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

            await using var applicationDbContext = serviceScope.ServiceProvider.GetRequiredService<AldertoDbContext>();
            var applicationDbContextLogger =
                serviceScope.ServiceProvider.GetRequiredService<ILogger<AldertoDbContext>>();

            applicationDbContextLogger.LogInformation("Initializing Application Database...");
            await applicationDbContext.Database.MigrateAsync();
            applicationDbContextLogger.LogInformation("Database Application Ready!");


            await using var persistedGrantDbContext =
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();
            var persistedGrantDbContextLogger =
                serviceScope.ServiceProvider.GetRequiredService<ILogger<PersistedGrantDbContext>>();

            persistedGrantDbContextLogger.LogInformation("Initializing Auth Database...");
            await persistedGrantDbContext.Database.MigrateAsync();
            persistedGrantDbContextLogger.LogInformation("Database Auth Ready!");
        }
    }
}
