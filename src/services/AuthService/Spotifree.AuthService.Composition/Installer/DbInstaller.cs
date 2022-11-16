using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spotifree.AuthService.Composition.Interfaces;
using Spotifree.AuthService.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotifree.AuthService.Composition.Installer
{
    public class DbInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            //TODO: Check the connectionstring and check if its possible to make an env variable
            services.AddTransient<ApplicationDbContext>()
                .AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException()));
        }
    }
}
