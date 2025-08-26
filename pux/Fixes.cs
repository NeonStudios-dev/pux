namespace pux
{
    public class Fixes
    {
        public static void LoadKeyFix()
        {

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("If you want to continue press any key");
            Console.ReadLine();
            rx.ExecuteCommand("rm -r /etc/pacman.d/gnupg", true);
            rx.ExecuteCommand("pacman -Sy --needed --noconfirm gnupg archlinux-keyring", true);
            rx.ExecuteCommand("pacman-key --init", true);
            rx.ExecuteCommand("pacman-key --populate archlinux", true);
            rx.ExecuteCommand("pacman-key --refresh-keys", true);
            rx.ExecuteCommand("pacman -Sy", true);
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}