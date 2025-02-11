using Omega_Sudoku.Handlers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omega_Sudoku.Exceptions;

namespace Omega_Sudoku
{
    internal class UserChoiceHandler
    {
        public static void HandleChoice(string choice)
        {
            if(choice.Equals("1"))
            {
                ConsoleChoiceHandler.HandleConsole();
            }
            else if(choice.Equals("2"))
            {
                FileChoiceHandler.HandleFile();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("you didn't choose a valid option. try again!");
                Console.ResetColor();
            }

        }
    }
}
