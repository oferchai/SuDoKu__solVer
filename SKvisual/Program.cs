using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SK;

namespace SKvisual
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            

            f = new Form1();
            

            Application.Run(f);
        }

        public static SKMattrix mtrx;
        public static Form1 f;


        public static SKMattrix CreateMattrixRnd()
        {
            return SKCreator.CreateRandomMattrixShuffle(SKCreator.GenerateSeedMattrix(),  (decimal)5.5 );
        }

        public static SKMattrix CreateMattrix()
        {
            SKSingle[] singles = new[]
            {
                new SKSingle(0, 8, 9),
                new SKSingle(1, 0, 9),
                new SKSingle(1, 1, 3),
                new SKSingle(1, 5, 6),
                new SKSingle(1, 6, 5),
                new SKSingle(2, 1, 8),
                new SKSingle(2, 2, 1),
                new SKSingle(2, 5, 7),
                new SKSingle(2, 7, 6),
                new SKSingle(3, 2, 5),
                new SKSingle(3, 4, 6),
                new SKSingle(3, 6, 7),
                new SKSingle(3, 7, 4),
                new SKSingle(4, 1, 6),
                new SKSingle(4, 4, 7),
                new SKSingle(4, 5, 4),
                new SKSingle(5, 2, 4),
                new SKSingle(6, 0, 7),
                new SKSingle(6, 3, 9),
                new SKSingle(6, 5, 2),
                new SKSingle(6, 6, 6),
                new SKSingle(7, 0, 4),
                new SKSingle(7, 4, 1),
                new SKSingle(7, 6, 9),
                new SKSingle(7, 7, 8),
                new SKSingle(8, 1, 9),
                new SKSingle(8, 2, 8),
                new SKSingle(8, 7, 2),

            };

            SKSingle[] singles1 = new[]
            {
                new SKSingle(0, 0, 2),
                new SKSingle(0, 1, 4),
                new SKSingle(0, 2, 6),
                new SKSingle(0, 3, 1),
                new SKSingle(0, 4, 5),
                new SKSingle(0, 5, 8),
                new SKSingle(0, 6, 3),
                new SKSingle(0, 7, 7),
                new SKSingle(0, 8, 9),

                new SKSingle(1, 0, 9),
                new SKSingle(1, 1, 3),
                new SKSingle(1, 2, 7),
                new SKSingle(1, 3, 4),
                new SKSingle(1, 4, 2),
                new SKSingle(1, 5, 6),
                new SKSingle(1, 6, 5),
                new SKSingle(1, 7, 1),
                new SKSingle(1, 8, 8),

                new SKSingle(2, 0, 5),
                new SKSingle(2, 1, 6),
                new SKSingle(2, 2, 1),
                new SKSingle(2, 3, 3),
                new SKSingle(2, 4, 9),
                new SKSingle(2, 5, 7),
                new SKSingle(2, 6, 4),
                new SKSingle(2, 7, 6),
                new SKSingle(2, 8, 2),

                new SKSingle(3, 0, 3),
                new SKSingle(3, 1, 2),
                new SKSingle(3, 2, 5),
                new SKSingle(3, 3, 8),
                new SKSingle(3, 4, 6),
                new SKSingle(3, 5, 9),
                new SKSingle(3, 6, 7),
                new SKSingle(3, 7, 4),
                new SKSingle(3, 8, 1),

                new SKSingle(4, 0, 1),
                new SKSingle(4, 1, 6),
                new SKSingle(4, 2, 9),
                new SKSingle(4, 3, 2),
                new SKSingle(4, 4, 7),
                new SKSingle(4, 5, 4),
                new SKSingle(4, 6, 8),
                new SKSingle(4, 7, 3),
                new SKSingle(4, 8, 5),

                new SKSingle(5, 0, 8),
                new SKSingle(5, 1, 7),
                new SKSingle(5, 2, 4),
                new SKSingle(5, 3, 5),
                new SKSingle(5, 4, 3),
                new SKSingle(5, 5, 1),
                new SKSingle(5, 6, 2),
                new SKSingle(5, 7, 9),
                new SKSingle(5, 8, 6),

                new SKSingle(6, 0, 7),
                new SKSingle(6, 1, 1),
                new SKSingle(6, 2, 3),
                new SKSingle(6, 3, 9),
                new SKSingle(6, 4, 8),
                new SKSingle(6, 5, 2),
                new SKSingle(6, 6, 6),
                new SKSingle(6, 7, 5),
                new SKSingle(6, 8, 4),

                new SKSingle(7, 0, 4),
                new SKSingle(7, 1, 5),
                new SKSingle(7, 2, 2),
                new SKSingle(7, 3, 6),
                new SKSingle(7, 4, 1),
                new SKSingle(7, 5, 3),
                new SKSingle(7, 6, 9),
                new SKSingle(7, 7, 8),
                new SKSingle(7, 8, 7),

                new SKSingle(8, 0, 6),
                new SKSingle(8, 1, 9),
                new SKSingle(8, 2, 8),
                new SKSingle(8, 3, 7),
                new SKSingle(8, 4, 4),
                new SKSingle(8, 5, 5),
                new SKSingle(8, 6, 1),
                new SKSingle(8, 7, 2),
                new SKSingle(8, 8, 3),


            };

            return new SKMattrix(singles1);

            
        }

        internal static SKMattrix CreateMattrixRnd(int level)
        {
            while (true)
            {
                if (level == 104)
                {
                    return SKCreator.CreatePuzzleFromText(new string[]
                    {
                        "9....8..2",
                        "..1....3.",
                        ".6.7.....",
                        ".....2..7",
                        "..4...9..",
                        ".8.5...6.",
                        ".7...9..1",
                        "..36..5..",
                        "2...4..8.",

                    });
                }
                if (level == 105)
                {
                    return SKCreator.CreatePuzzleFromText(new string[]
                    {
                        ".....4.1.",
                        ".5..3...2",
                        "8..7..6..",
                        ".........",
                        ".3......5",
                        "6..8.....",
                        "..2.5..7.",
                        ".9...6..3",
                        "4..1..8..",
                    });
                }

                var puzzle = SKCreator.CreateRandomMattrixShuffle(SKCreator.GenerateSeedMattrix(), (decimal) level);
                
                return puzzle;
                var testPuzzle = puzzle.HardCopy();
                var solver = new SKSolver(testPuzzle, null);
                solver.Solve();
                if (testPuzzle.IsSolved)
                    return puzzle;
            }
        }
    }
}
