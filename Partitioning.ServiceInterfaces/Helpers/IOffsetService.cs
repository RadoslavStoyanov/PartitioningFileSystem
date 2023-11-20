namespace Partitioning.ServiceInterfaces.FileHelpers
{
    public interface IOffsetService
    {
        IEnumerable<long> GetOffsets();
    }
}
