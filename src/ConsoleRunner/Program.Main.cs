using Microsoft.Extensions.Logging;

namespace ConsoleRunner;

partial class Program
{
    private static readonly CancellationTokenSource CancellationSource = new CancellationTokenSource();

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
            logger.LogError(ex, "An unexpected error occurred, see exception for more details.");
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