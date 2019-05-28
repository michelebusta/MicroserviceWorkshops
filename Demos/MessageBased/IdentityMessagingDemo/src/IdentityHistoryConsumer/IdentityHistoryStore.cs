using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json.Linq;
using Shared.Storage;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace IdentityHistoryConsumer
{
    class IdentityHistoryStore
    {
        private readonly DocumentClient _client;
        private readonly string _collection;
        private readonly string _database;

        public IdentityHistoryStore(DocDbConfigurationOptions options)
        {
            _database = options.Database;
            _collection = options.Collection;
            _client = new DocumentClient(options.ServiceEndpoint, options.Key);
        }

        public async Task Initialize()
        {
            await _client.CreateDatabaseIfNotExistsAsync(new Database() { Id = _database });
            await
                _client.CreateDocumentCollectionIfNotExistsAsync(
                    UriFactory.CreateDatabaseUri(_database),
                    new DocumentCollection() { Id = _collection });
        }

        public async Task Insert(JObject raw)
        {
            var bytes = Encoding.UTF8.GetBytes(raw.ToString());
            using (var ms = new MemoryStream(bytes))
            {

                await _client.CreateDocumentAsync(
                   UriFactory.CreateDocumentCollectionUri(_database, _collection),
                   JsonSerializable.LoadFrom<Document>(ms));
            }
        }
    }
}
