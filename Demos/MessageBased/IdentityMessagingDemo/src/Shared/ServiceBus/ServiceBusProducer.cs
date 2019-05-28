using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using Microsoft.Azure.ServiceBus;
using System.Threading.Tasks;

namespace Shared.ServiceBus
{
    public class ServiceBusProducer : IEventProducer
    {
        private readonly ILogger _logger;
        private readonly TopicClient topicClient;

        private readonly string _topic;

        public ServiceBusProducer(ILogger logger, IConfigurationSection config)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            _logger = logger;
            var sboptions = new ServiceBusProducerConfig();
            config.Bind(sboptions);

            _topic = sboptions.Topic;

            var connectionString = new ServiceBusConnectionStringBuilder(sboptions.EndpointAddress, sboptions.Topic,
                sboptions.AuthKeyName,
                sboptions.AuthKey);

            topicClient = new TopicClient(connectionString);
        }

        public Task SendAsync(string topicName, object message)
        {
            return SendAsync(message);
        }

        private async Task SendAsync(object message)
        {
            _logger.LogInformation("Sending servicebus message to {queuName}: {message}", _topic, message.GetType());

            var wrapper = new JObject(
                new JProperty("type", message.GetType().Name),
                new JProperty("message", JObject.FromObject(message))
                );

            var serializedWrapper = JsonConvert.SerializeObject(wrapper);
            _logger.LogDebug(serializedWrapper);

            var data = new Message(Encoding.UTF8.GetBytes(serializedWrapper));
            await topicClient.SendAsync(data);
        }
    }
}
