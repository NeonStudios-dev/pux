using System;
using System.IO;
using System.Text.Json;

namespace pux.core
{
    public class ColorTheme
    {
        public ConsoleColor PrimaryColor { get; set; } = ConsoleColor.Magenta;
        public ConsoleColor SecondaryColor { get; set; } = ConsoleColor.Green;
        public ConsoleColor AccentColor { get; set; } = ConsoleColor.Cyan;
        public ConsoleColor TextColor { get; set; } = ConsoleColor.White;
        public ConsoleColor ErrorColor { get; set; } = ConsoleColor.Red;
        public ConsoleColor WarningColor { get; set; } = ConsoleColor.Yellow;
    }

    public class Settings
    {
        private static readonly string ConfigPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".config",
            "pux",
            "settings.json"
        );

        private static Settings instance;
        
        public ColorTheme CurrentTheme { get; set; } = new ColorTheme();
        public string ThemeName { get; set; } = "Default";
        public string PreferredPackageManager { get; set; } = "pacman";

        public static Settings Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Load();
                }
                return instance;
            }
        }

        public static Settings Load()
        {
            try
            {
                if (File.Exists(ConfigPath))
                {
                    string json = File.ReadAllText(ConfigPath);
                    return JsonSerializer.Deserialize<Settings>(json) ?? CreateDefaultSettings();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading settings: {ex.Message}");
            }
            return CreateDefaultSettings();
        }

        public void Save()
        {
            try
            {
                string directoryPath = Path.GetDirectoryName(ConfigPath);
                if (!string.IsNullOrEmpty(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(ConfigPath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving settings: {ex.Message}");
            }
        }

        private static Settings CreateDefaultSettings()
        {
            return new Settings
            {
                ThemeName = "Default",
                CurrentTheme = new ColorTheme()
            };
        }

        public void ApplyTheme(string themeName)
        {
            switch (themeName.ToLower())
            {
                case "dark":
                    CurrentTheme = new ColorTheme
                    {
                        PrimaryColor = ConsoleColor.DarkMagenta,
                        SecondaryColor = ConsoleColor.DarkGreen,
                        AccentColor = ConsoleColor.DarkCyan,
                        TextColor = ConsoleColor.Gray,
                        ErrorColor = ConsoleColor.DarkRed,
                        WarningColor = ConsoleColor.DarkYellow
                    };
                    break;
                case "light":
                    CurrentTheme = new ColorTheme
                    {
                        PrimaryColor = ConsoleColor.Blue,
                        SecondaryColor = ConsoleColor.Green,
                        AccentColor = ConsoleColor.Cyan,
                        TextColor = ConsoleColor.Black,
                        ErrorColor = ConsoleColor.Red,
                        WarningColor = ConsoleColor.Yellow
                    };
                    break;
                case "hacker":
                    CurrentTheme = new ColorTheme
                    {
                        PrimaryColor = ConsoleColor.Green,
                        SecondaryColor = ConsoleColor.DarkGreen,
                        AccentColor = ConsoleColor.Gray,
                        TextColor = ConsoleColor.Green,
                        ErrorColor = ConsoleColor.Red,
                        WarningColor = ConsoleColor.Yellow
                    };
                    break;
                case "ocean":
                    CurrentTheme = new ColorTheme
                    {
                        PrimaryColor = ConsoleColor.Blue,
                        SecondaryColor = ConsoleColor.Cyan,
                        AccentColor = ConsoleColor.DarkCyan,
                        TextColor = ConsoleColor.White,
                        ErrorColor = ConsoleColor.Red,
                        WarningColor = ConsoleColor.Yellow
                    };
                    break;
                default: // Default theme
                    CurrentTheme = new ColorTheme();
                    break;
            }
            ThemeName = themeName;
            Save();
        }
    }
}
