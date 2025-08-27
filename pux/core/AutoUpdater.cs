using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;

namespace pux;

public class AutoUpdater
{
    private static readonly HttpClient client = new HttpClient();
    private const string Owner = "NeonStudios-dev";
    private const string Repo = "pux";
    private const string GitHubToken = "";
    
    public static async Task CheckForUpdates()
    {
        try
        {
            Console.WriteLine("Checking for updates...");
            Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version ?? new Version(0, 0, 0);
            
            client.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("Pux", "1.0"));
            if (!string.IsNullOrEmpty(GitHubToken))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GitHubToken);
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            using var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.github.com/repos/{Owner}/{Repo}/releases");
            using var response = await client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Repository or releases not found. Update check skipped.");
                    Thread.Sleep(2000);
                    return;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("GitHub API rate limit exceeded. Update check skipped.");
                    Thread.Sleep(2000);
                    return;
                }
                throw new Exception($"GitHub API error: {response.StatusCode}");
            }
            
            var releases = JArray.Parse(content);
            if (releases.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("No releases found. You might be running a development version.");
                
                Thread.Sleep(2000);
                return;
            }
            
            var json = (JObject)releases[0];
            
            string tagName = json["tag_name"]?.ToString() ?? "";
            if (!tagName.StartsWith("v")) return;
            
            Version latestVersion = Version.Parse(tagName.Substring(1));
            
            if (latestVersion > currentVersion)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"New version available: {latestVersion} (current: {currentVersion})");
                Console.WriteLine("Would you like to update? [Y/n]");
                
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Y || key.Key == ConsoleKey.Enter)
                {
                    await DownloadAndUpdate(json);
                }
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Failed to check for updates: {ex.Message}");
            Thread.Sleep(2000);
        }
        finally
        {
            Console.ResetColor();
        }
    }

    private static async Task DownloadAndUpdate(JObject releaseInfo)
    {
        try
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Downloading update...");

            var assets = releaseInfo["assets"] as JArray;
            if (assets == null || assets.Count == 0)
            {
                throw new Exception("No assets found in the release");
            }

            var asset = assets.FirstOrDefault(a => a["name"]?.ToString() == "pux");
            if (asset == null)
            {
                throw new Exception("Could not find 'pux' executable in release assets");
            }

            string downloadUrl = asset["browser_download_url"]?.ToString() ?? "";
            string currentExePath = Process.GetCurrentProcess().MainModule?.FileName ?? "";
            
            if (string.IsNullOrEmpty(currentExePath))
            {
                throw new Exception("Could not determine current executable path");
            }

            string tempDir = Path.GetTempPath();
            string tempFilePath = Path.Combine(tempDir, "pux_update");
            
            Console.WriteLine("Downloading new version...");
            byte[] newVersion = await client.GetByteArrayAsync(downloadUrl);
            await File.WriteAllBytesAsync(tempFilePath, newVersion);
            
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var chmodProcess = Process.Start(new ProcessStartInfo
                {
                    FileName = "chmod",
                    Arguments = $"+x \"{tempFilePath}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                });
                await chmodProcess.WaitForExitAsync();
            }

            bool needsSudo = IsSystemPath(currentExePath);
            
            if (needsSudo && RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Administrator privileges required for update. Please enter your password when prompted.");
                Console.ResetColor();
                
                await UpdateWithSudo(currentExePath, tempFilePath);
            }
            else
            {
                await UpdateWithoutElevation(currentExePath, tempFilePath);
            }

            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Update completed successfully! Press any key to restart the application.");
            Console.ReadKey(true);

            Process.Start(currentExePath);
            Environment.Exit(0);
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Failed to update: {ex.Message}");
            Console.WriteLine("Press any key to continue with current version...");
            Console.ReadKey(true);
        }
        finally
        {
            Console.ResetColor();
        }
    }

    private static bool IsSystemPath(string path)
    {
        var systemPaths = new[]
        {
            "/usr/local/bin",
            "/usr/bin",
            "/bin",
            "/sbin",
            "/usr/sbin"
        };

        return systemPaths.Any(systemPath => 
            path.StartsWith(systemPath, StringComparison.OrdinalIgnoreCase));
    }

    private static async Task UpdateWithSudo(string currentExePath, string tempFilePath)
    {
        string backupPath = currentExePath + ".backup";
        
        string scriptContent = $@"#!/bin/bash
if [ -f ""{backupPath}"" ]; then
    rm ""{backupPath}""
fi
mv ""{currentExePath}"" ""{backupPath}""

cp ""{tempFilePath}"" ""{currentExePath}""
chmod +x ""{currentExePath}""

echo ""Update completed successfully!""
";

        string scriptPath = Path.Combine(Path.GetTempPath(), "pux_update_script.sh");
        await File.WriteAllTextAsync(scriptPath, scriptContent);
        
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "sudo",
                Arguments = $"bash \"{scriptPath}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = false
            }
        };

        process.Start();
        
        string output = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();
        
        await process.WaitForExitAsync();
        
        if (File.Exists(scriptPath))
        {
            File.Delete(scriptPath);
        }

        if (process.ExitCode != 0)
        {
            throw new Exception($"Sudo update failed: {error}");
        }
        
        Console.WriteLine(output);
    }



    private static async Task UpdateWithoutElevation(string currentExePath, string tempFilePath)
    {
        string backupPath = currentExePath + ".backup";
        
        if (File.Exists(backupPath))
        {
            File.Delete(backupPath);
        }
        
        File.Move(currentExePath, backupPath);
        File.Copy(tempFilePath, currentExePath);
        
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            var chmodProcess = Process.Start(new ProcessStartInfo
            {
                FileName = "chmod",
                Arguments = $"+x \"{currentExePath}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            });
            await chmodProcess.WaitForExitAsync();
        }
    }
}