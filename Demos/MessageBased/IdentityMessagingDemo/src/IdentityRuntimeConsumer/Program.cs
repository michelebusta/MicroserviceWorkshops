using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shared.EventHubs;
using Shared.Kafka;
using Shared.Messages;
using Shared.ServiceBus;
using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Shared.RabbitMQ;

namespace IdentityRuntimeConsumer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const string applicationName = "Identity Runtime Consumer";

#if DEBUG
            Console.Title = applicationName;
#endif
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var loggerFactory = new LoggerFactory()
                .AddConsole(configuration.GetSection("Logging"));

            var logger = loggerFactory.CreateLogger(applicationName);

            logger.LogInformation("Starting up");

            var processor = new MessageProcessor(configuration, logger);
            if (string.Equals(configuration["EventsSystem"], "servicebus", StringComparison.CurrentCultureIgnoreCase))
            {
                var section = configuration.GetSection(nameof(ServiceBusConsumerConfig));

                //var producer = new ServiceBusProducer(logger, section);
                //producer.SendAsync("identity", new TopicCheck()).Wait();

                var tcs = new TaskCompletionSource<bool>();
                using (new ServiceBusConsumer(logger, section, processor.Handle))
                {
                    logger.LogInformation("Started");
                    tcs.Task.Wait();
                }
            }
            else if (string.Equals(configuration["EventsSystem"], "eventhub", StringComparison.CurrentCultureIgnoreCase))
            {
                var eventHubConfigurationSection = configuration.GetSection(nameof(EventHubConsumerConfig));

                var producer = new EventHubProducer(logger, eventHubConfigurationSection);
                producer.SendAsync("identity", new TopicCheck()).Wait();

                var tcs = new TaskCompletionSource<bool>();
                using (new EventHubConsumer(logger, eventHubConfigurationSection, processor.Handle))
                {
                    logger.LogInformation("Started");
                    tcs.Task.Wait();
                }
            }
            else if (string.Equals(configuration["EventsSystem"], "rabbitmq", StringComparison.CurrentCultureIgnoreCase))
            {
                var rabbitMQConfigurationSection = configuration.GetSection(nameof(RabbitMQConsumerConfig));

                var producer = new RabbitMQProducer(logger, rabbitMQConfigurationSection);
                producer.SendAsync("identity", new TopicCheck()).Wait();

                var tcs = new TaskCompletionSource<bool>();
                using (new RabbitMQConsumer(logger, rabbitMQConfigurationSection, processor.Handle))
                {
                    logger.LogInformation("Started");
                    tcs.Task.Wait();
                }
            }
            else
            {
                var kafkaConfigurationSection = configuration.GetSection("kafka");

                var p = new KafkaProducer(logger, kafkaConfigurationSection);
                p.SendAsync("identity", new TopicCheck()).Wait();

                var tcs = new TaskCompletionSource<bool>();
                using (new KafkaConsumer(logger, new List<string> { "identity" }, kafkaConfigurationSection, processor.Handle))
                {
                    logger.LogInformation("Started");
                    tcs.Task.Wait();
                }
            }
        }
    }
}
