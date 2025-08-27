using pux.features;
using pux.utils;

namespace pux.core
{
    public class menus
    {
        public static void ShowMainMenu()
        {
            float v = 0.8f;
            string[] menuItems = {
                "Update System",
                "Remove DB Lock",
                "Fix Pacman Keys",
                "Install Package manager",
                "Sync database",
                "Clean system",
                "About",
                "Exit"
            };
            
            int selectedIndex = 0;
            ConsoleKeyInfo keyInfo;

            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(@"
██████╗ ██╗   ██╗██╗  ██╗
██╔══██╗██║   ██║╚██╗██╔╝
██████╔╝██║   ██║ ╚███╔╝ 
██╔═══╝ ██║   ██║ ██╔██╗ 
██║     ╚██████╔╝██╔╝ ██╗
╚═╝      ╚═════╝ ╚═╝  ╚═╝
");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("╭─────────────────────────────────╮");
                Console.WriteLine($"│ PUX - Pacman Utils v{v}         │");
                Console.WriteLine("├─────────────────────────────────┤");

                for (int i = 0; i < menuItems.Length; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.Write("│ ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write($"► {menuItems[i],-29}");
                        Console.ResetColor();
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine(" │");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine($"│   {menuItems[i],-29} │");
                    }
                }
                
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("╰─────────────────────────────────╯");
                Console.ForegroundColor = ConsoleColor.Cyan;
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
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("YOU MAY GET RATE LIMITED WHEN A INSTANCE OF PACMAN IS ALREADY RUNNING");
                    Thread.Sleep(4000);
                    rx.ExecuteCommand("rm -rf /var/lib/pacman/db.lck", true);
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    break;
                case 2:
                    Fixes.LoadKeyFix();
                    break;
                case 3:
                    pkmgrMenu();
                    break;
                case 4:
                    Console.Clear();
                    rx.ExecuteCommand("pacman -Syy --noconfirm", true);
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    break;
                case 5:
                    Console.Clear();
                    pmclean.LoadClean();
                    break;
                case 6:
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("Developed by NeonStudios developement");
                    Console.WriteLine("Copyright 2025 NeonStudios developement. All rights reserved.");
                    Console.WriteLine("");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    break;
                case 7:
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkRed;
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
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("╭─────────────────────────────────╮");
                Console.WriteLine("│     Package Manager Menu        │");
                Console.WriteLine("├─────────────────────────────────┤");

                for (int i = 0; i < packageManagers.Length; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.Write("│ ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write($"► {packageManagers[i],-29}");
                        Console.ResetColor();
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine(" │");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine($"│   {packageManagers[i],-29} │");
                    }
                }
                
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("╰─────────────────────────────────╯");
                Console.ForegroundColor = ConsoleColor.Cyan;
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
    }
}