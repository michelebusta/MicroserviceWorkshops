namespace Shared.RabbitMQ
{
    public class RabbitMQProducerConfig
    {
        public string HostName { get; set; }
        public int Port { get; set; } = 5672;
        public string UserName { get; set; } = "guest";
        public string Password { get; set; } = "guest";
        public string TopicName { get; set; } = "identity";
    }
}
