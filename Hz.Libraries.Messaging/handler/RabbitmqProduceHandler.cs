using System;
using Hz.Libraries.Messaging.data;
using System.Collections.Generic;
using RabbitMQ.Client;
using Hz.Libraries.Messaging.rabbitmq;

namespace Hz.Libraries.Messaging.handler
{
    public class RabbitmqProduceHandler : MessageHandler
    {

        public string hostName { get; set; }
        public int port { get; set; }
        public string userName { get; set; } = null;
        public string password { get; set; } = null;
        public string queueName { get; set; }
        public string exchangeName { get; set; } = "";
        public string exchangeType { get; set; } = "";
        public string routingKey { get; set; } = "";
        public bool durable { get; set; } = false;
        public bool exclusive { get; set; } = false;
        public bool autoDelete { get; set; } = false;
        public IDictionary<string, object> argument { get; set; } = null;
        public uint prefetchSize { get; set; } = 0;
        public ushort prefetchCount { get; set; } = 0;
        public bool global { get; set; } = false;
        public IBasicProperties props { get; set; } = null;


        private readonly MessageParser messageParser = new MessageParserImp();
        private LogProducer logProducer = null;
        private readonly LogIntegrationEvent logIntegrationEvent = new LogIntegrationEvent();

        public RabbitmqProduceHandler()
        {
        

        }

        public void setup()
        {
            throw new NotSupportedException();
        }

        private void buildConnection() {
            try
            {
                var factory = new ConnectionFactory();
                if (string.IsNullOrEmpty(userName))
                {
                    factory = new ConnectionFactory() { HostName = this.hostName, Port = this.port };
                }
                else
                {
                    factory = new ConnectionFactory() { HostName = this.hostName, Port = this.port, UserName = this.userName, Password = this.password };
                }

                logProducer = new LogProducer(factory, null, null);
                logProducer.setExchangeName(exchangeName);
                logProducer.setRoutingKeyName(routingKey);
                logProducer.setLoggerExchange(exchangeName);
                logProducer.setLoggerQueue(queueName);
                logProducer.setLoggerQueueAndExchangeRoutingKey(routingKey);
                logProducer.setLoggerExchangeType(exchangeType);
                logProducer.setLoggerExclusive(exclusive);
                logProducer.setLoggerDurable(durable);
                logProducer.ConnectToRabbitMq();
            }
            catch (Exception ex)
            {

                throw new InvalidOperationException("Error while trying to setup the Producer - Build Connection", ex);
            }
        }

        public RabbitmqProduceHandler(string hostName, int port)
        {
            this.hostName = hostName;
            this.port = port;
            

        }

        public RabbitmqProduceHandler(string hostName, int port, string userName, string password)
        {
            this.hostName = hostName;
            this.port = port;
            this.password = password;
            this.userName = userName;

        }



        public void setup(string queueName, string exchangeName, string routingKey,string exchangeType, bool durable, bool exclusive, IDictionary<string, object> argument)
        {

            this.queueName = queueName;
            this.exchangeName = exchangeName;
            this.exchangeType = exchangeType;
            this.routingKey = routingKey;
            this.durable = durable;
            this.exclusive = exclusive;
            this.argument = argument;
            buildConnection();
        }

        public void setup(string queueName, string exchangeName, string routingKey, string exchangeType, bool durable, bool exclusive, bool autoDelete, IDictionary<string, object> argument, uint prefetchSize, ushort prefetchCount, bool global , IBasicProperties props)
        {
            this.queueName = queueName;
            this.exchangeName = exchangeName;
            this.exchangeType = exchangeType;
            this.routingKey = routingKey;
            this.durable = durable;
            this.exclusive = exclusive;
            this.autoDelete = autoDelete;
            this.argument = argument;
            this.prefetchSize = prefetchSize;
            this.prefetchCount = prefetchCount;
            this.global = global;
            this.props = props;
            buildConnection();


        }


        public bool produce(Message message) {
            try { 
            logIntegrationEvent.Message = this.messageParser.Serialize(message, message.header.dynamicType);
            logProducer.Publish(logIntegrationEvent);
                return true;
            }
            catch (Exception ex)
            {

                throw new InvalidOperationException("Error while trying to send message - produce", ex);
                
            }
           
        }


    }
}
