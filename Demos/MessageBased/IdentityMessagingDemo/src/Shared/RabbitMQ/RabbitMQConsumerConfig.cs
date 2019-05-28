namespace Shared.RabbitMQ
{
    public class RabbitMQConsumerConfig : RabbitMQProducerConfig
    {
        public string Group { get; set; }
    }
}
