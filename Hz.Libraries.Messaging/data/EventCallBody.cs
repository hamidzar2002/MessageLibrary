namespace Hz.Libraries.Messaging.data
{
    public class EventCallBody : Body
    {
        public string functionName { get; set; }
        public System.Collections.Generic.List<EventCallInput> inputs { get; set; }
    }
}
