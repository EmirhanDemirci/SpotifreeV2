using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotifree.UserService.Services.Interfaces
{
    public interface IRabbitManager
    {
        void Publish<T>(T message, string exchangeName, string exchangeType, string routeKey)
            where T : class;
    }
}
