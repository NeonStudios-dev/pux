using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pux.utils
{
    public class sync
    {
        public static void SyncDatabase()
        {
            Console.Clear();
            Console.WriteLine("Syncing database...");
            rx.ExecuteCommand("pacman -Syy", true);
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}