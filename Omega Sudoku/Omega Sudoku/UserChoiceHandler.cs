using Omega_Sudoku.Handlers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if(choice.Equals("2"))
            {
                FileChoiceHandler.HandleFile();
            }

        }
    }
}
