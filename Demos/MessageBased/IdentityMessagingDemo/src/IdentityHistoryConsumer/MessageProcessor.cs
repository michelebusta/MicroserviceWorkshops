using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Shared.Storage;
using System;
using System.Threading.Tasks;

namespace IdentityHistoryConsumer
{
    class MessageProcessor
    {
        private readonly IIdentityManagementStore _store;
        private readonly ILogger _logger;

        public MessageProcessor(IIdentityManagementStore store, ILogger logger)
        {
            _store = store;
            _logger = logger;
        }

        public async Task Handle(string type, JObject message)
        {
            try
            {
                _logger.LogInformation("Processing message {0}", type);
                if (type == "TopicCheck") return;
                message["__messageType"] = type;
                await _store.Create(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error processing message");
                throw;
            }
        }

        public void Initialize()
        { }
    }
}
