using MessageBus.Common;
using MessageBus.RabbitMQProducers;
using Microsoft.Extensions.DependencyInjection;

namespace MessageBus
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMessageBus(this IServiceCollection services)
        {
            services.AddSingleton<IMessageProducer, RabbitMQProducer>();
            return services;
        }
    }
}
