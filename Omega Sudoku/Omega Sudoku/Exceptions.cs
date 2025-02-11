using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omega_Sudoku.Exceptions
{
    //general sudoku exception.
    public class SudokuException : Exception
    {
        public SudokuException(string message) : base(message) { }
    }
    //raised if the amount of cells in the sudoku is invalid.
    public class InvalidCellsAmountException : SudokuException
    {
        public InvalidCellsAmountException(string message)
            : base(message)
        {
        }
    }
    //raised if there's an invalid char in the input.
    public class InvalidCharException : SudokuException
    {
        public InvalidCharException(string message)
            : base(message)
        {
        }
    }
    //raised if the sudoku is valid in syntax, but is unsolveable.
    public class UnsolveableSudokuException : SudokuException
    {
        public UnsolveableSudokuException(string message)
            : base(message) 
        { 
        }

    }
    public class InvalidUserChoice : SudokuException
    {
        public InvalidUserChoice(string message)
            : base(message)
        {
            {
            }
        }
    }
}
