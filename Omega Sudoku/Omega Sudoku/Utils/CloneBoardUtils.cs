using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Omega_Sudoku.Utils
{
    internal class CloneBoardUtils
    {
        public static int[,] CloneBoard(int[,] board)
        {
            //clone board
            int N = Globals.N;
            int[,] boardClone = new int[N, N];
            for (int row = 0; row < N; row++)
                for (int col = 0; col < N; col++)
                    boardClone[row, col] = board[row, col];
            return boardClone;
        }
        public static HashSet<int>[,] CloneCandidates()
        {
            int N = Globals.N;
            HashSet<int>[,] candidatesClone = new HashSet<int>[N, N];
            for (int row = 0; row < N; row++)
                for (int col = 0; col < N; col++)
                    candidatesClone[row, col] = new HashSet<int>(Globals.candidates[row, col]);
            return candidatesClone;
        }

        //clone the complete solver state (board, usage arrays, and candidate sets)
        public static (int[,] boardClone, bool[,] rowUsedClone, bool[,] colUsedClone,
            bool[,] boxUsedClone, HashSet<int>[,] candidatesClone)
            CloneState(int[,] board)
        {


            int[,] boardClone = CloneBoard(board);
            //clone usage arrays
            bool[,] rowUsedClone = (bool[,])Globals.rowUsed.Clone();
            bool[,] colUsedClone = (bool[,])Globals.colUsed.Clone();
            bool[,] boxUsedClone = (bool[,])Globals.boxUsed.Clone();

            //clone candidate sets: create new HashSet for each cell

            HashSet<int>[,] candidatesClone = CloneCandidates();


            return (boardClone, rowUsedClone, colUsedClone, boxUsedClone, candidatesClone);
        }

        //restore the solver state from a previously cloned state
        public static void RestoreState(
            (int[,] boardClone, bool[,] rowUsedClone, bool[,] colUsedClone,
            bool[,] boxUsedClone, HashSet<int>[,] candidatesClone) state,
            int[,] board)
        {
            //copy back the board values
            int rows = board.GetLength(0);
            int cols = board.GetLength(1);
            for (int row = 0; row < rows; row++)
                for (int col = 0; col < cols; col++)
                {
                    board[row, col] = state.boardClone[row, col];
                }
            //restore the static global state
            Globals.rowUsed = state.rowUsedClone;
            Globals.colUsed = state.colUsedClone;
            Globals.boxUsed = state.boxUsedClone;
            Globals.candidates = state.candidatesClone;
        }
    }
}

