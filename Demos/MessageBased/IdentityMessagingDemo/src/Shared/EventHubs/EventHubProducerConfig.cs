namespace Shared.EventHubs
{
    public class EventHubProducerConfig
    {
        public string Namespace { get; set; }
        public string AuthKeyName { get; set; }
        public string AuthKey { get; set; }
        public string TopicName { get; } = "eventdemos";
    }
}
