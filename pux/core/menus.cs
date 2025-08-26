using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using pux.features;
using pux.utils;

namespace pux.core
{
    public class menus
    {
        public static void ShowMainMenu()
        {
            float v = 0.7f;
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
│              PUX - Pacman             │
│                utils X        v {v}   │
├───────────────────────────────────────┤
│                                       │    
│  ► Update System               [1]    │
│  ► Remove DB Lock              [2]    │
│  ► Fix Pacman Keys             [3]    │
│  ► Install Package manager     [4]    │
│  ► Sync database               [5]    │
│  ► Clean system                [6]    │
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
                            pkmgrMenu();
                            break;
                        case "5":
                            Console.Clear();
                            rx.ExecuteCommand("pacman -Syy --noconfirm", true);
                            Console.WriteLine("\nPress any key to continue...");
                            Console.ReadKey();
                            break;
                        case "6":
                            Console.Clear();
                            pmclean.LoadClean();
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
        public static void pkmgrMenu()
        {
            Console.Clear();
            Console.WriteLine(@"► Install yay (yet another yogurt) [1]");
            Console.WriteLine(@"► Install paru                     [2]");
            Console.WriteLine(@"► Install aurutils                 [3]");
            Console.WriteLine(@"► Install pikaur                   [4]");
            Console.WriteLine(@"► Back                             [0]");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    pxswitch.install("yay");
                    break;
                case "2":
                    pxswitch.install("paru");
                    break;
                case "3":
                    pxswitch.install("aurutils");
                    break;
                case "4":
                    pxswitch.install("pikaur");
                    break;
                case "0":
                    core.menus.ShowMainMenu();
                    break;
            }
        }
    }
}