using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Hz.Libraries.Messaging;
using Hz.Libraries.Messaging.data;
using Hz.Libraries.Messaging.events;
using Hz.Libraries.Messaging.handler;
using Newtonsoft.Json;

namespace TestMessagingLib
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Sample use of message producer and consumer");

            //    string jsonMessage = "{body:\"{\"refMessageId\":\"00000000-0000-0000-0000-000000000000\",\"statusCode\":0,\"message\":null}\"}";
            string jsonMessage = "{\"body\":\"{\\\"header\\\":{\\\"messageId\\\":\\\"6511f067-ba98-46d6-8b3c-d070c594506f\\\",\\\"senderId\\\":\\\"\\\",\\\"receivers\\\":null,\\\"sentTimestamp\\\":\\\"\\\",\\\"expiryTimestamp\\\":\\\"\\\",\\\"encrypted\\\":false,\\\"acknowledgeRequired\\\":true,\\\"relationId\\\":\\\"e8b93f22-4cf2-4eb6-a7e9-9142c1e9e140\\\",\\\"messageType\\\":3,\\\"dynamicType\\\":null},\\\"body\\\":{\\\"refMessageId\\\":\\\"00000000-0000-0000-0000-000000000000\\\",\\\"statusCode\\\":0,\\\"message\\\":null}}\"}";
            StringMessage m2121 = JsonConvert.DeserializeObject<StringMessage>(jsonMessage);
            /*
            //build and send a new message without Dynamic type ack
            RabbitmqProduceHandler rabbitmqProduceHandler1 = new RabbitmqProduceHandler("185.239.106.227", 5672, "rab", "rab123");
            rabbitmqProduceHandler1.setup("testQQ", "testEXX", "testQQ", "direct", true, false, null);
            MessageBuilder messageBuilder1 = new MessageBuilderImp();
            Header header1 = messageBuilder1.BuildHeader(Guid.NewGuid(), "", null, "", "", true, false, Guid.NewGuid(), MessageType.ack, null);
            AckBody body1 = (AckBody)messageBuilder1.BuildBody(MessageType.ack);
            body1.statusCode = AckStatusCode.received;
            Message message1 = messageBuilder1.BuildMessage(header1, body1);
            rabbitmqProduceHandler1.produce(message1);
            */
            //build and send a new message without Dynamic type eventCall
            RabbitmqProduceHandler rabbitmqProduceHandler4 = new RabbitmqProduceHandler("185.239.106.227", 5672, "rab", "rab123");
            rabbitmqProduceHandler4.setup("testQQ", "testEXX", "testQQ", "direct", true, false, null);
            MessageBuilder messageBuilder4 = new MessageBuilderImp();
            Header header4 = messageBuilder4.BuildHeader(Guid.NewGuid(), "", null, "", "", false,false, Guid.NewGuid(), MessageType.eventCall, null);
            EventCallBody body4 = (EventCallBody)messageBuilder4.BuildBody(MessageType.eventCall);
            body4.functionName = "SendSMS";
            EventCallInput eventCallInput1 = new EventCallInput();
            eventCallInput1.inputName = "phoneNumber";
            eventCallInput1.inputValue = "09126222588";
            EventCallInput eventCallInput2 = new EventCallInput();
            eventCallInput2.inputName = "text";
            eventCallInput2.inputValue = "Hi!";
            List<EventCallInput> inputs = new List<EventCallInput>();
            inputs.Add(eventCallInput1);
            inputs.Add(eventCallInput2);
            body4.inputs = inputs;
            Message message4 = messageBuilder4.BuildMessage(header4, body4);
            rabbitmqProduceHandler4.produce(message4);

            /*
            //build and send a new message with Dynamic type DataBody
            RabbitmqProduceHandler rabbitmqProduceHandler2 = new RabbitmqProduceHandler("185.239.106.227", 5672, "rab", "rab123");
            rabbitmqProduceHandler2.setup("testQQ", "testEXX", "testQQ", "direct", true, false, null);
            MessageBuilder messageBuilder2 = new MessageBuilderImp();
            Header header2 = messageBuilder2.BuildHeader(Guid.NewGuid(), "", null, "", "", false, true, Guid.NewGuid(), MessageType.data, "Hz.Libraries.Messaging.Class1") ;
            Class1 c1 = new Class1(); 
            DataBody<Class1> body2 = (DataBody<Class1>)messageBuilder2.BuildBody<Class1>(MessageType.data, c1);
            body2.entityName = "c1";
            body2.entityBody.st = "sssss";
            Message message2 = messageBuilder2.BuildMessage(header2, body2);
            rabbitmqProduceHandler2.produce(message2);


            //build and send a new message with Dynamic type eventCallback
            RabbitmqProduceHandler rabbitmqProduceHandler3 = new RabbitmqProduceHandler("185.239.106.227", 5672, "rab", "rab123");
            rabbitmqProduceHandler3.setup("testQQ", "testEXX", "testQQ","direct", true, false, null);
            MessageBuilder messageBuilder3 = new MessageBuilderImp();
            Header header3 = messageBuilder3.BuildHeader(Guid.NewGuid(), "", null, "", "", false, true, Guid.NewGuid(), MessageType.eventCallback, "Hz.Libraries.Messaging.Class1");
            Class1 cc1 = new Class1();
            EventCallbackBody<Class1> body3 = (EventCallbackBody<Class1>)messageBuilder3.BuildBody<Class1>(MessageType.eventCallback, cc1);
            body3.functionName = "ff1";
            body3.output.st = "sssss";
            Message message3 = messageBuilder3.BuildMessage(header3, body3);
            rabbitmqProduceHandler3.produce(message3);
            */


            //receive and build up a message
           IEventHandler<HzEvent> HzEventHandlerImp = new HzEventHandlerImp();
            List<object> handlers = new List<object> {
                HzEventHandlerImp
            };
            RabbitmqConsumeHandler rabbitmqConsumeHandler = new RabbitmqConsumeHandler("185.239.106.227", 5672, "rab", "rab123");
            rabbitmqConsumeHandler.setup("testQQ", "testEXX", "testQQ", "direct", true, false, null, handlers);

            while (true)
            {
            }
                /*     object receivedMessage = new Message();
                     while (true)
                     {
                         if (InternalQueue.Instance.messageQueue.TryDequeue(out receivedMessage))
                         {
                             Message m = (Message)receivedMessage;
                             EventCallBody bod = (EventCallBody)m.body;
                             string phoneNumber = "";
                             string text = "";
                             foreach (EventCallInput ev in bod.inputs) {

                                 if (ev.inputName.Equals("phoneNumber")) {
                                     phoneNumber = (string)ev.inputValue;

                                 }
                                 if (ev.inputName.Equals("text")) {
                                     text = (string)ev.inputValue;

                                 }
                             }
                           Console.WriteLine(m.body.GetType());

                         }
                         Thread.Sleep(1000);
                     }
                */
                /*  
                  string jsonMessage = "{\"header\":{\"messageId\":\"9580d8f4-d404-4ee1-a2b4-4c944536d572\",\"senderId\":\"\",\"receivers\":null,\"sentTimestamp\":\"\",\"expiryTimestamp\":\"\",\"encrypted\":false,\"acknowledgeRequired\":true,\"relationId\":\"a252b747-b0e9-468d-90e8-15529fecbdb2\",\"messageType\":1,\"dynamicType\":null},\"body\":{\"functionName\":\"SendSMS\",\"inputs\":[{\"inputName\":\"phoneNumber\",\"inputValue\":\"09126222588\"},{\"inputName\":\"text\",\"inputValue\":\"Hi!\"}]}}";

                  MessageParser messageParser = new MessageParserImp();
                  messageParser.MessageParser(null, jsonMessage);
                */
                /*    MessageParser messageParser = new MessageParserImp();
                    MessageBuilder messageBuilderImp = new MessageBuilderImp();
                    RabbitmqProduceHandler mph = new RabbitmqProduceHandler("185.239.106.227", 5672 , "rab","rab123");
                    RabbitmqConsumeHandler mch = new RabbitmqConsumeHandler("185.239.106.227", 5672, "rab", "rab123");
                    string jsonHeader = "{\"senderId\":\"111\",\"sentTimestamp\":\"2323242342543\",\"messageType\":\"ack\"}";
                    Header header1 = messageBuilderImp.BuildHeader();
                    //header1 = messageParser.HeaderParser(header1,jsonHeader);


                    string jsonBody = "{\"entityName\":\"111\"}";

                    var x = typeof(String);
                    DataBody<String> ackBody = (DataBody<String>)messageBuilderImp.BuildBody<String>(MessageType.data,x.ToString());
                   //ackBody = (DataBody<String>)messageParser.BodyParser<String>(ackBody, jsonBody);


                    string jsonMessage = "{\"header\":{\"messageId\":\"00000000-0000-0000-0000-000000000000\",\"senderId\":\"111\",\"receivers\":null,\"sentTimestamp\":null,\"expiryTimestamp\":null,\"encrypted\":false,\"acknowledgeRequired\":false,\"relationId\":\"00000000-0000-0000-0000-000000000000\",\"messageType\":0,\"dynamicType\":\"Hz.Libraries.Messaging.data.Header\"},\"body\":{\"entityName\":\"header\",\"entityBody\":{\"senderId\":\"eee\"}}}";


                    string jsonMessage2 = "{\"body\":{\"entityName\":\"header\",\"entityBody\":{\"senderId\":\"eee\"}}," +
                       "\"header\":{\"senderId\":\"111\",\"dynamicType\":\"Hz.Libraries.Messaging.data.Header\",\"messageType\":\"data\"}" +
                       "}";

                    var xx = typeof(Header);
                    Header h = null;
                    DataBody<Header> dataBody = (DataBody<Header>)messageBuilderImp.BuildBody<Header>(MessageType.data, h);

                   // Console.WriteLine(jsonMessage);
                    Message message = (Message)messageBuilderImp.BuildMessage(header1, dataBody);
                    message = (Message)messageParser.MessageParser(message, jsonMessage);

                    string json = messageParser.Serialize(message,xx.ToString());

                    DataBody<object> ab = (DataBody<object>)message.body;
                    object b = ab.entityBody;

                    mph.setup("testQQ", "testEXX", "testQQ", false, false, null);
                    Console.WriteLine("mph is setup");
                    mch.setup("testQQ", "testEXX", "testQQ", false, false, null);
                    Console.WriteLine("mch is setup");

                    Thread.Sleep(2000);
                    for (int i = 0; i < 10; i++)
                    {
                        Console.WriteLine("message {$i} is sent");
                        mph.produce(message);
                    }

               //    mch.consume();
                  //  Console.ReadKey();
                            //    Task.Run(() => { mch.consume(); });
                //    Console.ReadKey();
                    object mm = new Message();
                    while (true) {
                        if (InternalQueue.Instance.messageQueue.TryDequeue(out mm))
                        {
                            Message m = (Message)mm;
                            Console.WriteLine(m.header.messageType);
                            Console.WriteLine("message is received");

                        }
                        Thread.Sleep(1000);
                    }

                    /*

                  //  else {
                  //      Console.WriteLine("empty");
                   // }

                    //   InternalQueue.Instance.messageQueue.TryDequeue<Message>(mm);

                    /*Message message = new Message();
                    MessageBuilder messageBuilderImp = new MessageBuilderImp();
                    AckBody ackBody = (AckBody)messageBuilderImp.BuildBody(MessageType.ack);
                    Header header = messageBuilderImp.BuildHeader(
                        Guid.NewGuid(), "", null, "", "", false, true, Guid.NewGuid(), MessageType.ack);
                    ackBody.message = "eee";
                    ackBody.statusCode = AckStatusCode.received;
                    ackBody.refMessageId = Guid.NewGuid();
                    message.header = header;
                    message.body = ackBody;
            }
                    Thread.Sleep(1000);
                }
    */
                /*

              //  else {
              //      Console.WriteLine("empty");
               // }

                //   InternalQueue.Instance.messageQueue.TryDequeue<Message>(mm);

                /*Message message = new Message();
                MessageBuilder messageBuilderImp = new MessageBuilderImp();
                AckBody ackBody = (AckBody)messageBuilderImp.BuildBody(MessageType.ack);
                Header header = messageBuilderImp.BuildHeader(
                    Guid.NewGuid(), "", null, "", "", false, true, Guid.NewGuid(), MessageType.ack);
                ackBody.message = "eee";
                ackBody.statusCode = AckStatusCode.received;
                ackBody.refMessageId = Guid.NewGuid();
                message.header = header;
                message.body = ackBody;*/
        }
    }
}
