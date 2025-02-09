using Omega_Sudoku.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omega_Sudoku
{
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
        private static int BoxIndex(int r, int c)
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

        //puts digit in board[row,col], updates usage arrays
        public static void PlaceNum(int[,] board, int row, int col, int num)
        {
            board[row, col] = num;
            Globals.rowUsed[row, num] = true;
            Globals.colUsed[col, num] = true;
            Globals.boxUsed[BoxIndex(row, col), num] = true;
            Globals.candidates[row, col].Clear();
        }

        // Removes digit from board, usage arrays
        public static void RemoveNum(int[,] board, int row, int col, int num)
        {
            board[row, col] = 0;
            Globals.rowUsed[row, num] = false;
            Globals.colUsed[col, num] = false;
            Globals.boxUsed[BoxIndex(row, col), num] = false;
        }

        //removes digit from empty neighbors' candidate sets, ignoring filled neighbors
        //if removing digit from a neighbor's set, then set count hits 0,
        //then contradiction, so return false
        public static bool ForwardCheck(int[,] board, int row, int col, int num)
        {
            // row
            if (!RemoveCandidatesFromRow(board, row, col, num))
                return false;

            // col
            if (!RemoveCandidatesFromCol(board, row, col, num))
                return false;

            // box

            if (!RemoveCandidatesFromBox(board, row, col, num))
                return false;

            return true;
        }
        public static bool RemoveCandidatesFromRow(int[,] board, int row, int col, int num)
        {
            for (int cc = 0; cc < N; cc++)
            {
                if (cc != col && board[row, cc] == 0)   
                {
                    if (Globals.candidates[row, cc].Remove(num))
                    {
                        if (Globals.candidates[row, cc].Count == 0) return false;
                    }
                }
            }
            return true;

        }
        public static bool RemoveCandidatesFromCol(int[,] board, int row, int col, int num)
        {
            for (int rr = 0; rr < N; rr++)
            {
                if (rr != row && board[rr, col] == 0)
                {
                    if (Globals.candidates[rr, col].Remove(num))
                    {
                        if (Globals.candidates[rr, col].Count == 0) return false;
                    }
                }
            }
            return true;
        }
        public static bool RemoveCandidatesFromBox(int[,] board, int row, int col, int num)
        {
            int boxIndex = BoxIndex(row, col);
            int startRow = (boxIndex / MiniSquare) * MiniSquare;
            int startCol = (boxIndex % MiniSquare) * MiniSquare;
            for (int rr = 0; rr < MiniSquare; rr++)
            {
                for (int cc = 0; cc < MiniSquare; cc++)
                {
                    int nr = startRow + rr;
                    int nc = startCol + cc;
                    if ((nr != row || nc != col) && board[nr, nc] == 0)
                    {
                        if (Globals.candidates[nr, nc].Remove(num))
                        {
                            if (Globals.candidates[nr, nc].Count == 0) return false;
                        }
                    }
                }
            }
            return true;
        }

        // undo forward-check changes
        public static void UndoForwardCheck(List<(int nr, int nc, int removed)> removedCandidates)
        {
            foreach (var (nr, nc, remDigit) in removedCandidates)
            {
                Globals.candidates[nr, nc].Add(remDigit);
            }
            removedCandidates.Clear();
        }



        public static int[,] CloneBoard(int[,] board)
        {
            //clone board
            
            int[,] boardClone = new int[N,N];
            for(int row = 0; row < N;row++)
                for (int col = 0; col < N;col++)
                    boardClone[row,col] = board[row,col];
            return boardClone;
        }
        public static HashSet<int>[,] CloneCandidates()
        {
            HashSet<int>[,] candidatesClone = new HashSet<int>[N, N];
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    candidatesClone[i, j] = new HashSet<int>(Globals.candidates[i, j]);
            return candidatesClone;
        }

        //clone the complete solver state (board, usage arrays, and candidate sets)
        public static (int[,] boardClone, bool[,] rowUsedClone, bool[,] colUsedClone,
            bool[,] boxUsedClone, HashSet<int>[,] candidatesClone)
            CloneState(int[,] board)
        {
            

            int[,] boardClone = CloneBoard(board);
            //clone usage arrays
            bool[,] rowUsedClone = (bool[,])Globals.rowUsed.Clone();
            bool[,] colUsedClone = (bool[,])Globals.colUsed.Clone();
            bool[,] boxUsedClone = (bool[,])Globals.boxUsed.Clone();

            //clone candidate sets: create new HashSet for each cell

            HashSet<int>[,] candidatesClone = CloneCandidates();


            return (boardClone, rowUsedClone, colUsedClone, boxUsedClone, candidatesClone);
        }

        //restore the solver state from a previously cloned state
        public static void RestoreState(
            (int[,] boardClone, bool[,] rowUsedClone, bool[,] colUsedClone,
            bool[,] boxUsedClone, HashSet<int>[,] candidatesClone) state,
            int[,] board)
        {
            //copy back the board values
            int rows = board.GetLength(0);
            int cols = board.GetLength(1);
            for(int row = 0; row < rows; row++) 
                for(int col = 0; col < cols;col++)
                {
                    board[row,col] = state.boardClone[row,col];
                }
            //restore the static global state
            Globals.rowUsed = state.rowUsedClone;
            Globals.colUsed = state.colUsedClone;
            Globals.boxUsed = state.boxUsedClone;
            Globals.candidates = state.candidatesClone;
        }
    }
}