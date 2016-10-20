using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SK;
using SKvisual;

namespace SKTest
{
    [TestFixture]
    class TestAlgorithems
    {
        [Test]
        public void TestSinglePossiableNumber()
        {
            var sk =  SKCreator.CreatePuzzleFromText(new string[]
                    {
                        "9....8..2",
                        "..1....3.",
                        ".6.7.....",
                    //-----------------
                        ".....2..7",
                        "..4...9..",
                        ".8.5...6.",
                    //-----------------
                        ".7...9..1",
                        "..36..5..",
                        "2...4..89",

                    });

            var res = SimpleSKAlgo.SingleNumberInRow(sk, (s, str) => { }, () => { });
            res.Count().Should().Be(1);
            res.First().Number.Should().Be(9);

        }

        [Test]
        public void TestSwordfishRow()
        {
            var sk = SKCreator.CreatePuzzleFromText(new string[]
                    {
                        ".2..43.69",
                        "..38962..",
                        "96..25.3.",
                    //-----------------
                        "89.56..13",
                        "6...3....",
                        ".3..81.26",
                    //-----------------
                        "3...1..7.",
                        "..96743.2",
                        "27.358.9.",

                    });

            var res = AdvanceSKAlgo.SwordfishAlgo(sk, (s, str,collection) => { }, () => { });
            res.Count().Should().Be(9);
            res.Select(s => s.ColId).Distinct().Count().Should().Be(3);

        }
        [Test]
        public void TestSwordfishCol()
        {
            var sk = SKCreator.CreatePuzzleFromText(new string[]
                    {
                        "52941.7.3",
                        "..6..3..2",
                        "..32.....",
                    //-----------------
                        ".523...76",
                        "637.5.2..",
                        "19.62753.",
                    //-----------------
                        "3...6942.",
                        "2..83.6..",
                        "96.7423.5",
                                                "",
                    });

            var res = AdvanceSKAlgo.SwordfishAlgo(sk, (s, str, collection) => { }, () => { });
            res.Count().Should().Be(7);
            res.Select(s => s.RowId).Distinct().Count().Should().Be(3);

            sk = SKCreator.CreatePuzzleFromText(new string[]
                    {
                        "926...1..",
                        "537.1.42.",
                        "841...6.3",
                    //-----------------
                        "259734816",
                        "714.6..3.",
                        "36812..4.",
                    //-----------------
                        "1.2....84",
                        "485.7136.",
                        "6.3.....1",
                    });
            res = AdvanceSKAlgo.SwordfishAlgo(sk, (s, str, collection) => { }, () => { });
            res.Count().Should().Be(8);
            res.Select(s => s.RowId).Distinct().Count().Should().Be(3);



        }

        [Test]
        public void TestXWingAlgo()
        {
            Console.Out.WriteLine("Test 1");

            var sk = SKCreator.CreatePuzzleFromText(new string[]
            {
                "1.....569",
                "492.561.8",
                ".561.924.",
                "..964.8.1",
                ".64.1....",
                "218.356.4",
                ".4.5...16",
                "9.5.614.2",
                "621.....5",
            });

            var res = AdvanceSKAlgo.XWingAlgo(sk, (s, str, collection) => { Console.Out.WriteLine(s.ToString() + " :: " + str); }, () => { });
            res.Count().Should().Be(8);
            res.Select(s => s.ColId).Distinct().Count().Should().Be(2);
            Console.Out.WriteLine("Test 2");


            sk = SKCreator.CreatePuzzleFromText(new string[]
            {
                "....6..94",
                "76.91..5.",
                ".9..72.81",
                ".7..5..1.",
                "...7.9...",
                ".8..31.67",
                "24.1...7.",
                ".1..9..4.5",
                "9......1..",
            });

            res = AdvanceSKAlgo.XWingAlgo(sk, (s, str, collection) => { Console.Out.WriteLine(s.ToString() + " :: " + str); }, () => { });
            //res.Count().Should().Be(8);
            //res.Select(s => s.ColId).Distinct().Count().Should().Be(2);
            Console.Out.WriteLine("Test 3");
            sk = SKCreator.CreatePuzzleFromText(new string[]
            {
                ".2.....94",
                "76.91..5.",
                ".9...2.81",
                ".7..5..1.",
                "...7.9...",
                ".8..31.67",
                "24.1...7.",
                ".1..9..45",
                "9.....1..",
            });

            var res1 = AdvanceSKAlgo.XWingAlgo(sk, (s, str, collection) => {Console.Out.WriteLine(s.ToString() + " :: " + str); }, () => { });
            res1.Count().Should().Be(13);
            //res.Select(s => s.ColId).Distinct().Count().Should().Be(2);

        }

        [Test]
        public void BoxLineReductionTest()
        {
            Console.Out.WriteLine("Test 3");
            var sk = SKCreator.CreatePuzzleFromText(new string[]
            {
                ".16..78.3",
                ".9.8.....",
                "87...1.6.",
                ".48...3..",
                "65...9.82",
                ".39...65.",
                ".6.9...2.",
                ".8...2936",
                "9246..51.",                
            });

            var sk1 = SKCreator.CreatePuzzleFromText(new string[]
            {
                ".16..78.3",
                ".9.8.....",
                "87...1.6.",
                ".48...3..",
                "65...9.82",
                ".39...65.",
                ".6.9...2.",
                ".8...2936",
                "9246..51.",
            });

            var res1 = SimpleSKAlgo.BoxLineReduction(sk, (s, str, collection) => { Console.Out.WriteLine(s.ToString() + " :: " + str); }, () => { });
            res1.Count().Should().Be(13);

        }

        [Test]
        public void TestPuzzeleSolver()
        {
            var sk = SKCreator.CreateRandomMattrixShuffle(SKCreator.GenerateSeedMattrix(), (decimal) 5.5);

            var solver  = new SKSolver(sk,null);
            solver.SolveEx();

            string res=string.Empty;
            sk.IsSolved.Should().BeTrue();
            sk.ValidateMattrix(ref res).Should().BeTrue();

        }
    }
}
