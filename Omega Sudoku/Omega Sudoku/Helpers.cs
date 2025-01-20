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
        static int N = 9;
        public static void PrintBoard(int[,] board)
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
        public static bool IsSafe(int[,] board, int row, int col,
                           int num)
        {

            // Check if we find the same num
            // in the similar row , we
            // return false
            for (int x = 0; x <= N - 1; x++)
                if (board[row, x] == num)
                    return false;

            // Check if we find the same num
            // in the similar column ,
            // we return false
            for (int x = 0; x <= N - 1; x++)
                if (board[x, col] == num)
                    return false;

            // Check if we find the same num
            // in the particular 3*3
            // matrix, we return false
            int startRow = row - row % (int)Math.Sqrt(N),
                startCol = col - col % 3;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < (int)Math.Sqrt(N); j++)
                    if (board[i + startRow, j + startCol] == num)
                        return false;

            return true;
        }
        public static void SolveProccess( string input)
        {
            try
            {
                
                Helpers.CheckStringValidity(input);

                int[,] board = Conversions.StringToBoard(input);
                Solve.SolveSudoku(board, 0, 0);
                Helpers.PrintBoard(board);
            }
            catch (SudokuException e)
            {
                Console.WriteLine("An error occurred: " + e.Message);

            }
            catch (Exception e)
            {
                Console.WriteLine("Error: couldn't find reason.");
            }
        }
    }
}
