using System;
using System.IO;
using System.Runtime.Serialization;
using System.Threading;
using ConsoleRunner.Model;

namespace ConsoleRunner
{
    partial class Program
    {
        private static readonly CancellationTokenSource CancellationSource =
            new CancellationTokenSource();

        private static readonly Configuration Config = ReadConfiguration();

        private static Configuration ReadConfiguration()
        {
            DataContractSerializer dcs = new DataContractSerializer(typeof(Configuration));
            using (var fs = File.OpenRead($@"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\Config.xml"))
            {
                return (Configuration)dcs.ReadObject(fs);
            }
        }
    }
}