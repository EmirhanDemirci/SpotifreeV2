using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spotifree.UserService.Composition.Installer.Interface;
using Spotifree.UserService.DataAccess.Data.Repository;
using Spotifree.UserService.DataAccess.Data.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotifree.UserService.Composition.Installer
{
    public class RepoInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
