using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spotifree.AuthService.Composition.Interfaces;
using Spotifree.AuthService.Logic;
using Spotifree.AuthService.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotifree.AuthService.Composition.Installer
{
    public class LogicInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthLogic, AuthLogic>();
        }
    }
}
