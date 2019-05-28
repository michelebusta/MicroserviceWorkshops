using Microsoft.Azure.EventHubs.Processor;
using Microsoft.Extensions.Logging;
using static Shared.EventHubs.EventHubConsumer;

namespace Shared.EventHubs
{
    class EventHubEventProcessorFactory : IEventProcessorFactory
    {
        private ILogger _logger;
        private MessageHandler _handler;

        public EventHubEventProcessorFactory(ILogger logger, MessageHandler handler)
        {
            _logger = logger;
            _handler = handler;
        }

        public IEventProcessor CreateEventProcessor(PartitionContext context)
        {
            return new EventHubEventProcessor(_logger, _handler);
        }
    }
}