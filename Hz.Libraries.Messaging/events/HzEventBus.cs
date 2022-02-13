using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hz.Libraries.Messaging.events
{
    public class HzEventBus : IEventBus
    {
        public List<object> _handlers = new List<object>();
        // readonly List<object> _handlers = new List<object>();
        // {
        //    new HzEventHandlerImp()
        //};

        public void  Raise<HzEvent>(HzEvent @event) where HzEvent : IEvent
        {
            var candidates = _handlers.OfType<IEventHandler<HzEvent>>().ToList();
            foreach (var handler in candidates)
            {
                 handler.Handle(@event);
            }
        }

       
    }
}