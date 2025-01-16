using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omega_Sudoku
{
    internal class Helpers
    {
        public static void PrintFinalBoard(int[,] board)
        {
            Console.WriteLine("Sudoku solved! Here is the 9×9 solution:");
            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    Console.Write(board[r, c] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
