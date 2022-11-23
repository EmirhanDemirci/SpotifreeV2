using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Spotifree.AuthService.Logic.Interfaces;
using Spotifree.AuthService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Spotifree.AuthService.Logic.Helpers
{
    public class ConsumeRabbitMQHostedService : BackgroundService
    {
        private readonly ILogger _logger;
        private IConnection _connection;
        private IModel _channel;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly RabbitOptions _options;

        public ConsumeRabbitMQHostedService(ILoggerFactory loggerFactory, IServiceScopeFactory scopeFactory)
        {
            _options = JsonConvert.DeserializeObject<RabbitOptions>(Environment.GetEnvironmentVariable("SPOTIFREE_RABBITMQ"));
            _logger = loggerFactory.CreateLogger<ConsumeRabbitMQHostedService>();
            _scopeFactory = scopeFactory;
            InitRabbitMq();
        }

        private void InitRabbitMq()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _options.HostName,
                UserName = _options.UserName,
                Password = _options.Password,
                Port = _options.Port,
                VirtualHost = _options.VHost,
            };

            // create connection  
            _connection = factory.CreateConnection();

            // create channel  
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare("exchange.topic.user.create", ExchangeType.Topic, true);
            _channel.QueueDeclare("queue.user.create", true, false, false, null);
            _channel.QueueBind("queue.user.create", "exchange.topic.user.create", "*.queue.user.create.#", null);
            _channel.BasicQos(0, 1, false);

            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                // received message  
                var content = System.Text.Encoding.UTF8.GetString(ea.Body.ToArray());
                HandleMessage(content);

                // handle the received message
                _channel.BasicAck(ea.DeliveryTag, false);
            };

            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerConsumerCancelled;

            _channel.BasicConsume("queue.user.create", false, consumer);
            return Task.CompletedTask;
        }
        private void HandleMessage(string content)
        {
            // we just print this message   
            _logger.LogInformation($"consumer received {content}");
            using (var scope = _scopeFactory.CreateScope())
            {
                var authLogic = scope.ServiceProvider.GetRequiredService<IAuthLogic>();
                var messageContent = JsonSerializer.Deserialize<MessageContent>(content);
                authLogic.CreateCredentials(messageContent.credentials);
            }
        }
        private void OnConsumerConsumerCancelled(object sender, ConsumerEventArgs e) { }
        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e) { }
        private void OnConsumerRegistered(object sender, ConsumerEventArgs e) { }
        private void OnConsumerShutdown(object sender, ShutdownEventArgs e) { }
        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e) { }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }

    }
}
