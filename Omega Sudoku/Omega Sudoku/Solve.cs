using System;
using System.Collections.Generic;

namespace Omega_Sudoku
{
    internal class Solve
    {
        static int N = 9;

        /*solve sudoku using backtracking + MRV (minimum remaining values)
        to pick the next cell*/
        public static bool SolveSudoku(int[,] board)
        {
            //find the empty cell with the fewest valid candidates.
            (int row, int col, List<int> candidates) = LogicHelpers.FindCellWithMRV(board);

            //if no empty cell found, puzzle is solved
            if (row == -1)
            {
                return true;
            }

            //try each candidate in that cell.
            foreach (int num in candidates)
            {
                board[row, col] = num;

                if (SolveSudoku(board))
                {
                    return true; //done.
                }

                //otherwise, backtrack.
                board[row, col] = 0;
            }

            //no candidate worked, unsolvable from this configuration.
            return false;
        }

        
        
    }
}
