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

                Console.WriteLine("select your desired input method:");
                Console.WriteLine("1 - Console");
                Console.WriteLine("2 - text file");
                Console.WriteLine("to close, type 'exit'");
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
