using Omega_Sudoku.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omega_Sudoku
{
    internal class BasicHelpers
    {

        //sudoku size.
        //for each cell [row,col], store a set of valid digits (1..9).


        //print sudoku board.
        public static void PrintBoard(int[,] board)
        {
            int N = board.GetLength(0);
            int boxSize = (int)Math.Sqrt(N); // Typically 3 for a 9x9 Sudoku

            Console.WriteLine($"Sudoku solved! Here is the {N}×{N} solution:\n");

            for (int r = 0; r < N; r++)
            {
                // Print horizontal separators after each box row, except after the last row
                if (r % boxSize == 0 && r != 0)
                {
                    PrintHorizontalSeparator(boxSize, N);
                }

                for (int c = 0; c < N; c++)
                {
                    // Print vertical separators after each box column, except after the last column
                    if (c % boxSize == 0 && c != 0)
                    {
                        Console.Write("| ");
                    }

                    // Convert the number to a character. If the cell is empty (0), print a dot.
                    char ch = board[r, c] != 0 ? (char)('0' + board[r, c]) : '.';
                    Console.Write(ch + " ");
                }
                Console.WriteLine(); // Move to the next line after each row
            }
        }
        private static void PrintHorizontalSeparator(int boxSize, int N)
        {
            for (int i = 0; i < N; i++)
            {
                Console.Write("--");
                if ((i + 1) % boxSize == 0 && i != N - 1)
                {
                    Console.Write("+");
                }
            }
            Console.WriteLine();
        }
        //check if the input represents a sudoku board
        public static void CheckStringValidity(string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new InvalidCellsAmountException("board cannot be empty.");
        }
        //just ensure the input size is N squared.


        //calls various function to solve the sudoku.
        public static void SolveProccess(string input)
        {
            try
            {

                CheckStringValidity(input);

                int[,] board = Conversions.StringToBoard(input);
                LogicHelpers.InitializeCells(board);
                bool solved = Solve.SolveSudoku(board);
                if (!solved)
                {
                    //sudoku not solveable. 
                    throw new UnsolveableSudokuException("This Sudoku puzzle is unsolvable!");
                }
                PrintBoard(board);
            }
            catch (SudokuException e)
            {
                Console.WriteLine("An error occurred: " + e.Message);

            }
            //catch (Exception e)
            {
                //  Console.WriteLine("Couldn't find the reason for crashing.");
            }

        }



    }
}