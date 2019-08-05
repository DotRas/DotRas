using System;
using System.Threading;

namespace ConsoleRunner
{
    partial class Program
    {
        private static readonly CancellationTokenSource CancellationSource = new CancellationTokenSource();

        public static void Main()
        {
            try
            {
                ConfigureIoC();
                ConfigureApplication();

                using (var program = new Program())
                using (var task = program.RunAsync())
                {
                    Console.WriteLine("Press any key to cancel...");
                    Console.ReadKey(true);

                    CancellationSource.Cancel();
                    task.Wait();
                }
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