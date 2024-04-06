namespace ConsoleRunner;

partial class Program
{
    private static CancellationTokenSource CancellationSource { get; } = new CancellationTokenSource();

    public static async Task Main()
    {
        Console.WriteLine("Press CTRL+C at any time to to cancel the application...");
        Console.WriteLine();

        AttachCancellationSourceToCancelKeyPress();

        try
        {
            ConfigureIoC();
            ConfigureDiagnostics();

            using var program = new Program();
            await program.RunAsync();
        }
        catch (Exception ex)
        {
            await Console.Error.WriteLineAsync($"An unexpected error occurred. See exception for more details:\r\n{ex}");
        }
        finally {
            CancellationSource.Dispose();
        }
    }

    private static void AttachCancellationSourceToCancelKeyPress()
    {
        Console.CancelKeyPress += (sender, e) =>
        {
            Console.WriteLine("Terminating the application...");

            CancellationSource.Cancel();
            e.Cancel = true;
        };
    }
}