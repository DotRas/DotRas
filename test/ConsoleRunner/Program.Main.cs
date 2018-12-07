using System;

namespace ConsoleRunner
{
    partial class Program
    {
        public static void Main()
        {
            try
            {
                using (var task = new Program().RunAsync())
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