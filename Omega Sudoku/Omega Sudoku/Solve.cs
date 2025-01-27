using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Omega_Sudoku
{
    internal class Solve
    {

        /*solve sudoku using backtracking + MRV (minimum remaining values)
        to pick the next cell*/
        public static bool SolveSudoku(int[,] board)
        {
            //find the empty cell with the fewest valid candidates.
            (int row, int col, HashSet<int> candidates) = LogicHelpers.FindCellWithMRV(board);

            //if no empty cell found, puzzle is solved
            if (row == -1)
            {
                return true;
            }

            //try each candidate in that cell.
            foreach (int num in candidates)
            {
                board[row, col] = num;

                var removedCandidates = new List<(int nr, int nc, int removed)>();

                if(!LogicHelpers.ForwardCheck(board, row, col,num,removedCandidates))
                {
                    LogicHelpers.UndoForwardCheck(removedCandidates);
                    board[row, col] = 0;
                    continue;
                }
                LogicHelpers.RepeatNakedSingles(board);

                //recurse.
                if (SolveSudoku(board))
                {
                    return true; //done.
                }
                //otherwise, backtrack.
                LogicHelpers.UndoForwardCheck(removedCandidates);
                board[row, col] = 0;
            }

            //no candidate worked, unsolvable from this configuration.
            return false;
        }

        
        
    }
}
