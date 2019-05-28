using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Microsoft.Azure.ServiceBus;
using System.Threading.Tasks;

namespace Shared.ServiceBus
{
    public class ServiceBusConsumer : IDisposable
    {
        public delegate Task MessageHandler(string type, JObject message);

        private readonly ILogger _logger;
        private readonly TopicClient topicClient;
        private readonly ISubscriptionClient subscriptionClient;

        public ServiceBusConsumer(ILogger logger, IConfigurationSection config, MessageHandler handler)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            _logger = logger;

            logger.LogInformation("Starting consumer");

            var sboptions = new ServiceBusConsumerConfig();
            config.Bind(sboptions);

            var connectionString = new ServiceBusConnectionStringBuilder(sboptions.EndpointAddress, sboptions.Topic,
                sboptions.AuthKeyName,
                sboptions.AuthKey);
            topicClient = new TopicClient(connectionString);

            var messageProcessorHostName = Guid.NewGuid().ToString();
            var processor = new ServiceBusMessageProcessor(topicClient, logger, handler);

            var handlerOptpions = new MessageHandlerOptions(processor.LogException);
            handlerOptpions.AutoComplete = true;
            subscriptionClient = new SubscriptionClient(connectionString, sboptions.Subscription);
            subscriptionClient.RegisterMessageHandler(processor.Process, handlerOptpions);
        }

        public void Dispose()
        {
            topicClient.CloseAsync();
        }
    }
}
