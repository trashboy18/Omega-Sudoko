using Omega_Sudoku.Exceptions;
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
        public static void CheckStringValidity(string input)
        {
            CheckStringSize(input);
            CheckStringChars(input);
        }
        public static void CheckStringSize(string input)
        {
            if (input.Length != 81)
            {
                throw new InvalidCellsAmountException(
                    "Input must be exactly 81 characters long.");
            }
        }
        public static void CheckStringChars(string input)
        {
            // just ensure every character is 0-9.
            foreach (char c in input)
            {
                if (c < '0' || c > '9')
                {
                    throw new InvalidCharException("Input must only contain digits 0-9.");
                }
            }
        }

    }
}
