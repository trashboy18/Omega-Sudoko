using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Omega_Sudoku
{
    internal class LogicHelpers
    {
        static int N = 9;
        static int MiniSquare = (int)Math.Sqrt(N);
        //for each cell [row,col], store a set of valid digits (1..9).
        public static HashSet<int>[,] candidates = new HashSet<int>[N, N];

        //initialize board cells.
        public static void InitializeCells(int[,] board)
        {
            for (int row = 0; row < N; row++)
            {
                for (int col = 0; col < N; col++)
                {
                    candidates[row, col] = new HashSet<int>();
                }
            }

            for(int row = 0; row < N; row++)
            {
                for(int col = 0;col < N; col++)
                {
                    if(board[row,col] == 0)
                    {
                        for(int num = 1; num <=N;num++)
                        {
                            if(IsSafe(board,row,col,num))
                                candidates[row,col].Add(num);
                        }
                    }
                }
            }
        }
        //check if it's valid to put num into cell(row,col).
        public static bool IsSafe(int[,] board, int row, int col,
                           int num)
        {

            //check if we find the same num in the similar row.
            for (int x = 0; x <= N - 1; x++)
                if (board[row, x] == num)
                    return false;

            // check if we find the same num in the similar column. 
            for (int x = 0; x <= N - 1; x++)
                if (board[x, col] == num)
                    return false;

            // check if we find the same num in the particular N*N matrix.
            int squareRib = (int)(Math.Sqrt(N));
            int startRow = row - row % squareRib,
                startCol = col - col % 3;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < squareRib; j++)
                    if (board[i + startRow, j + startCol] == num)
                        return false;

            return true;
        }
        //finds the cell with the least candidates.
        public static (int, int, HashSet<int>) FindCellWithMRV(int[,] board)
        {
            int bestRow = -1;
            int bestCol = -1;
            int bestCount = int.MaxValue;
            HashSet<int> bestCandidates = new HashSet<int>();

            for (int r = 0; r < N; r++)
            {
                for (int c = 0; c < N; c++)
                {
                    if (board[r, c] == 0)
                    {
                        
                        int count = candidates[r,c].Count;

                        if (count < bestCount)
                        {
                            bestCount = count;
                            bestRow = r;
                            bestCol = c;
                            bestCandidates = candidates[r, c];

                            /* Early exit if there's a cell with ZERO candidates,
                               no solution*/
                            if (bestCount == 0)
                            {
                                return (bestRow, bestCol, bestCandidates);
                            }
                        }
                    }
                }
            }
            //board solved.
            if(bestRow == -1)
                return (-1,-1,new HashSet<int>());

            //return a COPY, to keep the original board unchanged.
            return (bestRow, bestCol, new HashSet<int>(bestCandidates));    
        }

        public static bool RemoveCandidatesFromRow(int[,] board, int row, int col, int num, 
            List<(int nr, int nc, int removed)> removedCandidates)
        {
            for (int c = 0; c <N; c++)
            {
                if (c != col && board[row, c] == 0)
                {
                    if (candidates[row, c].Contains(num))
                    {
                        candidates[row, c].Remove(num);
                        removedCandidates.Add((row, c, num));
                        if (candidates[row, c].Count == 0) 
                        {
                            return false; 
                        }
                    }
                }
            }
            return true;
        }
        public static bool RemoveCandidatesFromCol(int[,] board, int row, int col, int num,
            List<(int nr, int nc, int removed)> removedCandidates)
        {

            for (int r = 0; r < 9; r++)
            {
                if (r != row && board[r, col] == 0)
                {
                    if (candidates[r, col].Contains(num))
                    {
                        candidates[r, col].Remove(num);
                        removedCandidates.Add((r, col, num));

                        if (candidates[r, col].Count == 0)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public static bool RemoveCandidatesFromBox(int[,] board, int row, int col, int num,
            List<(int nr, int nc, int removed)> removedCandidates)
        {
            int boxRow = (row / MiniSquare) * MiniSquare;
            int boxCol = (col / MiniSquare) * MiniSquare;
            for (int r = 0; r < MiniSquare; r++)
            {
                for (int c = 0; c < MiniSquare; c++)
                {
                    int nr = boxRow + r;
                    int nc = boxCol + c;
                    if ((nr != row || nc != col) && board[nr, nc] == 0)
                    {
                        if (candidates[nr, nc].Contains(num))
                        {
                            candidates[nr, nc].Remove(num);
                            removedCandidates.Add((nr, nc, num));
                            if (candidates[nr, nc].Count == 0)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
        public static bool ForwardCheck(int[,] board, int row, int col, int num, 
            List<(int nr, int nc, int removed)> removedCandidates)
        {
            //row.
            if (!RemoveCandidatesFromRow(board, row, col, num, removedCandidates))
            {
                return false;
            }
            //col.
            if(!RemoveCandidatesFromCol(board, row, col, num, removedCandidates))
            {
                return false;
            }
            //box.
            if (!RemoveCandidatesFromBox(board, row, col, num, removedCandidates))
            {
                return false;
            }
            //if everything succeeded, return true.
            return true;

        }


    }
}
