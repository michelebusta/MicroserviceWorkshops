using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Storage.Cosmos
{
    public class CosmosIdentityManagementStore : IIdentityManagementStore
    {
        private readonly string _database;
        private readonly string _collection;
        private readonly DocumentClient _client;

        public CosmosIdentityManagementStore(CosmosConfig options)
        {
            _database = options.Database;
            _collection = options.Collection;
            _client = new DocumentClient(new Uri(options.ServiceEndpoint), options.AuthKey);

            Initialize().Wait();
        }

        private async Task Initialize()
        {
            await _client.CreateDatabaseIfNotExistsAsync(new Database() { Id = _database });
            await
                _client.CreateDocumentCollectionIfNotExistsAsync(
                    UriFactory.CreateDatabaseUri(_database),
                    new DocumentCollection() { Id = _collection });
        }

        public async Task<string> Create(PasswordUser user)
        {
            try
            {
                var result =
                    await
                        _client.CreateDocumentAsync(
                            UriFactory.CreateDocumentCollectionUri(_database, _collection),
                            user);
                return result.Resource.Id;
            }
            catch (DocumentClientException ex)
            {
                if (ex.StatusCode != HttpStatusCode.Conflict)
                {
                    throw;
                }
                return user.Id.ToString("D");
            }
        }

        public async Task Create(JObject data)
        {
            var bytes = Encoding.UTF8.GetBytes(data.ToString());
            using (var ms = new MemoryStream(bytes))
            {
                await _client.CreateDocumentAsync(
                   UriFactory.CreateDocumentCollectionUri(_database, _collection),
                   JsonSerializable.LoadFrom<Document>(ms));
            }
        }

        private Uri MakeDocumentUri(Guid id)
        {
            return UriFactory.CreateDocumentUri(_database, _collection, id.ToString("D"));
        }

        public async Task Save(PasswordUser user)
        {
            await _client.ReplaceDocumentAsync(MakeDocumentUri(user.Id), user);
        }

        public Task<PasswordUser> Read(Guid id)
        {
            var options = new FeedOptions();
            options.EnableCrossPartitionQuery = true;
            var query = _client.CreateDocumentQuery<PasswordUser>(UriFactory.CreateDocumentCollectionUri(_database, _collection), options);

            var result = query
                .Where(x => x.Id == id)
                .ToList()
                .SingleOrDefault();
            return Task.FromResult(result);
        }
    }
}
