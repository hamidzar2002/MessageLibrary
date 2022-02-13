namespace Hz.Libraries.Messaging.data
{
    public class EventCallbackBody<T> : Body
    {
        public string functionName { get; set; }
        public T output { get; set; }

    }
}
