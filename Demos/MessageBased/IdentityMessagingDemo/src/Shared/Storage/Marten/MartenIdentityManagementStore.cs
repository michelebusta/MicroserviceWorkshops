using Marten;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Storage.Marten
{
    public class MartenIdentityManagementStore : IIdentityManagementStore
    {
        private readonly string _database;
        private readonly DocumentStore _store;

        public MartenIdentityManagementStore(MartenConfig options)
        {
            _store = DocumentStore.For(options.ConnectionString);
            _database = options.Database;

            Initialize(options.ConnectionString);
        }

        private void Initialize(string connectionString)
        {
            var store = DocumentStore.For(options =>
            {
                options.Connection(connectionString);
                options.CreateDatabasesForTenants(c =>
                {
                    c.ForTenant()
                    .CheckAgainstPgDatabase()
                    .WithOwner("postgres")
                    .WithEncoding("UTF-8")
                    .ConnectionLimit(-1)
                    .OnDatabaseCreated(_ => Console.WriteLine($"Created database: {_database}"));
                });
            });

            // Open a session for querying, loading, and
            // updating documents, triggers db create
            using (store.LightweightSession())
            {
            }
        }

        public async Task<string> Create(PasswordUser user)
        {
            try
            {
                using (var session = _store.LightweightSession())
                {
                    session.Store(user);
                    await session.SaveChangesAsync();
                }

                return user.Id.ToString("D");
            }
            catch (Exception)
            {
                return user.Id.ToString("D");
            }
        }

        public async Task Create(JObject data)
        {
            try
            {
                var wrapper = new MessageWrapper
                {
                    Id = Guid.NewGuid().ToString(),
                    Message = data
                };

                using (var session = _store.LightweightSession())
                {
                    session.Store(wrapper);
                    await session.SaveChangesAsync();
                }
            }
            catch (Exception)
            { }
        }

        public async Task Save(PasswordUser user)
        {
            using (var session = _store.LightweightSession())
            {
                session.Update(user);
                await session.SaveChangesAsync();
            }
        }

        public Task<PasswordUser> Read(Guid id)
        {
            PasswordUser result;
            using (var session = _store.LightweightSession())
            {
                result = session.Query<PasswordUser>()
                    .Where(x => x.Id == id)
                    .ToList()
                    .SingleOrDefault();
            }
            return Task.FromResult(result);
        }
    }
}
