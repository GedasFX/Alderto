using Alderto.Data;
using Alderto.Data.Models;
using Microsoft.AspNetCore.Authorization;
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
            services.AddDbContext<AldertoDbContext>(); // For Identity. Does not affect performance.

            // Identity management. 
            services.AddIdentity<ApplicationUser, IdentityRole<ulong>>()
                .AddEntityFrameworkStores<AldertoDbContext>();

            services.ConfigureApplicationCookie(options => {
                options.LoginPath = "/api/account/login";
                options.LogoutPath = "/api/account/logout";
            });

            // Use discord as authentication service.
            services.AddAuthentication()
                .AddDiscord(options =>
                {
                    options.ClientId = Configuration["DiscordAuth:ClientId"];
                    options.ClientSecret = Configuration["DiscordAuth:ClientSecret"];
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
