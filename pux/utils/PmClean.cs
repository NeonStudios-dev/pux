using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pux.utils
{
    public class PmClean
    {
        public static void LoadClean()
        {
            Console.Clear();
            Console.WriteLine("Cleaning up orphaned packages and cache...");
            rx.ExecuteCommand("pacman -Rns $(pacman -Qtdq) --noconfirm", true);
            rx.ExecuteCommand("pacman -Sc --noconfirm", true);
            rx.ExecuteCommand("pacman -Scc --noconfirm", true);
            rx.ExecuteCommand("du -sh ~/.cache/", false);
            rx.ExecuteCommand("rm -rf ~/.cache/", false);
            Console.ReadKey();
            core.Menus.ShowMainMenu();
        }
    }
}