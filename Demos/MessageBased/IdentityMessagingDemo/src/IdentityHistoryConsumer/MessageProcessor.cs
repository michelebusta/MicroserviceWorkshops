using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Shared.Storage;
using System;
using System.Threading.Tasks;

namespace IdentityHistoryConsumer
{
    class MessageProcessor
    {
        private readonly DocDbConfigurationOptions _options;
        private readonly ILogger _logger;

        public MessageProcessor(DocDbConfigurationOptions options, ILogger logger)
        {
            _options = options;
            _logger = logger;
        }

        public async Task Handle(string type, JObject message)
        {
            try
            {
                _logger.LogInformation("Processing message {0}", type);
                if (type == "TopicCheck") return;
                message["__messageType"] = type;
                var store = new IdentityHistoryStore(_options);
                await store.Insert(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error processing message");
                throw;
            }
        }

        public void Initialize()
        {
            new IdentityHistoryStore(_options).Initialize().Wait();
        }
    }
}
