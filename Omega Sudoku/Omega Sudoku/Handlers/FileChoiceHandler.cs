using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omega_Sudoku.Handlers
{
    internal class FileChoiceHandler
    {
        public static void HandleFile()
        {
            Console.WriteLine("Enter the full file path:");
            string filePath = Console.ReadLine().Trim();
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File not found. Please try again.");
            }
            try
            {
                string puzzleString = File.ReadAllText(filePath).Trim();
                StringBuilder solvedBoard = BasicHelpers.SolveProcess(puzzleString);
                // Write the formatted solved board back into the file.
                File.WriteAllText(filePath, solvedBoard.ToString());
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("An appropriate output is now in the input file.");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

        }
    }
}
