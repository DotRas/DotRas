using System.Net;
using ConsoleRunner.Configuration;
using ConsoleRunner.Exceptions;
using DotRas;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ConsoleRunner;

partial class Program : IDisposable
{
    private RasDialer Dialer { get; } = new RasDialer();
    private RasConnectionWatcher Watcher { get; } = new RasConnectionWatcher();

    private RasConnection connection;
    public bool IsConnected { get; private set; }

    public Program()
    {
        Dialer.StateChanged += OnStateChanged;
        Watcher.Connected += OnConnected;
        Watcher.Disconnected += OnDisconnected;
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
            Dialer.StateChanged -= OnStateChanged;
            Dialer.Dispose();

            Watcher.Connected -= OnConnected;
            Watcher.Disconnected -= OnDisconnected;
            Watcher.Dispose();
        }
    }

    public async Task RunAsync()
    {
        var config = ApplicationServices.GetRequiredService<IOptions<ApplicationOptions>>().Value;

        Dialer.EntryName = config.EntryName;
        Dialer.PhoneBookPath = config.PhoneBookPath;

        if (!string.IsNullOrWhiteSpace(config.Username) && !string.IsNullOrWhiteSpace(config.Password))
        {
            Dialer.Credentials = new NetworkCredential(config.Username, config.Password);
        }

        await RunCoreAsync();
    }

    private async Task RunCoreAsync()
    {
        try
        {
            Watcher.Start();

            while (ShouldContinueExecution())
            {
                using var tcs = CancellationTokenSource.CreateLinkedTokenSource(CancellationSource.Token);
                
                try
                {
                    await RunOnceAsync(tcs.Token);
                }
                finally
                {
                    await WaitForALittleWhileAsync(false, tcs.Token);
                }
            }
        }
        finally
        {
            Watcher.Stop();
        }
    }

    protected async Task RunOnceAsync(CancellationToken runningToken)
    {
        try
        {
            await ConnectAsync(runningToken);

            await WaitForALittleWhileAsync(true, runningToken);

            await DisconnectAsync(runningToken);
        }
        catch (OperationCanceledException) // Ensure the exception gets propogated
        {
            throw;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An error occurred while attempting to connect, see exception for more details.");
        }
    }

    private async static Task WaitForALittleWhileAsync(bool allowThrowCancellationException, CancellationToken cancellationToken)
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

        connection = RasConnection.EnumerateConnections().SingleOrDefault(o => o.EntryName == Dialer.EntryName);
        if (connection != null)
        {
            Logger.LogInformation("Already connected: {EntryName}", Dialer.EntryName);
            SetConnected();
        }
        else
        {
            Logger.LogInformation("Starting connection...");
            connection = await Dialer.ConnectAsync(cancellationToken);
        }
    }

    private void OnConnected(object sender, RasConnectionEventArgs e)
    {
        Logger.LogInformation("Connected: {EntryName}", e.ConnectionInformation.EntryName);
        SetConnected();
    }

    private async Task DisconnectAsync(CancellationToken cancellationToken)
    {
        if (!IsConnected)
        {
            return;
        }

        Logger.LogInformation("Starting disconnect...");
        await connection.DisconnectAsync(cancellationToken);
    }

    private void OnDisconnected(object sender, RasConnectionEventArgs e)
    {
        Logger.LogInformation("Disconnected: {EntryName}", e.ConnectionInformation.EntryName);
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

    private static bool ShouldContinueExecution()
    {
        return !CancellationSource.IsCancellationRequested;
    }

    private void OnStateChanged(object sender, StateChangedEventArgs e)
    {
        Logger.LogInformation("  State: {State}", e.State);
        RandomlyThrowException();
    }

    private static void RandomlyThrowException()
    {
        if (ShouldThrowRandomException())
        {
            throw new RandomException();
        }
    }

    private static bool ShouldThrowRandomException()
    {
        var rand = new Random();
        return rand.Next(1, 100) >= 98;
    }
}