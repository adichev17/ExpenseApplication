using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using MessageBus.Common;
using System.Diagnostics;
using FNSApi.Common.Cache;

namespace FNSApi.Messaging
{
    public class RabbitMQPhoneConsumer : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMemoryCache _memoryCache;
        public RabbitMQPhoneConsumer(IServiceScopeFactory scopeFactory, IMemoryCache memoryCache)
        {
            var factory = new ConnectionFactory
            {
                HostName = "rabbitmq",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(MessageBusConstants.ExchangeFns, ExchangeType.Fanout, durable: true);
            _channel.QueueDeclare(queue: MessageBusConstants.QueueFnsPhone, durable: true, exclusive: false, autoDelete: false, arguments: null);

            _memoryCache = memoryCache;
            _scopeFactory = scopeFactory;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                if (message is null) throw new ArgumentNullException();

                _memoryCache.Set(CacheItemKeys.PhoneCode, message);
                // Обрабатываем полученное сообщение
                Debug.WriteLine($"Получено сообщение: {message}");
                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(MessageBusConstants.QueueFnsPhone, false, consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
