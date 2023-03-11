using ExpenseTracker.Application.Common.Interfaces.Repositories;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Infrastructure.Persistence;
using MessageBus.Common;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace ExpenseTracker.API.Messaging
{
    public class RabbitMQConsumer : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceScopeFactory _scopeFactory;

        public RabbitMQConsumer(IServiceScopeFactory scopeFactory)
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
            _channel.ExchangeDeclare(MessageBusConstants.ExchangeUsers, ExchangeType.Fanout, durable: true);
            _channel.QueueDeclare(queue: MessageBusConstants.QueueUserRegister, durable: true, exclusive: false, autoDelete: false, arguments: null);

            _scopeFactory = scopeFactory;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                var message = JsonSerializer.Deserialize<UserEntity>(content);
                if (message is null) throw new ArgumentNullException();

                var userEntity = new UserEntity
                {
                    Login = message.Login,
                    Password = message.Password,
                    CreatedOnUtc = message.CreatedOnUtc,
                };

                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ExpenseTrackerDBContext>();
                    using var transaction = dbContext.Database.BeginTransaction();
                    try
                    {                  
                        dbContext.Add(userEntity);
                        dbContext.SaveChanges();
                        transaction.Commit();
                    }                 
                    catch (Exception)
                    {
                        transaction.Rollback();
                        //Should be logging
                        throw;
                    }
                }

                // Обрабатываем полученное сообщение
                Debug.WriteLine($"Получено сообщение: {content}");

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(MessageBusConstants.QueueUserRegister, false, consumer);

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
