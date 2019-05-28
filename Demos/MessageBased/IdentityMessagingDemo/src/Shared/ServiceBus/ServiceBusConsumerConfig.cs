using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.ServiceBus
{
    public class ServiceBusConsumerConfig
    {
        public string AuthKeyName { get; set; }
        public string AuthKey { get; set; }
        public string Topic { get; set; }
        public string Subscription { get; set; }
        public string EndpointAddress { get; set; }

        public static ServiceBusConsumerConfig Bind(IConfiguration configuration)
        {
            var options = new ServiceBusConsumerConfig();
            configuration.GetSection(nameof(ServiceBusConsumerConfig)).Bind(options);
            return options;
        }
    }
}
