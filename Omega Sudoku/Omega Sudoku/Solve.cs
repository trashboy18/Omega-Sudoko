using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omega_Sudoku
{
    internal class Solve
    {
        static int N = 9;

        //solving the sudoku board using pure backtracking.
        public static bool SolveSudoku(int[,] board, int row,
                                int col)
        {
            //finished backtracking.
            if (row == N - 1 && col == N)
                return true;
           //go to next row.
            if (col == N)
            {
                row++;
                col = 0;
            }
            //check if a cell is empty.
            if (board[row, col] != 0)
                return SolveSudoku(board, row, col + 1);

            for (int num = 1; num <=N; num++)
            {
                if (Helpers.IsSafe(board, row, col, num))
                {
                    board[row, col] = num;
                    // Checking for next possibility with next col.
                    if (SolveSudoku(board, row, col + 1))
                        return true;
                }
                //false assumption, empty the cell.
                board[row, col] = 0;
            }
            return false;
        }
    }
}
