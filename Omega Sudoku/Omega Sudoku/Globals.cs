using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omega_Sudoku
{
    public static class Globals
    {
        // Board size (for a 9x9 board, N = 9)
        public static int N;

        // Size of a subgrid (for 9x9, MiniSquare = 3)
        public static int MiniSquare;

        // Usage arrays – if a digit is used in a given row, column, or box.
        public static bool[,] rowUsed;
        public static bool[,] colUsed;
        public static bool[,] boxUsed;

        // Candidate sets for each cell
        public static HashSet<int>[,] candidates;
    }
}
