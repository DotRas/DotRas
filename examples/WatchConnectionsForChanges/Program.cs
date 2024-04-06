﻿using DotRas;

namespace WatchConnectionsForChanges;

class Program
{
    private readonly RasConnectionWatcher watcher;

    static void Main()
    {
        try
        {
            new Program().Run();
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
}