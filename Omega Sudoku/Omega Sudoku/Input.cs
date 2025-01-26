using Omega_Sudoku.Exceptions;
using System;
using System.Diagnostics;

namespace Omega_Sudoku
{
    internal class Input
    {
        public static void Main(string[] args)
        {
            while (true)
            {
                
                Console.WriteLine("\nEnter a Sudoku puzzle string (e.g. length 81 for 9×9, 256 for 16×16).");
                Console.WriteLine("Type 'exit' to quit.");
                string puzzleString = Console.ReadLine().Trim();
                if (puzzleString.Equals("exit"))
                {
                    break;
                }
                Stopwatch sw = new Stopwatch();
                sw.Start();
                BasicHelpers.SolveProccess(puzzleString);
                sw.Stop();
                Console.WriteLine($"Sudoku solved in {sw.ElapsedMilliseconds} ms");
            }
        }
    }
}
