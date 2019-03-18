using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DotRas;

namespace ConsoleRunner
{
    partial class Program
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

        public Task RunAsync()
        {
            return Task.Run(() => Run());
        }

        private void Run()
        {
            watcher.Start();
         
            while (ShouldContinueExecution())
            {
                try
                {
                    using (var tcs = CancellationTokenSource.CreateLinkedTokenSource(CancellationSource.Token))
                    {
                        try
                        {
                            Connect(tcs.Token);
                        }
                        finally
                        {
                            Disconnect();
                        }

                        WaitUntilNextExecution(tcs.Token);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            watcher.Stop();
        }

        private void Connect(CancellationToken cancellationToken)
        {
            if (IsConnected)
            {
                return;
            }

            Console.WriteLine("Starting connection...");
            connection = dialer.Connect(cancellationToken);
            Console.WriteLine("Completed connecting.");
        }

        private void Disconnect()
        {
            if (!IsConnected)
            {
                return;
            }

            Console.WriteLine("Starting disconnect...");
            connection.Disconnect();
            Console.WriteLine("Completed disconnect.");
        }

        private void WaitUntilNextExecution(CancellationToken cancellationToken)
        {
            cancellationToken.WaitHandle.WaitOne(TimeSpan.FromSeconds(5));
        }

        private void OnConnected(object sender, RasConnectionEventArgs e)
        {
            Console.WriteLine($"Connected: {e.ConnectionInformation.EntryName}");
            SetConnected();
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
            Console.WriteLine($"State: {e.State}");
            RandomlyThrowException();
        }

        private void RandomlyThrowException()
        {
            if (ShouldThrowRandomException())
            {
                throw new Exception("A random exception occurred.");
            }
        }

        private bool ShouldThrowRandomException()
        {
            var rand = new Random();
            return rand.Next(1, 100) >= 98;
        }
    }
}