using MediatR;

namespace Partitioning.Models.Requests
{
    public class FileChunkRequest : IRequest
    {
        public Stream Chunk { get; set; }
    }
}
