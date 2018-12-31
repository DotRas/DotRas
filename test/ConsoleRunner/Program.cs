using System;
using System.Threading;
using System.Threading.Tasks;
using DotRas;

namespace ConsoleRunner
{
    partial class Program
    {
        private readonly RasDialer dialer;

        private RasConnection connection;
        public bool IsConnected { get; private set; }

        public Program()
        {
            dialer = new RasDialer
            {
                EntryName = Config.EntryName,
                PhoneBookPath = Config.PhoneBookPath
            };

            dialer.StateChanged += OnStateChanged;
        }

        private async Task RunAsync()
        {
            while (ShouldContinueExecution())
            {
                try
                {
                    using (var tcs = CancellationTokenSource.CreateLinkedTokenSource(CancellationSource.Token))
                    {
                        await ConnectAsync(tcs.Token);
                        if (IsConnected)
                        {
                            DisconnectAsync(tcs.Token);
                        }

                        tcs.Token.WaitHandle.WaitOne(500);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        private async Task ConnectAsync(CancellationToken cancellationToken)
        {
            connection = await dialer.ConnectAsync(cancellationToken);
            if (connection != null)
            {
                SetConnected();

                var stats = connection.GetStatistics();
                if (stats != null)
                {
                }

                connection.ClearStatistics();
            }
        }

        private void DisconnectAsync(CancellationToken cancellationToken)
        {
            connection.Disconnect(cancellationToken);
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
            return !IsConnected && !CancellationSource.IsCancellationRequested;
        }

        private void OnStateChanged(object sender, StateChangedEventArgs e)
        {
            Console.WriteLine($"State: {e.State}");
            RandomlyThrowException();
        }

        private void RandomlyThrowException()
        {
            var rand = new Random();
            if (rand.Next(1, 100) >= 98)
            {
                throw new Exception("A random exception occurred.");
            }
        }
    }
}