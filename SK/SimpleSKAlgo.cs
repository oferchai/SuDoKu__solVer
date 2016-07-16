using System.Collections.Generic;
using System.Linq;

namespace SK
{
    public static class SimpleSKAlgo
    {
        public static IEnumerable<SKSingle> SinglePossiableNumber(SKMattrix mattrix)
        {
            List<SKSingle> changed = new List<SKSingle>();
            foreach (var s in mattrix.AllSingles.Where(s=>!s.IsNumberSet && s.Possible.Count==1) ) 
            {
                s.SetNumber(s.Possible[0]);
                changed.Add(s);
            }
            return changed;
        }

        public static IEnumerable<SKSingle> SingleNumberInCube(SKMattrix mattrix)
        {
            List<SKSingle> changed = new List<SKSingle>();

            foreach (var c in mattrix.Cubes.Values)
            {
                foreach (int num in SKMattrix.AllNumbers)
                {
                    var singles = c.Where(s => !s.IsNumberSet && s.Possible.Contains(num));
                    if (singles.Count() == 1)
                    {
                        var s = singles.First();
                        s.SetNumber(num);
                        changed.Add(s);
                    }
                }                
            }

            return changed;

        }
        
        public static IEnumerable<SKSingle> SingleNumberInRow(SKMattrix mattrix)
        {
            List<SKSingle> changed = new List<SKSingle>();

            foreach (var c in mattrix.Rows.Values)
            {
                foreach (int num in SKMattrix.AllNumbers)
                {
                    var singles = c.Where(s => !s.IsNumberSet && s.Possible.Contains(num));
                    if (singles.Count() == 1)
                    {
                        var s = singles.First();
                        s.SetNumber(num);
                        changed.Add(s);
                    }
                }
            }

            return changed;

        }

    }
}
