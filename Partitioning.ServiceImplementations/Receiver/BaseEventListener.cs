using Partitioning.ServiceInterfaces.Receiver;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Partitioning.ServiceImplementations.Receiver
{
    public abstract class BaseEventListener : IEventListener
    {
        private IConnection _connection;
        private IModel _channel;

        public virtual void ReceiveMessage(object? sender, BasicDeliverEventArgs ea)
        {
            _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
        }

        protected void Configure()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "read_file_on_chunks_queue",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += ReceiveMessage;

            _channel.BasicConsume(queue: "read_file_on_chunks_queue",
                                 autoAck: false,
                                 consumer: consumer);
        }
    }
}
