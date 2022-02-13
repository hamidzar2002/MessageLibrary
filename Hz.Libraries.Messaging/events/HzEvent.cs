using System;
using Hz.Libraries.Messaging.data;

namespace Hz.Libraries.Messaging.events
{
    public class HzEvent : IEvent
    {
        public string Name { get; set; }
        public DateTime EndDateTime { get; set; }
        public Message Message { get; set; }
    }
}