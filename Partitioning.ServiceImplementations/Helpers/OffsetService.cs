using Partitioning.ServiceImplementations.DistributedFileSystem;
using Partitioning.ServiceInterfaces.FileHelpers;

namespace Partitioning.ServiceImplementations.Helpers
{
    public class OffsetService : IOffsetService
    {
        public IEnumerable<long> GetOffsets()
        {
            var instance = DistributedFS.Instance;

            var fileLength = instance.FileLength;
            var threshold = instance.Threshold;

            var offset = 0l;
            var chunks = fileLength / threshold;
            var step = 1;

            var offsets = new List<long>();

            while (chunks >= 0)
            {
                offsets.Add(offset);
                offset = step * threshold;
                step++;
                chunks--;
            }

            return offsets;
        }
    }
}
