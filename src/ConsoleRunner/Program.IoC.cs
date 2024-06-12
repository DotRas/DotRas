using ConsoleRunner.Configuration;
using ConsoleRunner.Diagnostics;
using DotRas.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace ConsoleRunner;

internal partial class Program {
    /// <summary>
    /// Gets the application services.
    /// </summary>
    private static IServiceProvider ApplicationServices { get; set; }

    /// <summary>
    /// Gets the application configuration.
    /// </summary>
    private static IConfiguration Configuration { get; set; }

    /// <summary>
    /// Gets the logger.
    /// </summary>
    private static ILogger<Program> Logger { get; set; }

    private static void ConfigureIoC() {
        Configuration = new ConfigurationBuilder()
            .AddJsonFile("appSettings.json")
            .AddJsonFile($"appSettings.{Environment.GetEnvironmentVariable("ENV")}.json", true)
            .Build();

        var services = new ServiceCollection();
        services.AddTransient<DotRasLoggingAdapter>();
        services.AddSingleton(Configuration);
        services.AddOptions<ApplicationOptions>().Bind(Configuration.GetSection("App"));

        services.AddLogging(builder =>
            builder
                .AddConfiguration(Configuration.GetSection("Logging"))
                .AddSimpleConsole(opts => {
                    opts.SingleLine = true;
                    opts.IncludeScopes = true;
                    opts.UseUtcTimestamp = true;
                })
        );

        ApplicationServices = services.BuildServiceProvider();
    }

    private static void ConfigureDiagnostics() {
        LoggerLocator.SetLocator(ApplicationServices.GetRequiredService<DotRasLoggingAdapter>);
        Logger = ApplicationServices.GetRequiredService<ILoggerFactory>().CreateLogger<Program>();
    }
}
