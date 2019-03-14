using System;
using System.IO;
using System.Runtime.Serialization;
using ConsoleRunner.Model;

namespace ConsoleRunner
{
    partial class Program
    {
        private static Configuration Config;

        private static void ConfigureApplication()
        {
            DataContractSerializer dcs = new DataContractSerializer(typeof(Configuration));
            using (var fs = File.OpenRead($@"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\Config.xml"))
            {
                Config = (Configuration)dcs.ReadObject(fs);
            }
        }
    }
}