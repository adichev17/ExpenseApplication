using MessageBus.Common;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace MessageBus.RabbitMQProducers
{
    public sealed class RabbitMQProducer : IMessageProducer
    {
        public void SendMessage<T>(T message, string exchange, string topic)
        {
            //Here we specify the Rabbit MQ Server. we use rabbitmq docker image and use it
            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };
            //Create the RabbitMQ connection using connection factory details as i mentioned above
            var connection = factory.CreateConnection();
            //Here we create channel with session and model
            using (var channel = connection.CreateModel())
            {
                //Serialize the message
                var json = JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(json);
                //put the data on to the product queue
                channel.BasicPublish(exchange: exchange, routingKey: topic, body: body);
            }
        }
    }
}
