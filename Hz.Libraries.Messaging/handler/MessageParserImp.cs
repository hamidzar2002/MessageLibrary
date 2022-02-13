using System;
using Hz.Libraries.Messaging.data;
using Newtonsoft.Json;


namespace Hz.Libraries.Messaging.handler
{
    public class MessageParserImp : MessageParser
    {
        readonly MessageBuilder  messageBuilder = new MessageBuilderImp();

        public Header HeaderParser(Header header, string jsonHeader)
        {
            try
            {
                header = JsonConvert.DeserializeObject<Header>(jsonHeader);
                return header;
            }
            catch (Exception ex)
            {

                throw new InvalidCastException("Error while trying to parse the Header  - HeaderParser", ex);
            }
        }
    

        public Body BodyParser<T>(Body body, string jsonBody )
        {
            try { 
           if(body is AckBody)
            {
                
                body = JsonConvert.DeserializeObject<AckBody>(jsonBody);
            }
            else if (body is DataBody<T>)
            {
              
                body = JsonConvert.DeserializeObject<DataBody<T>>(jsonBody);
                
            }
            else if (body is EventCallBody)
            {
                body = JsonConvert.DeserializeObject<EventCallBody>(jsonBody);
            }

            else if (body is EventCallbackBody<T>)
            {
                body = JsonConvert.DeserializeObject<EventCallbackBody<T>>(jsonBody);
            }

            return body;
            }
            catch (Exception ex)
            {

                throw new InvalidCastException("Error while trying to parse the Body  - BodyParser<T>", ex);
            }
        }


        public Body BodyParser(Body body, string jsonBody)
        {
            try { 
            if (body is AckBody)
            {
                body = JsonConvert.DeserializeObject<AckBody>(jsonBody);
            }
            else if (body is EventCallBody)
            {
                body = JsonConvert.DeserializeObject<EventCallBody>(jsonBody);
            }


            return body;
            }
            catch (Exception ex)
            {

                throw new InvalidCastException("Error while trying to parse the Body  - BodyParser", ex);
            }
        

        }   

        public Message MessageParser(Message message, string jsonMessage) {
            try {



                jsonMessage = jsonMessage.Replace("\"", "\\\"");
                jsonMessage =  jsonMessage.Replace("\\\"header\\\":{", "\"header\":\"{");
                jsonMessage = jsonMessage.Replace("\\\"body\\\":{", "\"body\":\"{");

                if (jsonMessage.Contains(",\"body\"")) {
                    jsonMessage = jsonMessage.Replace(",\"body\"", "\",\"body\"");

                }
                else {
                    jsonMessage = jsonMessage.Replace(",\"header\"", "\",\"header\"");
                }

                jsonMessage = jsonMessage.Insert(jsonMessage.Length -1, "\"");
                StringMessage m = JsonConvert.DeserializeObject<StringMessage>(jsonMessage);

             
                Header header = this.HeaderParser(message.header, m.header);


            if (header.messageType == MessageType.ack || header.messageType == MessageType.eventCall)
            {
                var body = messageBuilder.BuildBody(header.messageType);
                body = this.BodyParser(body, m.body);
                message.body = body;
            }
            else if(header.messageType == MessageType.data || header.messageType == MessageType.eventCallback) {
               var dynamicClass=  System.Activator.CreateInstance(Type.GetType(header.dynamicType));
                var body = messageBuilder.BuildBody<dynamic>(header.messageType,dynamicClass);
                body = this.BodyParser<dynamic>(body, m.body);
                message.body = body;
                

            }

            message.header = header;
            return message;
            }
            catch (Exception ex)
            {

                throw new InvalidCastException("Error while trying to parse the Message  - MessageParser", ex);
            }

        }


        public string Serialize(Message message , string dynamicType) {
            try { 
            string json = "";

            message.header.dynamicType = dynamicType;
            json = JsonConvert.SerializeObject(message);


            return json;
            }
            catch (Exception ex)
            {

                throw new InvalidCastException("Error while trying to Serialize the Mesasge with Dynamic Type  - Serialize", ex);
            }
        }

        public string Serialize(Message message)
        {

            try
            {
                string json = "";
                json = JsonConvert.SerializeObject(message);
                return json;
            }
            catch (Exception ex)
            {

                throw new InvalidCastException("Error while trying to Serialize the Mesasge without Dynamic Type  - Serialize", ex);
            }

        }





    }
}
