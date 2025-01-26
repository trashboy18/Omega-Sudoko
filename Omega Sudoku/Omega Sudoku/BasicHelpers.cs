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
            Console.WriteLine($"Sudoku solved! Here is the {N}×{N} solution:");
            for (int r = 0; r < N; r++)
            {
                for (int c = 0; c < N; c++)
                {
                    char ch = (char)('0' + board[r, c]);
                    Console.Write(ch + " ");
                }
                Console.WriteLine();
            }
        }
        //check if the input represents a sudoku board
        public static void CheckStringValidity(string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new InvalidCellsAmountException("board cannot be empty.");
        }
        //just ensure the input size is N squared.
        
        
        //calls various function to solve the sudoku.
        public static void SolveProccess( string input)
        {
            try
            {
                
                CheckStringValidity(input);

                int[,] board = Conversions.StringToBoard(input);
                LogicHelpers.InitializeCells(board);
                //fill any 'naked singles' repeatedly before backtracking.
                LogicHelpers.RepeatNakedSingles(board);
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
