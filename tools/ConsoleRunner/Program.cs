using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ConsoleRunner.Exceptions;
using DotRas;

namespace ConsoleRunner
{
    partial class Program : IDisposable
    {
        private readonly RasDialer dialer = new RasDialer();
        private readonly RasConnectionWatcher watcher = new RasConnectionWatcher();

        private RasConnection connection;
        public bool IsConnected { get; private set; }

        public Program()
        {
            dialer.StateChanged += OnStateChanged;

            dialer.EntryName = Config.EntryName;
            dialer.PhoneBookPath = Config.PhoneBookPath;
            dialer.Credentials = new NetworkCredential(Config.Username, Config.Password);

            watcher.Connected += OnConnected;
            watcher.Disconnected += OnDisconnected;
        }

        ~Program()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                dialer.StateChanged -= OnStateChanged;
                dialer.Dispose();

                watcher.Connected -= OnConnected;
                watcher.Disconnected -= OnDisconnected;
                watcher.Dispose();
            }
        }

        public async Task RunAsync()
        {
            await RunCoreAsync();
        }

        private async Task RunCoreAsync()
        {
            watcher.Start();

            while (ShouldContinueExecution())
            {
                using var tcs = CancellationTokenSource.CreateLinkedTokenSource(CancellationSource.Token);

                try
                {
                    await RunOnceAsync(tcs.Token);
                }
                finally
                {
                    await WaitForALittleWhileAsync(tcs.Token, false);
                }
            }

            watcher.Stop();
        }

        protected async Task RunOnceAsync(CancellationToken runningToken)
        {
            try
            {
                await ConnectAsync(runningToken);

                await WaitForALittleWhileAsync(runningToken, true);

                await DisconnectAsync(runningToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private async Task WaitForALittleWhileAsync(CancellationToken cancellationToken, bool allowThrowCancellationException)
        {
            try
            {
                await Task.Delay(5000, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                // Swallow if cancellation has occurred.
                if (allowThrowCancellationException)
                {
                    throw;
                }
            }
        }

        private async Task ConnectAsync(CancellationToken cancellationToken)
        {
            if (IsConnected)
            {
                return;
            }

            connection = RasConnection.EnumerateConnections().SingleOrDefault(o => o.EntryName == dialer.EntryName);
            if (connection != null)
            {
                Console.WriteLine($"Already connected: {dialer.EntryName}");
                SetConnected();
            }
            else
            {
                Console.WriteLine("Starting connection...");
                connection = await dialer.ConnectAsync(cancellationToken);
            }
        }

        private void OnConnected(object sender, RasConnectionEventArgs e)
        {
            Console.WriteLine($"Connected: {e.ConnectionInformation.EntryName}");
            SetConnected();
        }

        private async Task DisconnectAsync(CancellationToken cancellationToken)
        {
            if (!IsConnected)
            {
                return;
            }

            Console.WriteLine("Starting disconnect...");
            await connection.DisconnectAsync(cancellationToken);
        }

        private void OnDisconnected(object sender, RasConnectionEventArgs e)
        {
            Console.WriteLine($"Disconnected: {e.ConnectionInformation.EntryName}");            
            SetNotConnected();
        }

        private void SetConnected()
        {
            IsConnected = true;
        }

        private void SetNotConnected()
        {
            IsConnected = false;
        }

        private bool ShouldContinueExecution()
        {
            return !CancellationSource.IsCancellationRequested;
        }

        private void OnStateChanged(object sender, StateChangedEventArgs e)
        {
            Console.WriteLine($"  State: {e.State}");
            RandomlyThrowException();
        }

        private void RandomlyThrowException()
        {
            if (ShouldThrowRandomException())
            {
                throw new RandomException();
            }
        }

        private bool ShouldThrowRandomException()
        {
            var rand = new Random();
            return rand.Next(1, 100) >= 98;
        }
    }
}