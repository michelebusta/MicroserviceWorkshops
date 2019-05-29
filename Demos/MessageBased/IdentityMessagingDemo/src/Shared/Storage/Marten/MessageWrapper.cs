namespace Shared.Storage.Marten
{
    public class MessageWrapper
    {
        public string Id { get; set; } = string.Empty;
        public object Message { get; set; }
    }
}
