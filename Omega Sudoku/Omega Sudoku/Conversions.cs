using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omega_Sudoku.Exceptions;
namespace Omega_Sudoku
{
    /// <summary>
    /// responsible for converting stuff.
    /// </summary>
    internal class Conversions
    {
        static int N;
        static int MiniSquare;
        /// <summary>
        /// deduce the length of the board
        /// </summary>
        
        public static int DeduceBoardSize(int length)
        {
            //check if length is a perfect square: length = N*N.
            Globals.N = (int)Math.Round(Math.Sqrt(length));
            N = Globals.N;
            if (N * N != length)
            {
                throw new InvalidCellsAmountException($"Puzzle length {length} isn't N*N for an integer N (e.g. 9x9 => 81).");
            }

            //check if N itself is a perfect square.
            Globals.MiniSquare = (int)Math.Round(Math.Sqrt(N));
            MiniSquare = Globals.MiniSquare;
            if (MiniSquare * MiniSquare != N)
            {
                throw new InvalidCellsAmountException($"N={N} isn't a perfect square. Standard Sudoku requires sqrt(N) to be integer.");
            }

            return N;
        }
        /// <summary>
        /// convert the input string to a board
        /// </summary>
        public static int[,] StringToBoard(string input)
        {
            int length = input.Length;
            int N = DeduceBoardSize(length);

            int[,] board = new int[N, N];
            int index = 0;
            for (int row = 0; row < N; row++)
            {
                for (int col = 0; col < N; col++)
                {
                    char ch = input[index++];
                    int val = ch - '0'; // offset from '0'

                    // allowed range:
                    if (val < 0 || val > N)
                    {
                        throw new InvalidCharException(
                            $"Invalid character '{ch}' => ASCII offset {val}. Must be in [0..{N}].");
                    }

                    board[row, col] = val;
                }
            }
            return board;
        }

        /// <summary>
        /// converts the board to a string.
        /// </summary>
        public static string BoardToString(int[,] board)
        {
            int N = Globals.N;
            if (N != board.GetLength(1))
            {
                throw new InvalidCellsAmountException($"Board must be NxN. Found {N}x{board.GetLength(1)}.");
            }

            StringBuilder sb = new StringBuilder();
            string beginning = "Here is the solved board as a string:";
            sb.AppendLine(beginning);

            for (int row = 0; row < N; row++)
            {
                for (int col = 0; col < N; col++)
                {
                    int val = board[row, col];
                    if (val < 0 || val > N)
                    {
                        throw new InvalidCellsAmountException ($"Cell value {val} out of [0..{N}] range.");
                    }
                    sb.Append((char)('0' + val));
                }
            }
            return sb.ToString();
        }
    }
}
