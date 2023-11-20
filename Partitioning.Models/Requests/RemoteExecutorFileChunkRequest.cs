using MediatR;

namespace Partitioning.Models.Requests
{
    public class RemoteExecutorFileChunkRequest : IRequest<Stream>
    {
        public int ServiceId { get; set; }

        public long Offset { get; set; }
    }
}
