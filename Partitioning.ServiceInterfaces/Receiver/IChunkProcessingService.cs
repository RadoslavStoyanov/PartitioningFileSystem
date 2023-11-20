namespace Partitioning.ServiceInterfaces.Receiver
{
    public interface IChunkProcessingService
    {
        void ReadChunkOfFile(Stream stream);
    }
}
