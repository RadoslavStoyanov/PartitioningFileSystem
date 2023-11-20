using Microsoft.Extensions.Hosting;
using Partitioning.PublisherApp.Extension;

namespace Partitioning.PublisherApp
{
    public class Program
    {
        public static void Main(string[] args)
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