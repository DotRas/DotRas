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
            ConfigureApplication();

            using var program = new Program();
            await program.RunAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        finally
        {
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