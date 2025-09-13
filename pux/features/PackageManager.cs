using System;
using pux.core;

namespace pux.features
{
    public class PackageManager
    {
        public static void SearchPackage(string packageName)
        {
            var currentManager = Settings.Instance.PreferredPackageManager;
            Console.ForegroundColor = Settings.Instance.CurrentTheme.SecondaryColor;
            Console.WriteLine($"Searching for package: {packageName}");
            Console.ResetColor();

            string searchCommand = GetSearchCommand(currentManager, packageName);
            rx.ExecuteCommand(searchCommand, true);
        }

        public static void InstallPackage(string packageName)
        {
            var currentManager = Settings.Instance.PreferredPackageManager;
            Console.ForegroundColor = Settings.Instance.CurrentTheme.SecondaryColor;
            Console.WriteLine($"Installing package: {packageName}");
            Console.ResetColor();

            string installCommand = GetInstallCommand(currentManager, packageName);
            rx.ExecuteCommand(installCommand, true);
        }

        private static string GetSearchCommand(string manager, string package)
        {
            return manager switch
            {
                "pacman" => $"pacman -Ss {package}",
                "yay" => $"yay -Ss {package}",
                "paru" => $"paru -Ss {package}",
                "aurutils" => $"aur search {package}",
                "pikaur" => $"pikaur -Ss {package}",
                _ => $"pacman -Ss {package}"
            };
        }

        private static string GetInstallCommand(string manager, string package)
        {
            return manager switch
            {
                "pacman" => $"pacman -S {package}",
                "yay" => $"yay -S {package}",
                "paru" => $"paru -S {package}",
                "aurutils" => $"aur sync {package} && sudo pacman -S {package}",
                "pikaur" => $"pikaur -S {package}",
                _ => $"pacman -S {package}"
            };
        }

        private static string GetRemoveCommand(string manager, string package)
        {
            return manager switch
            {
                "pacman" => $"pacman -Rns {package}",
                "yay" => $"yay -Rns {package}",
                "paru" => $"paru -Rns {package}",
                "aurutils" => $"sudo pacman -Rns {package}",
                "pikaur" => $"pikaur -Rns {package}",
                _ => $"pacman -Rns {package}"
            };
        }

        public static void RemovePackage(string packageName)
        {
            var currentManager = Settings.Instance.PreferredPackageManager;
            Console.ForegroundColor = Settings.Instance.CurrentTheme.WarningColor;
            Console.WriteLine($"Removing package: {packageName}");
            Console.WriteLine("This will remove the package and its dependencies that aren't required by other packages.");
            Console.ResetColor();

            string removeCommand = GetRemoveCommand(currentManager, packageName);
            rx.ExecuteCommand(removeCommand, true);
        }

        public static void SetPreferredManager(string manager)
        {
            if (IsValidManager(manager))
            {
                Settings.Instance.PreferredPackageManager = manager;
                Settings.Instance.Save();
                Console.ForegroundColor = Settings.Instance.CurrentTheme.SecondaryColor;
                Console.WriteLine($"Default package manager set to: {manager}");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = Settings.Instance.CurrentTheme.ErrorColor;
                Console.WriteLine("Invalid package manager. Valid options are: pacman, yay, paru, aurutils, pikaur");
                Console.ResetColor();
            }
        }

        public static bool IsValidManager(string manager)
        {
            return manager switch
            {
                "pacman" or "yay" or "paru" or "aurutils" or "pikaur" => true,
                _ => false
            };
        }

        public static void ShowCurrentManager()
        {
            Console.ForegroundColor = Settings.Instance.CurrentTheme.SecondaryColor;
            Console.WriteLine($"Current package manager: {Settings.Instance.PreferredPackageManager}");
            Console.ResetColor();
        }
    }
}
