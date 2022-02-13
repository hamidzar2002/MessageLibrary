using System.Threading.Tasks;

namespace Hz.Libraries.Messaging.events
{
    /// <summary>
    /// گذرگاه event ها
    /// تنها چیزی هست که کلاینت ها میشناسن 
    /// </summary>
    public interface IEventBus
    {
        //یک متد داره که ما یک
        //@event 
        //بهش میدیم میگیم برو این و اجرا کن
        void Raise<HzEvent>(HzEvent @event) where HzEvent : IEvent;
    }
}