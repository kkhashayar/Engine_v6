using System.Diagnostics;

namespace Engine.External_Resources;

public class StockfishIntegration
{
    private Process stockfish;
    private readonly string stockfishPath;

    public StockfishIntegration(string path)
    {
        stockfishPath = path;
    }

    public void StartStockfish()
    {
        try
        {
            stockfish = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = stockfishPath,
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };

            stockfish.Start();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to start Stockfish: " + ex.Message);
            throw;
        }
    }

    public void SendCommand(string command)
    {
        if (stockfish != null && !stockfish.HasExited)
        {
            stockfish.StandardInput.WriteLine(command);
            stockfish.StandardInput.Flush();
        }
    }

    public string ReadOutput()
    {
        if (stockfish == null)
        {
            return null;
        }

        try
        {
            string output = stockfish.StandardOutput.ReadLine();
            while (string.IsNullOrWhiteSpace(output) || output.Contains("info depth") || output.StartsWith("info string"))
            {
                output = stockfish.StandardOutput.ReadLine();
            }
            return output;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error reading output from Stockfish: " + ex.Message);
            return null;
        }
    }

    public int StopStockfish()
    {
        if (stockfish != null && !stockfish.HasExited)
        {
            try
            {
                stockfish.Kill();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error stopping Stockfish: " + ex.Message);
            }
            finally
            {
                stockfish.Dispose();
            }
        }

        return 0;
    }
}

