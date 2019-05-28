using System.Threading.Tasks;

namespace Shared
{
    public interface IEventProducer
    {
        Task SendAsync(string topicName, object message);
    }
}
