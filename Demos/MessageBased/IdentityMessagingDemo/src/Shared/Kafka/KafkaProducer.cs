using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using RdKafka;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Kafka
{
    public class KafkaProducer : IEventProducer
    {
        private readonly ILogger _logger;
        private readonly Producer _producer;

        public KafkaProducer(ILogger logger, IConfigurationSection kafkaConfig)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            _logger = logger;
            var config = KafkaConfigReader.Read(kafkaConfig);
            KafkaConfigReader.LogConfig(_logger, config);
            config.Logger += OnLog;
            _producer = new Producer(config);
        }


        private void OnLog(string handle, int level, string fac, string buf)
        {
            if (string.IsNullOrEmpty(buf)) return;
            switch (level)
            {
                case 0:
                case 1:
                case 2:
                    _logger.LogCritical(buf);
                    break;
                case 3:
                    _logger.LogError(buf);
                    break;
                case 4:
                    _logger.LogWarning(buf);
                    break;
                case 5:
                case 6:
                    _logger.LogWarning(buf);
                    break;
                case 7:
                    _logger.LogDebug(buf);
                    break;
                default:
                    _logger.LogInformation(buf);
                    break;
            }
        }

        public async Task SendAsync(string topicName, object message)
        {
            JObject wrapper = new JObject(
                new JProperty("type", message.GetType().Name),
                new JProperty("message", JObject.FromObject(message))
                );
            using (var topic = _producer.Topic(topicName))
                await topic.Produce(Encoding.UTF8.GetBytes(wrapper.ToString()));
        }
    }
}
