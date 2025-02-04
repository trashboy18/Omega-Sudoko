using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Omega_Sudoku.Heuristics
{
    internal class NakedSingles
    {
        public static int N;
        public static int MiniSquare;

        //for each row, col, box, true means digit is used in row.
        public static bool[,] rowUsed;
        public static bool[,] colUsed;
        public static bool[,] boxUsed;

        //for forward checking with MRV
        public static HashSet<int>[,] candidates;


        public static bool FindNakedSingles(int[,] board)
        {
            bool changedSomething = false;

            for (int row = 0; row < N; row++)
            {

                for(int col = 0; col < N; col++)
                {
                    if (candidates[row, col].Count == 1)
                    {
                        int num = candidates[row, col].First();
                        if (LogicHelpers.IsSafe(row, col, num))
                        {
                            //Console.ForegroundColor = ConsoleColor.Red;
                            //Console.WriteLine("placing num because of hidden singles");
                            //Console.ResetColor();
                            LogicHelpers.PlaceNum(board, row, col, num);
                            var removed = new List<(int nr, int nc, int removed)>();
                            if (!LogicHelpers.ForwardCheck(board, row, col, num, removed))
                            {
                                //if contradiction, revert
                                LogicHelpers.UndoForwardCheck(removed);
                                LogicHelpers.RemoveNum(board, row, col, num);
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
            return changedSomething;
        }
        



    }
}
