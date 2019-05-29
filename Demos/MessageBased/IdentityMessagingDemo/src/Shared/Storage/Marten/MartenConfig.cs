namespace Shared.Storage.Marten
{
    public class MartenConfig
    {
        public string UserId { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Database { get; set; }

        public string ConnectionString =>
            $"User ID={UserId};Password={Password};Host={Host};Port={Port};Database={Database};Pooling=true;";
    }
}
