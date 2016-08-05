using System;
using System.Collections.Generic;
using System.Linq;
using SK;

namespace SKvisual
{
    public static class AdvanceSKAlgo
    {

        public static IEnumerable<SKSingle> SwordfishAlgo(SKMattrix mattrix,
            Action<SKSingle, string, IEnumerable<SKSingle>> highlight,
            Action waitForUser)
        {
            var changesRow =  XWingPatternEx(mattrix.Rows.Values,mattrix.Cols.Values ,3,(s)=>s.ColId,highlight,waitForUser,"Swordfish on rows");
            var changesCol =  XWingPatternEx(mattrix.Cols.Values,mattrix.Rows.Values ,3,(s)=>s.RowId,highlight,waitForUser,"Swordfish on cols");
            return Enumerable.Concat(changesCol, changesRow);
        }

        public static IEnumerable<SKSingle> XWingAlgo(SKMattrix mattrix,
            Action<SKSingle, string, IEnumerable<SKSingle>> highlight,
            Action waitForUser)
        {
            var changesRow = XWingPatternEx(mattrix.Rows.Values, mattrix.Cols.Values, 2, (s) => s.ColId, highlight, waitForUser, "Swordfish on rows");
            var changesCol = XWingPatternEx(mattrix.Cols.Values, mattrix.Rows.Values, 2, (s) => s.RowId, highlight, waitForUser, "Swordfish on cols");
            return Enumerable.Concat(changesCol, changesRow);
        }


        private static IEnumerable<SKSingle> XWingPatternEx(IEnumerable<SinglesCollection> testVectors, IEnumerable<SinglesCollection> effectedVectors , int VectorSize, Func<SKSingle,int> getSingleLocation ,Action<SKSingle, string, IEnumerable<SKSingle>> highlight,
            Action waitForUser, string algoDescription)
        {
            // VectorSize 3 i Swordfisj algo. size 2 in X-Wing pattern 
            

            List<SKSingle> changed = new List<SKSingle>();

            
            foreach (int num in SKMattrix.AllNumbers)
            {
                // STEP 1: get all locations of num in the collections , that apear max VectorSize time 
                var rawCandidates = testVectors.Select(r =>
                    new
                    {
                        Num = num,
                        Singles = r.Where(s => !s.IsNumberSet && s.Possible.Contains(num)),
                        Locations =
                            r.Where(s => !s.IsNumberSet && s.Possible.Contains(num))
                                .Aggregate(string.Empty, (c, n) => c + "," + getSingleLocation(n))
                    })
                    .Where(n => !string.IsNullOrEmpty(n.Locations) && n.Singles.Count() >= 2 && n.Singles.Count() <= VectorSize)
                    .ToList();

                var candidatesPowerSet = SimpleSKAlgo.GetPowerSet(rawCandidates).Where(set => set.Count() == VectorSize);
                foreach (var candidateSet in candidatesPowerSet)
                {
                    // STEP 2: Get all unique Col for canindates vector
                    var swordFishSingles = candidateSet.SelectMany(c => c.Singles);
                    var swordfishColIdList = swordFishSingles.Select(c => getSingleLocation(c)).Distinct();

                    // if it apears in exactly 3 unique cols, we have a possiable algo affect on the puzzle
                    if (swordfishColIdList.Count() == VectorSize)
                    {
                        //bingo! now remove 'num' from cells not in swordfish pattern
                        foreach (var colId in swordfishColIdList)
                        {
                            var wordfishSinglesInCol = swordFishSingles.Where(s => getSingleLocation(s) == colId);

                            foreach (
                                var single in
                                    effectedVectors.ElementAt(colId)
                                        .Where(s => !s.IsNumberSet)
                                        .Except(wordfishSinglesInCol))
                            {

                                if (single.RemoveFromPossiable(Enumerable.Repeat(num, 1)))
                                {
                                    highlight(single, algoDescription + " on " + num, swordFishSingles);
                                    changed.Add(single);
                                    waitForUser();
                                }
                                //else
                                //    changed.RemoveAt(changed.Count - 1);
                            }

                        }
                    }
                }
            }


            


                


                return changed;
        }
    }
}
