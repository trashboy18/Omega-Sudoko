using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omega_Sudoku.Tests
{
    [TestClass]
    public class SuccessTests
    {
        // Tests for solvable 9*9 boards
        [TestMethod]
        public void Test9x9a()
        {
            string input = "900800000000000500000000000020010003010000060000400070708600000000030100400000200";
            //Assert
            Assert.IsTrue(BasicHelpers.SolveProcess(input).Item2);
        }

        [TestMethod]
        public void Test9x9b()
        {
            string input = "400000805030000000000700000020000060000080400000010000000603070500200000104000000";
            //Assert
            Assert.IsTrue(BasicHelpers.SolveProcess(input).Item2);
        }

        [TestMethod]
        public void Test9x9c()
        {
            string input = "507084000008000070000100000000040002000000000900020000000001000070000200000350708";
            //Assert
            Assert.IsTrue(BasicHelpers.SolveProcess(input).Item2);
        }

        [TestMethod]
        public void Test9x9d()
        {
            string input = "005300000800000020070010500400005300010070006003200080060500009004000030000009700";
            //Assert
            Assert.IsTrue(BasicHelpers.SolveProcess(input).Item2);
        }

        [TestMethod]
        public void Test9x9e()
        {
            string input = "000006000059000008200008000045000000003000000006003054000325006000000000000000000";
            //Assert
            Assert.IsTrue(BasicHelpers.SolveProcess(input).Item2);
        }

        // Tests for solvable 16x16 boards
        [TestMethod]
        public void Test16x16a()
        {
            string input = "000=5;7000<6304150800000000?0000030000>00;20000570;00000>00000002000900:0750000;0090400>0200<700;00?600=00>901030000?00080=000>0010000<640007?0=000029000300;0000<7003000008402000000:=0007<0069300>0<805:120076000;00043600500>005:0001009000;40600000008000000";
            //Assert
            Assert.IsTrue(BasicHelpers.SolveProcess(input).Item2);
        }

        [TestMethod]
        public void Test16x16b()
        {
            string input = "01002=:000<80>@00965@>?0=70010000@3000100000<=7;00>=53000000?090@>?000;326=901000=10000@5000000058:00400300702007009:0=>0810;<300;5>0:09000<=06700<01?500;7:000>1000007000402;032400<000@>80501:0<@0000000203000020007@:0?0300010?4309600050@:000001004080:09000";
            //Assert
            Assert.IsTrue(BasicHelpers.SolveProcess(input).Item2);
        }

        [TestMethod]
        public void Test16x16c()
        {
            string input = "0<00;04000>0@0070600000?;1308>20@0;01060082=<?00080>9250@0?006;:80?950;06000:@00475=6380000@01<000<:?=100;9800040@620000<30?98=00?7@80904063=05>002040:700000060000600009007300@5;000?0080=04002?0000074390:5<00:001<930=7@600?;6400:@?8>0;270000000>6=0?0010008";
            //Assert
            Assert.IsTrue(BasicHelpers.SolveProcess(input).Item2);
        }

        [TestMethod]
        public void Test16x16d()
        {
            string input = "00;:00007050@000002008;000300:09000001:39008000060004@050>:0;00<00800?0001005000?000000400;<00030:00008@0000400>020000003=95000:4@00?0=0000092570000@4100000<000907>0006000:=00000000020000006>00608<0?0052000700000030:@0000900000=02>000?00300>00009000800000=";
            //Assert
            Assert.IsTrue(BasicHelpers.SolveProcess(input).Item2);
        }

        [TestMethod]
        public void Test16x16e()
        {
            string input = "60400;>1090<=2701000700?000:000>>00000=0?01030808:9;45300070001097;6100><30?080000000:<000050060@0006700:8=000?900:504?0@100003008000002679000=30000507:2000@<000003<986000004000000?00@500010>00@09=0:73<;800003;0:00000>0600@0?1040008=@200090=0670?@00501002;";
            //Assert
            Assert.IsTrue(BasicHelpers.SolveProcess(input).Item2);
        }

        // Tests for solvable 25x25 boards
        [TestMethod]
        public void Test25x25a()
        {
            string input = "0E487:009200I300000=<;0?0090:50>00G=1B00;60A<87FE003000=1BC00;0?070008:5@9200D=1<00?080FE450920000006?A0;80FE400092>I0G0010C0E48705I000003G00000100?0<90:0I>B30H10C00F00<;00E4000H>B1000=0000<@E4075I92:CD=060F?A07@E40I00:50B30H00000000000I02:B0GH>10000D=00000A00@94070000I00GH>A00FE00487030:5C0H00600=000009030:00C000?0006000<;2:5I0B000>6?0=1EA<;0@9000G0>B060D01FEA<;94870002:0:5000CD00B?A=004<0F092870H0BC00A010040;02800930:50=1000000000200@0:0I3CDH>000FE40087030:000000C0A=108709230:0IC00>BA=06000<007090:GH5I0D0>B00160A08;F05I00H00>00A006080FE00:000>000=A000048;0E00002G000006?A000;FE2:7@905I3G000000FE402:0@90H003000CDA<100";
            //Assert
            Assert.IsTrue(BasicHelpers.SolveProcess(input).Item2);
        }

        [TestMethod]
        public void Test25x25b()
        {
            string input = "00085:002900H400000;>=0?0020:30<00G;1F00=600005B0004000;0FC00=0?050008:3@2900D;1>00?080B0730290000006?A0=80B0700029<H0G0010C0078503H000004G00000100?0>20:0H<F40I10C00B00>=0007000I<F1000;0000>@07053H29:0D;060B?A05@070H00:30F40I00000000000H09:F0GI<10000D;00000A00@27050000H00GI<A00BE00785040:3C0I00600;000002040:00C000?0006000>=900H0F000<6?0;1E0>=0@2000G0<F060D01BE0>=27850009:0:3000CD00F?0;007>0B029850I0FC000010070=09800240:30;1000000000900@0:0H4CDI<000BE70085040:000000C0A;108502940:0HC00<FA;06000>005020:GI3H0D0<F00160A08=B03H00I00<00A0060800E00:000<000;A000078=0E00009G000006?A000=009:5@203H4G000000BE709:0@20I004000CDA>100";
            //Assert
            Assert.IsTrue(BasicHelpers.SolveProcess(input).Item2);
        }
        //tests for empty boards.
        [TestMethod]
        public void TestEmpty1x1()
        {
            string input = "0";
            //Assert
            Assert.IsTrue(BasicHelpers.SolveProcess(input).Item2);
        }

        [TestMethod]
        public void TestEmpty4x4()
        {
            string input = "0000000000000000";
            //Assert
            Assert.IsTrue(BasicHelpers.SolveProcess(input).Item2);
        }

        [TestMethod]
        public void TestEmpty9x9()
        {
            string input = "000000000000000000000000000000000000000000000000000000000000000000000000000000000";
            //Assert
            Assert.IsTrue(BasicHelpers.SolveProcess(input).Item2);
        }

        [TestMethod]
        public void TestEmpty16x16()
        {
            string input = "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000";
            //Assert
            Assert.IsTrue(BasicHelpers.SolveProcess(input).Item2);
        }

        [TestMethod]
        public void TestEmpty25x25()
        {
            string input = "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000";
            //Assert
            Assert.IsTrue(BasicHelpers.SolveProcess(input).Item2);
        }

        // Solved Board Test

        [TestMethod]
        public void TestSolved9x9()
        {
            string input = "972853614146279538583146729624718953817395462359462871798621345265934187431587296";
            //Assert
            Assert.IsTrue(BasicHelpers.SolveProcess(input).Item2);
        }
    }
}
