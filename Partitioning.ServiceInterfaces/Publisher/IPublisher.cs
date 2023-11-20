namespace Partitioning.ServiceInterfaces.Publisher
{
    public interface IPublisher
    {
        void Publish(IEnumerable<long> messages);
    }
}
