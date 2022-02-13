namespace Hz.Libraries.Messaging.rabbitmq
{
    public interface IRabbitMqProducer<in T>
    {
        void Publish(T @event);
    }
}
