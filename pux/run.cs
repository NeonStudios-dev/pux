using System;
using System.Diagnostics;

namespace pux;

public class rx
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

            using (Process process = Process.Start(startInfo))
            {
                while (!process.StandardOutput.EndOfStream)
                {
                    string line = process.StandardOutput.ReadLine();
                    if (line != null)
                        Console.WriteLine(line);
                }

                while (!process.StandardError.EndOfStream)
                {
                    string line = process.StandardError.ReadLine();
                    if (line != null)
                        Console.WriteLine(line);
                }

                process.WaitForExit();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error executing command: {ex.Message}");
        }
    }
}