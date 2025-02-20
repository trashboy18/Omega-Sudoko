﻿using System;

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
    //raised if the user's choice is Invalid
    public class InvalidUserChoice : SudokuException
    {
        public InvalidUserChoice(string message)
            : base(message)
        {
            {
            }
        }
    }
    //raised if the initial board is illegal.
    public class InvalidBoard : SudokuException
    {
        public InvalidBoard(string message)
            : base(message)
        {
            {
            }
        }
    }
}
