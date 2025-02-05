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
            ////apply naked singles repeatedly.
            //if (!NakedSingles.RepeatNakedSingles(board))
            //{
            //    return false;
            //}
            
            ////apply hidden singles repeatedly.
            //if (!HiddenSingles.RepeatHiddenSingles(board))
            //{
            //    //hidden singles produced a contradiction; backtrack.
            //    return false;
            //}
            //apply hidden pairs repeatedly.
            HiddenPairsResult hpResult = HiddenPairs.RepeatHiddenPairs(board);
            if (hpResult == HiddenPairsResult.Contradiction)
            {
                // Hidden pairs processing detected a contradiction; backtrack.
                return false;
            }


            return true;
        }
    }
}
