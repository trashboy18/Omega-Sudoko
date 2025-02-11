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

    }
}
