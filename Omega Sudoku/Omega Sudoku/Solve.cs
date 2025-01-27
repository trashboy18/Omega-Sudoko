using System;
using System.Collections.Generic;

namespace Omega_Sudoku
{
    internal class Solve
    {
        public static bool SolveSudoku(int[,] board)
        {
            //find the empty cell with the fewest candidates
            (int row, int col, HashSet<int> cellCandidates) = LogicHelpers.FindCellWithMRV(board);

            //if row=-1, puzzle solved
            if (row == -1)
            {
                return true;
            }

            //if that cell's candidates are empty, then unsolvable
            if (cellCandidates.Count == 0)
            {
                return false;
            }

            //try each candidate
            foreach (int num in cellCandidates)
            {
                //check if safe with O(1).
                if (!LogicHelpers.IsSafe(board, row, col, num))
                {
                    continue;
                }

                //place the number updating usage arrays
                LogicHelpers.PlaceNum(board, row, col, num);

                //forward check: remove number from neighbors' sets
                var removed = new List<(int nr, int nc, int removed)>();
                if (!LogicHelpers.ForwardCheck(board, row, col, num, removed))
                {
                    //if contradiction, revert
                    LogicHelpers.UndoForwardCheck(removed);
                    LogicHelpers.RemoveNum(board, row, col, num);
                    continue;
                }

                //recurse
                if (SolveSudoku(board))
                {
                    return true;
                }

                //backtrack
                LogicHelpers.UndoForwardCheck(removed);
                LogicHelpers.RemoveNum(board, row, col, num);
            }

            //no candidate worked, bpard is unsolvable
            return false;
        }
    }
}
