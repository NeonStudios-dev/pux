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
            string fileName;
            string arguments;

            if (root)
            {
                fileName = "sudo";
                arguments = $"/bin/bash -c \"{command}\"";
            }
            else
            {
                fileName = "/bin/bash";
                arguments = $"-c \"{command}\"";
            }

            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = fileName,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            using (Process process = Process.Start(startInfo))
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                while (!process.StandardOutput.EndOfStream)
                {
                    string line = process.StandardOutput.ReadLine();
                    if (line != null)
                        Console.WriteLine(line);
                }
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                while (!process.StandardError.EndOfStream)
                {
                    string line = process.StandardError.ReadLine();
                    if (line != null)
                        Console.WriteLine(line);
                }

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