using System;
using System.Threading.Tasks;
using Hz.Libraries.Messaging.data;
using Hz.Libraries.Messaging.events;

namespace TestMessagingLib
{
    public  class HzEventHandlerImp : IEventHandler<HzEvent>
    {
        public  Message Handle(HzEvent @event)
        {
          
            Console.WriteLine(@event.Name + "......Handling HzMessage in program......");

            EventCallBody bod = (EventCallBody)@event.Message.body;
            string phoneNumber = "";
            string text = "";
            foreach (EventCallInput ev in bod.inputs)
            {

                if (ev.inputName.Equals("phoneNumber"))
                {
                    phoneNumber = (string)ev.inputValue;

                }
                if (ev.inputName.Equals("text"))
                {
                    text = (string)ev.inputValue;

                }
                Console.WriteLine(phoneNumber +" --> "+ text);
            }

            return @event.Message;
        }
    }
}