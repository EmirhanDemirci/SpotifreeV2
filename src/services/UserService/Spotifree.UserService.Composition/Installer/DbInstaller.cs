using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spotifree.UserService.Composition.Installer.Interface;
using Spotifree.UserService.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotifree.UserService.Composition.Installer
{
    public class DbInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            //debug purposes
            services.AddTransient<ApplicationDbContext>()
                .AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException()));

            //services.AddTransient<ApplicationDbContext>()
            //    .AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
            //    Environment.GetEnvironmentVariable("SPOTIFREE_USERSERVICE_DB") ?? throw new InvalidOperationException()));
        }
    }
}