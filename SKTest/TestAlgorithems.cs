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
        public void TestSwordfish()
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
