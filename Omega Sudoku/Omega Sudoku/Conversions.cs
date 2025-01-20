using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omega_Sudoku.Exceptions;
namespace Omega_Sudoku
{
    internal class Conversions
    {
        public static int[,] StringToBoard(string input)
        {
            

            int[,] board = new int[9, 9];
            int index = 0;
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    board[row, col] = input[index] - '0'; // '0'->0, '1'->1, etc.
                    index++;
                }
            }
            return board;
        }

        public static string BoardToString(int[,] board)
        {
            if (board.GetLength(0) != 9 || board.GetLength(1) != 9)
            {
                throw new System.ArgumentException(
                    "Board must be a 9x9 matrix.");
            }

            StringBuilder sb = new StringBuilder(81);
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    sb.Append(board[row, col]);
                }
            }
            return sb.ToString();
        }
    }
}
