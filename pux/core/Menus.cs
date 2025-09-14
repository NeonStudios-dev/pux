using pux.features;
using pux.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace pux.core
{
    public class Menus
    {
        private class MenuCategory
        {
            public string Name { get; }
            public List<MenuItem> Items { get; }

            public MenuCategory(string name, List<MenuItem> items)
            {
                Name = name;
                Items = items;
            }
        }

        private class MenuItem
        {
            public string Name { get; }
            public Action Action { get; }

            public MenuItem(string name, Action action)
            {
                Name = name;
                Action = action;
            }
        }

        private static readonly List<MenuCategory> menuCategories = new List<MenuCategory>
        {
            new MenuCategory("Package Management", new List<MenuItem>
            {
                new MenuItem("Update System", () => ExecuteMenuAction(0)),
                new MenuItem("Search Package", () => ExecuteMenuAction(1)),
                new MenuItem("Install Package", () => ExecuteMenuAction(2)),
                new MenuItem("Remove Package", () => ExecuteMenuAction(3))
            }),
            new MenuCategory("System Maintenance", new List<MenuItem>
            {
                new MenuItem("Remove DB Lock", () => ExecuteMenuAction(4)),
                new MenuItem("Fix Pacman Keys", () => ExecuteMenuAction(5)),
                new MenuItem("Sync database", () => ExecuteMenuAction(8)),
                new MenuItem("Clean system", () => ExecuteMenuAction(9))
            }),
            new MenuCategory("Configuration", new List<MenuItem>
            {
                new MenuItem("Install Package Manager", () => ExecuteMenuAction(6)),
                new MenuItem("Set Package Manager", () => ExecuteMenuAction(7)),
                new MenuItem("Change Theme", () => ExecuteMenuAction(10))
            }),
            new MenuCategory("Program", new List<MenuItem>
            {
                new MenuItem("About", () => ExecuteMenuAction(11)),
                new MenuItem("Exit", () => ExecuteMenuAction(12))
            })
        };

        public static void ShowMainMenu()
        {
            float v = 1.0f;
            int selectedCategoryIndex = 0;
            int selectedItemIndex = 0;
            bool inCategoryPanel = true;

            ConsoleKeyInfo keyInfo;

            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = Settings.Instance.CurrentTheme.SecondaryColor;
                Console.Write(
@"
██████╗ ██╗   ██╗██╗  ██╗
██╔══██╗██║   ██║╚██╗██╔╝
██████╔╝██║   ██║ ╚███╔╝ 
██╔═══╝ ██║   ██║ ██╔██╗ 
██║     ╚██████╔╝██╔╝ ██╗
╚═╝      ╚═════╝ ╚═╝  ╚═╝
");
                Console.ForegroundColor = Settings.Instance.CurrentTheme.PrimaryColor;
                Console.WriteLine($"PUX - Pacman Utils v{v}");
                Console.WriteLine();

                DrawPanels(selectedCategoryIndex, selectedItemIndex, inCategoryPanel);

                keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (inCategoryPanel)
                        {
                            selectedCategoryIndex = (selectedCategoryIndex > 0) ? selectedCategoryIndex - 1 : menuCategories.Count - 1;
                        }
                        else
                        {
                            selectedItemIndex = (selectedItemIndex > 0) ? selectedItemIndex - 1 : menuCategories[selectedCategoryIndex].Items.Count - 1;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (inCategoryPanel)
                        {
                            selectedCategoryIndex = (selectedCategoryIndex < menuCategories.Count - 1) ? selectedCategoryIndex + 1 : 0;
                        }
                        else
                        {
                            selectedItemIndex = (selectedItemIndex < menuCategories[selectedCategoryIndex].Items.Count - 1) ? selectedItemIndex + 1 : 0;
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        inCategoryPanel = true;
                        break;
                    case ConsoleKey.RightArrow:
                        inCategoryPanel = false;
                        break;
                    case ConsoleKey.Enter:
                        if (!inCategoryPanel)
                        {
                            var action = menuCategories[selectedCategoryIndex].Items[selectedItemIndex].Action;
                            action.Invoke();
                        }
                        break;
                    case ConsoleKey.Escape:
                        selectedCategoryIndex = menuCategories.FindIndex(c => c.Name == "Program");
                        if(selectedCategoryIndex != -1){
                            selectedItemIndex = menuCategories[selectedCategoryIndex].Items.FindIndex(i => i.Name == "Exit");
                            if(selectedItemIndex != -1){
                                inCategoryPanel = false;
                                menuCategories[selectedCategoryIndex].Items[selectedItemIndex].Action.Invoke();
                            }
                        }
                        break;
                }
            }
        }

        private static void DrawPanels(int selectedCategoryIndex, int selectedItemIndex, bool inCategoryPanel)
        {
            const int categoryPanelWidth = 25;
            const int itemPanelWidth = 40;
            Console.Write("┌");
            Console.Write(new string('─', categoryPanelWidth));
            Console.Write("┬");
            Console.Write(new string('─', itemPanelWidth));
            Console.WriteLine("┐");

            int maxRows = Math.Max(menuCategories.Count, menuCategories.Select(c => c.Items.Count).DefaultIfEmpty(0).Max());


            for (int i = 0; i < maxRows; i++)
            {
                Console.Write("│");
                if (i < menuCategories.Count)
                {
                    bool isSelectedCategory = (i == selectedCategoryIndex);
                    string text = menuCategories[i].Name;
                    string toPrint;
                    if (isSelectedCategory && inCategoryPanel)
                    {
                        toPrint = " ► " + text;
                        Console.ForegroundColor = Settings.Instance.CurrentTheme.SecondaryColor;
                    }
                    else
                    {
                        toPrint = "   " + text;
                        Console.ForegroundColor = Settings.Instance.CurrentTheme.PrimaryColor;
                    }
                    Console.Write(toPrint.PadRight(categoryPanelWidth));
                }
                else
                {
                    Console.Write(new string(' ', categoryPanelWidth));
                }
                Console.ForegroundColor = Settings.Instance.CurrentTheme.PrimaryColor;
                Console.Write("│");

                if (i < menuCategories[selectedCategoryIndex].Items.Count)
                {
                    bool isSelectedItem = (i == selectedItemIndex);
                    string text = menuCategories[selectedCategoryIndex].Items[i].Name;
                    string toPrint;
                    if (isSelectedItem && !inCategoryPanel)
                    {
                        toPrint = " ► " + text;
                        Console.ForegroundColor = Settings.Instance.CurrentTheme.SecondaryColor;
                    }
                    else
                    {
                        toPrint = "   " + text;
                        Console.ForegroundColor = Settings.Instance.CurrentTheme.PrimaryColor;
                    }
                    Console.Write(toPrint.PadRight(itemPanelWidth));
                }
                else
                {
                    Console.Write(new string(' ', itemPanelWidth));
                }
                Console.ForegroundColor = Settings.Instance.CurrentTheme.PrimaryColor;
                Console.WriteLine("│");
            }

            Console.Write("└");
            Console.Write(new string('─', categoryPanelWidth));
            Console.Write("┴");
            Console.Write(new string('─', itemPanelWidth));
            Console.WriteLine("┘");

            Console.ForegroundColor = Settings.Instance.CurrentTheme.AccentColor;
            Console.WriteLine("Use ←→ arrows to switch panels, ↑↓ to navigate, Enter to select, Esc to Exit");
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
                    PmClean.LoadClean();
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
                    PxSwitch.install("yay");
                    break;
                case 1:
                    PxSwitch.install("paru");
                    break;
                case 2:
                    PxSwitch.install("aurutils");
                    break;
                case 3:
                    PxSwitch.install("pikaur");
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