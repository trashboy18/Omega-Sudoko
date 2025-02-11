using Omega_Sudoku.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Omega_Sudoku
{
    internal class BasicHelpers
    {


        //print sudoku board.
        public static StringBuilder FinalBoard(int[,] board)
        {
            int boxSize = Globals.MiniSquare; // Typically 3 for a 9x9 Sudoku

            StringBuilder output = new StringBuilder();
            string boardPrint = GetFormattedBoard(board);
            string boardStr = Conversions.BoardToString(board);
            output.AppendLine(boardPrint);
            output.AppendLine(boardStr);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(output);
            Console.ResetColor();
            return output;
        }

        public static string GetFormattedBoard(int[,] board)
        {
            int N = Globals.N;
            int mini = Globals.MiniSquare;
            StringBuilder sb = new StringBuilder();
            string beggining = $"Sudoku solved! Here is the {Globals.N}×{Globals.N} solution:\n";

            sb.AppendLine(beggining);
            for (int r = 0; r < N; r++)
            {
                if (r > 0 && r % mini == 0)
                {
                    for (int i = 0; i < N * 2 + mini - 1; i++)
                        sb.Append("-");
                    sb.AppendLine();
                }
                for (int c = 0; c < N; c++)
                {
                    if (c > 0 && c % mini == 0)
                        sb.Append("| ");
                    int num = board[r, c];
                    char cell = num == 0 ? '0' : (char)('0'+num);
                    sb.Append(cell + " ");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
        
        //check if the input represents a sudoku board
        public static void CheckStringValidity(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new Exception("You can't do that!");
            }

        }

        public static bool ValidateBoardRows(int[,] board)
        {
            for (int row = 0; row < Globals.N; row++)
            {
                //create an array to track digits (1..n). Index 0 is unused.
                bool[] seen = new bool[Globals.N + 1];
                for (int col = 0; col < Globals.N; col++)
                {
                    int num = board[row, col];
                    if (num != 0)
                    {
                        if (seen[num])
                        {
                            //duplicate found in the row.
                            return false;
                        }
                        seen[num] = true;
                    }
                }
            }
            return true;
        }
        
        public static bool ValidateBoardCols(int[,] board)
        {
            for (int col = 0; col < Globals.N; col++)
            {
                bool[] seen = new bool[Globals.N + 1];
                for (int row = 0; row < Globals.N; row++)
                {
                    int num = board[row, col];
                    if (num != 0)
                    {
                        if (seen[num])
                        {
                            //duplicate found in the column.
                            return false;
                        }
                        seen[num] = true;
                    }
                }
            }
            return true;
        }
        public static bool ValidateBoardBoxes(int[,] board)
        {
            int MiniSquare = Globals.MiniSquare;
            for (int boxRow = 0; boxRow < MiniSquare; boxRow++)
            {
                for (int boxCol = 0; boxCol < MiniSquare; boxCol++)
                {
                    bool[] seen = new bool[Globals.N + 1];
                    for (int r = boxRow * MiniSquare; r < boxRow * MiniSquare + MiniSquare; r++)
                    {
                        for (int c = boxCol * MiniSquare; c < boxCol * MiniSquare + MiniSquare; c++)
                        {
                            int num = board[r, c];
                            if (num != 0)
                            {
                                if (seen[num])
                                {
                                    // Duplicate found in the box.
                                    return false;
                                }
                                seen[num] = true;
                            }
                        }
                    }
                }
            }
            return true;
        }
        public static void ValidateInitialBoard(int[,] board)
        {
            if (!ValidateBoardRows(board) || !ValidateBoardCols(board)
                || !ValidateBoardBoxes(board))
            {
                throw new InvalidBoard("This Sudoku board is impossible!");
            }
        }

        //calls various function to solve the sudoku.
        public static StringBuilder SolveProcess(string input)
        {
            try
            {

                CheckStringValidity(input);

                //convert the board to a materix.
                int[,] board = Conversions.StringToBoard(input);
                ValidateInitialBoard(board);

                //initialize the cells.(candidates, etc...)
                LogicHelpers.InitializeCells(board);
                //check if the initial board is invalid.(for example: '1' appears in a row twice.
                bool solved = Solve.SolveSudoku(board);
                if (!solved)
                {
                    //sudoku not solveable. 
                    throw new UnsolveableSudokuException("This Sudoku puzzle is unsolvable!");
                }
                //sudoku solved!
                StringBuilder output = FinalBoard(board);
                return output;
            }
            catch (SudokuException es)
            {

                throw new SudokuException(es.Message);

            }
            catch (Exception e)
            {
                throw new SudokuException(e.Message);

            }
        }
    }
}