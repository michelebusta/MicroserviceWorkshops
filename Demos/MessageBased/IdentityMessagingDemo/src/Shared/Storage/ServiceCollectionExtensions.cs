using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared.Storage.Cosmos;
using Shared.Storage.Marten;

namespace Shared.Storage
{
    public static class ServiceCollectionExtensions
    {
        public static void AddIdentityManagementStore(this IServiceCollection services, ILogger logger, IConfiguration configuration)
        {
            var dbConfig = configuration
                .GetSection(nameof(DocumentDbConfig))
                .Get<DocumentDbConfig>();

            logger.LogInformation("Using DB backend {dbBackend}", dbConfig.DbBackend);
            switch (dbConfig.DbBackend)
            {
                case DbBackend.CosmosDb:
                    logger.LogInformation("Using Cosmos with config: {config}", JsonConvert.SerializeObject(dbConfig.CosmosConfig, Formatting.Indented));
                    services.AddSingleton(dbConfig.CosmosConfig);
                    services.AddTransient<IIdentityManagementStore, CosmosIdentityManagementStore>();
                    break;

                case DbBackend.Marten:
                    logger.LogInformation("Using Marten with config: {config}", JsonConvert.SerializeObject(dbConfig.MartenConfig, Formatting.Indented));
                    services.AddSingleton(dbConfig.MartenConfig);
                    services.AddTransient<IIdentityManagementStore, MartenIdentityManagementStore>();
                    break;
            }
        }
    }
}
