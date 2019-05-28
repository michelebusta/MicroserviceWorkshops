using Microsoft.Azure.EventHubs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Shared.EventHubs
{
    public class EventHubProducer : IEventProducer
    {
        private readonly ILogger _logger;
        private readonly EventHubClient _client;
        private readonly string _eventHubName;

        public EventHubProducer(ILogger logger, IConfigurationSection config)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            _logger = logger;

            var options = new EventHubProducerConfig();
            config.Bind(options);
            _eventHubName = options.TopicName;

            var connectionStringBuilder = new EventHubsConnectionStringBuilder($"Endpoint=sb://{options.Namespace}.servicebus.windows.net/;SharedAccessKeyName={options.AuthKeyName};SharedAccessKey={options.AuthKey}")
            {
                EntityPath = options.TopicName
            };

            _client = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());
        }

        public Task SendAsync(string topicName, object message)
        {
            return SendAsync(message);
        }

        private async Task SendAsync(object message)
        {
            _logger.LogInformation("Sending eventhub message to {topicName}: {message}", _eventHubName, message.GetType());

            var wrapper = new JObject(
                new JProperty("type", message.GetType().Name),
                new JProperty("message", JObject.FromObject(message))
                );

            var serializedWrapper = JsonConvert.SerializeObject(wrapper);
            _logger.LogDebug(serializedWrapper);

            var data = new EventData(Encoding.UTF8.GetBytes(serializedWrapper));
            await _client.SendAsync(data);
        }
    }
}
