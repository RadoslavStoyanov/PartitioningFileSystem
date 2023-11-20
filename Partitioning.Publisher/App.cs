using Microsoft.Extensions.Hosting;
using Partitioning.ServiceInterfaces.FileHelpers;
using Partitioning.ServiceInterfaces.Publisher;

namespace Partitioning.PublisherApp
{
    public class App : BackgroundService
    {
        private readonly IPublisher _publisher;
        private readonly IOffsetService _offsetService;

        public App(IPublisher publisher, IOffsetService offsetService)
        {
            _publisher = publisher;
            _offsetService = offsetService;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            var offsets = _offsetService.GetOffsets();
            _publisher.Publish(offsets);

            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000);
            }

            return;
        }
    }
}
