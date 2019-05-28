using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace Shared.Storage
{
    public class IdentityManagementStore : IIdentityManagementStore
    {
        private readonly string _database;
        private readonly string _collection;
        private readonly DocumentClient _client;

        public IdentityManagementStore(DocDbConfigurationOptions options)
        {
            _database = options.Database;
            _collection = options.Collection;
            _client = new DocumentClient(options.ServiceEndpoint, options.Key);
        }

        public async Task Initialize()
        {
            await _client.CreateDatabaseIfNotExistsAsync(new Database() {Id = _database});
            await
                _client.CreateDocumentCollectionIfNotExistsAsync(
                    UriFactory.CreateDatabaseUri(_database),
                    new DocumentCollection() {Id = _collection});
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
