using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using Alderto.Application;
using Alderto.Bot;
using Alderto.Bot.Services;
using Alderto.Data;
using Alderto.Web.Helpers;
using Alderto.Web.Middleware;
using Discord;
using Discord.Commands;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

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

            // === <Storage> ===
            services.AddDbContext<AldertoDbContext>(options =>
            {
                if (Env.IsDevelopment())
                {
                    options.EnableSensitiveDataLogging();
                    IdentityModelEventSource.ShowPII = true;
                }

                options.UseNpgsql(dbConnectionString,
                    builder => builder.MigrationsAssembly("Alderto.Data"));
            });
            services.AddMemoryCache();

            // === <Domain> ===
            services.AddAlderto();

            // === <Bot> ===
            services.AddDiscordBot(Configuration["DiscordAPI:BotToken"],
                cfg => { cfg.LogLevel = LogSeverity.Debug; },
                cfg =>
                {
                    cfg.DefaultRunMode = RunMode.Sync;
                    cfg.IgnoreExtraArgs = true;
                });

            // === <Web> ===
            // For use behind reverse proxy
            services.Configure<ForwardedHeadersOptions>(o =>
            {
                o.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                o.KnownNetworks.Clear();
                o.KnownProxies.Clear();
            });

            // Security
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new JsonWebKey(File.ReadAllText("pki.jwk"))
                    };
                });

            services.AddAuthorization(o =>
            {
                o.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
            });

            // Mvc
            services
                .AddMvcCore()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                    options.JsonSerializerOptions.Converters.Add(new SnowflakeConverter());
                    options.JsonSerializerOptions.Converters.Add(new NullableSnowflakeConverter());
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app, CommandHandler cmdHandler)
        {
            // Make sure the database is up to date
            Task.Run(() => UpdateDatabase(app));

            // Start the bot.
            Task.Run(cmdHandler.StartAsync);

            app.UseForwardedHeaders();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<ValidateGuildPermissionsMiddleware>();
            app.UseMiddleware<DomainErrorHandlingMiddleware>();

            app.UseEndpoints(p => { p.MapControllers(); });
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
        }
    }
}
