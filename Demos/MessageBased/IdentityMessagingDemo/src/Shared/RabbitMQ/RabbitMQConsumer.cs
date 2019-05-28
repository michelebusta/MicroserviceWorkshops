using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Shared.RabbitMQ
{
    public class RabbitMQConsumer : IDisposable
    {
        public delegate Task MessageHandler(string type, JObject message);

        private readonly ILogger _logger;
        private readonly MessageHandler _handler;
        private readonly ConnectionFactory _connectionFactory;
        private readonly IConnection _connection;
        private readonly IModel _model;
        private readonly EventingBasicConsumer _consumer;

        public RabbitMQConsumer(ILogger logger, IConfiguration config, MessageHandler handler)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));

            _logger = logger;
            _handler = handler;

            var options = new RabbitMQConsumerConfig();
            config.Bind(options);

            logger.LogInformation("Starting consumer");

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
            _model.QueueDeclare(options.Group, true, false, false, null);
            _model.QueueBind(options.Group, options.TopicName, string.Empty, null);

            _consumer = new EventingBasicConsumer(_model);
            _consumer.Received += Received;

            _model.BasicConsume(options.Group, false, _consumer);
        }

        private void Received(object sender, BasicDeliverEventArgs eventData)
        {
            _logger.LogInformation("Receiving message");
            try
            {
                var data = Encoding.UTF8.GetString(eventData.Body);
                _logger.LogDebug($"Message received. Data: '{data}'");

                if (string.IsNullOrWhiteSpace(data))
                {
                    _logger.LogCritical("Received empty message");
                    return;
                }

                var json = JObject.Parse(data);
                var type = json["type"].Value<string>();
                var message = (JObject)json["message"];
                Handle(eventData, type, message).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(0, ex, "Failed to process message");
            }
        }

        private async Task Handle(BasicDeliverEventArgs eventData, string type, JObject message)
        {
            await _handler(type, message);
            _model.BasicAck(eventData.DeliveryTag, false);
        }

        public void Dispose()
        {
            _consumer.Received -= Received;
            _model.Dispose();
            _connection.Dispose();
        }
    }
}
