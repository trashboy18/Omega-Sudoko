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
        //check if it's valid to put num into cell(row,col).
        public static bool IsSafe(int[,] board, int row, int col,
                           int num)
        {

            //check if we find the same num in the similar row.
            for (int x = 0; x <= N - 1; x++)
                if (board[row, x] == num)
                    return false;

            // check if we find the same num in the similar column. 
            for (int x = 0; x <= N - 1; x++)
                if (board[x, col] == num)
                    return false;

            // check if we find the same num in the particular N*N matrix.
            int squareRib = (int)(Math.Sqrt(N));
            int startRow = row - row % squareRib,
                startCol = col - col % 3;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < squareRib; j++)
                    if (board[i + startRow, j + startCol] == num)
                        return false;

            return true;
        }
        //calls various function to solve the sudoku.
        public static void SolveProccess( string input)
        {
            try
            {
                
                Helpers.CheckStringValidity(input);

                int[,] board = Conversions.StringToBoard(input);
                Solve.SolveSudoku(board);
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

        //finds the cell with the least candidates.
        public static (int, int, List<int>) FindCellWithMRV(int[,] board)
        {
            int bestRow = -1;
            int bestCol = -1;
            int bestCount = int.MaxValue;
            List<int> bestCandidates = new List<int>();

            for (int r = 0; r < N; r++)
            {
                for (int c = 0; c < N; c++)
                {
                    if (board[r, c] == 0)
                    {
                        // cell is empty, find how many valid candidates
                        List<int> candidates = GetCandidates(board, r, c);
                        int count = candidates.Count;

                        if (count < bestCount)
                        {
                            bestCount = count;
                            bestRow = r;
                            bestCol = c;
                            bestCandidates = candidates;

                            /* Early exit if there's a cell with ZERO candidates,
                               no solution*/
                            if (bestCount == 0)
                            {
                                return (bestRow, bestCol, bestCandidates);
                            }
                        }
                    }
                }
            }

            /*if bestRow is still -1, that means no empty cell was found => 
             board is solved*/
            return (bestRow, bestCol, bestCandidates);
        }


        //gets all valid candidates for the given empty cell.
        public static List<int> GetCandidates(int[,] board, int row, int col)
        {
            List<int> candidates = new List<int>();

            for (int num = 1; num <= N; num++)
            {
                if (Helpers.IsSafe(board, row, col, num))
                {
                    candidates.Add(num);
                }
            }

            return candidates;
        }

    }
}
