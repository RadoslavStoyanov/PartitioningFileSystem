using Microsoft.Extensions.DependencyInjection;

namespace Partitioning.DistributedFileStorageApp.Extensions
{
    public static class Extensions
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddHostedService<App>();
        }
    }
}
