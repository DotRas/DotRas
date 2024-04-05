using System;
using System.IO;
using System.Runtime.Serialization;
using ConsoleRunner.Infrastructure.Diagnostics;
using DotRas.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Configuration = ConsoleRunner.Model.Configuration;

namespace ConsoleRunner
{
    partial class Program
    {
        private static Configuration Config;

        private static void ConfigureApplication()
        {
            LoggerLocator.SetLocator(applicationServices.GetService<DotRasLoggingAdapter>);

            var dcs = new DataContractSerializer(typeof(Configuration));
            using (var fs = File.OpenRead($@"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\Config.xml"))
            {
                Config = (Configuration)dcs.ReadObject(fs);
            }

            GuardConfiguration();
        }

        private static void GuardConfiguration()
        {
            if (!string.IsNullOrWhiteSpace(Config.PhoneBookPath) && !File.Exists(Config.PhoneBookPath))
            {
                throw new NotSupportedException("The phonebook has not been configured, or does not exist.");
            }
            else if (string.IsNullOrWhiteSpace(Config.EntryName))
            {
                throw new NotSupportedException("The entry name has not been configured.");
            }
        }
    }
}