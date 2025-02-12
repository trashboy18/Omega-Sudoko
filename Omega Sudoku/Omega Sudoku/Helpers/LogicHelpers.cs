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
            for (int r = 0; r < N; r++)
            {
                for (int c = 0; c < N; c++)
                {
                    Globals.candidates[r, c] = new HashSet<int>();
                }
            }

            //fill usage arrays from the board, and build candidates for empty cells
            for (int r = 0; r < N; r++)
            {
                for (int c = 0; c < N; c++)
                {
                    int num = board[r, c];
                    if (num != 0)
                    {
                        // Mark usage
                        Globals.rowUsed[r, num] = true;
                        Globals.colUsed[c, num] = true;
                        Globals.boxUsed[BoxIndex(r, c), num] = true;
                    }
                }
            }

            //for each empty cell, build its candidate set
            for (int r = 0; r < N; r++)
            {
                for (int c = 0; c < N; c++)
                {
                    if (board[r, c] == 0)
                    {
                        // digits in [1..N]
                        for (int d = 1; d <= N; d++)
                        {
                            if (IsSafe(r, c, d))
                            {
                                Globals.candidates[r, c].Add(d);
                            }
                        }
                    }
                }
            }
        }

        //helper to map row,col -> box index
        public static int BoxIndex(int r, int c)
        {
            int boxRow = r / MiniSquare;
            int boxCol = c / MiniSquare;
            return boxRow * MiniSquare + boxCol;
        }


        //check using rowUsed, colUsed, boxUsed
        public static bool IsSafe(int row, int col, int num)
        {
            int b = BoxIndex(row, col);
            if (Globals.rowUsed[row, num]) return false;
            if (Globals.colUsed[col, num]) return false;
            if (Globals.boxUsed[b, num]) return false;
            return true;
        }
        //finds the empty cell with the fewest candidates, returns (-1,-1, empty) if solved
        public static (int, int, HashSet<int>) FindCellWithMRV(int[,] board)
        {
            int bestRow = -1;
            int bestCol = -1;
            int bestCount = int.MaxValue;
            HashSet<int> bestCandidates = new HashSet<int>();

            for (int r = 0; r < N; r++)
            {
                for (int c = 0; c < N; c++)
                {
                    if (board[r, c] == 0)
                    {
                        int count = Globals.candidates[r, c].Count;
                        if (count < bestCount)
                        {
                            bestCount = count;
                            bestRow = r;
                            bestCol = c;
                            bestCandidates = Globals.candidates[r, c];

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