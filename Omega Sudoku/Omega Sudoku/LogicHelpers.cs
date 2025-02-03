﻿using Omega_Sudoku.Exceptions;
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

        //for each row, col, box, true means digit is used in row.
        public static bool[,] rowUsed;
        public static bool[,] colUsed;
        public static bool[,] boxUsed;

        //for forward checking with MRV
        public static HashSet<int>[,] candidates;

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
            rowUsed = new bool[N, N + 1];
            colUsed = new bool[N, N + 1];
            boxUsed = new bool[N, N + 1];

            //create the candidate sets for each cell
            candidates = new HashSet<int>[N, N];
            for (int r = 0; r < N; r++)
            {
                for (int c = 0; c < N; c++)
                {
                    candidates[r, c] = new HashSet<int>();
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
                        rowUsed[r, num] = true;
                        colUsed[c, num] = true;
                        boxUsed[BoxIndex(r, c), num] = true;
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
                                candidates[r, c].Add(d);
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
            if (rowUsed[row, num]) return false;
            if (colUsed[col, num]) return false;
            if (boxUsed[b, num]) return false;
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
                        int count = candidates[r, c].Count;
                        if (count < bestCount)
                        {
                            bestCount = count;
                            bestRow = r;
                            bestCol = c;
                            bestCandidates = candidates[r, c];

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
            rowUsed[row, num] = true;
            colUsed[col, num] = true;
            boxUsed[BoxIndex(row, col), num] = true;
            candidates[row, col].Clear();
        }

        // Removes digit from board, usage arrays
        public static void RemoveNum(int[,] board, int row, int col, int num)
        {
            board[row, col] = 0;
            rowUsed[row, num] = false;
            colUsed[col, num] = false;
            boxUsed[BoxIndex(row, col), num] = false;
        }

        //removes digit from empty neighbors' candidate sets, ignoring filled neighbors
        //if removing digit from a neighbor's set, then set count hits 0,
        //then contradiction, so return false
        public static bool ForwardCheck(int[,] board, int row, int col, int num,
                                        List<(int nr, int nc, int removed)> removedCandidates)
        {
            // row
            if (!RemoveCandidatesFromRow(board, row, col, num, removedCandidates))
                return false;
            // col

            if (!RemoveCandidatesFromCol(board, row, col, num, removedCandidates))
                return false;
            // box

            if (!RemoveCandidatesFromBox(board, row, col, num, removedCandidates))
                return false;

            return true;
        }
        public static bool RemoveCandidatesFromRow(int[,] board, int row, int col, int num,
            List<(int nr, int nc, int removedNum)> removedCandidates)
        {
            for (int cc = 0; cc < N; cc++)
            {
                if (cc != col && board[row, cc] == 0)
                {
                    if (candidates[row, cc].Remove(num))
                    {
                        removedCandidates.Add((row, cc, num));
                        if (candidates[row, cc].Count == 0) return false;
                    }
                }
            }
            return true;

        }
        public static bool RemoveCandidatesFromCol(int[,] board, int row, int col, int num,
            List<(int nr, int nc, int removedNum)> removedCandidates)
        {
            for (int rr = 0; rr < N; rr++)
            {
                if (rr != row && board[rr, col] == 0)
                {
                    if (candidates[rr, col].Remove(num))
                    {
                        removedCandidates.Add((rr, col, num));
                        if (candidates[rr, col].Count == 0) return false;
                    }
                }
            }
            return true;
        }
        public static bool RemoveCandidatesFromBox(int[,] board, int row, int col, int num,
           List<(int nr, int nc, int removedNum)> removedCandidates)
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
                        if (candidates[nr, nc].Remove(num))
                        {
                            removedCandidates.Add((nr, nc, num));
                            if (candidates[nr, nc].Count == 0) return false;
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
                candidates[nr, nc].Add(remDigit);
            }
            removedCandidates.Clear();
        }

        public static bool FindHiddenSinglesInRow(int[,] board)
        {
            bool changedSomething = false;
            for (int row = 0; row < N; row++)
            {
                Dictionary<int, int> keyValuePairs = new Dictionary<int, int>();

                for (int num = 1; num <= N; num++)
                {

                    if (rowUsed[row,num])
                    {
                        continue;
                    }
                    for (int col = 0; col < N; col++)
                    {
                        if (candidates[row, col].Contains(num))
                        {
                            if (!keyValuePairs.ContainsKey(num))
                                keyValuePairs.Add(num, 1);
                            else
                                keyValuePairs[num] += 1;

                        }

                    }
                }
                foreach (int num in keyValuePairs.Keys)
                {
                    if (keyValuePairs[num] == 1)
                    {
                        for (int hCol = 0; hCol < N; hCol++)
                        {
                            if(candidates[row, hCol].Contains(num) && board[row,hCol] == 0)
                            {
                                if(IsSafe(row, hCol,num))
                                {
                                    //Console.ForegroundColor = ConsoleColor.Red;
                                    //Console.WriteLine("placing num because of hidden singles");
                                    //Console.ResetColor();
                                    PlaceNum(board, row, hCol,num);
                                    var removed = new List<(int nr, int nc, int removed)>();
                                    if (!ForwardCheck(board, row, hCol, num, removed))
                                    {
                                        //if contradiction, revert
                                        UndoForwardCheck(removed);
                                        RemoveNum(board, row, hCol, num);
                                        //BasicHelpers.PrintBoard(board);
                                        continue;
                                    }
                                    //BasicHelpers.PrintBoard(board);
                                    changedSomething = true;
                                    //Console.ForegroundColor = ConsoleColor.Red;
                                    //Console.WriteLine("finished hidden singles");
                                    //Console.ResetColor();
                                }
                            }
                        }
                    }
                }
            }
            return changedSomething;
        }

        public static bool FindHiddenSinglesInCol(int[,] board)
        {
            bool changedSomething = false;
            for (int col = 0; col < N; col++)
            {
                Dictionary<int, int> keyValuePairs = new Dictionary<int, int>();
                // Count appearances of each candidate in the column (only for empty cells)
                for (int num = 1; num <= N; num++)
                {
                    if (colUsed[col, num])
                        continue;
                    for (int row = 0; row < N; row++)
                    {
                        if (board[row, col] == 0 && candidates[row, col].Contains(num))
                        {
                            if (!keyValuePairs.ContainsKey(num))
                                keyValuePairs[num] = 1;
                            else
                                keyValuePairs[num]++;
                        }
                    }
                }
                // For each candidate that appears exactly once in this column, place it
                foreach (int num in keyValuePairs.Keys)
                {
                    if (keyValuePairs[num] == 1)
                    {
                        for (int row = 0; row < N; row++)
                        {
                            if (board[row, col] == 0 && candidates[row, col].Contains(num))
                            {
                                if (IsSafe(row, col, num))
                                {
                                    PlaceNum(board, row, col, num);
                                    var removed = new List<(int nr, int nc, int removed)>();
                                    if (!ForwardCheck(board, row, col, num, removed))
                                    {
                                        UndoForwardCheck(removed);
                                        RemoveNum(board, row, col, num);
                                        continue;
                                    }
                                    changedSomething = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return changedSomething;
        }

        public static bool FindHiddenSinglesInBox(int[,] board)
        {
            bool changedSomething = false;
            // There are N boxes (for a 9x9, N==9)
            for (int box = 0; box < N; box++)
            {
                Dictionary<int, int> keyValuePairs = new Dictionary<int, int>();
                int startRow = (box / MiniSquare) * MiniSquare;
                int startCol = (box % MiniSquare) * MiniSquare;
                // Count candidate appearances in this box
                for (int num = 1; num <= N; num++)
                {
                    if (boxUsed[box, num])
                        continue;
                    for (int r = startRow; r < startRow + MiniSquare; r++)
                    {
                        for (int c = startCol; c < startCol + MiniSquare; c++)
                        {
                            if (board[r, c] == 0 && candidates[r, c].Contains(num))
                            {
                                if (!keyValuePairs.ContainsKey(num))
                                    keyValuePairs[num] = 1;
                                else
                                    keyValuePairs[num]++;
                            }
                        }
                    }
                }
                // For each candidate that appears exactly once in this box, place it
                foreach (int num in keyValuePairs.Keys)
                {
                    if (keyValuePairs[num] == 1)
                    {
                        // Find the unique cell in this box
                        bool placed = false;
                        for (int r = startRow; r < startRow + MiniSquare && !placed; r++)
                        {
                            for (int c = startCol; c < startCol + MiniSquare && !placed; c++)
                            {
                                if (board[r, c] == 0 && candidates[r, c].Contains(num))
                                {
                                    if (IsSafe(r, c, num))
                                    {
                                        PlaceNum(board, r, c, num);
                                        var removed = new List<(int nr, int nc, int removed)>();
                                        if (!ForwardCheck(board, r, c, num, removed))
                                        {
                                            UndoForwardCheck(removed);
                                            RemoveNum(board, r, c, num);
                                            continue;
                                        }
                                        changedSomething = true;
                                        placed = true; // Break out after placement
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return changedSomething;
        }
        public static bool FindHiddenSinglesAll(int[,] board)
        {
            bool changed = false;
            bool rowChanged = FindHiddenSinglesInRow(board);
            bool colChanged = FindHiddenSinglesInCol(board);
            bool boxChanged = FindHiddenSinglesInBox(board);
            changed = rowChanged || colChanged || boxChanged;
            return changed;
        }

        public static bool RepeatHiddenSingles(int[,] board)
        {
            //clone the current state before applying hidden singles.
            //var contains the board, array usage and candidates
            var savedState = CloneState(board);

            bool changedSomething;
            do
            {
                changedSomething = FindHiddenSinglesAll(board);
            } while (changedSomething);

            //now check if any empty cell has no candidates (i.e. contradiction)
            for (int r = 0; r < N; r++)
            {
                for (int c = 0; c < N; c++)
                {
                    if (board[r, c] == 0 && candidates[r, c].Count == 0)
                    {
                        //contradiction: restore the saved state and signal failure.
                        RestoreState(savedState, board);
                        return false;
                    }
                }
            }
            return true;
        }

        public static int[,] CloneBoard(int[,] board,int rows, int cols)
        {
            //clone board
            int[,] boardClone = new int[rows, cols];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    boardClone[i, j] = board[i, j];
            return boardClone;
        }
        public static HashSet<int>[,] CloneCandidates(int rows, int cols)
        {
            HashSet<int>[,] candidatesClone = new HashSet<int>[rows, cols];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    candidatesClone[i, j] = new HashSet<int>(candidates[i, j]);
            return candidatesClone;
        }
        
        //clone the complete solver state (board, usage arrays, and candidate sets)
        public static (int[,] boardClone, bool[,] rowUsedClone, bool[,] colUsedClone, 
            bool[,] boxUsedClone, HashSet<int>[,] candidatesClone)
            CloneState(int[,] board)
        {
            int rows = board.GetLength(0);
            int cols = board.GetLength(1);

            int[,] boardClone = CloneBoard(board,rows,cols);
            //clone usage arrays
            bool[,] rowUsedClone = (bool[,])rowUsed.Clone();
            bool[,] colUsedClone = (bool[,])colUsed.Clone();
            bool[,] boxUsedClone = (bool[,])boxUsed.Clone();

            //clone candidate sets: create new HashSet for each cell

            HashSet<int>[,] candidatesClone = CloneCandidates(rows, cols);
            

            return (boardClone, rowUsedClone, colUsedClone, boxUsedClone, candidatesClone);
        }

        //restore the solver state from a previously cloned state
        public static void RestoreState(
            (int[,] boardClone, bool[,] rowUsedClone, bool[,] colUsedClone, 
            bool[,] boxUsedClone, HashSet<int>[,] candidatesClone) state,
            int[,] board)
        {
            int rows = board.GetLength(0);
            int cols = board.GetLength(1);

            //copy back the board values
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    board[i, j] = state.boardClone[i, j];

            //restore the static global state
            rowUsed = state.rowUsedClone;
            colUsed = state.colUsedClone;
            boxUsed = state.boxUsedClone;
            candidates = state.candidatesClone;
           
        }

        
    }
}