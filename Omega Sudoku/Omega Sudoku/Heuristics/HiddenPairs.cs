using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Omega_Sudoku.Helpers.Enum;

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

        // In LogicHelpers.cs

        /// <summary>
        /// Hidden pairs in rows.
        /// For each row, if two candidate numbers appear only in exactly two columns, restrict those cells to just that pair.
        /// </summary>
        public static HiddenPairsResult FindHiddenPairsInRow(int[,] board)
        {
            HiddenPairsResult result = HiddenPairsResult.NoChange;
            for (int row = 0; row < N; row++)
            {
                // Build a dictionary mapping candidate number → count of appearances in the row.
                Dictionary<int, int> keyValuePairs = new Dictionary<int, int>();
                for (int num = 1; num <= N; num++)
                {
                    if (rowUsed[row, num])
                        continue;
                    keyValuePairs[num] = 0;
                    for (int col = 0; col < N; col++)
                    {
                        if (board[row, col] == 0 && candidates[row, col].Contains(num))
                        {
                            keyValuePairs[num]++;
                        }
                    }
                }
                // Build a list of numbers that appear exactly twice in this row.
                List<int> hiddenNums = new List<int>();
                foreach (int num in keyValuePairs.Keys)
                {
                    if (keyValuePairs[num] == 2)
                    {
                        hiddenNums.Add(num);
                    }
                }
                // Build a dictionary mapping a cell (by column index) to a list of candidate numbers (from hiddenNums) that appear in that cell.
                Dictionary<int, List<int>> hiddenCells = new Dictionary<int, List<int>>();
                for (int col = 0; col < N; col++)
                {
                    if (board[row, col] != 0)
                        continue;
                    foreach (int num in hiddenNums)
                    {
                        if (candidates[row, col].Contains(num))
                        {
                            if (!hiddenCells.ContainsKey(col))
                                hiddenCells[col] = new List<int> { num };
                            else
                                hiddenCells[col].Add(num);
                        }
                    }
                }
                // Now check each cell in the row.
                foreach (int hCol in hiddenCells.Keys)
                {
                    // If more than 2 candidate numbers appear exactly twice in this row for that cell,
                    // then that is a contradiction.
                    if (hiddenCells[hCol].Count > 2)
                    {
                        return HiddenPairsResult.Contradiction;
                    }
                    // If the cell currently has at least 2 candidates and hidden singles have found exactly 2 numbers,
                    // restrict its candidate set.
                    else if (hiddenCells[hCol].Count == 2 && candidates[row, hCol].Count > 2)
                    {
                        candidates[row, hCol] = new HashSet<int>(hiddenCells[hCol]);
                        result = HiddenPairsResult.Changed;
                    }
                }
            }
            return result;
        }


        /// <summary>
        /// Hidden pairs in columns.
        /// For each column, if two candidate numbers appear only in exactly two rows, restrict those cells to that pair.
        /// </summary>
        public static HiddenPairsResult FindHiddenPairsInCol(int[,] board)
        {
            HiddenPairsResult result = HiddenPairsResult.NoChange;
            for (int col = 0; col < N; col++)
            {
                Dictionary<int, int> keyValuePairs = new Dictionary<int, int>();
                for (int num = 1; num <= N; num++)
                {
                    if (colUsed[col, num])
                        continue;
                    keyValuePairs[num] = 0;
                    for (int row = 0; row < N; row++)
                    {
                        if (board[row, col] == 0 && candidates[row, col].Contains(num))
                        {
                            keyValuePairs[num]++;
                        }
                    }
                }
                List<int> hiddenNums = new List<int>();
                foreach (int num in keyValuePairs.Keys)
                {
                    if (keyValuePairs[num] == 2)
                    {
                        hiddenNums.Add(num);
                    }
                }
                Dictionary<int, List<int>> hiddenCells = new Dictionary<int, List<int>>();
                for (int row = 0; row < N; row++)
                {
                    if (board[row, col] == 0)
                    {
                        foreach (int num in hiddenNums)
                        {
                            if (candidates[row, col].Contains(num))
                            {
                                if (!hiddenCells.ContainsKey(row))
                                    hiddenCells[row] = new List<int> { num };
                                else
                                    hiddenCells[row].Add(num);
                            }
                        }
                    }
                }
                foreach (int hRow in hiddenCells.Keys)
                {
                    if (hiddenCells[hRow].Count > 2)
                    {
                        return HiddenPairsResult.Contradiction;
                    }
                    else if (hiddenCells[hRow].Count == 2 && candidates[hRow, col].Count > 2)
                    {
                        candidates[hRow, col] = new HashSet<int>(hiddenCells[hRow]);
                        result = HiddenPairsResult.Changed;
                    }
                }
            }
            return result;
        }


        /// <summary>
        /// Hidden pairs in boxes.
        /// For each box, if two candidate numbers appear only in exactly two cells, restrict those cells to that pair.
        /// </summary>
        public static HiddenPairsResult FindHiddenPairsInBox(int[,] board)
        {
            HiddenPairsResult result = HiddenPairsResult.NoChange;
            // There are N boxes (e.g., 9 boxes for a 9x9 board)
            for (int box = 0; box < N; box++)
            {
                Dictionary<int, int> keyValuePairs = new Dictionary<int, int>();
                for (int num = 1; num <= N; num++)
                {
                    if (boxUsed[box, num])
                        continue;
                    keyValuePairs[num] = 0;
                }
                int startRow = (box / MiniSquare) * MiniSquare;
                int startCol = (box % MiniSquare) * MiniSquare;
                for (int r = startRow; r < startRow + MiniSquare; r++)
                {
                    for (int c = startCol; c < startCol + MiniSquare; c++)
                    {
                        if (board[r, c] == 0)
                        {
                            foreach (int num in keyValuePairs.Keys.ToList())
                            {
                                if (candidates[r, c].Contains(num))
                                {
                                    keyValuePairs[num]++;
                                }
                            }
                        }
                    }
                }
                List<int> hiddenNums = new List<int>();
                foreach (int num in keyValuePairs.Keys)
                {
                    if (keyValuePairs[num] == 2)
                    {
                        hiddenNums.Add(num);
                    }
                }
                // Map each cell (identified by its row and column in the box) to a list of candidate numbers from hiddenNums.
                Dictionary<(int, int), List<int>> hiddenCells = new Dictionary<(int, int), List<int>>();
                for (int r = startRow; r < startRow + MiniSquare; r++)
                {
                    for (int c = startCol; c < startCol + MiniSquare; c++)
                    {
                        if (board[r, c] == 0)
                        {
                            foreach (int num in hiddenNums)
                            {
                                if (candidates[r, c].Contains(num))
                                {
                                    var cell = (r, c);
                                    if (!hiddenCells.ContainsKey(cell))
                                        hiddenCells[cell] = new List<int> { num };
                                    else
                                        hiddenCells[cell].Add(num);
                                }
                            }
                        }
                    }
                }
                // Now, for each cell in the box, if it has more than 2 hidden candidate numbers, that is a contradiction.
                foreach (var kvp in hiddenCells)
                {
                    if (kvp.Value.Count > 2)
                    {
                        return HiddenPairsResult.Contradiction;
                    }
                }
                // Alternatively, we can scan pairs by grouping candidate numbers by the cells they appear in.
                // Here we use a simpler approach similar to the row and column routines:
                // For every unordered pair (a, b) in hiddenNums, collect all cells in the box where either appears.
                List<int> nums = hiddenNums; // already collected
                for (int i = 0; i < nums.Count; i++)
                {
                    for (int j = i + 1; j < nums.Count; j++)
                    {
                        int a = nums[i], b = nums[j];
                        HashSet<(int, int)> unionCells = new HashSet<(int, int)>();
                        for (int r = startRow; r < startRow + MiniSquare; r++)
                        {
                            for (int c = startCol; c < startCol + MiniSquare; c++)
                            {
                                if (board[r, c] == 0 && candidates[r, c].Contains(a) || candidates[r, c].Contains(b))
                                {
                                    unionCells.Add((r, c));
                                }
                            }
                        }
                        if (unionCells.Count == 2)
                        {
                            // For each cell in this union, restrict its candidate set to {a, b}
                            foreach (var (r, c) in unionCells)
                            {
                                HashSet<int> oldSet = new HashSet<int>(candidates[r, c]);
                                HashSet<int> newSet = new HashSet<int>(oldSet.Intersect(new int[] { a, b }));
                                if (newSet.Count < oldSet.Count)
                                {
                                    candidates[r, c] = newSet;
                                    result = HiddenPairsResult.Changed;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }


        /// <summary>
        /// Master hidden pairs function: applies hidden pairs in rows, columns, and boxes.
        /// Returns:
        ///   Contradiction - if any unit produced a contradiction,
        ///   Changed      - if any changes were made (and no contradiction),
        ///   NoChange     - if no hidden pairs were applied.
        /// </summary>
        public static HiddenPairsResult FindHiddenPairsAll(int[,] board)
        {
            HiddenPairsResult rowResult = FindHiddenPairsInRow(board);
            if (rowResult == HiddenPairsResult.Contradiction)
                return HiddenPairsResult.Contradiction;

            HiddenPairsResult colResult = FindHiddenPairsInCol(board);
            if (colResult == HiddenPairsResult.Contradiction)
                return HiddenPairsResult.Contradiction;

            HiddenPairsResult boxResult = FindHiddenPairsInBox(board);
            if (boxResult == HiddenPairsResult.Contradiction)
                return HiddenPairsResult.Contradiction;

            // If any of them changed the state, report Changed; otherwise, NoChange.
            if (rowResult == HiddenPairsResult.Changed ||
                colResult == HiddenPairsResult.Changed ||
                boxResult == HiddenPairsResult.Changed)
            {
                return HiddenPairsResult.Changed;
            }
            return HiddenPairsResult.NoChange;
        }

        /// <summary>
        /// Repeatedly applies hidden pairs until no further changes occur.
        /// Before processing, clones the state; if a contradiction is detected after processing,
        /// restores the state and returns Contradiction; otherwise returns Changed if any change was made, or NoChange.
        /// </summary>
        public static HiddenPairsResult RepeatHiddenPairs(int[,] board)
        {
            var savedState = LogicHelpers.CloneState(board);
            HiddenPairsResult result;
            do
            {
                result = FindHiddenPairsAll(board);
                // If a contradiction is found at any point, restore and return.
                if (result == HiddenPairsResult.Contradiction)
                {
                    LogicHelpers.RestoreState(savedState, board);
                    return HiddenPairsResult.Contradiction;
                }
            } while (result == HiddenPairsResult.Changed);

            // After hidden pairs processing, check for contradictions in any empty cell.
            for (int r = 0; r < N; r++)
            {
                for (int c = 0; c < N; c++)
                {
                    if (board[r, c] == 0 && candidates[r, c].Count == 0)
                    {
                        LogicHelpers.RestoreState(savedState, board);
                        return HiddenPairsResult.Contradiction;
                    }
                }
            }
            return HiddenPairsResult.NoChange;
        }


     
        


    }
}
