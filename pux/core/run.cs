using System;
using System.Diagnostics;

namespace pux;

#pragma warning disable CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
public class rx
#pragma warning restore CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
{
    public static void ExecuteCommand(string command, bool root)
    {
        try
        {
            var startInfo = new ProcessStartInfo()
            {
                UseShellExecute = false,
                RedirectStandardOutput = false,
                RedirectStandardError = false,
                RedirectStandardInput = false
            };

            if (root)
            {
                startInfo.FileName = "sudo";
                startInfo.Arguments = command;
            }
            else
            {
                startInfo.FileName = "/bin/bash";
                startInfo.Arguments = $"-c \"{command}\"";
            }

            using (var process = new Process { StartInfo = startInfo })
            {
                process.Start();
                process.WaitForExit();
            }
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error executing command: {ex.Message}");
        }
    }
}