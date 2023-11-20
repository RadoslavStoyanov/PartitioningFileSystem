namespace Partitioning.ServiceInterfaces.DistributedFileSystem
{
    public interface IDistributedFS
    {
        long FileLength { get; }

        Stream GetData(long offset);
    }
}
