using ImageLibrary;
using ImageLibrary.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace ImageService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(configBuilder =>
                {
                })
                .ConfigureAppConfiguration((hostingContext, configBuilder) =>
                {
                    configBuilder.AddEnvironmentVariables(prefix: "ImageService_");
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<ImageFileWatcher>();
                    services.AddImageLibrary(hostContext.Configuration,
                        new ImageConfiguration
                        {
                            CompressionLevel = 0.2M
                        },
                        thumbnailSizeConfiguration =>
                        {
                            thumbnailSizeConfiguration.FilePrefix = $"{ImageConfiguration.ConfigurationNameThumbnail}-";
                        });
                });
    }
}
