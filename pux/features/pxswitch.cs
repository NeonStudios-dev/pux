using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace pux.features
{
    public class pxswitch
    {
        
        public static void install(string manager)
        {
            switch (manager)
            {
                case "yay":
                    rx.ExecuteCommand("sudo pacman -S --needed --noconfirm git base-devel;git clone https://aur.archlinux.org/yay.git;cd yay; makepkg -si --noconfirm;cd ..; rm -rf yay", false);
                    break;
                case "paru":
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("THIS MAY TAKE LONGER THEN EXPECTED PEASE WAIT A BIT LONGER (5 - 10 minutes) FOR IT TO FULLY COMPILE");
                    Console.ForegroundColor = ConsoleColor.White;
                    Thread.Sleep(4000);
                    rx.ExecuteCommand("sudo pacman -S --needed --noconfirm base-devel git;git clone https://aur.archlinux.org/paru.git;cd paru;makepkg -si --noconfirm; cd ..; rm -rf paru", false);
                    break;
                case "aurutils":
                    rx.ExecuteCommand("sudo pacman -S --needed --noconfirm base-devel git;git clone https://aur.archlinux.org/aurutils.git;cd aurutils;makepkg -si --noconfirm; cd ..; rm -rf aurutils", false);
                    break;
                case "pikaur":
                    rx.ExecuteCommand(@"sudo pacman -S --needed --noconfirm base-devel git;git clone https://aur.archlinux.org/pikaur.git;cd pikaur;makepkg -fsri --noconfirm;cd ..;rm -rf pikaur", false);
                    break;
                default:
                    Console.WriteLine("Invalid");
                    break;
        } 
        }
    }
}