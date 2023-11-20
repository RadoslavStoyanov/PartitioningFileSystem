using MediatR;
using Partitioning.Models.Requests;
using Partitioning.ServiceInterfaces.Receiver;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Text;

namespace Partitioning.ServiceImplementations.Receiver
{
    public class LoadBalancer : BaseEventListener, ILoadBalancer
    {
        private const int oneSecondInMilliseconds = 1000;
        private static SemaphoreSlim nodesSemaphore = new SemaphoreSlim(1, 1);

        private readonly ConcurrentDictionary<int, bool> nodes;

        private readonly IMediator _mediator;

        public LoadBalancer(IMediator mediator)
            : base()
        {
            _mediator = mediator;

            nodes = new ConcurrentDictionary<int, bool>(Enumerable
                .Range(1, 100)
                .Select(i =>
                    new
                    {
                        Key = i,
                        Value = false
                    })
                .ToDictionary(x => x.Key, x => x.Value));
        }

        public void ConfigureInternalListener()
        {
            base.Configure();
        }

        public async Task<Stream> SendToRemoteExecutor(int node, long offset)
        {
            var remoteExecutorRequest = new RemoteExecutorFileChunkRequest
            {
                ServiceId = node,
                Offset = offset
            };

            var chunk = await _mediator.Send(remoteExecutorRequest);

            return chunk;
        }

        public override async void ReceiveMessage(object? sender, BasicDeliverEventArgs ea)
        {
            var body = ea.Body.ToArray();
            var message = Convert.ToInt64(Encoding.UTF8.GetString(body));

            base.ReceiveMessage(sender, ea);

            await Handle(message);
        }

        protected virtual async Task Handle(long offset)
        {
            var node = FindAndUseFreeNode();

            var chunk = await SendToRemoteExecutor(node, offset);

            ReleaseNode(node);

            var fileChunkRequest = new FileChunkRequest
            {
                Chunk = chunk
            };

            await _mediator.Send(fileChunkRequest);
        }

        private int FindAndUseFreeNode()
        {
            var semaphoreAcquired = false;

            try
            {
                semaphoreAcquired = nodesSemaphore.Wait(oneSecondInMilliseconds);
                var nodeIsInUse = true;

                if (semaphoreAcquired)
                {
                    var node = nodes
                        .First(x => x.Value != nodeIsInUse)
                        .Key;

                    nodes[node] = nodeIsInUse;

                    return node;
                }

                throw new Exception("Semaphore could not be acquired");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                throw;
            }
            finally
            {
                if (semaphoreAcquired)
                {
                    nodesSemaphore.Release();
                }
                else
                {
                    throw new Exception("Semaphore could not be acquired");
                }
            }
        }

        private void ReleaseNode(int serviceId)
        {
            var nodeIsInUse = false;
            nodes[serviceId] = nodeIsInUse;
        }
    }
}
