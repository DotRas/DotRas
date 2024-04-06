﻿using System.Net;
using DotRas;

namespace DialConnectionAndWatchForDisconnect;

class Program
{
    private readonly RasDialer dialer;
    private readonly RasConnectionWatcher watcher;

    static async Task Main()
    {
        try
        {
            await new Program().RunAsync();
        }
        catch (Exception ex)
        {
            await Console.Error.WriteLineAsync(ex.ToString());
        }

        await Console.Out.WriteLineAsync("Press any key to terminate...");
        Console.ReadKey(true);
    }

    public Program()
    {
        dialer = new RasDialer();

        watcher = new RasConnectionWatcher();
        watcher.Disconnected += OnConnectionDisconnected;
    }

    private async Task RunAsync()
    {
        // This should contain the name.
        dialer.EntryName = "Your Entry";

        // If your account requires credentials that have not been persisted, they can be passed here.
        dialer.Credentials = new NetworkCredential("Username", "Password");

        // This specifies the default location for Windows phone books.
        dialer.PhoneBookPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            @"Microsoft\Network\Connections\Pbk\rasphone.pbk");
        
        // Dials the connection synchronously. This will still raise events, and it will also allow for timeouts
        // if the cancellation token is passed into the api via the overload.
        var connection = await dialer.ConnectAsync();

        await Console.Out.WriteLineAsync($"Connected: [{connection.EntryName}] @ {connection.Handle}");

        watcher.Connection = connection;
        watcher.Start();

        // This will just force disconnect, however this could also be external if the connection is dropped due to the
        // network on the machine being physically disconnected.
        await Console.Out.WriteLineAsync("Just waiting for a bit before forcing disconnect...");
        Thread.Sleep(TimeSpan.FromSeconds(10));

        await connection.DisconnectAsync(CancellationToken.None);
        watcher.Stop();
    }

    private void OnConnectionDisconnected(object sender, RasConnectionEventArgs e)
    {
        Console.WriteLine($"Disconnected: [{e.ConnectionInformation.EntryName}] @ {e.ConnectionInformation.Handle}");
    }
}