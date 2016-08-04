using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SK;

namespace SKvisual
{
    public static class AdvanceSKAlgo
    {
        public static IEnumerable<SKSingle> SwordfishAlgo(SKMattrix mattrix, Action<SKSingle, string, IEnumerable<SKSingle>> highlight,
            Action waitForUser)
        {
            List<SKSingle> changed = new List<SKSingle>();
            // STEP 1: get all numbers , with that apears 'vectorLen' time in the collection


            foreach (int num in SKMattrix.AllNumbers)
            {
                var candidates = mattrix.Rows.Select(r => r.Value as SinglesCollection).Select(r =>
                    new
                    {
                        Num = num,
                        Singles = r.Where(s => !s.IsNumberSet && s.Possible.Contains(num)),
                        Locations =
                            r.Where(s => !s.IsNumberSet && s.Possible.Contains(num))
                                .Aggregate(string.Empty, (c, n) => c + "," + n.ColId)
                    })
                    .Where(n => !string.IsNullOrEmpty(n.Locations) && n.Singles.Count() >= 2 && n.Singles.Count() <= 3)
                    .ToList();

                var swordFishSingles = candidates.SelectMany(c => c.Singles);
                var swordfishColIdList = swordFishSingles.Select(c => c.ColId).Distinct();
                if (swordfishColIdList.Count() == 3)
                {
                    //bingo! now remove 'num' from cells not in swordfish pattern
                    foreach (var colId in swordfishColIdList)
                    {
                        var wordfishSinglesInCol = swordFishSingles.Where(s => s.ColId == colId);

                        foreach (var single in mattrix.Cols[colId].Where(s => !s.IsNumberSet).Except(wordfishSinglesInCol))
                        {
                            highlight(single, "Swordfish on " + num , swordFishSingles);
                            changed.Add(single);
                            waitForUser();
                            if (!single.RemoveFromPossiable(Enumerable.Repeat(num, 1)))
                                changed.RemoveAt(changed.Count - 1);
                        }

                    }
                }
            }


            //}


                


                return changed;
        }
    }
}
