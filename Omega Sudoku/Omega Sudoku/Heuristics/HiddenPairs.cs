using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omega_Sudoku
{
    internal class HiddenPairs
    {
        public static int N;
        public static int MiniSquare;

        //for each row, col, box, true means digit is used in row.
        public static bool[,] rowUsed;
        public static bool[,] colUsed;
        public static bool[,] boxUsed;

        //for forward checking with MRV
        public static HashSet<int>[,] candidates;


        /// <summary>
        /// Hidden pairs in rows.
        /// For each row, if two candidate numbers appear only in exactly two columns, restrict those cells to just that pair.
        /// </summary>
        public static bool FindHiddenPairsInRow(int[,] board)
        {
            bool changedSomething = false;
            for (int row = 0; row < N; row++)
            {
                Dictionary<int, int> keyValuePairs = new Dictionary<int, int>();

                for (int num = 1; num <= N; num++)
                {

                    if (rowUsed[row, num])
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
                List<int> hiddenNums = new List<int>();
                foreach(int num in keyValuePairs.Keys)
                {
                    if (keyValuePairs[num] == 2)
                    {
                        hiddenNums.Append(num);
                    }
                }
                Dictionary<int,List<int>> hiddenCells = new Dictionary<int,List<int>>();
                for(int col = 0; col < N; col++)
                {
                    foreach(int num in hiddenNums)
                    {
                        if (candidates[row, col].Contains(num))
                        {
                            if (!hiddenCells.ContainsKey(col))
                                hiddenCells[col] = new List<int>(num);
                            else
                                hiddenCells[col].Append(num);
                        }
                    }
                }
                foreach(int hCol in  hiddenCells.Keys)
                {
                    candidates[row, hCol] = new HashSet<int>(hiddenCells[hCol]);
                }
            }
            return changedSomething;
        }
        /// <summary>
        /// Hidden pairs in columns.
        /// For each column, if two candidate numbers appear only in exactly two rows, restrict those cells to that pair.
        /// </summary>
        public static bool FindHiddenPairsInCol(int[,] board)
        {
            bool changed = false;
            for (int c = 0; c < N; c++)
            {
                // Build a dictionary mapping candidate number -> list of row indices (for empty cells in column c)
                Dictionary<int, List<int>> candidateToRows = new Dictionary<int, List<int>>();
                for (int num = 1; num <= N; num++)
                {
                    if (!colUsed[c, num])
                        candidateToRows[num] = new List<int>();
                }
                // Populate the dictionary
                for (int r = 0; r < N; r++)
                {
                    if (board[r, c] == 0)
                    {
                        foreach (int num in candidateToRows.Keys.ToList())
                        {
                            if (candidates[r, c].Contains(num))
                            {
                                candidateToRows[num].Add(r);
                            }
                        }
                    }
                }
                // Check each unordered pair (a, b)
                List<int> candidateNums = candidateToRows.Keys.ToList();
                for (int i = 0; i < candidateNums.Count; i++)
                {
                    for (int j = i + 1; j < candidateNums.Count; j++)
                    {
                        int a = candidateNums[i], b = candidateNums[j];
                        HashSet<int> unionRows = new HashSet<int>(candidateToRows[a]);
                        unionRows.UnionWith(candidateToRows[b]);
                        if (unionRows.Count == 2)
                        {
                            // For each row in the union, restrict the candidate set at (row, c) to {a, b}
                            foreach (int row in unionRows)
                            {
                                HashSet<int> oldSet = new HashSet<int>(candidates[row, c]);
                                HashSet<int> newSet = new HashSet<int>(oldSet.Intersect(new int[] { a, b }));
                                if (newSet.Count < oldSet.Count)
                                {
                                    candidates[row, c] = newSet;
                                    changed = true;
                                }
                            }
                        }
                    }
                }
            }
            return changed;
        }

        /// <summary>
        /// Hidden pairs in boxes.
        /// For each box, if two candidate numbers appear only in exactly two cells, restrict those cells to that pair.
        /// </summary>
        public static bool FindHiddenPairsInBox(int[,] board)
        {
            bool changed = false;
            // There are N boxes (for a 9x9 board, N == 9)
            for (int box = 0; box < N; box++)
            {
                Dictionary<int, List<(int r, int c)>> candidateToCells = new Dictionary<int, List<(int, int)>>();
                for (int num = 1; num <= N; num++)
                {
                    if (!boxUsed[box, num])
                        candidateToCells[num] = new List<(int, int)>();
                }
                int startRow = (box / MiniSquare) * MiniSquare;
                int startCol = (box % MiniSquare) * MiniSquare;
                for (int r = startRow; r < startRow + MiniSquare; r++)
                {
                    for (int c = startCol; c < startCol + MiniSquare; c++)
                    {
                        if (board[r, c] == 0)
                        {
                            foreach (int num in candidateToCells.Keys.ToList())
                            {
                                if (candidates[r, c].Contains(num))
                                {
                                    candidateToCells[num].Add((r, c));
                                }
                            }
                        }
                    }
                }
                // Check each unordered pair (a, b)
                List<int> candidateNums = candidateToCells.Keys.ToList();
                for (int i = 0; i < candidateNums.Count; i++)
                {
                    for (int j = i + 1; j < candidateNums.Count; j++)
                    {
                        int a = candidateNums[i], b = candidateNums[j];
                        HashSet<(int, int)> unionCells = new HashSet<(int, int)>(candidateToCells[a]);
                        unionCells.UnionWith(candidateToCells[b]);
                        if (unionCells.Count == 2)
                        {
                            // Restrict the candidate set of those two cells to {a, b}
                            foreach (var (r, c) in unionCells)
                            {
                                HashSet<int> oldSet = new HashSet<int>(candidates[r, c]);
                                HashSet<int> newSet = new HashSet<int>(oldSet.Intersect(new int[] { a, b }));
                                if (newSet.Count < oldSet.Count)
                                {
                                    candidates[r, c] = newSet;
                                    changed = true;
                                }
                            }
                        }
                    }
                }
            }
            return changed;
        }

        /// <summary>
        /// Master function: applies hidden pairs in rows, columns, and boxes.
        /// Returns true if any changes were made.
        /// </summary>
        public static bool FindHiddenPairsAll(int[,] board)
        {
            bool changed = false;
            if (FindHiddenPairsInRow(board))
                changed = true;
            if (FindHiddenPairsInCol(board))
                changed = true;
            if (FindHiddenPairsInBox(board))
                changed = true;
            return changed;
        }

        /// <summary>
        /// Repeatedly applies hidden pairs until no more changes occur.
        /// Uses state cloning to restore the state if a contradiction is detected.
        /// </summary>
        public static bool RepeatHiddenPairs(int[,] board)
        {
            // Clone state before applying hidden pairs
            var savedState = LogicHelpers.CloneState(board);

            bool changed;
            do
            {
                changed = FindHiddenPairsAll(board);
            } while (changed);

            // Check for contradictions: if any empty cell has no candidates, restore state and return false.
            for (int r = 0; r < N; r++)
            {
                for (int c = 0; c < N; c++)
                {
                    if (board[r, c] == 0 && candidates[r, c].Count == 0)
                    {
                        LogicHelpers.RestoreState(savedState, board);
                        return false;
                    }
                }
            }
            return true;
        }


    }
}
