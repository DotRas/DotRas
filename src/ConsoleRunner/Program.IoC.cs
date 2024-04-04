using ConsoleRunner.Diagnostics;
using DotRas.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleRunner;

partial class Program
{
    private static IServiceProvider serviceProvider;

    private static void ConfigureIoC()
    {
        var services = new ServiceCollection();
        services.AddTransient<DotRasLoggingAdapter>();

        serviceProvider = services.BuildServiceProvider();
    }
}