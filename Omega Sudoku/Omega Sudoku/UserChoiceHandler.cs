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
                Console.WriteLine("Enter a Sudoku puzzle string (e.g., 81 characters for a 9x9 board):");
                string puzzleString = Console.ReadLine().Trim();
                Stopwatch sw = new Stopwatch();
                sw.Start();
                BasicHelpers.SolveProcess(puzzleString);
                sw.Stop();
                Console.WriteLine($"Sudoku solved in {sw.ElapsedMilliseconds} ms");

            }

        }
    }
}
