using Alderto.Data;
using Alderto.Data.Models;
using AspNet.Security.OAuth.Discord;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
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
            // Add database
            services.AddDbContext<IAldertoDbContext, AldertoDbContext>();

            // Identity management. 
            services.AddIdentity<ApplicationUser, IdentityRole<ulong>>();

            // Use discord as authentication service.
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = DiscordAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = DiscordAuthenticationDefaults.AuthenticationScheme;
                })
                .AddDiscord(options =>
                {
                    options.ClientId = Configuration["DiscordApp:ClientId"];
                    options.ClientSecret = Configuration["DiscordApp:ClientSecret"];
                });

            // Add Mvc
            services.AddMvc();

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration => { configuration.RootPath = "ClientApp/dist"; });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
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
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                    //spa.UseAngularCliServer("start");
                }
            });
        }
    }
}
