using System;
using System.Text;
using System.Threading.Tasks;
using Hz.Libraries.Messaging.data;
using Hz.Libraries.Messaging.handler;
using MediatR;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Hz.Libraries.Messaging.rabbitmq
{
    public abstract class ConsumerBase : RabbitMqClientBase
    {
        private readonly MessageParser messageParser = new MessageParserImp();

        private readonly IMediator _mediator;
        private readonly ILogger<ConsumerBase> _logger;
        protected  string queueName { get; set; }

        public ConsumerBase(
            IMediator mediator,
            ConnectionFactory connectionFactory,
            ILogger<ConsumerBase> consumerLogger,
            ILogger<RabbitMqClientBase> logger) :
            base(connectionFactory, logger)
        {
            _mediator = mediator;
            _logger = consumerLogger;
        }

        protected virtual async Task OnEventReceived(object sender, BasicDeliverEventArgs @event)
        {
            Console.WriteLine(" New event recevied! ");
            try
            {
                var message = Encoding.UTF8.GetString(@event.Body.ToArray());
                  Message convertedMessage = new Message();
                 convertedMessage = messageParser.MessageParser(convertedMessage, message);
                 InternalQueue.Instance.messageQueue.Enqueue(convertedMessage);


                  await _mediator.Publish(message);
                
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Error while retrieving message from queue.");
            }
            finally
            {
                Channel.BasicAck(@event.DeliveryTag, false);
            }
        }
    }
}
