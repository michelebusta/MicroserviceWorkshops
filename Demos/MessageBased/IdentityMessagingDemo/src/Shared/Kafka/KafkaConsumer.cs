using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using RdKafka;

namespace Shared.Kafka
{
    public class KafkaConsumer : IDisposable
    {
        public delegate Task MessageHandler(string type, JObject message);

        private readonly ILogger _logger;
        private readonly MessageHandler _handler;
        private readonly EventConsumer _consumer;

        public KafkaConsumer(ILogger logger, List<string> topics, IConfigurationSection kafkaConfig, MessageHandler handler)
        {
            _logger = logger;
            _handler = handler;
            var config = KafkaConfigReader.Read(kafkaConfig);
            config.EnableAutoCommit = false;
            KafkaConfigReader.LogConfig(_logger, config);
            config.Logger = OnLog;
            _consumer = new EventConsumer(config);
            _consumer.Subscribe(topics);
            
            _consumer.OnMessage += OnMessage;
            logger.LogInformation("Starting consumer");
            _consumer.Start();
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

        private void OnMessage(object sender, Message e)
        {
            _logger.LogInformation("Receiving message");
            try
            {
                var data = Encoding.UTF8.GetString(e.Payload);
                var json = JObject.Parse(data);
                var type = json["type"].Value<string>();
                var message = (JObject) json["message"];
                Handle(e, type, message).Wait();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(0, ex, "Failed to process message");
            }
        }

        private async Task Handle(Message kafkaMessage, string type, JObject message)
        {
            await _handler(type, message);
            await _consumer.Commit(kafkaMessage).ConfigureAwait(false);
        }

        public void Dispose()
        {
            _consumer.Dispose();
        }
    }
}