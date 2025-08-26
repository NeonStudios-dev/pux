using System;
using System.Diagnostics;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;
namespace pux;

public class AutoUpdater
{
    private static readonly HttpClient client = new HttpClient();
    private const string Owner = "NeonStudios-dev";
    private const string Repo = "pux";
    private const string GitHubToken = ""; // Optional: Add your GitHub token here
    
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

            byte[] newVersion = await client.GetByteArrayAsync(downloadUrl);
            
            string backupPath = currentExePath + ".backup";
            if (File.Exists(backupPath))
            {
                File.Delete(backupPath);
            }
            File.Move(currentExePath, backupPath);

            await File.WriteAllBytesAsync(currentExePath, newVersion);
            
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                rx.ExecuteCommand($"chmod +x \"{currentExePath}\"", false);
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
}
