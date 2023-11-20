using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Partitioning.DistributedFileStorageApp.Extensions;

namespace Partitioning.DistributedFileStorageApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.ConfigureServices();
                });
    }
}