using RabbitMQ.Client.Events;

namespace Partitioning.ServiceInterfaces.Receiver
{
    public interface IEventListener
    {
        void ReceiveMessage(object? sender, BasicDeliverEventArgs ea);
    }
}
