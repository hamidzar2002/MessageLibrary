using System;
namespace Hz.Libraries.Messaging.data
{
    public class AckBody : Body
    {
        public Guid refMessageId { get; set; }
        public AckStatusCode statusCode { get; set; }
        public string message { get; set; }
    }
}
