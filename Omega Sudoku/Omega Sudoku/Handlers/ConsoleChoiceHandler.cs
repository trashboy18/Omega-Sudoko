using Omega_Sudoku.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omega_Sudoku.Handlers
{
    internal class ConsoleChoiceHandler
    {
        public static void HandleConsole()
        {
            try
            {
                Console.WriteLine("Enter a Sudoku puzzle string (e.g., 81 characters for a 9x9 board):");
                string puzzleString = Console.ReadLine();
                BasicHelpers.CheckStringValidity(puzzleString);
                puzzleString = puzzleString.Trim();
                Stopwatch sw = new Stopwatch();
                sw.Start();
                BasicHelpers.SolveProcess(puzzleString);
                sw.Stop();
                Console.WriteLine($"Sudoku solved in {sw.ElapsedMilliseconds} ms");
            }
            catch(SudokuException se)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("an error occured: " + se.Message);
                Console.ResetColor();
            }
            
            catch(Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("an error occured: " + e.Message);
                Console.ResetColor();
            }
        }
    }
}
