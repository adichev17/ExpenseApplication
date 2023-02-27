namespace MessageBus.Common
{
    public interface IMessageProducer
    {
        void SendMessage<T>(T message, string exchange, string topic);
    }
}
