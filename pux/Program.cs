using pux.termmgr;

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
╚═╝      ╚═════╝ ╚═╝  ╚═╝");
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
└───────────────────────────────────────┘");
    
    string choice = Console.ReadLine();
    
    if (choice == "1")
    {
        Console.Clear();
        termmgr.ExecuteCommand("pacman -Syu", true);
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }
    else if (choice == "2")
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("YOU MAY GET RATE LIMITED WHEN A INSTANCE OF PACMAN IS ALREADY RUNNING");
        Thread.Sleep(4000);
        termmgr.ExecuteCommand("rm -rf /var/lib/pacman/db.lck", true);
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }
    else if (choice == "3")
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("If you want to continue press any key");
        Console.ReadLine();
        termmgr.ExecuteCommand("rm -r /etc/pacman.d/gnupg", true);
        termmgr.ExecuteCommand("pacman -Sy gnupg archlinux-keyring", true);
        termmgr.ExecuteCommand("pacman-key --init", true);
        termmgr.ExecuteCommand("pacman-key --populate archlinux", true);
        termmgr.ExecuteCommand("pacman-key --refresh-keys", true);
        termmgr.ExecuteCommand("pacman -Syu", true);
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
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
        Console.WriteLine("You need to press any key to continue");
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