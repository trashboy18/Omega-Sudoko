﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Omega_Sudoku.Utils
{
    internal class ChangeBoardUtils
    {
        
        //puts digit in board[row,col], updates usage arrays
        public static void PlaceNum(int[,] board, int row, int col, int num)
        {
            board[row, col] = num;
            Globals.rowUsed[row, num] = true;
            Globals.colUsed[col, num] = true;
            Globals.boxUsed[LogicHelpers.BoxIndex(row, col), num] = true;
            Globals.candidates[row, col].Clear();
        }

        // Removes digit from board, usage arrays
        public static void RemoveNum(int[,] board, int row, int col, int num)
        {
            board[row, col] = 0;
            Globals.rowUsed[row, num] = false;
            Globals.colUsed[col, num] = false;
            Globals.boxUsed[LogicHelpers.BoxIndex(row, col), num] = false;
        }
        //removes digit from empty neighbors' candidate sets, ignoring filled neighbors
        //if removing digit from a neighbor's set, then set count hits 0,
        //then contradiction, so return false
        public static bool ForwardCheck(int[,] board, int row, int col, int num)
        {
            // row
            if (!RemoveCandidatesFromRow(board, row, col, num))
                return false;

            // col
            if (!RemoveCandidatesFromCol(board, row, col, num))
                return false;

            // box

            if (!RemoveCandidatesFromBox(board, row, col, num))
                return false;

            return true;
        }
        //removes candidates from row
        public static bool RemoveCandidatesFromRow(int[,] board, int row, int col, int num)
        {
            int N = Globals.N;
            for (int relativeCol = 0; relativeCol < N; relativeCol++)
            {
                if (relativeCol != col && board[row, relativeCol] == 0)
                {
                    if (Globals.candidates[row, relativeCol].Remove(num))
                    {
                        if (Globals.candidates[row, relativeCol].Count == 0) return false;
                    }
                }
            }
            return true;

        }
        //removes candidates from col
        public static bool RemoveCandidatesFromCol(int[,] board, int row, int col, int num)
        {
            int N = Globals.N;
            for (int relativeRow = 0; relativeRow < N; relativeRow++)
            {
                if (relativeRow != row && board[relativeRow, col] == 0)
                {
                    if (Globals.candidates[relativeRow, col].Remove(num))
                    {
                        if (Globals.candidates[relativeRow, col].Count == 0) return false;
                    }
                }
            }
            return true;
        }
        //removes candidates from box
        public static bool RemoveCandidatesFromBox(int[,] board, int row, int col, int num)
        {
            int MiniSquare = Globals.MiniSquare;
            int boxIndex = LogicHelpers.BoxIndex(row, col);
            int startRow = (boxIndex / MiniSquare) * MiniSquare;
            int startCol = (boxIndex % MiniSquare) * MiniSquare;
            for (int relativeRow = 0; relativeRow < MiniSquare; relativeRow++)
            {
                for (int relativecol = 0; relativecol < MiniSquare; relativecol++)
                {
                    int nr = startRow + relativeRow;
                    int nc = startCol + relativecol;
                    if ((nr != row || nc != col) && board[nr, nc] == 0)
                    {
                        if (Globals.candidates[nr, nc].Remove(num))
                        {
                            if (Globals.candidates[nr, nc].Count == 0) return false;
                        }
                    }
                }
            }
            return true;
        }

    }
}
