using MediatR;

namespace Hz.Libraries.Messaging.rabbitmq
{
    public class LogCommand : IRequest<Unit>
    {
        public string Message { get; set; }
    }
}
