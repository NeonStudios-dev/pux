using System.Runtime.InteropServices;
namespace pux.root
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
                core.menus.ShowMainMenu();
                return;
            }
            else if (args.Length > 0 && args[0] == "syncdb")
            {
                rx.ExecuteCommand("pacman -Syy --noconfirm", true);
                return;
            }
            else if (args.Length > 0 && args[0] == "ipkgm")
            {
                core.menus.pkmgrMenu();
                return;
            }
            else if (args.Length > 0 && args[0] == "fixkeys")
            {
                Fixes.LoadKeyFix();
                return;
            } else if(args.Length > 0 && args[0] == "clean")
            {

                return;
            }
            else if (args.Length > 0 && args[0] == "--help")
            {
                Console.WriteLine("PUX - Pacman Utils X");
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
            
            core.menus.ShowMainMenu();
        }

    }
}