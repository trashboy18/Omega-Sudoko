using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omega_Sudoku
{
    internal class Conversions
    {
        public static int[,] StringToBoard(string input)
        {
            if (input.Length != 81)
            {
                throw new System.ArgumentException(
                    "Input must be exactly 81 characters long.");
            }

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
    }
}
