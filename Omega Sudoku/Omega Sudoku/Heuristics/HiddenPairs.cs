using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Omega_Sudoku.Helpers.Enum;

namespace Omega_Sudoku
{
    internal class HiddenPairs
    {
        public static int N;
        public static int MiniSquare;

        /// <summary>
        /// Hidden pairs in rows.
        /// For each row, if two candidate numbers appear only in exactly two columns, restrict those cells to just that pair.
        /// </summary>
        public static Result FindHiddenPairsInRow(int[,] board)
        {
            
            Result result = Result.NoChange;
            for (int row = 0; row < N; row++)
            {
                // Build a dictionary mapping candidate num → indexes of appearances in the row.
                Dictionary<int, List<int>> keyValuePairs = new Dictionary<int, List<int>>();
                for (int num = 1; num <= N; num++)
                {
                    if (Globals.rowUsed[row, num])
                        continue;
                    
                    for (int col = 0; col < N; col++)
                    {
                        if (board[row, col] == 0 && Globals.candidates[row, col].Contains(num))
                        {
                            if (!keyValuePairs.ContainsKey(num))
                                keyValuePairs[num] = new List<int> { col };
                            else
                                keyValuePairs[num].Add(col);
                        }
                    }
                }
                foreach(int hNum1 in keyValuePairs.Keys)
                {
                    if (keyValuePairs[hNum1].Count == 2)
                    {
                        int hCol1 = keyValuePairs[hNum1].First();
                        int hCol2 = keyValuePairs[hNum1].Last();
                        HashSet<int> values = new HashSet<int>();
                        foreach (int hNum2 in keyValuePairs.Keys)
                        {
                            if(keyValuePairs[hNum2].Count == 2 && 
                                keyValuePairs[hNum2].Contains(hCol1) && 
                                keyValuePairs[hNum2].Contains(hCol2))
                            {
                                values.Add(hNum2);
                            }
                        }
                        if(values.Count == 2)
                        {
                            if (!Globals.candidates[row, hCol1].SetEquals(values))
                            {
                                Globals.candidates[row, hCol1] = new HashSet<int>(values);
                                result = Result.Changed;
                            }
                        }
                        if(values.Count > 2)
                        {
                            return Result.Contradiction;
                        }
                    }
                }
               
            }
            return result;
        }


        /// <summary>
        /// Hidden pairs in columns.
        /// For each column, if two candidate numbers appear only in exactly two rows, restrict those cells to that pair.
        /// </summary>
        public static Result FindHiddenPairsInCol(int[,] board)
        {
            Result result = Result.NoChange;
            for (int col = 0; col < N; col++)
            {
                // Build a dictionary mapping candidate num → indexes of appearances in the row.
                Dictionary<int, List<int>> keyValuePairs = new Dictionary<int, List<int>>();
                for (int num = 1; num <= N; num++)
                {
                    if (Globals.rowUsed[col, num])
                        continue;

                    for (int row = 0; row < N; row++)
                    {
                        if (board[row, col] == 0 && Globals.candidates[row, col].Contains(num))
                        {
                            if (!keyValuePairs.ContainsKey(num))
                                keyValuePairs[num] = new List<int> { row };
                            else
                                keyValuePairs[num].Add(row);
                        }
                    }
                }
                foreach (int hNum1 in keyValuePairs.Keys)
                {
                    if (keyValuePairs[hNum1].Count == 2)
                    {
                        int hRow1 = keyValuePairs[hNum1].First();
                        int hRow2 = keyValuePairs[hNum1].Last();
                        HashSet<int> values = new HashSet<int>();
                        foreach (int hNum2 in keyValuePairs.Keys)
                        {
                            if (keyValuePairs[hNum2].Count == 2 &&
                                keyValuePairs[hNum2].Contains(hRow1) &&
                                keyValuePairs[hNum2].Contains(hRow2))
                            {
                                values.Add(hNum2);
                            }
                        }
                        if (values.Count == 2)
                        {
                            if (!Globals.candidates[hRow1, col].SetEquals(values))
                            {
                                Globals.candidates[hRow1, col] = new HashSet<int>(values);
                                result = Result.Changed;
                            }
                        }
                        if (values.Count > 2)
                        {
                            return Result.Contradiction;
                        }
                    }
                }

            }
            return result;
        }


        /// <summary>
        /// Hidden pairs in boxes.
        /// For each box, if two candidate numbers appear only in exactly two cells, restrict those cells to that pair.
        /// </summary>
        public static Result FindHiddenPairsInBox(int[,] board)
        {
            Result result = Result.NoChange;
            // There are N boxes in an N×N puzzle (with N being a perfect square).
            for (int box = 0; box < N; box++)
            {
                // Compute starting row and column for this box.
                int startRow = (box / MiniSquare) * MiniSquare;
                int startCol = (box % MiniSquare) * MiniSquare;

                // Build a dictionary mapping candidate number → list of positions (row, col)
                // in the current box where that candidate appears.
                Dictionary<int, List<(int, int)>> keyValuePairs = new Dictionary<int, List<(int, int)>>();
                for (int num = 1; num <= N; num++)
                {
                    if (Globals.boxUsed[box, num])
                        continue;

                    // Initialize an empty list for candidate num.
                    keyValuePairs[num] = new List<(int, int)>();

                    // Iterate over every cell in the box.
                    for (int r = startRow; r < startRow + MiniSquare; r++)
                    {
                        for (int c = startCol; c < startCol + MiniSquare; c++)
                        {
                            if (board[r, c] == 0 && Globals.candidates[r, c].Contains(num))
                            {
                                keyValuePairs[num].Add((r, c));
                            }
                        }
                    }
                }

                // Now, for each candidate that appears exactly twice in the box, try to form a hidden pair.
                foreach (int hNum1 in keyValuePairs.Keys)
                {
                    if (keyValuePairs[hNum1].Count == 2)
                    {
                        // Get the two positions where hNum1 appears.
                        var pair = keyValuePairs[hNum1];
                        // Build a union of candidate numbers that appear exactly in these same two cells.
                        HashSet<int> unionCandidates = new HashSet<int>();
                        foreach (int hNum2 in keyValuePairs.Keys)
                        {
                            if (keyValuePairs[hNum2].Count == 2 && 
                                keyValuePairs[hNum2].Contains(pair.First()) && 
                                keyValuePairs[hNum2].Contains(pair.Last()))
                            {
                                unionCandidates.Add(hNum2);                                
                            }
                        }

                        // If exactly two candidate numbers share these two cells, we have a hidden pair.
                        if (unionCandidates.Count == 2)
                        {
                            // Restrict each of the two cells’ candidate sets to exactly that pair.
                            foreach (var pos in pair)
                            {
                                int r = pos.Item1;
                                int c = pos.Item2;
                                if (!Globals.candidates[r, c].SetEquals(unionCandidates))
                                {
                                    Globals.candidates[r, c] = new HashSet<int>(unionCandidates);
                                    result = Result.Changed;
                                }

                            }
                        }
                        // If more than 2 candidates appear exactly in these two cells, that is a contradiction.
                        if (unionCandidates.Count > 2)
                        {
                            return Result.Contradiction;
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
        public static Result FindHiddenPairsAll(int[,] board)
        {
            Result rowResult = FindHiddenPairsInRow(board);
            if (rowResult == Result.Contradiction)
                return Result.Contradiction;

            Result colResult = FindHiddenPairsInCol(board);
            if (colResult == Result.Contradiction)
                return Result.Contradiction;

            Result boxResult = FindHiddenPairsInBox(board);
            if (boxResult == Result.Contradiction)
                return Result.Contradiction;
            
            // If any of them changed the state, report Changed; otherwise, NoChange.
            if (rowResult == Result.Changed ||
                colResult == Result.Changed ||
                boxResult == Result.Changed)
            {
                return Result.Changed;
            }
            return Result.NoChange;
        }

        /// <summary>
        /// Repeatedly applies hidden pairs until no further changes occur.
        /// Before processing, clones the state; if a contradiction is detected after processing,
        /// restores the state and returns Contradiction; otherwise returns Changed if any change was made, or NoChange.
        /// </summary>
        public static Result RepeatHiddenPairs(int[,] board)
        {
            N = Globals.N;
            MiniSquare = Globals.MiniSquare;
            int count = 0;
            Result result;
            do
            {
                count++;
                result = FindHiddenPairsAll(board);
            } while (result == Result.Changed);
            // If a contradiction is found at any point, restore and return.
            if (result == Result.Contradiction)
            {
                return Result.Contradiction;
            }
            if (count > 1)
                return Result.Changed;
            return Result.NoChange;
        }


     
        


    }
}
