using System;
using Alderto.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(Alderto.Web.Areas.Identity.IdentityHostingStartup))]
namespace Alderto.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<AldertoWebContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("AldertoWebContextConnection")));

                services.AddDefaultIdentity<IdentityUser>()
                    .AddEntityFrameworkStores<AldertoWebContext>();
            });
        }
    }
}