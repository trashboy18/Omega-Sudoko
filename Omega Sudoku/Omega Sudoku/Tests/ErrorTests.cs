using Microsoft.VisualStudio.TestTools.UnitTesting;
using Omega_Sudoku.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omega_Sudoku.Tests
{
    [TestClass]
    public class ErrorTests
    {
        [TestMethod]
        //unsolveable 9x9 board
        public void TestUnsolvable9x9a()
        {
            string input = "000005080000601043000000000010500000000106000300000005530000061000000004000000000";
            Exception ex = null;
            try
            {
                BasicHelpers.SolveProcess(input);
            }
            catch (Exception e)
            {
                ex = e;
            }
            //Assert
            Assert.IsInstanceOfType(ex, typeof(SudokuException));
        }

        [TestMethod]
        //unsolveable 9x9 board
        public void Test2Unsolvable9x9b()
        {
            string input = "704000002000801000300000000506001002000400000000000900003700000900005000800000060";
            Exception ex = null;
            try
            {
                BasicHelpers.SolveProcess(input);
            }
            catch (Exception e)
            {
                ex = e;
            }
            //Assert
            Assert.IsInstanceOfType(ex, typeof(SudokuException));
        }

        [TestMethod]
        //unsolveable 16x16 board
        public void TestUnsolvable16x16()
        {
            string input = ";0?0=>010690000000710000500:?0;4000000<0400070=005<3000800000000500@000:?80>10004<30>?8;00=20000>?8;270060000000000000900000000?0000?00000>0=000?3:0000>0026000000;>61029@0<00000100<0@00:40000800500:0?;>012600800?0;0000090<0@0;07000005<00?8:00003050:4080709";
            //Assert
            Exception ex = null;
            try
            {
                BasicHelpers.SolveProcess(input);
            }
            catch (Exception e)
            {
                ex = e;
            }
            //Assert
            Assert.IsInstanceOfType(ex, typeof(SudokuException));
        }

        // tests for different Exceptions
        [TestMethod]
        public void TestInvalidBoard()
        {
            //Arrange
            string input = "100002000001000000000000000000000000000000000000000000000000000000000000000000000";
            //Act
            Exception ex = null;
            try
            {
                StringBuilder cs = BasicHelpers.SolveProcess(input).Item1;
            }
            catch (Exception e)
            {
                ex = e;
            }
            //Assert
            Assert.IsInstanceOfType(ex, typeof(SudokuException));
        }

        [TestMethod]
        //empty input
        public void TestEmptyInput()
        {
            //Arrange
            string input = "";
            //Act
            Exception ex = null;
            try
            {
                BasicHelpers.CheckStringValidity(input);
            }
            catch (Exception e)
            {
                ex = e;
            }
            //Assert
            Assert.IsInstanceOfType(ex, typeof(Exception));
        }

        [TestMethod]
        
        public void TestInvalidBoardSize()
        {
            string input = "00";
            Exception ex = null;
            try
            {
                Conversions.DeduceBoardSize(input.Length);
            }
            catch (Exception e)
            {
                ex = e;
            }
            //Assert
            Assert.IsInstanceOfType(ex, typeof(InvalidCellsAmountException));
        }
        [TestMethod]
        public void TestInvalidCellInfo()
        {
            string input = "0000000000005000";
            Exception ex = null;
            try
            {
                Conversions.StringToBoard(input);
            }
            catch (Exception e)
            {
                ex = e;
            }
            //Assert
            Assert.IsInstanceOfType(ex, typeof(InvalidCharException));
        }
    }
}
