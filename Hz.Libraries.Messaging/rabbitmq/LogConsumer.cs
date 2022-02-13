using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hz.Libraries.Messaging.data;
using Hz.Libraries.Messaging.events;
using Hz.Libraries.Messaging.handler;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Autofac;
using System.Collections.Generic;

namespace Hz.Libraries.Messaging.rabbitmq
{
    public class LogConsumer : ConsumerBase, IHostedService
    {
        protected string queueName { get; set; }

        public void setLoggerQueue(string LoggerQueue)
        {
            this.LoggerQueue = LoggerQueue;
        }
        public void setLoggerExchange(string LoggerExchange)
        {
            this.LoggerExchange = LoggerExchange;
        }
        public void setLoggerQueueAndExchangeRoutingKey(string LoggerQueueAndExchangeRoutingKey)
        {
            this.LoggerQueueAndExchangeRoutingKey = LoggerQueueAndExchangeRoutingKey;
        }

        public void setQueueName(string queueName)
        {
            this.queueName = queueName;
        }

        public void setLoggerExchangeType(string LoggerExchangeType)
        {
            this.LoggerExchangeType = LoggerExchangeType;
        }
        public void setLoggerDurable(bool LoggerDurable)
        {
            this.LoggerDurable = LoggerDurable;
        }
        public void setLoggerExclusive(bool LoggerExclusive)
        {
            this.LoggerExclusive = LoggerExclusive;
        }


        private readonly MessageParser messageParser = new MessageParserImp();

        public LogConsumer(
            IMediator mediator,
            ConnectionFactory connectionFactory,
            ILogger<LogConsumer> logConsumerLogger,
            ILogger<ConsumerBase> consumerLogger,
            ILogger<RabbitMqClientBase> logger,

             List<object> handlers,

            string queueName, string LoggerExchange, string LoggerQueueAndExchangeRoutingKey,string LoggerExchangeType , bool LoggerDurable, bool LoggerExclusive) :
            base(mediator, connectionFactory, consumerLogger, logger)
        {
            try
            {


                var bus = new HzEventBus();//container.Resolve<IEventBus>();

                bus._handlers = handlers;

                this.queueName = queueName;
                this.LoggerQueue = queueName;
                this.LoggerExchange = LoggerExchange;
                this.LoggerQueueAndExchangeRoutingKey = LoggerQueueAndExchangeRoutingKey;
                this.LoggerExchangeType = LoggerExchangeType;
                this.LoggerExclusive = LoggerExclusive;
                this.LoggerDurable = LoggerDurable;

                this.ConnectToRabbitMq();
                  var consumer = new EventingBasicConsumer(Channel);
                Channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

                consumer.Received +=  (ch, ea) =>
                     {

                         var body = ea.Body.ToArray();
                         var message = Encoding.UTF8.GetString(body);

                         Message convertedMessage = new Message();
                         LogCommand lc = JsonConvert.DeserializeObject<LogCommand>(message);
                         convertedMessage = messageParser.MessageParser(convertedMessage, lc.Message);
                       //      InternalQueue.Instance.messageQueue.Enqueue(convertedMessage);

                         // invock event


                         var HzEvent = new HzEvent()
                         {
                             Name = "new event!",
                             Message = convertedMessage,
                             EndDateTime = DateTime.Now.AddDays(10)
                         };
                         
                          bus.Raise(HzEvent);

                         Console.WriteLine("......Hanxxxdling HzMessage in program......");

                         Channel.BasicAck(ea.DeliveryTag, false);
                     };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "......ee HzMessage in program......");

                logConsumerLogger.LogCritical(ex, "Error while consuming message");
            }
        }

        public virtual Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        public virtual Task StopAsync(CancellationToken cancellationToken)
        {
            Dispose();
            return Task.CompletedTask;
        }
    }
}