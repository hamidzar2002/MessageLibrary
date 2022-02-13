 using System.Threading.Tasks;
using Hz.Libraries.Messaging.data;

namespace Hz.Libraries.Messaging.events
{
    public interface IEventHandler<in HzEvent> where HzEvent : IEvent
    {
        Message Handle(HzEvent @event);
    }
}