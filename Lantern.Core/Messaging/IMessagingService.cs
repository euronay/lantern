using System.Threading.Tasks;

namespace Lantern.Core.Messaging
{
    public interface IMessagingService
    {
        Task SendMessageAsync(LightCommandMessage message);
    }
}