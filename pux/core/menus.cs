using pux.features;
using pux.utils;

namespace pux.core
{
    public class menus
    {
        public static void ShowMainMenu()
        {
            float v = 0.9f;
            string[] menuItems = {
                "Update System",
                "Search Package",
                "Install Package",
                "Remove Package",
                "Remove DB Lock",
                "Fix Pacman Keys",
                "Install Package Manager",
                "Set Package Manager",
                "Sync database",
                "Clean system",
                "Change Theme",
                "About",
                "Exit"
            };
            
            int selectedIndex = 0;
            ConsoleKeyInfo keyInfo;

            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = Settings.Instance.CurrentTheme.SecondaryColor;
                Console.Write(@"
██████╗ ██╗   ██╗██╗  ██╗
██╔══██╗██║   ██║╚██╗██╔╝
██████╔╝██║   ██║ ╚███╔╝ 
██╔═══╝ ██║   ██║ ██╔██╗ 
██║     ╚██████╔╝██╔╝ ██╗
╚═╝      ╚═════╝ ╚═╝  ╚═╝
");
                Console.ForegroundColor = Settings.Instance.CurrentTheme.PrimaryColor;
                Console.WriteLine("╭─────────────────────────────────╮");
                Console.WriteLine($"│ PUX - Pacman Utils v{v}         │");
                Console.WriteLine("├─────────────────────────────────┤");

                for (int i = 0; i < menuItems.Length; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.ForegroundColor = Settings.Instance.CurrentTheme.PrimaryColor;
                        Console.Write("│ ");
                        Console.ForegroundColor = Settings.Instance.CurrentTheme.SecondaryColor;
                        Console.Write($"► {menuItems[i],-29}");
                        Console.ResetColor();
                        Console.ForegroundColor = Settings.Instance.CurrentTheme.PrimaryColor;
                        Console.WriteLine(" │");
                    }
                    else
                    {
                        Console.ForegroundColor = Settings.Instance.CurrentTheme.PrimaryColor;
                        Console.WriteLine($"│   {menuItems[i],-29} │");
                    }
                }
                
                Console.ForegroundColor = Settings.Instance.CurrentTheme.PrimaryColor;
                Console.WriteLine("╰─────────────────────────────────╯");
                Console.ForegroundColor = Settings.Instance.CurrentTheme.AccentColor;
                Console.WriteLine("Use ↑↓ arrows to navigate, Enter to select");

                keyInfo = Console.ReadKey(true);
                
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = selectedIndex > 0 ? selectedIndex - 1 : menuItems.Length - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedIndex = selectedIndex < menuItems.Length - 1 ? selectedIndex + 1 : 0;
                        break;
                    case ConsoleKey.Enter:
                        ExecuteMenuAction(selectedIndex);
                        break;
                    case ConsoleKey.Escape:
                        selectedIndex = menuItems.Length - 1;
                        break;
                }
            }
        }

        private static void ExecuteMenuAction(int selectedIndex)
        {
            switch (selectedIndex)
            {
                case 0:
                    Console.Clear();
                    rx.ExecuteCommand("pacman -Syu", true);
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    break;
                case 1:
                    Console.Clear();
                    Console.Write("Enter package name to search: ");
                    string? searchPackage = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(searchPackage))
                    {
                        PackageManager.SearchPackage(searchPackage);
                    }
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    break;
                case 2:
                    Console.Clear();
                    Console.Write("Enter package name to install: ");
                    string? installPackage = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(installPackage))
                    {
                        PackageManager.InstallPackage(installPackage);
                    }
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    break;
                case 3:
                    Console.Clear();
                    Console.Write("Enter package name to remove: ");
                    string? removePackage = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(removePackage))
                    {
                        PackageManager.RemovePackage(removePackage);
                    }
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    break;
                case 4:
                    Console.ForegroundColor = Settings.Instance.CurrentTheme.ErrorColor;
                    Console.WriteLine("YOU MAY GET RATE LIMITED WHEN A INSTANCE OF PACMAN IS ALREADY RUNNING");
                    Thread.Sleep(4000);
                    rx.ExecuteCommand("rm -rf /var/lib/pacman/db.lck", true);
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    break;
                case 5:
                    Fixes.LoadKeyFix();
                    break;
                case 6:
                    pkmgrMenu();
                    break;
                case 7:
                    SetPackageManagerMenu();
                    break;
                case 8:
                    Console.Clear();
                    rx.ExecuteCommand("pacman -Syy --noconfirm", true);
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    break;
                case 9:
                    Console.Clear();
                    pmclean.LoadClean();
                    break;
                case 10:
                    ThemeMenu();
                    break;
                case 11:
                    Console.Clear();
                    Console.ForegroundColor = Settings.Instance.CurrentTheme.SecondaryColor;
                    Console.WriteLine("Developed by NeonStudios developement");
                    Console.WriteLine("Copyright 2025 NeonStudios developement. All rights reserved.");
                    Console.WriteLine("");
                    Console.ForegroundColor = Settings.Instance.CurrentTheme.TextColor;
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    break;
                case 12:
                    Console.Clear();
                    Console.ForegroundColor = Settings.Instance.CurrentTheme.ErrorColor;
                    Console.WriteLine("You need to press any key to Exit");
                    Console.ReadLine();
                    Console.Clear();
                    Environment.Exit(0);
                    break;
            }
        }

        public static void pkmgrMenu()
        {
            string[] packageManagers = {
                "Install yay",
                "Install paru", 
                "Install aurutils",
                "Install pikaur",
                "Back"
            };
            
            int selectedIndex = 0;
            ConsoleKeyInfo keyInfo;

            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = Settings.Instance.CurrentTheme.PrimaryColor;
                Console.WriteLine("╭─────────────────────────────────╮");
                Console.WriteLine("│     Package Manager Menu        │");
                Console.WriteLine("├─────────────────────────────────┤");

                for (int i = 0; i < packageManagers.Length; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.ForegroundColor = Settings.Instance.CurrentTheme.PrimaryColor;
                        Console.Write("│ ");
                        Console.ForegroundColor = Settings.Instance.CurrentTheme.SecondaryColor;
                        Console.Write($"► {packageManagers[i],-29}");
                        Console.ResetColor();
                        Console.ForegroundColor = Settings.Instance.CurrentTheme.PrimaryColor;
                        Console.WriteLine(" │");
                    }
                    else
                    {
                        Console.ForegroundColor = Settings.Instance.CurrentTheme.PrimaryColor;
                        Console.WriteLine($"│   {packageManagers[i],-29} │");
                    }
                }
                
                Console.ForegroundColor = Settings.Instance.CurrentTheme.PrimaryColor;
                Console.WriteLine("╰─────────────────────────────────╯");
                Console.ForegroundColor = Settings.Instance.CurrentTheme.AccentColor;
                Console.WriteLine("Use ↑↓ arrows to navigate, Enter to select");

                keyInfo = Console.ReadKey(true);
                
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = selectedIndex > 0 ? selectedIndex - 1 : packageManagers.Length - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedIndex = selectedIndex < packageManagers.Length - 1 ? selectedIndex + 1 : 0;
                        break;
                    case ConsoleKey.Enter:
                        ExecutePackageManagerAction(selectedIndex);
                        if (selectedIndex == 4) return;
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
            }
        }

        private static void ExecutePackageManagerAction(int selectedIndex)
        {
            switch (selectedIndex)
            {
                case 0:
                    pxswitch.install("yay");
                    break;
                case 1:
                    pxswitch.install("paru");
                    break;
                case 2:
                    pxswitch.install("aurutils");
                    break;
                case 3:
                    pxswitch.install("pikaur");
                    break;
                case 4:
                    return;
            }
        }

        public static void SetPackageManagerMenu()
        {
            string[] managers = { "pacman", "yay", "paru", "aurutils", "pikaur", "Back" };
            int selectedIndex = 0;
            ConsoleKeyInfo keyInfo;

            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = Settings.Instance.CurrentTheme.PrimaryColor;
                Console.WriteLine("╭─────────────────────────────────╮");
                Console.WriteLine("│    Select Package Manager       │");
                Console.WriteLine("├─────────────────────────────────┤");

                for (int i = 0; i < managers.Length; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.ForegroundColor = Settings.Instance.CurrentTheme.PrimaryColor;
                        Console.Write("│ ");
                        Console.ForegroundColor = Settings.Instance.CurrentTheme.SecondaryColor;
                        Console.Write($"► {managers[i],-29}");
                        Console.ResetColor();
                        Console.ForegroundColor = Settings.Instance.CurrentTheme.PrimaryColor;
                        Console.WriteLine(" │");
                    }
                    else
                    {
                        Console.ForegroundColor = Settings.Instance.CurrentTheme.PrimaryColor;
                        Console.WriteLine($"│   {managers[i],-29} │");
                    }
                }
                
                Console.ForegroundColor = Settings.Instance.CurrentTheme.PrimaryColor;
                Console.WriteLine("╰─────────────────────────────────╯");
                Console.ForegroundColor = Settings.Instance.CurrentTheme.AccentColor;
                Console.WriteLine($"Current: {Settings.Instance.PreferredPackageManager}");
                Console.WriteLine("Use ↑↓ arrows to navigate, Enter to select, Esc to cancel");

                keyInfo = Console.ReadKey(true);
                
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = selectedIndex > 0 ? selectedIndex - 1 : managers.Length - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedIndex = selectedIndex < managers.Length - 1 ? selectedIndex + 1 : 0;
                        break;
                    case ConsoleKey.Enter:
                        if (selectedIndex == managers.Length - 1)
                            return;
                        PackageManager.SetPreferredManager(managers[selectedIndex]);
                        Thread.Sleep(1000);
                        return;
                    case ConsoleKey.Escape:
                        return;
                }
            }
        }

        public static void ThemeMenu()
        {
            string[] themes = { "Default", "Dark", "Light", "Hacker", "Ocean", "Back" };
            int selectedIndex = 0;
            ConsoleKeyInfo keyInfo;

            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = Settings.Instance.CurrentTheme.PrimaryColor;
                Console.WriteLine("╭─────────────────────────────────╮");
                Console.WriteLine("│         Theme Menu              │");
                Console.WriteLine("├─────────────────────────────────┤");

                for (int i = 0; i < themes.Length; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.ForegroundColor = Settings.Instance.CurrentTheme.PrimaryColor;
                        Console.Write("│ ");
                        Console.ForegroundColor = Settings.Instance.CurrentTheme.SecondaryColor;
                        Console.Write($"► {themes[i],-29}");
                        Console.ResetColor();
                        Console.ForegroundColor = Settings.Instance.CurrentTheme.PrimaryColor;
                        Console.WriteLine(" │");
                    }
                    else
                    {
                        Console.ForegroundColor = Settings.Instance.CurrentTheme.PrimaryColor;
                        Console.WriteLine($"│   {themes[i],-29} │");
                    }
                }
                
                Console.ForegroundColor = Settings.Instance.CurrentTheme.PrimaryColor;
                Console.WriteLine("╰─────────────────────────────────╯");
                Console.ForegroundColor = Settings.Instance.CurrentTheme.AccentColor;
                Console.WriteLine($"Current theme: {Settings.Instance.ThemeName}");
                Console.WriteLine("Use ↑↓ arrows to navigate, Enter to select, Esc to cancel");

                keyInfo = Console.ReadKey(true);
                
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = selectedIndex > 0 ? selectedIndex - 1 : themes.Length - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedIndex = selectedIndex < themes.Length - 1 ? selectedIndex + 1 : 0;
                        break;
                    case ConsoleKey.Enter:
                        if (selectedIndex == themes.Length - 1)
                            return;
                        Settings.Instance.ApplyTheme(themes[selectedIndex]);
                        Console.ForegroundColor = Settings.Instance.CurrentTheme.SecondaryColor;
                        Console.WriteLine($"\nTheme changed to: {themes[selectedIndex]}");
                        Thread.Sleep(1000);
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
            }
        }
    }
}