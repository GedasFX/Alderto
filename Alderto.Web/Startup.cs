using Alderto.Bot;
using Alderto.Bot.Services;
using Alderto.Data;
using Alderto.Services;
using Alderto.Web.Helpers;
using Discord;
using Discord.Commands;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddDbContext<IAldertoDbContext, AldertoDbContext>(options =>
                {
                    options.UseSqlServer(Configuration["DbConnectionString"]);
                });

            // Add database accessors.
            services.AddBotManagers();

            // === <Web> ===
            // Use discord as authentication service.
            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddDiscord(options =>
                {
                    options.ClientId = Configuration["DiscordApp:ClientId"];
                    options.ClientSecret = Configuration["DiscordApp:ClientSecret"];
                    options.SaveTokens = true;

                    options.Scope.Add("guilds");
                })
                .AddCookie(options =>
                {
                    options.Cookie.Name = ".Session";
                });

            // Add Mvc
            services
                .AddMvc()
                .AddJsonOptions(options =>
                {
                    options.UseCamelCasing(true);
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.Converters.Add(new SnowflakeConverter<ulong>());
                    options.SerializerSettings.Converters.Add(new SnowflakeConverter<ulong?>());
                });

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration => configuration.RootPath = "ClientApp/dist");

            // === <Bot> ===
            // Add discord socket client
            services.AddDiscordSocketClient(Configuration["DiscordApp:BotToken"],
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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, CommandHandler cmdHandler)
        {
            // Start the bot.
            cmdHandler.StartAsync().ConfigureAwait(false);

            if (env.IsDevelopment())
            {
                // app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseCookiePolicy();

            app.UseSpaStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "api",
                    template: "api/[controller]/[action]");
                routes.MapRoute(
                    name: "spa",
                    template: "{*url}");
            });

            app.UseSpa(spa =>
            {
                if (env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }
            });
        }
    }
}
