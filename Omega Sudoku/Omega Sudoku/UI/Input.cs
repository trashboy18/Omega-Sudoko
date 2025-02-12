using Omega_Sudoku.Exceptions;
using Omega_Sudoku.UI;
using System;
using System.Diagnostics;
using System.IO;

namespace Omega_Sudoku
{
    /// <summary>
    /// responsible for getting the input from the user and solving the boards.
    /// </summary>
    internal class Input
    {
        public static void Main(string[] args)
        {
            //show introduction.
            Menu.ShowIntroduction();
            while (true)
            {
                try
                {
                    //show menu.
                    Menu.ShowMenu();
                    string choice = Console.ReadLine();
                    BasicHelpers.CheckStringValidity(choice);
                    choice = choice.Trim();
                    if (choice.ToLower().Equals("exit"))
                    {
                        break;
                    }
                    //operate based on what the user chose
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
