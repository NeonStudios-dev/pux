using pux;
while (true)
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine(@"
██████╗ ██╗   ██╗██╗  ██╗
██╔══██╗██║   ██║╚██╗██╔╝
██████╔╝██║   ██║ ╚███╔╝ 
██╔═══╝ ██║   ██║ ██╔██╗ 
██║     ╚██████╔╝██╔╝ ██╗
╚═╝      ╚═════╝ ╚═╝  ╚═╝
");
    Console.ForegroundColor = ConsoleColor.Magenta;
    Console.WriteLine(@"
┌═══════════════════════════════════════┐
│              PUX TERMINAL             │
│           MANAGEMENT SYSTEM   v 0.1   │
├───────────────────────────────────────┤
│                                       │    
│  ► Update System               [1]    │
│  ► Remove DB Lock              [2]    │
│  ► Fix Pacman Keys             [3]    │
│  ► About                       [4]    │
│  ► Exit                        [0]    │
└───────────────────────────────────────┘
");

#pragma warning disable CS8600
    string choice = Console.ReadLine();
#pragma warning restore CS8600

    if (choice == "1")
    {
        Console.Clear();
        rx.ExecuteCommand("pacman -Syu", true);
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }
    else if (choice == "2")
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("YOU MAY GET RATE LIMITED WHEN A INSTANCE OF PACMAN IS ALREADY RUNNING");
        Thread.Sleep(4000);
        rx.ExecuteCommand("rm -rf /var/lib/pacman/db.lck", true);
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }
    else if (choice == "3")
    {
        Fixes.LoadKeyFix();
    }
    else if (choice == "4")
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine("Developed by NeonStudios developement");
        Console.WriteLine("Copyright 2025 NeonStudios developement. All rights reserved.");
        Console.WriteLine("");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
    else if (choice == "0")
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("You need to press any key to Exit");
        Console.ReadLine();
        Console.Clear();
        Environment.Exit(0);
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Invalid option! Press any key to continue...");
        Console.ReadKey();
    }
}