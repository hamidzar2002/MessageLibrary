using System;
using System.Collections.Generic;
using Hz.Libraries.Messaging.data;

namespace Hz.Libraries.Messaging.handler
{
    public interface MessageBuilder
    {
        public Message BuildMessage(Header header,Body body);
        public Body BuildBody<T>(MessageType messageType,T type) where T : class;
        public Body BuildBody(MessageType messageType);
        public Header BuildHeader();
        public Header BuildHeader(
            Guid messageId,
            string senderId,
            List<string> receivers,
            string sentTimestamp,
            string expiryTimestamp,
            bool encrypted,
            bool acknowledgeRequired,
            Guid relationId,
            MessageType messageType,
            string dynamicType
            );

      


    }
}
