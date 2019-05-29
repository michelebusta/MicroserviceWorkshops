using Shared.Storage.Cosmos;
using Shared.Storage.Marten;

namespace Shared.Storage
{
    public class DocumentDbConfig
    {
        public DbBackend DbBackend { get; set; }
        public CosmosConfig CosmosConfig { get; set; }
        public MartenConfig MartenConfig { get; set; }
    }
}
