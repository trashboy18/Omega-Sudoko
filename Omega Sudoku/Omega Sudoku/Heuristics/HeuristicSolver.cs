using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Omega_Sudoku.Helpers.Enum;

namespace Omega_Sudoku.Heuristics
{
    internal class HeuristicSolver
    {
        public static bool HeuristicSolving(int[,] board)
        {

            //apply naked singles repeatedly.
            Result nsResult = NakedSingles.RepeatNakedSingles(board);
            if (nsResult == Result.Contradiction)
            {

                return false;
            }
            //apply hidden singles repeatedly.
            Result hsResult = HiddenSingles.RepeatHiddenSingles(board);

            if (hsResult == Result.Contradiction)
            {
                //hidden singles produced a contradiction; backtrack.
                return false;
            }

            //apply naked pairs repeatedly.
            Result npResult = NakedPairs.RepeatNakedPairs(board);
            if(npResult == Result.Contradiction)
            {
                return false;
            }
            //apply hidden pairs repeatedly.
            Result hpResult = HiddenPairs.RepeatHiddenPairs(board);
            if (hpResult == Result.Contradiction)
            {
                //Hidden pairs processing detected a contradiction; backtrack.

                return false;
            }
            //if any change was made, go back and re-do the heuristics.
            if (hsResult == Result.Changed || hpResult == Result.Changed ||
                npResult == Result.Changed)
                HeuristicSolving(board);
            return true;
        }
    }
}
