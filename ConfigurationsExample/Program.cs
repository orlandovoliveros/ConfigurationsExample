using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace ConfigurationsExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Override configuration with command-line arguments.
            //args = new string[] {
            //    "Level1:Level2:Value1=1",
            //    "Level1:Level2:Value2=2"
            //};

            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var type1 = services.GetRequiredService<Type1>();

                    var options = services.GetRequiredService<IOptions<Level2Configuration>>();
                    var value1 = options.Value.Value1;
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error has occured");
                }
            }

            await host.RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostBuilderContext, services) =>
            {
                services.AddOptions();

                var configuration = hostBuilderContext.Configuration;
                services.Configure<Level1Configuration>(configuration.GetSection("Level1"));
                services.Configure<Level2Configuration>(configuration.GetSection("Level1:Level2"));

                services.AddScoped<Type1>();

            });
    }

    public class Level1Configuration
    {
        public Level2Configuration Level2 { get; set; }
    }

    public class Level2Configuration
    {
        public int Value1 { get; set; }

        public int Value2 { get; set; }
    }

    public class Type1
    {
        private readonly ILogger<Type1> _logger;
        private readonly Level1Configuration _level1Configuration;

        public Type1(ILogger<Type1> logger, IOptionsMonitor<Level1Configuration> options)
        {
            _logger = logger;
            _level1Configuration = options.CurrentValue;

            _logger.LogInformation("Value1: {value1}", _level1Configuration.Level2.Value1);
            _logger.LogInformation("Value2: {value2}", _level1Configuration.Level2.Value2);
        }
    }
}
