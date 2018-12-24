using System;
using System.Net;
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
                PhoneBookPath = Config.PhoneBookPath,
                Credentials = new NetworkCredential(Config.Username, Config.Password)
            };

            dialer.DialStateChanged += OnStateChanged;
        }

        private async Task RunAsync()
        {
            while (ShouldContinueExecution())
            {
                using (var tcs = CancellationTokenSource.CreateLinkedTokenSource(CancellationSource.Token))
                {
                    await ConnectAsync(tcs.Token);
                    if (IsConnected)
                    {
                        DisconnectAsync(tcs.Token);
                    }                 
                }
            }
        }

        private async Task ConnectAsync(CancellationToken cancellationToken)
        {
            connection = await dialer.DialAsync(cancellationToken);
            if (connection != null)
            {
                SetConnected();

                var stats = connection.GetStatistics();
                if (stats != null)
                {
                }
            }
        }

        private void DisconnectAsync(CancellationToken cancellationToken)
        {
            connection.HangUp(cancellationToken);
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

        private void OnStateChanged(object sender, DialStateChangedEventArgs e)
        {
            Console.WriteLine($"State: {e.State}");
        }
    }
}