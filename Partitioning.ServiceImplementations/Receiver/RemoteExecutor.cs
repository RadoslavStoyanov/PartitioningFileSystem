using MediatR;
using Partitioning.Models.Requests;
using Partitioning.ServiceImplementations.DistributedFileSystem;
using Partitioning.ServiceInterfaces.Receiver;

namespace Partitioning.ServiceImplementations.Receiver
{
    public class RemoteExecutorFileChunk : IRequestHandler<RemoteExecutorFileChunkRequest, Stream>, IRemoteExecutor<Stream>
    {
        public async Task<Stream> Handle(RemoteExecutorFileChunkRequest request, CancellationToken cancellationToken)
        {
            return Run(request.ServiceId, () => DistributedFS.Instance.GetData(request.Offset));
        }

        public Stream Run(int serviceId, Func<Stream> func)
        {
            Task.Delay(100);

            return func();
        }
    }
}
