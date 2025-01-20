using Omega_Sudoku.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Omega_Sudoku
{
    internal class Input
    {
        public static void Main(string[] args)
        {

            while (true)
            {
                

                Console.WriteLine("\nEnter an 81-digit Sudoku puzzle (0 for empty cells).");
                Console.WriteLine("Or type 'exit' to quit:");
                string input = Console.ReadLine().Trim().ToLower();
                if (input == "exit")
                {
                    break; // End the program
                }
                Stopwatch sw = new Stopwatch();
                sw.Start();
                Helpers.SolveProccess(input);
                sw.Stop();
                Console.WriteLine($"Sudoku solved in {sw.ElapsedMilliseconds} ms");

            }
        }
    }
}
