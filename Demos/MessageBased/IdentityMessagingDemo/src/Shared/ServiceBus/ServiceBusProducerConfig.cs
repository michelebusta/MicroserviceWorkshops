using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.ServiceBus
{
    public class ServiceBusProducerConfig
    {
        public string AuthKeyName { get; set; }
        public string AuthKey { get; set; }
        public string Topic { get; set; }
        public string EndpointAddress { get; set; }

        public static ServiceBusProducerConfig Bind(IConfiguration configuration)
        {
            var options = new ServiceBusProducerConfig();
            configuration.GetSection(nameof(ServiceBusProducerConfig)).Bind(options);
            return options;
        }
    }
}
