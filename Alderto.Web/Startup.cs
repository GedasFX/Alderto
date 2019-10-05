using System;
using System.Threading.Tasks;
using Alderto.Bot;
using Alderto.Bot.Services;
using Alderto.Data;
using Alderto.Services;
using Alderto.Web.Helpers;
using Discord;
using Discord.Commands;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

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
        public void ConfigureServices(IServiceCollection services)
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

            ulong.TryParse(Configuration["Discord:NewsChannelId"], out var newsChannelId);
            services.AddNewsProvider(o => o.NewsChannelId = newsChannelId);
            services.AddMessagesManager();

            // === <Web> ===
            // Use discord as authentication service.
            services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddDiscord(options =>
                {
                    options.ClientId = Configuration["DiscordAPI:ClientId"];
                    options.ClientSecret = Configuration["DiscordAPI:ClientSecret"];
                    options.SaveTokens = true;

                    options.Scope.Add("guilds");
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
                .AddCookie(options =>
                {
                    options.Cookie.Name = ".Session";
                });
            services.AddAuthorization();

            // Add Mvc
            services
                .AddMvcCore()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new SnowflakeConverter());
                    options.JsonSerializerOptions.Converters.Add(new NullableSnowflakeConverter());
                });

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration => configuration.RootPath = "ClientApp/dist");

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

            // Configure api routing
            app.Map("/api", api =>
            {
                api.UseRouting();

                api.UseAuthentication();
                api.UseAuthorization();

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

        public async Task UpdateDatabase(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();

            await using var context = serviceScope.ServiceProvider.GetService<AldertoDbContext>();
            var logger = serviceScope.ServiceProvider.GetService<ILogger<DbContext>>();

            logger.Log(LogLevel.Information, "Initializing database...");
            await context.Database.MigrateAsync();
            logger.LogInformation("Database ready!");
        }
    }
}
