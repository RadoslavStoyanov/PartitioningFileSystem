using Partitioning.ServiceInterfaces.Publisher;
using RabbitMQ.Client;
using System.Text;

namespace Partitioning.ServiceImplementations.Publisher
{
    public class Publisher : IPublisher
    {
        private IConnection _connection;
        private IModel _channel;

        public Publisher()
        {
            Configure();
        }

        public void Publish(IEnumerable<long> messages)
        {
            foreach (var message in messages)
            {
                var body = GetMessageToPublish(message);

                var properties = _channel.CreateBasicProperties();
                properties.Persistent = true;

                _channel.BasicPublish(exchange: "",
                                routingKey: "read_file_on_chunks_queue",
                                basicProperties: properties,
                                body: body);

                Console.WriteLine($"Sent message with body {message}");
            }
        }

        private byte[] GetMessageToPublish(long offset)
        {
            var message = offset.ToString();

            return Encoding.UTF8.GetBytes(message);
        }

        private void Configure()
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
        }
    }
}
