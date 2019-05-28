using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RdKafka;

namespace Shared.Kafka
{
    public static class KafkaConfigReader
    {
        public static Config Read(IConfigurationSection configuration)
        {
            var config = new Config();
            foreach (var setting in configuration.GetChildren())
            {
                // some environments capitalize config keys, we need them to be lower case
                // for the underlying c lib 
                var key = setting.Key.ToLowerInvariant();
                if (key != "default.topic.config")
                {
                    config[key] = setting.Value;
                }
                else
                {
                    TopicConfig tc = new TopicConfig();
                    var topicConfig = setting.GetChildren();
                    foreach (var topicSetting in topicConfig)
                    {
                        key = topicSetting.Key.ToLowerInvariant();
                        tc[key] = topicSetting.Value;
                    }
                    config.DefaultTopicConfig = tc;
                }
            }
            config.EnableAutoCommit = false;
            return config;
        }

        public static void LogConfig(ILogger logger, Config config)
        {
            var final = config.Dump();
            var kafkaDump = string.Join(Environment.NewLine, final.Select(x => $"{x.Key} : {x.Value}"));
            if (config.DefaultTopicConfig != null)
            {
                var topicDump = string.Join(Environment.NewLine,
                    config.DefaultTopicConfig.Dump().Select(x => $"{x.Key} : {x.Value}"));
                logger.LogInformation("Kafka config: \r\n {0}\r\n\r\nTopic config: \r\n{1}", kafkaDump, topicDump);
            }
            else
            {
                logger.LogInformation("Kafka config: \r\n {0}", kafkaDump);
            }
        }

    }
}