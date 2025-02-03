using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Omega_Sudoku
{
    internal class HiddenSingles
    {
        public static int N;
        public static int MiniSquare;

        //for each row, col, box, true means digit is used in row.
        public static bool[,] rowUsed;
        public static bool[,] colUsed;
        public static bool[,] boxUsed;

        //for forward checking with MRV
        public static HashSet<int>[,] candidates;
        public static bool FindHiddenSinglesInRow(int[,] board)
        {
            bool changedSomething = false;
            for (int row = 0; row < N; row++)
            {
                Dictionary<int, int> keyValuePairs = new Dictionary<int, int>();

                for (int num = 1; num <= N; num++)
                {

                    if (rowUsed[row, num])
                    {
                        continue;
                    }
                    for (int col = 0; col < N; col++)
                    {
                        if (candidates[row, col].Contains(num))
                        {
                            if (!keyValuePairs.ContainsKey(num))
                                keyValuePairs.Add(num, 1);
                            else
                                keyValuePairs[num] += 1;

                        }

                    }
                }
                foreach (int num in keyValuePairs.Keys)
                {
                    if (keyValuePairs[num] == 1)
                    {
                        for (int hCol = 0; hCol < N; hCol++)
                        {
                            if (candidates[row, hCol].Contains(num) && board[row, hCol] == 0)
                            {
                                if (LogicHelpers.IsSafe(row, hCol, num))
                                {
                                    //Console.ForegroundColor = ConsoleColor.Red;
                                    //Console.WriteLine("placing num because of hidden singles");
                                    //Console.ResetColor();
                                    LogicHelpers.PlaceNum(board, row, hCol, num);
                                    var removed = new List<(int nr, int nc, int removed)>();
                                    if (!LogicHelpers.ForwardCheck(board, row, hCol, num, removed))
                                    {
                                        //if contradiction, revert
                                        LogicHelpers.UndoForwardCheck(removed);
                                        LogicHelpers.RemoveNum(board, row, hCol, num);
                                        //BasicHelpers.PrintBoard(board);
                                        continue;
                                    }
                                    //BasicHelpers.PrintBoard(board);
                                    changedSomething = true;
                                    //Console.ForegroundColor = ConsoleColor.Red;
                                    //Console.WriteLine("finished hidden singles");
                                    //Console.ResetColor();
                                }
                            }
                        }
                    }
                }
            }
            return changedSomething;
        }

        public static bool FindHiddenSinglesInCol(int[,] board)
        {
            bool changedSomething = false;
            for (int col = 0; col < N; col++)
            {
                Dictionary<int, int> keyValuePairs = new Dictionary<int, int>();
                // Count appearances of each candidate in the column (only for empty cells)
                for (int num = 1; num <= N; num++)
                {
                    if (colUsed[col, num])
                        continue;
                    for (int row = 0; row < N; row++)
                    {
                        if (board[row, col] == 0 && candidates[row, col].Contains(num))
                        {
                            if (!keyValuePairs.ContainsKey(num))
                                keyValuePairs[num] = 1;
                            else
                                keyValuePairs[num]++;
                        }
                    }
                }
                // For each candidate that appears exactly once in this column, place it
                foreach (int num in keyValuePairs.Keys)
                {
                    if (keyValuePairs[num] == 1)
                    {
                        for (int row = 0; row < N; row++)
                        {
                            if (board[row, col] == 0 && candidates[row, col].Contains(num))
                            {
                                if (LogicHelpers.IsSafe(row, col, num))
                                {
                                    LogicHelpers.PlaceNum(board, row, col, num);
                                    var removed = new List<(int nr, int nc, int removed)>();
                                    if (!LogicHelpers.ForwardCheck(board, row, col, num, removed))
                                    {
                                        LogicHelpers.UndoForwardCheck(removed);
                                        LogicHelpers.RemoveNum(board, row, col, num);
                                        continue;
                                    }
                                    changedSomething = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return changedSomething;
        }

        public static bool FindHiddenSinglesInBox(int[,] board)
        {
            bool changedSomething = false;
            // There are N boxes (for a 9x9, N==9)
            for (int box = 0; box < N; box++)
            {
                Dictionary<int, int> keyValuePairs = new Dictionary<int, int>();
                int startRow = (box / MiniSquare) * MiniSquare;
                int startCol = (box % MiniSquare) * MiniSquare;
                // Count candidate appearances in this box
                for (int num = 1; num <= N; num++)
                {
                    if (boxUsed[box, num])
                        continue;
                    for (int r = startRow; r < startRow + MiniSquare; r++)
                    {
                        for (int c = startCol; c < startCol + MiniSquare; c++)
                        {
                            if (board[r, c] == 0 && candidates[r, c].Contains(num))
                            {
                                if (!keyValuePairs.ContainsKey(num))
                                    keyValuePairs[num] = 1;
                                else
                                    keyValuePairs[num]++;
                            }
                        }
                    }
                }
                // For each candidate that appears exactly once in this box, place it
                foreach (int num in keyValuePairs.Keys)
                {
                    if (keyValuePairs[num] == 1)
                    {
                        // Find the unique cell in this box
                        bool placed = false;
                        for (int r = startRow; r < startRow + MiniSquare && !placed; r++)
                        {
                            for (int c = startCol; c < startCol + MiniSquare && !placed; c++)
                            {
                                if (board[r, c] == 0 && candidates[r, c].Contains(num))
                                {
                                    if (LogicHelpers.IsSafe(r, c, num))
                                    {
                                        LogicHelpers.PlaceNum(board, r, c, num);
                                        var removed = new List<(int nr, int nc, int removed)>();
                                        if (!LogicHelpers.ForwardCheck(board, r, c, num, removed))
                                        {
                                            LogicHelpers.UndoForwardCheck(removed);
                                            LogicHelpers.RemoveNum(board, r, c, num);
                                            continue;
                                        }
                                        changedSomething = true;
                                        placed = true; // Break out after placement
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return changedSomething;
        }
        public static bool FindHiddenSinglesAll(int[,] board)
        {
            bool changed = false;
            bool rowChanged = FindHiddenSinglesInRow(board);
            bool colChanged = FindHiddenSinglesInCol(board);
            bool boxChanged = FindHiddenSinglesInBox(board);
            changed = rowChanged || colChanged || boxChanged;
            return changed;
        }

        public static bool RepeatHiddenSingles(int[,] board)
        {
            //clone the current state before applying hidden singles.
            //var contains the board, array usage and candidates
            var savedState = LogicHelpers.CloneState(board);

            bool changedSomething;
            do
            {
                changedSomething = FindHiddenSinglesAll(board);
            } while (changedSomething);

            //now check if any empty cell has no candidates (i.e. contradiction)
            for (int r = 0; r < N; r++)
            {
                for (int c = 0; c < N; c++)
                {
                    if (board[r, c] == 0 && candidates[r, c].Count == 0)
                    {
                        //contradiction: restore the saved state and signal failure.
                        LogicHelpers.RestoreState(savedState, board);
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
