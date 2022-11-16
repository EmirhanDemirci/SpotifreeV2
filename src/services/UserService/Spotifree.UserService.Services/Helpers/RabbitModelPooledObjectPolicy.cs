using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotifree.UserService.Services.Helpers
{
    public class RabbitModelPooledObjectPolicy : IPooledObjectPolicy<IModel>
    {
        private readonly RabbitOptions _options;
        private readonly IConnection _connection;
        public RabbitModelPooledObjectPolicy()
        {
            _options = JsonConvert.DeserializeObject<RabbitOptions>(Environment.GetEnvironmentVariable("SPOTIFREE_RABBITMQ"));
            _connection = GetConnection();
        }

        private IConnection GetConnection()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _options.HostName,
                UserName = _options.UserName,
                Password = _options.Password,
                Port = _options.Port,
                VirtualHost = _options.VHost,
            };

            return factory.CreateConnection();
        }
        public IModel Create()
        {
            return _connection.CreateModel();
        }

        public bool Return(IModel obj)
        {
            if (obj.IsOpen)
            {
                return true;
            }

            obj?.Dispose();
            return false;
        }
    }
}
