using pux;
using System.Runtime.InteropServices;
namespace pux
{
    class pux
    {
        private static bool IsRunningAsRoot()
        {
            try
            {
                return Environment.GetEnvironmentVariable("SUDO_USER") != null || 
                       geteuid() == 0;
            }
            catch
            {
                return false;
            }
        }

        [DllImport("libc")]
        private static extern uint geteuid();

        public static void Main(string[] args)
        {
            if (IsRunningAsRoot())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: PUX should not be run as root or with sudo.");
                Console.WriteLine("Please run the program as a regular user.");
                Console.WriteLine("Individual commands that require root will prompt for sudo when needed.");
                Console.ResetColor();
                Environment.Exit(1);
                return;
            }

            Console.WriteLine("Starting PUX...");
            if (args.Length > 0 && args[0] == "--no-update")
            {
                Mmenu();
                return;
            }
            else if (args.Length > 0 && args[0] == "syncdb")
            {
                rx.ExecuteCommand("sudo pacman -Syy", true);
                return;
            }
            else if (args.Length > 0 && args[0] == "ipkgm")
            {
                pxswitch.LoadMenu();
                return;
            }else if( args.Length > 0 && args[0] == "fixkeys")
            {
                Fixes.LoadKeyFix();
                return;
            }
            else if (args.Length > 0 && args[0] == "--help")
            {
                Console.WriteLine("PUX - Terminal Management System");
                Console.WriteLine("Usage: pux [options]");
                Console.WriteLine("Options:");
                Console.WriteLine("  --no-update    Skip the update check and go directly to the main menu.");
                Console.WriteLine("  --help         Display this help message.");
                Console.WriteLine("  syncdb         Sync the package database (equivalent to 'sudo pacman -Syy').");
                Console.WriteLine("  ipkgm          Open the package manager installation menu.");
                Console.WriteLine("  fixkeys       Apply fixes for Pacman key issues.");
                Console.WriteLine("If no options are provided, PUX will check for updates before displaying the main menu.");   
                return;
            }
            
            Task.Run(async () => 
            {
                await AutoUpdater.CheckForUpdates();
            }).GetAwaiter().GetResult();
            
            Mmenu();

        }
        public static void Mmenu()
        {
            float v = 0.5f;
            {
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
                    Console.Write(@$"
┌═══════════════════════════════════════┐
│              PUX TERMINAL             │
│           MANAGEMENT SYSTEM   v {v}   │
├───────────────────────────────────────┤
│                                       │    
│  ► Update System               [1]    │
│  ► Remove DB Lock              [2]    │
│  ► Fix Pacman Keys             [3]    │
│  ► Install Package manager     [4]    │
│  ► Sync database               [5]    │
│  ► About                       [a]    │
│  ► Exit                        [0]    │
└───────────────────────────────────────┘
");
#pragma warning disable CS8600
                    string choice = Console.ReadLine();
#pragma warning restore CS8600
                    switch (choice)
                    {
                        case "1":
                            Console.Clear();
                            rx.ExecuteCommand("pacman -Syu", true);
                            Console.WriteLine("\nPress any key to continue...");
                            Console.ReadKey();
                            break;
                        case "2":
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("YOU MAY GET RATE LIMITED WHEN A INSTANCE OF PACMAN IS ALREADY RUNNING");
                            Thread.Sleep(4000);
                            rx.ExecuteCommand("rm -rf /var/lib/pacman/db.lck", true);
                            Console.WriteLine("\nPress any key to continue...");
                            Console.ReadKey();
                            break;
                        case "3":
                            Fixes.LoadKeyFix();
                            break;
                        case "4":
                            pxswitch.LoadMenu();
                            break;
                        case "a":
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.WriteLine("Developed by NeonStudios developement");
                            Console.WriteLine("Copyright 2025 NeonStudios developement. All rights reserved.");
                            Console.WriteLine("");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Press any key to continue...");
                            Console.ReadKey();
                            break;
                        case "0":
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine("You need to press any key to Exit");
                            Console.ReadLine();
                            Console.Clear();
                            Environment.Exit(0);
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid option! Press any key to continue...");
                            Console.ReadKey();
                            break;
                    }
                }
            }
        }

    }
}