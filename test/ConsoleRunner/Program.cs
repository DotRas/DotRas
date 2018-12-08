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
                Credentials = new NetworkCredential(Config.Username, Config.Password)
            };

            dialer.StateChanged += OnStateChanged;
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
                        await DisconnectAsync(tcs.Token);
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
            }
        }

        private async Task DisconnectAsync(CancellationToken cancellationToken)
        {
            await connection.DisconnectAsync(cancellationToken);
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

        private void OnStateChanged(object sender, DialerStateChangedEventArgs e)
        {
            Console.WriteLine($"State: {e.State}");
        }
    }
}