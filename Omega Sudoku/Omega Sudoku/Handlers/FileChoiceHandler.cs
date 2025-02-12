using Omega_Sudoku.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omega_Sudoku.Handlers
{
    /// <summary>
    /// handles what happens if the user chose file.
    /// </summary>
    internal class FileChoiceHandler
    {
        public static void HandleFile()
        {
            Console.WriteLine("Enter the full file path:");
            string filePath = "";
            try
            {
                filePath = Console.ReadLine();
                BasicHelpers.CheckStringValidity(filePath);
                filePath = filePath.Trim();
                if (!File.Exists(filePath))
                {
                    throw new IOException("File not found. Make sure you write the full path " +
                        "correctly, or ensure it is located inside bin/debug.");
            }
            
            //read the whole file
                string puzzleString = File.ReadAllText(filePath).Trim();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.ResetColor();
                Stopwatch sw = new Stopwatch();
                sw.Start();
                StringBuilder solvedBoard = Solve.SolveProcess(puzzleString).Item1;
                sw.Stop();
                Console.WriteLine("An appropriate output is now inside the input file.");

                //print into file.
                File.WriteAllText(filePath, solvedBoard.ToString());
                Console.WriteLine($"Sudoku solved in {sw.ElapsedMilliseconds} ms");


                // Write the formatted solved board back into the file.

            }
            catch (SudokuException se)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                string output = "an error occured: " + se.Message;
                Console.WriteLine(output);
                //print into file
                File.WriteAllText (filePath,output);  
                Console.ResetColor();

            }
            catch(IOException ioe)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ioe.Message);
                Console.ResetColor();

            }
            catch(Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ResetColor();
            }
        }
    }
}
