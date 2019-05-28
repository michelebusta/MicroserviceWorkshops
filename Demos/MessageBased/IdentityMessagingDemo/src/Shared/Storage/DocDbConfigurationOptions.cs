using System;
using Microsoft.Extensions.Configuration;

namespace Shared.Storage
{
    public class DocDbConfigurationOptions
    {
        public DocDbConfigurationOptions(IConfigurationSection configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration), "Configuration section cannot be null.");
            }
            if (string.IsNullOrWhiteSpace(configuration["serviceEndpoint"]))
            {
                throw new ArgumentException("Configuration value for serviceEndpoint cannot be null.", nameof(configuration));
            }

            if (string.IsNullOrWhiteSpace(configuration["authKeyOrResourceToken"]))
            {
                throw new ArgumentException("Configuration value for authKeyOrResourceToken cannot be null.", nameof(configuration));
            }

            if (string.IsNullOrWhiteSpace(configuration["database"]))
            {
                throw new ArgumentException("Configuration value for database cannot be null.", nameof(configuration));
            }

            if (string.IsNullOrWhiteSpace(configuration["collection"]))
            {
                throw new ArgumentException("Configuration value for collection cannot be null.", nameof(configuration));
            }

            ServiceEndpoint = new Uri(configuration["serviceEndpoint"]);
            Key = configuration["authKeyOrResourceToken"];
            Database = configuration["database"];
            Collection = configuration["collection"];
        }
        public Uri ServiceEndpoint { get; }
        public string Database { get; }
        public string Collection { get; }
        public string Key { get; }
    }
}