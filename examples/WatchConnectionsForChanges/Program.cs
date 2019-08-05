using System;
using DotRas;

namespace WatchConnectionsForChanges
{
    internal class Program : IDisposable
    {
        private readonly RasConnectionWatcher watcher;

        public static void Main()
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
            watcher = new RasConnectionWatcher();

            watcher.Connected += OnConnectionConnected;
            watcher.Disconnected += OnConnectionDisconnected;
        }

        ~Program()
        {
            Dispose(false);
        }        

        /// <summary>
        /// This method gets called when the operating system notifies DotRas that a new connection has been established.
        /// </summary>
        /// <param name="sender">This is the object which raised the event.</param>
        /// <param name="e">This object carries the event data.</param>
        private void OnConnectionConnected(object sender, RasConnectionEventArgs e)
        {
            Console.WriteLine($"Connected: {e.ConnectionInformation.EntryName}");
        }

        /// <summary>
        /// This method gets called when the operating system notifies DotRas that an existing connection has disconnected.
        /// </summary>
        /// <param name="sender">This is the object which raised the event.</param>
        /// <param name="e">This object carries the event data.</param>
        private void OnConnectionDisconnected(object sender, RasConnectionEventArgs e)
        {
            Console.WriteLine($"Disconnected: {e.ConnectionInformation.EntryName}");
        }

        private void Run()
        {
            // Start watching for connection changes.
            watcher.Start();

            Console.WriteLine("Press any key to stop watching for connection changes...");
            Console.ReadKey(true);

            // Stop watching for connection changes.
            watcher.Stop();
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
                watcher.Connected -= OnConnectionConnected;
                watcher.Disconnected -= OnConnectionDisconnected;
                watcher.Dispose();            
            }
        }
    }
}