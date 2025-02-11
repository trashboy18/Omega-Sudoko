using Omega_Sudoku.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Omega_Sudoku
{
    internal class BasicHelpers
    {

    /// <summary>
    /// im building a string that contains the 
    /// whole board, as well as the board as a string, 
    /// so i can transfer it easily to a file
    /// </summary>
        public static StringBuilder FinalBoard(int[,] board)
        {
            int boxSize = Globals.MiniSquare; // Typically 3 for a 9x9 Sudoku

            StringBuilder output = new StringBuilder();
            string beggining = $"Sudoku solved! Here is the {Globals.N}×{Globals.N} solution:\n";

            output.AppendLine(beggining);
            string boardPrint = GetFormattedBoard(board);
            string boardStr = Conversions.BoardToString(board);
            output.AppendLine(boardPrint);
            output.AppendLine(boardStr);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(output);
            Console.ResetColor();
            return output;
        }

        /// <summary>
        ///builds the board itself.
        /// </summary>
        
        public static string GetFormattedBoard(int[,] board)
        {
            int N = Globals.N;
            int mini = Globals.MiniSquare;
            StringBuilder sb = new StringBuilder();
            
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
        
        /// <summary>
        /// check if the input represents a sudoku board
        /// </summary
        public static void CheckStringValidity(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new Exception("You can't do that!");
            }

        }
        /// <summary>
        /// check if the starting board is valid row-wise 
        /// </summary>
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
        /// <summary>
        /// check if the starting board is valid col-wise 
        /// </summary>
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
        /// <summary>
        /// check if the starting board is valid box-wise 
        /// </summary>
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
        /// <summary>
        /// general function to check if the initial board is legal.
        /// </summary>
        
        public static void ValidateInitialBoard(int[,] board)
        {
            if (!ValidateBoardRows(board) || !ValidateBoardCols(board)
                || !ValidateBoardBoxes(board))
            {
                throw new InvalidBoard("This Sudoku board is impossible!");
            }
        }

        //calls various function to solve the sudoku.
        public static (StringBuilder,bool) SolveProcess(string input)
        {
            try
            {
                //check if the string is valid.
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
                return (output,true);
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