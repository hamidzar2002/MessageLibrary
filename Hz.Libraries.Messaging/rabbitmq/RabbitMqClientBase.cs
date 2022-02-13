using System;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Hz.Libraries.Messaging.rabbitmq
{
    public abstract class RabbitMqClientBase : IDisposable
    {
        protected const string VirtualHost = "CUSTOM_HOST";
        protected string LoggerExchange { get; set; }= $"{VirtualHost}.LoggerExchange";
        protected string LoggerQueue { get; set; } = $"{VirtualHost}.log.message";
        protected  string LoggerQueueAndExchangeRoutingKey { get; set; }  = "log.message";
        protected string LoggerExchangeType { get; set; } = "direct";
        protected bool LoggerDurable { get; set; } = true;
        protected bool LoggerExclusive { get; set; } = false;

        protected IModel Channel { get; private set; }
        private IConnection _connection;
        private readonly ConnectionFactory _connectionFactory;
        private readonly ILogger<RabbitMqClientBase> _logger;

        protected RabbitMqClientBase(
            ConnectionFactory connectionFactory,
            ILogger<RabbitMqClientBase> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public void ConnectToRabbitMq()
        {
            if (_connection == null || _connection.IsOpen == false)
            {
                _connection = _connectionFactory.CreateConnection();
            }

            if (Channel == null || Channel.IsOpen == false)
            {
                Channel = _connection.CreateModel();
                Channel.ExchangeDeclare(
                    exchange: LoggerExchange,
                    type: LoggerExchangeType,
                    durable: LoggerDurable,
                    autoDelete: false);

                Channel.QueueDeclare(
                    queue: LoggerQueue,
                    durable: LoggerDurable,
                    exclusive: LoggerExclusive,
                    autoDelete: false);

                Channel.QueueBind(
                    queue: LoggerQueue,
                    exchange: LoggerExchange,
                    routingKey: LoggerQueueAndExchangeRoutingKey);
            }
        }

        public void Dispose()
        {
            try
            {
                Channel?.Close();
                Channel?.Dispose();
                Channel = null;

                _connection?.Close();
                _connection?.Dispose();
                _connection = null;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Cannot dispose RabbitMQ channel or connection");
            }
        }
    }
}
