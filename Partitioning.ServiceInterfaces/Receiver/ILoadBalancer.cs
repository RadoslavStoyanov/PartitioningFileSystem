namespace Partitioning.ServiceInterfaces.Receiver
{
    public interface ILoadBalancer
    {
        Task<Stream> SendToRemoteExecutor(int node, long offset);
        void ConfigureInternalListener();
    }
}
