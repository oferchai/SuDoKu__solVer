using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK
{
    class Program
    {
        static void Main(string[] args)
        {
            SKSingle[] singles = new[]
            {
                new SKSingle(0, 5, 4),
                new SKSingle(1, 2, 9),
                new SKSingle(1, 7, 5),
                new SKSingle(2, 1, 2),
                new SKSingle(2, 3, 5),
                new SKSingle(2, 6, 7),
                new SKSingle(3, 2, 3),
                new SKSingle(3, 4, 8),
                new SKSingle(3, 6, 4),
                new SKSingle(3, 8, 7),
                new SKSingle(4, 3, 6),
                new SKSingle(4, 6, 8),
                new SKSingle(5, 0, 9),
                new SKSingle(5, 6, 3),
                new SKSingle(6, 2, 4),
                new SKSingle(6, 3, 3),
                new SKSingle(6, 4, 1),
                new SKSingle(6, 8, 5),
                new SKSingle(7, 1, 5),
                new SKSingle(7, 5, 9),
                new SKSingle(7, 7, 1),
                new SKSingle(7, 8, 4), 
                new SKSingle(8, 3, 7),
                new SKSingle(8, 6, 9),
                new SKSingle(8, 7, 8),
                new SKSingle(8, 8, 3),

            };
            var sk = new SKMattrix(singles);
            sk.PrintMattrix();
            bool changed;
            do
            {
                changed = false;
                changed |= IsMatrixChanged("SinglePossiableNumber",SimpleSKAlgo.SinglePossiableNumber(sk),sk);
                changed |= IsMatrixChanged("SingleNumberInCube", SimpleSKAlgo.SingleNumberInCube(sk),sk);
                changed |= IsMatrixChanged("SingleNumberInRow", SimpleSKAlgo.SingleNumberInRow(sk),sk);
            } while (!sk.IsSolved && changed );

        
       }
         
        private static bool IsMatrixChanged(string algoName, IEnumerable<SKSingle> changedSingles, SKMattrix sk)
        {
            Console.Out.WriteLine("=== " + algoName + " ===");
            bool mattrixChanged = changedSingles.Count() > 0;
            
            foreach (var single in  changedSingles)
            {
                Console.Out.WriteLine(single.ToString());
            }

            if (mattrixChanged)
                sk.PrintMattrix();

            return mattrixChanged;
        }
    }
}
