using System;
using ConsoleRunner.Configuration;
using ConsoleRunner.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using DotRas.Diagnostics;

namespace ConsoleRunner;

partial class Program
{
    private static IServiceProvider applicationServices;
    private static IConfiguration configuration;

    private static void ConfigureIoC()
    {
        configuration = new ConfigurationBuilder()
            .AddJsonFile("appSettings.json")
            .AddJsonFile($"appSettings.{Environment.GetEnvironmentVariable("ENV")}.json", true)
            .Build();

        var services = new ServiceCollection();
        services.AddTransient<DotRasLoggingAdapter>();
        services.AddSingleton(configuration);
        services.AddOptions<ApplicationOptions>().Bind(configuration.GetSection("App"));

        services.AddLogging(builder => builder
            .AddConfiguration(configuration.GetSection("Logging"))
            .AddSimpleConsole());

        applicationServices = services.BuildServiceProvider();
    }

    private static void ConfigureDiagnostics()
    {
        LoggerLocator.SetLocator(applicationServices.GetRequiredService<DotRasLoggingAdapter>);
    }
}