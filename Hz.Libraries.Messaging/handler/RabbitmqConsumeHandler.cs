using System;
using System.Collections.Generic;
using RabbitMQ.Client;
using Hz.Libraries.Messaging.rabbitmq;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hz.Libraries.Messaging.handler
{
    public class RabbitmqConsumeHandler : MessageHandler
    {

        public string hostName { get; set; }
        public int port { get; set; }
        public string userName { get; set; } = null;
        public string password { get; set; } = null;
        public string queueName { get; set; }
        public string exchangeName { get; set; } = "";
        public string exchangeType { get; set; } = "";
        public string routingKey { get; set; } = "";
        public bool durable { get; set; } = true;
        public bool exclusive { get; set; } = false;
        public bool autoDelete { get; set; } = false;
        public IDictionary<string, object> argument { get; set; } = null;
        public uint prefetchSize { get; set; } = 0;
        public ushort prefetchCount { get; set; } = 0;
        public bool global { get; set; } = false;

        private LogConsumer logConsumer;
        private readonly IMediator mediator = null;
        private ILogger<LogConsumer> logConsumerLogger = null;
        private ILogger<ConsumerBase> consumerLogger = null;
        private ILogger<RabbitMqClientBase> logger = null;


        public RabbitmqConsumeHandler()
        {
        }
        public void setup()
        {
            throw new NotSupportedException();
        }


        private void buildConnection(List<object> handlers)
        {
            try { 
            var factory = new ConnectionFactory();
            if (string.IsNullOrEmpty(userName))
            {
                factory = new ConnectionFactory() { HostName = this.hostName, Port = this.port };
            }
            else
            {
                factory = new ConnectionFactory() { HostName = this.hostName, Port = this.port, UserName = this.userName, Password = this.password };
            }

            ILoggerFactory loggerFactory =  new LoggerFactory();
            logConsumerLogger = new Logger<LogConsumer>(loggerFactory);
            consumerLogger = new Logger<ConsumerBase>(loggerFactory);
            logger = new Logger<RabbitMqClientBase>(loggerFactory);

            logConsumer = new LogConsumer(mediator, factory, logConsumerLogger, consumerLogger, logger,handlers, queueName, exchangeName, routingKey,exchangeType,durable,exclusive);

            }
            catch (Exception ex) {

                throw new InvalidOperationException( "Error while trying to setup the Consumer - Build Connection", ex);
            }

        }


        public RabbitmqConsumeHandler(string hostName, int port)
        {
            this.hostName = hostName;
            this.port = port;


        }

        public RabbitmqConsumeHandler(string hostName, int port, string userName, string password)
        {
            this.hostName = hostName;
            this.port = port;
            this.password = password;
            this.userName = userName;


        }




        public void setup(string queueName, string exchangeName, string routingKey, string exchangeType, bool durable, bool exclusive, IDictionary<string, object> argument, List<object> handlers)

        {

            this.queueName = queueName;
            this.exchangeName = exchangeName;
            this.exchangeType = exchangeType;
            this.routingKey = routingKey;
            this.durable = durable;
            this.exclusive = exclusive;
            this.argument = argument;
            buildConnection(handlers);
        }

        public void setup(string queueName, string exchangeName, string routingKey, string exchangeType, bool durable, bool exclusive, bool autoDelete, IDictionary<string, object> argument, uint prefetchSize, ushort prefetchCount, bool global, IBasicProperties props, List<object> handlers)
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
            buildConnection(handlers);


        }



    }
}
