using System.Threading.Tasks;
using Alderto.Data;
using Alderto.Data.Models;
using AspNet.Security.OAuth.Discord;
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
            services.AddDbContext<AldertoDbContext>(); // For identity.

            // Identity management. 
            services.AddIdentity<ApplicationUser, IdentityRole<ulong>>()
                .AddEntityFrameworkStores<AldertoDbContext>();

            services.ConfigureApplicationCookie(options =>
            {
                options.Events.OnRedirectToLogin = context =>
                {
                    context.HttpContext.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
                options.Events.OnRedirectToAccessDenied = context =>
                {
                    context.HttpContext.Response.StatusCode = 403;
                    return Task.CompletedTask;
                };
            });

            // Use discord as authentication service.
            services
                .AddAuthentication(
                    options =>
                    {
                        options.DefaultScheme = DiscordAuthenticationDefaults.AuthenticationScheme;
                    })
                .AddDiscord(options =>
                {
                    options.ClientId = Configuration["DiscordApp:ClientId"];
                    options.ClientSecret = Configuration["DiscordApp:ClientSecret"];

                    var oldAuth = options.Events.OnRedirectToAuthorizationEndpoint;
                    options.Events.OnRedirectToAuthorizationEndpoint = context =>
                    {
                        // TODO: Find a more elegant solution.
                        // Check if request header contains login.
                        // Design is that only those requests are intended to be logged in.
                        if (context.HttpContext.Request.Headers["login"].Count != 0)
                            return oldAuth(context);

                        // Return to SPA that client is unauthorized.
                        context.HttpContext.Response.StatusCode = 401;
                        return Task.CompletedTask;

                    };
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
