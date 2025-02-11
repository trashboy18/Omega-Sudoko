using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omega_Sudoku.UI
{
    internal class Menu
    {
        public static void ShowMenu()
        {
            
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("select your desired input method:");
            Console.WriteLine("1 - Console");
            Console.WriteLine("2 - text file");
            Console.WriteLine("to close, type 'exit'");
            Console.ResetColor();

        }
        public static void ShowIntroduction()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("ΩΩ Welcome to the ultimate sudoku omega! ΩΩ");
            Console.WriteLine("Here you can type in sudokus(through console/file)," +
                "and get a fast solution!");
            Console.ResetColor();
        }
    }
}
