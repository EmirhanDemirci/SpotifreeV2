using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using Spotifree.UserService.Composition.Installer.Interface;
using Spotifree.UserService.Services.Interfaces;
using RabbitMQ.Client;
using Spotifree.UserService.Services.Helpers;
using Spotifree.UserService.Services;

namespace Spotifree.UserService.Composition.Installer
{
    public class RabbitInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            services.AddSingleton<IPooledObjectPolicy<IModel>, RabbitModelPooledObjectPolicy>();
            services.AddSingleton<IRabbitManager, RabbitManager>();

        }
    }
}
