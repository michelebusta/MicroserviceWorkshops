using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace Shared.EventHubs
{
    public class EventHubConsumer : IDisposable
    {
        public delegate Task MessageHandler(string type, JObject message);

        private readonly ILogger _logger;
        private readonly EventProcessorHost _eventProcessorHost;
        private readonly string _eventHubName;

        public EventHubConsumer(ILogger logger, IConfigurationSection config, MessageHandler handler)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            _logger = logger;

            var options = new EventHubConsumerConfig();
            config.Bind(options);
            _eventHubName = options.TopicName;

            logger.LogInformation("Starting consumer");

            var connectionStringBuilder = new EventHubsConnectionStringBuilder($"Endpoint=sb://{options.Namespace}.servicebus.windows.net/;SharedAccessKeyName={options.AuthKeyName};SharedAccessKey={options.AuthKey}")
            {
                EntityPath = options.TopicName
            };

            var eventProcessorHostName = Guid.NewGuid().ToString();
            _eventProcessorHost = new EventProcessorHost(
                eventProcessorHostName,
                options.TopicName,
                options.Group,
                connectionStringBuilder.ToString(),
                $"DefaultEndpointsProtocol=https;AccountName={options.StorageAccountName};AccountKey={options.StorageAccountKey};EndpointSuffix=core.windows.net",
                options.StorageContainerName);

            _eventProcessorHost.RegisterEventProcessorFactoryAsync(new EventHubEventProcessorFactory(logger, handler)).Wait();
        }

        public void Dispose()
        {
            _eventProcessorHost.UnregisterEventProcessorAsync().Wait();
        }
    }
}
