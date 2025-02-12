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
    /// <summary>
    /// handles the choice of the user
    /// </summary>
    internal class UserChoiceHandler
    {
        public static void HandleChoice(string choice)
        {
            if(choice.Equals("1"))
            {
                //through console.
                ConsoleChoiceHandler.HandleConsole();
            }
            else if(choice.Equals("2"))
            {
                //through file.
                FileChoiceHandler.HandleFile();
            }
            else
            {
                //invalid option.
                throw new InvalidUserChoice("you need to choose a valid option. try again!");
            }

        }
    }
}
