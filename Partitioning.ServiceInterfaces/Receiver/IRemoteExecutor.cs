namespace Partitioning.ServiceInterfaces.Receiver
{
    public interface IRemoteExecutor<T>
    {
        T Run(int serviceId, Func<T> func);
    }
}
