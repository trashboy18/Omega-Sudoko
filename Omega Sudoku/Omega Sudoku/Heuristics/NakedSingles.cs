using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Omega_Sudoku.Helpers.Enum; 

namespace Omega_Sudoku.Heuristics
{
    internal class NakedSingles
    {
        public static int N;
        public static int MiniSquare;
        /// <summary>
        /// Scans the board and for each empty cell that has exactly one candidate,
        /// places that candidate (if safe) and applies forward checking.
        /// Returns Changed if at least one placement was made; otherwise, NoChange.
        /// (Any contradiction is handled by not placing and simply continuing.)
        /// </summary>
        public static Result FindNakedSingles(int[,] board)
        {
            bool madeChange = false;
            for (int row = 0; row < N; row++)
            {
                for (int col = 0; col < N; col++)
                {
                    // Process only empty cells.
                    if (board[row, col] != 0)
                        continue;
                    // If the candidate set for this cell has exactly one candidate...
                    if (Globals.candidates[row, col].Count == 1)
                    {
                        int num = Globals.candidates[row, col].First();
                        if (LogicHelpers.IsSafe(row, col, num))
                        {
                            LogicHelpers.PlaceNum(board, row, col, num);
                            if (!LogicHelpers.ForwardCheck(board, row, col, num))
                            {
                                // Forward check failed: undo changes.
                                return Result.Contradiction;
                                
                            }
                            madeChange = true;
                            
                        }
                        else
                            return Result.Contradiction;
                    }
                }
            }

            return madeChange ? Result.Changed : Result.NoChange;
        }

        /// <summary>
        /// Repeatedly applies naked singles until no further changes occure
        /// if a contradiction is found, return.
        /// </summary>
        public static Result RepeatNakedSingles(int[,] board)
        {
            N = Globals.N;
            int count = 0;
            Result res;
            do
            {
                count++;
                res = FindNakedSingles(board);
                
            } while (res == Result.Changed);
            if(res == Result.Contradiction)
            {
                return Result.Contradiction;
            }
            if (count > 1)
                return Result.Changed;
            return Result.NoChange;
        }
    }
}
