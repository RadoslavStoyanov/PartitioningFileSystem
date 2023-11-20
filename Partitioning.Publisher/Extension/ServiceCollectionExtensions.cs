using Microsoft.Extensions.DependencyInjection;
using Partitioning.ServiceImplementations.Helpers;
using Partitioning.ServiceImplementations.Publisher;
using Partitioning.ServiceImplementations.Receiver;
using Partitioning.ServiceInterfaces.FileHelpers;
using Partitioning.ServiceInterfaces.Publisher;
using System.Reflection;

namespace Partitioning.PublisherApp.Extension
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddSingleton<IPublisher, Publisher>();
            services.AddScoped<IOffsetService, OffsetService>();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetAssembly(typeof(RemoteExecutorFileChunk))));

            services.AddHostedService<App>();
        }
    }
}
