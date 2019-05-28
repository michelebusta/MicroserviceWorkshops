using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using static Shared.EventHubs.EventHubConsumer;

namespace Shared.EventHubs
{
    class EventHubEventProcessor : IEventProcessor
    {
        private ILogger _logger;
        private MessageHandler _handler;
        private Stopwatch _checkpointStopwatch;

        public EventHubEventProcessor(ILogger logger, MessageHandler handler)
        {
            _logger = logger;
            _handler = handler;
        }

        public async Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            _logger.LogInformation($"Processor Shutting Down. Partition '{context.PartitionId}', Reason: '{reason}'.");
            if (reason == CloseReason.Shutdown)
            {
                await context.CheckpointAsync();
            }
            //return Task.CompletedTask;
        }

        public Task OpenAsync(PartitionContext context)
        {
            _logger.LogInformation($"{nameof(EventHubEventProcessor)} initialized. Partition: '{context.PartitionId}'");
            _checkpointStopwatch = new Stopwatch();
            _checkpointStopwatch.Start();
            return Task.CompletedTask;
        }

        public Task ProcessErrorAsync(PartitionContext context, Exception error)
        {
            _logger.LogError($"Error on Partition: {context.PartitionId}, Error: {error.Message}");
            return Task.CompletedTask;
        }

        public async Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            _logger.LogInformation("Receiving messages");

            foreach (var eventData in messages)
            {
                try
                {
                    var data = Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);
                    _logger.LogDebug($"Message received. Partition: '{context.PartitionId}', Data: '{data}'");

                    var json = JObject.Parse(data);
                    var type = json["type"].Value<string>();
                    var message = (JObject)json["message"];
                    await HandleAsync(eventData, type, message);
                }
                catch (Exception e)
                {
                    _logger.LogCritical(0, e, "Failed to process message");
                }
            }

            //Call checkpoint every 5 minutes, so that worker can resume processing from 5 minutes back if it restarts.
            if (_checkpointStopwatch.Elapsed > TimeSpan.FromMinutes(5))
            {
                await context.CheckpointAsync();
                _checkpointStopwatch.Restart();
            }

            //return context.CheckpointAsync();
        }

        private async Task HandleAsync(EventData eventData, string type, JObject message)
        {
            await _handler(type, message);
        }
    }
}