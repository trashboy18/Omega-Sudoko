using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Omega_Sudoku.Helpers.Enum; // Bring in the Result enum

namespace Omega_Sudoku.Heuristics
{
    internal class NakedSingles
    {
        public static int N;
        public static int MiniSquare;

        // For each row, col, box – true means a digit is already used.
        public static bool[,] rowUsed;
        public static bool[,] colUsed;
        public static bool[,] boxUsed;

        // Candidate sets for each cell (for forward checking with MRV)
        public static HashSet<int>[,] candidates;

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
                    Console.WriteLine(candidates[row,col].First());
                    // If the candidate set for this cell has exactly one candidate...
                    if (candidates[row, col].Count == 1)
                    {
                        int num = candidates[row, col].First();
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
        /// Repeatedly applies naked singles until no further changes occur.
        /// Before processing, it clones the solver state. Then, after processing,
        /// it checks if any empty cell has zero candidates (i.e. a contradiction).
        /// If a contradiction is detected, it restores the saved state and returns Contradiction;
        /// otherwise, returns Changed if any changes were made (or NoChange if none).
        /// </summary>
        public static Result RepeatNakedSingles(int[,] board)
        {
            // Clone the current solver state (board, usage arrays, candidate sets)
            //var savedState = LogicHelpers.CloneState(board);
            N = Globals.N;
            Result res;
            do
            {
                res = FindNakedSingles(board);
                
            } while (res == Result.Changed);
            if(res == Result.Contradiction)
            {
               // LogicHelpers.RestoreState(savedState, board);
                return Result.Contradiction;
            }
            
            return Result.NoChange;
        }
    }
}
