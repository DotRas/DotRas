using System;
using ConsoleRunner.Infrastructure.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace ConsoleRunner
{
    partial class Program
    {
        private static IServiceProvider applicationServices;
        private static IConfiguration configuration;

        private static void ConfigureIoC()
        {
            configuration = new ConfigurationBuilder()
                .AddJsonFile("appSettings.json")
                .Build();

            var services = new ServiceCollection();
            services.AddTransient<DotRasLoggingAdapter>();
            services.AddSingleton(configuration);

            services.AddLogging(builder => builder
                .AddConfiguration(configuration.GetSection("Logging"))
                .AddSimpleConsole());

            applicationServices = services.BuildServiceProvider();
        }
    }
}