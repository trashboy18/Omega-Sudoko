using Omega_Sudoku.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omega_Sudoku
{
    /// <summary>
    /// helpers that include logic.
    /// </summary>
    internal class LogicHelpers
    {
        public static int N;
        public static int MiniSquare;



        //called once from BasicHelpers.SolveProcess, after parsing the board
        public static void InitializeCells(int[,] board)
        {
            N = board.GetLength(0);
            if (N != board.GetLength(1))
            {
                throw new InvalidCellsAmountException("Board must be NxN.");
            }
            MiniSquare = (int)Math.Sqrt(N);

            //initialize the usage arrays
            Globals.rowUsed = new bool[N, N + 1];
            Globals.colUsed = new bool[N, N + 1];
            Globals.boxUsed = new bool[N, N + 1];

            //create the candidate sets for each cell
            Globals.candidates = new HashSet<int>[N, N];
            for (int row = 0; row < N; row++)
            {
                for (int col = 0; col < N; col++)
                {
                    Globals.candidates[row, col] = new HashSet<int>();
                }
            }

            //fill usage arrays from the board, and build candidates for empty cells
            for (int row = 0; row < N; row++)
            {
                for (int col = 0; col < N; col++)
                {
                    int num = board[row, col];
                    if (num != 0)
                    {
                        // Mark usage
                        Globals.rowUsed[row, num] = true;
                        Globals.colUsed[col, num] = true;
                        Globals.boxUsed[BoxIndex(row, col), num] = true;
                    }
                }
            }

            //for each empty cell, build its candidate set
            for (int row = 0; row < N; row++)
            {
                for (int col = 0; col < N; col++)
                {
                    if (board[row, col] == 0)
                    {
                        // digits in [1..N]
                        for (int d = 1; d <= N; d++)
                        {
                            if (IsSafe(row, col, d))
                            {
                                Globals.candidates[row, col].Add(d);
                            }
                        }
                    }
                }
            }
        }

        //helper to map row,col -> box index
        public static int BoxIndex(int row, int col)
        {
            int boxRow = row / MiniSquare;
            int boxCol = col / MiniSquare;
            return boxRow * MiniSquare + boxCol;
        }


        //check using rowUsed, colUsed, boxUsed
        public static bool IsSafe(int row, int col, int num)
        {
            int boxIndex = BoxIndex(row, col);
            if (Globals.rowUsed[row, num]) return false;
            if (Globals.colUsed[col, num]) return false;
            if (Globals.boxUsed[boxIndex, num]) return false;
            return true;
        }
        //finds the empty cell with the fewest candidates, returns (-1,-1, empty) if solved
        public static (int, int, HashSet<int>) FindCellWithMRV(int[,] board)
        {
            int bestRow = -1;
            int bestCol = -1;
            int bestCount = int.MaxValue;
            HashSet<int> bestCandidates = new HashSet<int>();

            for (int row = 0; row < N; row++)
            {
                for (int col = 0; col < N; col++)
                {
                    if (board[row, col] == 0)
                    {
                        int count = Globals.candidates[row, col].Count;
                        if (count < bestCount)
                        {
                            bestCount = count;
                            bestRow = row;
                            bestCol = col;
                            bestCandidates = Globals.candidates[row, col];

                            if (bestCount == 0)
                            {
                                // means impossible config,early exit
                                return (bestRow, bestCol, bestCandidates);
                            }
                        }
                    }
                }
            }

            //if bestRow == -1 => no empty => solved
            if (bestRow == -1)
            {
                return (-1, -1, new HashSet<int>());
            }

            //return a copy so we don't mutate the original
            return (bestRow, bestCol, new HashSet<int>(bestCandidates));
        }

    }       
}