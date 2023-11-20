using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Partitioning.ServiceImplementations.Helpers;
using Partitioning.ServiceImplementations.Receiver;
using Partitioning.ServiceInterfaces.Helpers;
using Partitioning.ServiceInterfaces.Receiver;
using System.Reflection;

namespace Partitioning.ReceiverApp.Extensions
{
    public static class Extensions
    {
        public static void ConfigureServices(this IServiceCollection services, HostBuilderContext hostContext)
        {
            services.AddScoped<IChunkProcessingService, ChunkProcessingService>();
            services.AddSingleton<ILoadBalancer, LoadBalancer>();
            services.AddSingleton<IWordsPerLineService, WordsPerLineService>();

            services.AddScoped<IRemoteExecutor<Stream>, RemoteExecutorFileChunk>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetAssembly(typeof(RemoteExecutorFileChunk))));

            services.AddHostedService<App>();
        }
    }
}
