namespace Shared.EventHubs
{
    public class EventHubConsumerConfig : EventHubProducerConfig
    {
        public string Group { get; set; }
        public string StorageAccountName { get; set; }
        public string StorageAccountKey { get; set; }
        public string StorageContainerName { get; set; }
    }
}
