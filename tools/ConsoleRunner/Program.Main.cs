using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleRunner
{
    partial class Program
    {
        private static readonly CancellationTokenSource CancellationSource = new CancellationTokenSource();

        public static async Task Main()
        {
            try
            {
                ConfigureIoC();
                ConfigureApplication();

                using var program = new Program();
                var task = program.RunAsync();

                Console.WriteLine("Press any key to cancel...");
                Console.ReadKey(true);

                CancellationSource.Cancel();
                await task;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.WriteLine("Press any key to terminate...");
            Console.ReadKey(true);
        }
    }
}