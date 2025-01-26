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
        static int N = 9;
        private static int DeduceBoardSize(int length)
        {
            //check if length is a perfect square: length = N*N.
            int N = (int)Math.Round(Math.Sqrt(length));
            if (N * N != length)
            {
                throw new InvalidCellsAmountException($"Puzzle length {length} isn't N*N for an integer N (e.g. 9x9 => 81).");
            }

            //check if N itself is a perfect square.
            int boxSize = (int)Math.Round(Math.Sqrt(N));
            if (boxSize * boxSize != N)
            {
                throw new InvalidCellsAmountException($"N={N} isn't a perfect square. Standard Sudoku requires sqrt(N) to be integer.");
            }

            return N;
        }
        public static int[,] StringToBoard(string input)
        {
            int length = input.Length;
            int N = DeduceBoardSize(length);

            int[,] board = new int[N, N];
            int index = 0;
            for (int r = 0; r < N; r++)
            {
                for (int c = 0; c < N; c++)
                {
                    char ch = input[index++];
                    int val = ch - '0'; // offset from '0'

                    // allowed range:
                    if (val < 0 || val > N)
                    {
                        throw new InvalidCharException(
                            $"Invalid character '{ch}' => ASCII offset {val}. Must be in [0..{N}].");
                    }

                    board[r, c] = val;
                }
            }
            return board;
        }

        public static string BoardToString(int[,] board)
        {
            int N = board.GetLength(0);
            if (N != board.GetLength(1))
            {
                throw new InvalidCellsAmountException($"Board must be NxN. Found {N}x{board.GetLength(1)}.");
            }

            StringBuilder sb = new StringBuilder(N * N);
            for (int r = 0; r < N; r++)
            {
                for (int c = 0; c < N; c++)
                {
                    int val = board[r, c];
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
