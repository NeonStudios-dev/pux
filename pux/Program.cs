using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using pux.core;
using pux.features;
using pux.utils;
namespace pux.core
{
    static class Pux
    {
        [ModuleInitializer]
        internal static void Init()
        {
            if (IsRunningAsRoot())
            {
                Console.ForegroundColor = Settings.Instance.CurrentTheme.ErrorColor;
                Console.WriteLine("Error: PUX should not be run as root or with sudo.");
                Console.WriteLine("Please run the program as a regular user.");
                Console.WriteLine("Individual commands that require root will prompt for sudo when needed.");
                Console.ResetColor();
                Environment.Exit(1);
                return;
            }
        }

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
            if (args.Length > 0 && args[0] == "--no-update")
            {
                menus.ShowMainMenu();
                return;
            }
            else if (args.Length > 0 && args[0] == "syncdb")
            {
                rx.ExecuteCommand("pacman -Syy --noconfirm", true);
                return;
            }
            else if (args.Length > 0 && args[0] == "ipkgm")
            {
                menus.pkmgrMenu();
                return;
            }
            else if (args.Length > 0 && args[0] == "fixkeys")
            {
                Fixes.LoadKeyFix();
                return;
            }
            else if (args.Length > 0 && args[0] == "clean")
            {
                pmclean.LoadClean();
                 return;
            }
            else if (args.Length > 0 && args[0] == "theme")
            {
                if (args.Length > 1)
                {
                    Settings.Instance.ApplyTheme(args[1]);
                    Console.ForegroundColor = Settings.Instance.CurrentTheme.SecondaryColor;
                    Console.WriteLine($"Theme changed to: {args[1]}");
                    return;
                }
                menus.ThemeMenu();
                return;
            }
            else if (args.Length > 0 && (args[0].Equals("@S", StringComparison.OrdinalIgnoreCase) || 
                                              args[0].Equals("search", StringComparison.OrdinalIgnoreCase) || 
                                              args[0].Equals("s", StringComparison.OrdinalIgnoreCase)))
            {
                if (args.Length < 2)
                {
                    Console.ForegroundColor = Settings.Instance.CurrentTheme.ErrorColor;
                    Console.WriteLine("Please provide a package name to search");
                    Console.ResetColor();
                    return;
                }
                PackageManager.SearchPackage(args[1]);
                return;
            }
            else if (args.Length > 0 && (args[0].Equals("@R", StringComparison.OrdinalIgnoreCase) || 
                                       args[0].Equals("remove", StringComparison.OrdinalIgnoreCase) ||
                                       args[0].Equals("r", StringComparison.OrdinalIgnoreCase)))
            {
                if (args.Length < 2)
                {
                    Console.ForegroundColor = Settings.Instance.CurrentTheme.ErrorColor;
                    Console.WriteLine("Please provide a package name to remove");
                    Console.ResetColor();
                    return;
                }
                PackageManager.RemovePackage(args[1]);
                return;
            }
            else if (args.Length > 0 && (args[0].Equals("@I", StringComparison.OrdinalIgnoreCase) || 
                                              args[0].Equals("install", StringComparison.OrdinalIgnoreCase) ||
                                              args[0].Equals("i", StringComparison.OrdinalIgnoreCase)))
            {
                if (args.Length < 2)
                {
                    Console.ForegroundColor = Settings.Instance.CurrentTheme.ErrorColor;
                    Console.WriteLine("Please provide a package name to install");
                    Console.ResetColor();
                    return;
                }
                PackageManager.InstallPackage(args[1]);
                return;
            }
            else if (args.Length > 0 && args[0] == "set-manager")
            {
                if (args.Length < 2)
                {
                    PackageManager.ShowCurrentManager();
                    return;
                }
                PackageManager.SetPreferredManager(args[1]);
                return;
            }
            else if (args.Length > 0 && args[0] == "--help")
            {
                Console.WriteLine("PUX - Pacman Utils X");
                Console.WriteLine("Usage: pux [options]");
                Console.WriteLine("Options:");
                Console.WriteLine("  --no-update     Skip the update check and go directly to the main menu.");
                Console.WriteLine("  --help          Display this help message.");
                Console.WriteLine("  syncdb          Sync the package database (equivalent to 'sudo pacman -Syy').");
                Console.WriteLine("  ipkgm           Open the package manager installation menu.");
                Console.WriteLine("  fixkeys         Apply fixes for Pacman key issues.");
                Console.WriteLine("  clean           Open the system cleaning menu.");
                Console.WriteLine("  theme [name]    Change color theme (Available: Default, Dark, Light, Hacker, Ocean).");
                Console.WriteLine("Package Management:");
                Console.WriteLine("  @S <package>    Search for a package");
                Console.WriteLine("  @I <package>    Install a package");
                Console.WriteLine("  @R <package>    Remove a package and its orphaned dependencies");
                Console.WriteLine("  s <pkg>        Short command for package search");
                Console.WriteLine("  i <pkg>        Short command for package installation");
                Console.WriteLine("  r <pkg>        Short command for package removal");
                Console.WriteLine("  search <pkg>   Alternative to @S for package search");
                Console.WriteLine("  install <pkg>  Alternative to @I for package installation");
                Console.WriteLine("  remove <pkg>   Alternative to @R for package removal");
                Console.WriteLine("  set-manager [pm] Set preferred package manager (pacman, yay, paru, aurutils, pikaur)");
                Console.WriteLine("If no options are provided, PUX will check for updates before displaying the main menu.");
                return;
            }

            Task.Run(async () =>
            {
                await AutoUpdater.CheckForUpdates();
            }).GetAwaiter().GetResult();
            
            Console.WriteLine("Starting PUX...");
            menus.ShowMainMenu();
        }
    }
}