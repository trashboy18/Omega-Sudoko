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
        static int N = 9;
        //for each cell [row,col], store a set of valid digits (1..9).
        public static HashSet<int>[,] Candidates = new HashSet<int>[N, N];


        //print sudoku board.
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
        //check if the input represents a sudoku board
        public static void CheckStringValidity(string input)
        {
            CheckStringSize(input);
            CheckStringChars(input);
        }
        //just ensure the input size is N squared.
        public static void CheckStringSize(string input)
        {
            if (input.Length != 81)
            {
                throw new InvalidCellsAmountException(
                    "Input must be exactly 81 characters long.");
            }
        }
        // just ensure every character is 0-9.
        public static void CheckStringChars(string input)
        {
            foreach (char c in input)
            {
                if (c < '0' || c > '9')
                {
                    throw new InvalidCharException("Input must only contain digits 0-9.");
                }
            }
        }
        
        //calls various function to solve the sudoku.
        public static void SolveProccess( string input)
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
            
        }

        

    }
}
