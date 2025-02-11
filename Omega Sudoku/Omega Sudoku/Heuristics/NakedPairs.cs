using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Omega_Sudoku.Helpers.Enum;

namespace Omega_Sudoku.Heuristics
{

    internal class NakedPairs
    {
        static int N;
        static int MiniSquare;
        public static Result FindNakedPairsInRow(int[,] board)
        {
            Result overallResult = Result.NoChange;
            for (int row = 0; row < N; row++)
            {
                //dictionary that contains a col index and its 2 candidates
                Dictionary<int,HashSet<int>> cellPairs = new Dictionary<int,HashSet<int>>();
                for (int col = 0; col < N; col++)
                {
                    
                    if (board[row,col] == 0 && Globals.candidates[row,col].Count == 2)
                    {
                        cellPairs[col] = new HashSet<int>(Globals.candidates[row,col]);
                    }
                }

                //save which columns we have already processed.
                List<int> processedCols = new List<int>();

                foreach (int col1 in cellPairs.Keys)
                {
                    if (processedCols.Contains(col1))
                        continue;

                    //start a group containing col1.
                    List<int> group = new List<int> { col1 };

                    //compare col1's candidate set with all other cells.
                    foreach (int col2 in cellPairs.Keys)
                    {
                        if (col2 == col1)
                            continue;
                        if (processedCols.Contains(col2))
                            continue;
                        //compare candidate sets.
                        if (cellPairs[col1].SetEquals(cellPairs[col2]))
                        {
                            group.Add(col2);
                        }
                    }

                    //if more than 2 cells share the same candidate pair, that is a contradiction.
                    if (group.Count > 2)
                    {
                        return Result.Contradiction;
                    }
                    //if exactly two cells share the same candidate pair, then eliminate these two numbers from all other empty cells in the row.
                    else if (group.Count == 2)
                    {
                        //mark these columns as processed.
                        processedCols.AddRange(group);
                        //let the naked pair be the candidate set of the first cell.
                        HashSet<int> nakedPair = cellPairs[col1];
                        //for every other empty cell in the same row...
                        for (int col = 0; col < N; col++)
                        {
                            if (board[row, col] != 0)
                                continue;
                            if (group.Contains(col))
                                continue;
                            int beforeCount = Globals.candidates[row, col].Count;
                            //remove each candidate in the naked pair.
                            foreach (int candidate in nakedPair)
                            {
                                Globals.candidates[row, col].Remove(candidate);
                            }
                            if (Globals.candidates[row, col].Count < beforeCount)
                            {
                                overallResult = Result.Changed;
                                //if removal causes the candidate set to become empty, signal a contradiction.
                                if (Globals.candidates[row, col].Count == 0)
                                    return Result.Contradiction;
                            }
                        }
                    }
                }
            }
            return overallResult;
        }
        public static Result FindNakedPairsInCol(int[,] board)
        {
            Result overallResult = Result.NoChange;
            for (int col = 0; col < N; col++)
            {
                // Build a dictionary mapping a row index to the candidate pair (if exactly 2 candidates) in this column.
                Dictionary<int, HashSet<int>> cellPairs = new Dictionary<int, HashSet<int>>();
                for (int row = 0; row < N; row++)
                {
                    if (board[row, col] == 0 && Globals.candidates[row, col].Count == 2)
                    {
                        cellPairs[row] = new HashSet<int>(Globals.candidates[row, col]);
                    }
                }
                // Keep track of which rows have been processed.
                List<int> processedRows = new List<int>();
                foreach (int row1 in cellPairs.Keys)
                {
                    if (processedRows.Contains(row1))
                        continue;
                    List<int> group = new List<int> { row1 };
                    foreach (int row2 in cellPairs.Keys)
                    {
                        if (row2 == row1)
                            continue;
                        if (processedRows.Contains(row2))
                            continue;
                        if (cellPairs[row1].SetEquals(cellPairs[row2]))
                        {
                            group.Add(row2);
                        }
                    }
                    if (group.Count > 2)
                    {
                        return Result.Contradiction;
                    }
                    else if (group.Count == 2)
                    {
                        processedRows.AddRange(group);
                        HashSet<int> nakedPair = cellPairs[row1];
                        // For every other empty cell in the column, remove the candidates.
                        for (int row = 0; row < N; row++)
                        {
                            if (board[row, col] != 0)
                                continue;
                            if (group.Contains(row))
                                continue;
                            int beforeCount = Globals.candidates[row, col].Count;
                            foreach (int candidate in nakedPair)
                            {
                                Globals.candidates[row, col].Remove(candidate);
                            }
                            if (Globals.candidates[row, col].Count < beforeCount)
                            {
                                overallResult = Result.Changed;
                                if (Globals.candidates[row, col].Count == 0)
                                    return Result.Contradiction;
                            }
                        }
                    }
                }
            }
            return overallResult;
        }

        public static Result FindNakedPairsInBox(int[,] board)
        {
            Result overallResult = Result.NoChange;
            // There are N boxes in a board (e.g., 9 for 9x9).
            for (int box = 0; box < N; box++)
            {
                int startRow = (box / MiniSquare) * MiniSquare;
                int startCol = (box % MiniSquare) * MiniSquare;
                // Build a dictionary mapping cell position (row, col) to candidate pair for empty cells in this box.
                Dictionary<(int, int), HashSet<int>> cellPairs = new Dictionary<(int, int), HashSet<int>>();
                for (int r = startRow; r < startRow + MiniSquare; r++)
                {
                    for (int c = startCol; c < startCol + MiniSquare; c++)
                    {
                        if (board[r, c] == 0 && Globals.candidates[r, c].Count == 2)
                        {
                            cellPairs[(r, c)] = new HashSet<int>(Globals.candidates[r, c]);
                        }
                    }
                }
                // List to mark processed cells (by their (row, col) key).
                List<(int, int)> processedCells = new List<(int, int)>();
                foreach (var key in cellPairs.Keys)
                {
                    if (processedCells.Contains(key))
                        continue;
                    List<(int, int)> group = new List<(int, int)> { key };
                    foreach (var other in cellPairs.Keys)
                    {
                        if (other.Equals(key))
                            continue;
                        if (processedCells.Contains(other))
                            continue;
                        if (cellPairs[key].SetEquals(cellPairs[other]))
                        {
                            group.Add(other);
                        }
                    }
                    if (group.Count > 2)
                    {
                        return Result.Contradiction;
                    }
                    else if (group.Count == 2)
                    {
                        processedCells.AddRange(group);
                        HashSet<int> nakedPair = cellPairs[key];
                        // For every other empty cell in this box, remove the two candidates.
                        for (int r = startRow; r < startRow + MiniSquare; r++)
                        {
                            for (int c = startCol; c < startCol + MiniSquare; c++)
                            {
                                if (board[r, c] != 0)
                                    continue;
                                if (group.Contains((r, c)))
                                    continue;
                                int beforeCount = Globals.candidates[r, c].Count;
                                foreach (int candidate in nakedPair)
                                {
                                    Globals.candidates[r, c].Remove(candidate);
                                }
                                if (Globals.candidates[r, c].Count < beforeCount)
                                {
                                    overallResult = Result.Changed;
                                    if (Globals.candidates[r, c].Count == 0)
                                        return Result.Contradiction;
                                }
                            }
                        }
                    }
                }
            }
            return overallResult;
        }

        public static Result FindNakedPairsAll(int[,] board)
        {
            Result rowResult = FindNakedPairsInRow(board);
            if (rowResult == Result.Contradiction)
                return Result.Contradiction;

            Result colResult = FindNakedPairsInCol(board);
            if (colResult == Result.Contradiction)
                return Result.Contradiction;

            Result boxResult = FindNakedPairsInBox(board);
            if (boxResult == Result.Contradiction)
                return Result.Contradiction;

            if (rowResult == Result.Changed || colResult == Result.Changed || boxResult == Result.Changed)
                return Result.Changed;
            return Result.NoChange;
        }
        public static Result RepeatNakedPairs(int[,] board)
        {
            Result result = Result.NoChange;
            int count = 0;

            do
            {
                count++;
                result = FindNakedPairsAll(board);
            } while (result == Result.Changed);
            if (result == Result.Contradiction)
            {
                result = Result.Contradiction;
            }
            if (count > 1)
                return Result.Changed;
            return Result.NoChange;
        }
    }
}
