using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Hz.Libraries.Messaging.rabbitmq
{
    public class LogProducer : ProducerBase<LogIntegrationEvent>
    {
        public LogProducer(
            ConnectionFactory connectionFactory,
            ILogger<RabbitMqClientBase> logger,
            ILogger<ProducerBase<LogIntegrationEvent>> producerBaseLogger) :
            base(connectionFactory, logger, producerBaseLogger)
        {

        }


   
        protected override string AppId => "LogProducer";

        public void setExchangeName(string ExchangeName) {
            
            this.ExchangeName = ExchangeName;
        }
        public void setRoutingKeyName(string RoutingKeyName)
        {
            this.RoutingKeyName = RoutingKeyName;
        }
        public void setLoggerQueue(string LoggerQueue) {
            this.LoggerQueue = LoggerQueue;
        }
        public void setLoggerExchange(string LoggerExchange)
        {
            this.LoggerExchange = LoggerExchange;
        }
        public void setLoggerQueueAndExchangeRoutingKey(string LoggerQueueAndExchangeRoutingKey) {
            this.LoggerQueueAndExchangeRoutingKey = LoggerQueueAndExchangeRoutingKey;
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

    }
}
