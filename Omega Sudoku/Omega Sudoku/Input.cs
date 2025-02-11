using Omega_Sudoku.Exceptions;
using System;
using System.Diagnostics;

namespace Omega_Sudoku
{
    internal class Input
    {
        public static void Main(string[] args)
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("select your desired input method:");
                Console.WriteLine("1 - Console");
                Console.WriteLine("2 - text file");
                Console.WriteLine("to close, type 'exit'");
                Console.ResetColor();
                try
                {
                    string choice = Console.ReadLine();
                    BasicHelpers.CheckStringValidity(choice);
                    choice = choice.Trim();
                    if (choice.ToLower().Equals("exit"))
                    {
                        break;
                    }

                    UserChoiceHandler.HandleChoice(choice);
                }
                catch(Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Annn error occured: " + e.Message);
                    Console.ResetColor();
                }
                

            }
        }
    }
}
