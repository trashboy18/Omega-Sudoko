using Omega_Sudoku.Exceptions;
using Omega_Sudoku.Heuristics;
using Omega_Sudoku.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using static Omega_Sudoku.Helpers.Enum;

namespace Omega_Sudoku
{
    /// <summary>
    /// The main solver. here the heuristics are called, and everything else that 
    /// relates to solving the board.
    /// </summary>
    internal class Solve
    {
        public static bool SolveSudoku(int[,] board)
        {
            //first, try heuristics.
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
                 var savedState = CloneBoardUtils.CloneState(board);

                //place the candidate number on the clone.
                ChangeBoardUtils.PlaceNum(board, row, col, num);
                //BasicHelpers.PrintBoard(board);

                //apply forward checking on the clone.
                if (!ChangeBoardUtils.ForwardCheck(board, row, col, num))
                {                    
                    //BasicHelpers.PrintBoard(board);
                    CloneBoardUtils.RestoreState(savedState, board);
                    continue;
                }

                //recurse on the updated clone.
                if (SolveSudoku(board))
                {
                    return true;
                }

                //if the recursive call failed, backtrack: undo changes and restore state.


                CloneBoardUtils.RestoreState(savedState, board);
            }

            //if no candidate worked, the board is unsolvable!
            return false;
        }
        //calls various function to solve the sudoku.
        public static (StringBuilder, bool) SolveProcess(string input)
        {
            try
            {
                //check if the string is valid.
                BasicHelpers.CheckStringValidity(input);

                //convert the board to a materix.
                int[,] board = Conversions.StringToBoard(input);
                BasicHelpers.ValidateInitialBoard(board);

                //initialize the cells.(candidates, etc...)
                LogicHelpers.InitializeCells(board);
                //check if the initial board is invalid.(for example: '1' appears in a row twice.
                bool solved = Solve.SolveSudoku(board);
                if (!solved)
                {
                    //sudoku not solveable. 

                    throw new UnsolveableSudokuException("This Sudoku puzzle is unsolvable!");
                }
                //sudoku solved!
                StringBuilder output = BasicHelpers.FinalBoard(board);
                return (output, true);
            }
            catch (SudokuException es)
            {

                throw new SudokuException(es.Message);

            }
            catch (Exception e)
            {
                throw new SudokuException(e.Message);

            }
        }
    }
}
