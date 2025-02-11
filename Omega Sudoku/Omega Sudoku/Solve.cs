using Omega_Sudoku.Heuristics;
using System;
using System.Collections.Generic;
using static Omega_Sudoku.Helpers.Enum;

namespace Omega_Sudoku
{
    internal class Solve
    {
        public static bool SolveSudoku(int[,] board)
        {
            if (!HeuristicSolver.HeuristicSolving(board))
            {
               return false;
            }
            //find the empty cell with the fewest candidates using MRV.
            (int row, int col, HashSet<int> cellCandidates) = LogicHelpers.FindCellWithMRV(board);

            //if no empty cells remain, the puzzle is solved.
            if (row == -1)
            {
                return true;
            }

            //if the cell has no candidates, unsolvable.
            if (cellCandidates.Count == 0)
            {
                return false;
            }

            //for each candidate number for the chosen cell...
            foreach (int num in cellCandidates)
            {
                //check if placing 'num' is safe.
                if (!LogicHelpers.IsSafe(row, col, num))
                {
                    continue;
                }

                //clone the current state before guessing.
                 var savedState = LogicHelpers.CloneState(board);

                //place the candidate number on the clone.
                LogicHelpers.PlaceNum(board, row, col, num);
                //BasicHelpers.PrintBoard(board);

                //apply forward checking on the clone.
                if (!LogicHelpers.ForwardCheck(board, row, col, num))
                {                    
                    //BasicHelpers.PrintBoard(board);
                    LogicHelpers.RestoreState(savedState, board);
                    continue;
                }

                //recurse on the updated clone.
                if (SolveSudoku(board))
                {
                    return true;
                }

                //if the recursive call failed, backtrack: undo changes and restore state.


                LogicHelpers.RestoreState(savedState, board);
            }

            //if no candidate worked, the board is unsolvable.
            return false;
        }
    }
}
