using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Shared.ServiceBus
{
    public class ServiceBusMessageProcessor
    {
        private readonly TopicClient _client;
        private readonly ServiceBusConsumer.MessageHandler _handler;
        private readonly ILogger _logger;
        
        public ServiceBusMessageProcessor(TopicClient client, ILogger logger, ServiceBusConsumer.MessageHandler handler)
        {
            _client = client;
            _handler = handler;
            _logger = logger;
        }

        public async Task Process(Message msg, CancellationToken ct)
        {
            _logger.LogInformation("Receiving message");

            string data = Encoding.UTF8.GetString(msg.Body);
            if (string.IsNullOrWhiteSpace(data))
            {
                _logger.LogWarning("Received empty message");
            }

            JObject json = JObject.Parse(data);
            var type = json["type"].Value<string>();
            var message = (JObject)json["message"];
            await HandleAsync(type, message);
            
        }

        private async Task HandleAsync(string type, JObject message)
        {
            Console.WriteLine(message);
            if (type=="EmailConfirmed")
            {
                Console.WriteLine("");
            }
            await _handler(type, message);
        }

        public Task LogException(ExceptionReceivedEventArgs arg)
        {
            _logger.LogError("Exception handling message {clientId} {action} {endpoint} {entityPath} {message}",
                arg.ExceptionReceivedContext.ClientId,
                arg.ExceptionReceivedContext.Action,
                arg.ExceptionReceivedContext.Endpoint,
                arg.ExceptionReceivedContext.EntityPath,
                arg.Exception.Message);
            return Task.CompletedTask;
        }
    }
}