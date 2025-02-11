using Omega_Sudoku.Exceptions;
using Omega_Sudoku.UI;
using System;
using System.Diagnostics;
using System.IO;

namespace Omega_Sudoku
{
    internal class Input
    {
        public static void Main(string[] args)
        {
            Menu.ShowIntroduction();
            while (true)
            {
                try
                {
                    Menu.ShowMenu();
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
                    Console.WriteLine("An error occured: " + e.Message);
                    Console.ResetColor();
                }
                

            }
        }
    }
}
