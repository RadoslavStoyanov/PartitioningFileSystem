using Microsoft.Extensions.Hosting;
using Partitioning.ServiceInterfaces.Receiver;

namespace Partitioning.ReceiverApp
{
    public class App : BackgroundService
    {
        private readonly ILoadBalancer _loadBalancer;

        public App(ILoadBalancer loadBalancer)
        {
            _loadBalancer = loadBalancer;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _loadBalancer.ConfigureInternalListener();

            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000);
            }

            return;
        }

    }
}
