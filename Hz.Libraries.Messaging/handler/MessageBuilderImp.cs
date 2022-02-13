using System;
using Hz.Libraries.Messaging.data;
using System.Collections.Generic;


namespace Hz.Libraries.Messaging.handler
{
    public class MessageBuilderImp : MessageBuilder
    {


        public Message BuildMessage(Header header, Body body) {

            Message message = new Message();
            message.header = header;
            message.body = body;
            return message;
        }
        public Body BuildBody<T>(MessageType messageType, T type) where T : class
        {



            switch (messageType)
            {
                case MessageType.ack:
                    {
                        AckBody ackBody = new AckBody();
                        return ackBody;
                    }
                case MessageType.data:
                    {
                        var dataBody = new DataBody<T>() { entityBody = type };
                        return dataBody;

                    }
                case MessageType.eventCall:
                    {
                        EventCallBody eventCallBody = new EventCallBody();
                        return eventCallBody;
                    }
                case MessageType.eventCallback:
                    {
                        var eventCallbackBody = new EventCallbackBody<T>() { output = type };
                        return eventCallbackBody;
                    }
                default:
                    {
                        return null;
                    }
            }

        }

        public Body BuildBody(MessageType messageType)
        {


            if (messageType.Equals(MessageType.data) || messageType.Equals(MessageType.eventCallback)) {
                throw new System.InvalidOperationException("This message type in not supported by this function! You may fix it by calling BuildBody<T> function."); ;
            }
            switch (messageType)
            {
                case MessageType.ack:
                    {
                        AckBody ackBody = new AckBody();
                        return ackBody;
                    }
                case MessageType.eventCall:
                    {
                        EventCallBody eventCallBody = new EventCallBody();
                        return eventCallBody;
                    }

                default:
                    {
                        return null;
                    }
            }

        }
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
            )
        {
            Header header = new Header();
            header.messageId = messageId;
            header.senderId = senderId;
            header.receivers = receivers;
            header.sentTimestamp = sentTimestamp;
            header.expiryTimestamp = expiryTimestamp;
            header.encrypted = encrypted;
            header.acknowledgeRequired = acknowledgeRequired;
            header.relationId = relationId;
            header.messageType = messageType;
            header.dynamicType = dynamicType;

            return header;
        }

        public Header BuildHeader() {
            return new Header();
    }
    }
}
