using MediatR;
using Partitioning.Models.Requests;
using Partitioning.ServiceInterfaces.Helpers;
using Partitioning.ServiceInterfaces.Receiver;
using System.Text;

namespace Partitioning.ServiceImplementations.Receiver
{
    public class ChunkProcessingService : IRequestHandler<FileChunkRequest>, IChunkProcessingService
    {
        private readonly IWordsPerLineService _wordsPerLineService;

        public ChunkProcessingService(IWordsPerLineService wordsPerLineService)
        {
            _wordsPerLineService = wordsPerLineService;
        }

        public Task Handle(FileChunkRequest request, CancellationToken cancellationToken)
        {
            ReadChunkOfFile(request.Chunk);

            return Task.CompletedTask;
        }

        public void ReadChunkOfFile(Stream stream)
        {
            var buffer = ((MemoryStream)stream).GetBuffer();
            var chunk = new StringBuilder();

            foreach (var b in buffer)
            {
                chunk.Append((char)b);
            }

            _wordsPerLineService.ParseChunk(chunk.ToString());
        }
    }
}
