using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spotifree.AuthService.Composition.Interfaces;
using Spotifree.AuthService.DataAccess.Data.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotifree.AuthService.Composition.Installer
{
    public class ServicesInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthService, DataAccess.Data.Services.AuthService>();
        }
    }
}
