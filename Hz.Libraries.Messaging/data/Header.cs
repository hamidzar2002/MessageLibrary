using System;
namespace Hz.Libraries.Messaging.data
{
    public class Header
    {
        public Guid messageId { get; set; }
        public string senderId { get; set; }
        public System.Collections.Generic.List<string> receivers { get; set; }
        public string sentTimestamp { get; set; }
        public string expiryTimestamp { get; set; }
        public bool encrypted { get; set; }
        public bool acknowledgeRequired { get; set; }
        public Guid relationId { get; set; }
        public MessageType messageType { get;set; }
        public string dynamicType { get; set; }


    }
}
