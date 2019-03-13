using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using DotRas;

namespace DialConnectionAsynchronously
{
    class Program : IDisposable
    {
        private readonly RasDialer dialer;

        static void Main(string[] args)
        {
            try
            {
                using (var p = new Program())
                {
                    p.Run();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.WriteLine("Press any key to terminate...");
            Console.ReadKey(true);
        }

        public Program()
        {
            dialer = new RasDialer();
            dialer.StateChanged += OnDialerStateChanged;
        }

        ~Program()
        {
            Dispose(false);
        }

        private void OnDialerStateChanged(object sender, StateChangedEventArgs e)
        {
            Console.WriteLine($"State: {e.State}");
        }

        private void Run()
        {
            RunAsync().Wait();
        }

        private async Task RunAsync()
        {
            // This should contain the name 
            dialer.EntryName = "Your Entry";

            // If your account requires credentials that have not been persisted, they can be passed here.
            dialer.Credentials = new NetworkCredential("Username", "Password");

            // This specifies the default location for Windows phone books.
            dialer.PhoneBookPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                @"Microsoft\Network\Connections\Pbk\rasphone.pbk");

            Console.WriteLine("Connecting...");

            // Dials the connection synchronously. This will still raise events, and it will also allow for timeouts
            // if the cancellation token is passed into the api via the overload.
            var connection = await dialer.ConnectAsync();

            Console.WriteLine($"Connected: [{connection.EntryName}] @ {connection.Handle}");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                dialer.Dispose();
            }
        }
    }
}