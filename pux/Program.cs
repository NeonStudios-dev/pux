using pux;
namespace pux
{
    class pux
    {
        public static async Task Main(string[] args)
        {
            await AutoUpdater.CheckForUpdates();
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
                    Console.Write(@$"┌═══════════════════════════════════════┐
│              PUX TERMINAL             │
│           MANAGEMENT SYSTEM   v {v}   │
├───────────────────────────────────────┤
│                                       │    
│  ► Update System               [1]    │
│  ► Remove DB Lock              [2]    │
│  ► Fix Pacman Keys             [3]    │
│  ► Install Package manager     [4]    │
│  ► About                       [5]    │
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
                        case "5":
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