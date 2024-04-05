using System;
using ConsoleRunner.Infrastructure.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ConsoleRunner
{
    partial class Program
    {
        private static IServiceProvider applicationServices;

        private static void ConfigureIoC()
        {
            var services = new ServiceCollection();
            services.AddTransient<DotRasLoggingAdapter>();

            services.AddLogging(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Debug);
            });

            applicationServices = services.BuildServiceProvider();
        }
    }
}