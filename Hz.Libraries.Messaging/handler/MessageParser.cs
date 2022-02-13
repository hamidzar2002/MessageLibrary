using Hz.Libraries.Messaging.data;

namespace Hz.Libraries.Messaging.handler
{
    public interface MessageParser
    {
        public Header HeaderParser(Header header,string jsonHeader);
        public Body BodyParser<T>(Body body, string jsonBody);
        public Body BodyParser(Body body, string jsonBody);
        public Message MessageParser(Message message, string jsonMessage);
        public string Serialize(Message message,string dynamicType);
        public string Serialize(Message message);





    }
}
