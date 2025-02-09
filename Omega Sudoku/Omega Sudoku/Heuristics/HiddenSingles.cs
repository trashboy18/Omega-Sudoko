using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Omega_Sudoku.Helpers.Enum;

namespace Omega_Sudoku
{
    internal class HiddenSingles
    {
        // Assume these static variables are shared with your LogicHelpers (or are initialized there)
        public static int N;
        public static int MiniSquare;
        

        /// <summary>
        /// Process hidden singles in each row.
        /// For each row, if a candidate number appears exactly once (in an empty cell), 
        /// and if it is safe, place it.
        /// Returns Changed if at least one placement was made,
        /// Contradiction immediately if forward checking fails,
        /// or NoChange if nothing happened.
        /// </summary>
        public static Result FindHiddenSinglesInRow(int[,] board)
        {
            Result overallResult = Result.NoChange;
            for (int row = 0; row < N; row++)
            {
                // Build a dictionary mapping candidate number:count of appearances in this row.
                Dictionary<int, int> keyValuePairs = new Dictionary<int, int>();
                for (int num = 1; num <= N; num++)
                {
                    if (Globals.rowUsed[row, num])
                        continue;
                    keyValuePairs[num] = 0;
                    for (int col = 0; col < N; col++)
                    {
                        if (board[row, col] == 0 && Globals.candidates[row, col].Contains(num))
                        {
                            keyValuePairs[num]++;
                        }
                    }
                }
                // For each candidate that appears exactly once in the row, try to place it.
                foreach (int num in keyValuePairs.Keys)
                {
                    if (keyValuePairs[num] == 1)
                    {
                        // Find the unique cell in this row that contains the candidate.
                        for (int col = 0; col < N; col++)
                        {
                            if (board[row, col] == 0 && Globals.candidates[row, col].Contains(num))
                            {
                                Console.WriteLine($"row trying: ({row},{col},{num})");
                                BasicHelpers.PrintBoard(board);

                                if (LogicHelpers.IsSafe(row, col, num))
                                {
                                    LogicHelpers.PlaceNum(board, row, col, num);
                                    if (!LogicHelpers.ForwardCheck(board, row, col, num))
                                    {
                                        // Forward check failure: contradiction found.
                                        Console.WriteLine($"row forwardChecking: ({row},{col},{num})");

                                        return Result.Contradiction;
                                    }
                                    BasicHelpers.PrintBoard(board);

                                    overallResult = Result.Changed;

                                    break;
                                }
                                else
                                {
                                    Console.WriteLine($"row isn't safe: ({row},{col},{num})");
                                    return Result.Contradiction;
                                }
                            }
                        }
                    }
                }
            }

            return overallResult;
        }

        /// <summary>
        /// Process hidden singles in each column.
        /// Returns Changed if at least one placement was made,
        /// Contradiction if one is found, otherwise NoChange.
        /// </summary>
        public static Result FindHiddenSinglesInCol(int[,] board)
        {
            Result overallResult = Result.NoChange;

            for (int col = 0; col < N; col++)
            {
                Dictionary<int, int> keyValuePairs = new Dictionary<int, int>();
                for (int num = 1; num <= N; num++)
                {
                    if (Globals.colUsed[col, num])
                        continue;
                    keyValuePairs[num] = 0;
                    for (int row = 0; row < N; row++)
                    {
                        if (board[row, col] == 0 && Globals.candidates[row, col].Contains(num))
                        {
                            keyValuePairs[num]++;
                        }
                    }
                }
                foreach (int num in keyValuePairs.Keys)
                {
                    if (keyValuePairs[num] == 1)
                    {
                        for (int row = 0; row < N; row++)
                        {
                            if (board[row, col] == 0 && Globals.candidates[row, col].Contains(num))
                            {
                                Console.WriteLine($"col trying: ({row},{col},{num})");
                                BasicHelpers.PrintBoard(board);

                                if (LogicHelpers.IsSafe(row, col, num))
                                {
                                    LogicHelpers.PlaceNum(board, row, col, num);
                                    var removed = new List<(int nr, int nc, int removed)>();
                                    if (!LogicHelpers.ForwardCheck(board, row, col, num))
                                    {
                                        Console.WriteLine($"col failed forward checking: ({row},{col},{num})");

                                        return Result.Contradiction;
                                    }
                                    BasicHelpers.PrintBoard(board);

                                    overallResult = Result.Changed;

                                    break;  // Only one cell per candidate in the column.
                                }
                                else
                                {
                                    Console.WriteLine($"col isnt safe: ({row},{col},{num})");
                                    return Result.Contradiction;
                                }
                            }
                        }
                    }
                }
            }

            return overallResult;
        }

        /// <summary>
        /// Process hidden singles in each box.
        /// Returns Changed if at least one placement was made,
        /// Contradiction if one is found, otherwise NoChange.
        /// </summary>
        public static Result FindHiddenSinglesInBox(int[,] board)
        {
            Result overallResult = Result.NoChange;

            // There are N boxes (for a 9x9 board, N is 9)
            for (int box = 0; box < N; box++)
            {
                Dictionary<int, int> keyValuePairs = new Dictionary<int, int>();
                int startRow = (box / MiniSquare) * MiniSquare;
                int startCol = (box % MiniSquare) * MiniSquare;
                for (int num = 1; num <= N; num++)
                {
                    if (Globals.boxUsed[box, num])
                        continue;
                    keyValuePairs[num] = 0;
                    for (int r = startRow; r < startRow + MiniSquare; r++)
                    {
                        for (int c = startCol; c < startCol + MiniSquare; c++)
                        {
                            if (board[r, c] == 0 && Globals.candidates[r, c].Contains(num))
                            {
                                keyValuePairs[num]++;
                            }
                        }
                    }
                }
                foreach (int num in keyValuePairs.Keys)
                {
                    if (keyValuePairs[num] == 1)
                    {
                        bool placed = false;
                        for (int r = startRow; r < startRow + MiniSquare && !placed; r++)
                        {
                            for (int c = startCol; c < startCol + MiniSquare && !placed; c++)
                            {
                                if (board[r, c] == 0 && Globals.candidates[r, c].Contains(num))
                                {
                                    Console.WriteLine($"Box trying: ({r},{c},{num})");
                                    BasicHelpers.PrintBoard(board);

                                    if (LogicHelpers.IsSafe(r, c, num))
                                    {
                                        LogicHelpers.PlaceNum(board, r, c, num);
                                        if (!LogicHelpers.ForwardCheck(board, r, c, num))
                                        {
                                            Console.WriteLine($"box isn't forwardChecking:({r},{c},{num})");

                                            return Result.Contradiction;
                                        }
                                        overallResult = Result.Changed;
                                        BasicHelpers.PrintBoard(board);

                                        placed = true;
                                    }
                                    else
                                    {
                                        Console.WriteLine($"box isn't safe:({r},{c},{num})");
                                        return Result.Contradiction;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return overallResult;
        }

        /// <summary>
        /// Master function: applies hidden singles in rows, columns, and boxes.
        /// Returns:
        ///    Contradiction if any unit produces a contradiction,
        ///    Changed if at least one placement was made,
        ///    NoChange if nothing was placed.
        /// </summary>
        public static Result FindHiddenSinglesAll(int[,] board)
        {
            Result rResult = FindHiddenSinglesInRow(board);
            if (rResult == Result.Contradiction)
                return Result.Contradiction;

            Result cResult = FindHiddenSinglesInCol(board);
            if (cResult == Result.Contradiction)
                return Result.Contradiction;
            Result bResult = FindHiddenSinglesInBox(board);
            if (bResult == Result.Contradiction)
                return Result.Contradiction;
            if (rResult == Result.Changed || cResult == Result.Changed || bResult == Result.Changed)
                return Result.Changed;
            return Result.NoChange;
        }

        /// <summary>
        /// 
        /// </summary>
        public static Result RepeatHiddenSingles(int[,] board)
        {
            N = Globals.N;
            MiniSquare = Globals.MiniSquare;
            //var savedState = LogicHelpers.CloneState(board);
            Result result;
            do
            {
                result = FindHiddenSinglesAll(board);
                
            } while (result == Result.Changed);

            if (result == Result.Contradiction)
            {
               // LogicHelpers.RestoreState(savedState, board);
                return Result.Contradiction;
            }
            
            return Result.NoChange;
        }
    }
}
