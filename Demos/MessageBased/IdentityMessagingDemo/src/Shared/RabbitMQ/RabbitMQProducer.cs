using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Shared.RabbitMQ
{
    public class RabbitMQProducer : IEventProducer
    {
        private readonly ILogger _logger;
        private readonly ConnectionFactory _connectionFactory;
        private readonly IConnection _connection;
        private readonly IModel _model;
        private readonly string _exchangeName;

        public RabbitMQProducer(ILogger logger, IConfigurationSection config)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            var options = new RabbitMQProducerConfig();
            config.Bind(options);
            _exchangeName = options.TopicName;

            _connectionFactory = new ConnectionFactory
            {
                HostName = options.HostName,
                Port = options.Port,
                UserName = options.UserName,
                Password = options.Password
            };

            _connection = _connectionFactory.CreateConnection();
            _model = _connection.CreateModel();
            _model.ExchangeDeclare(options.TopicName, ExchangeType.Fanout, true);
        }

        public Task SendAsync(string topicName, object message)
        {
            return SendAsync(message);
        }

        private async Task SendAsync(object message)
        {
            _logger.LogInformation("Sending rabbitmq message to {topicName}: {message}", _exchangeName, message.GetType());

            var wrapper = new JObject(
                new JProperty("type", message.GetType().Name),
                new JProperty("message", JObject.FromObject(message))
            );

            var serializedWrapper = JsonConvert.SerializeObject(wrapper);
            _logger.LogDebug(serializedWrapper);

            var data = Encoding.UTF8.GetBytes(serializedWrapper);
            _model.BasicPublish(_exchangeName, string.Empty, null, data);

            await Task.CompletedTask;
        }
    }
}
