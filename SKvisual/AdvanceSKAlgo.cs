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
            var changesRow = XWingPatternEx(mattrix.Rows.Values, mattrix.Cols.Values, 2, (s) => s.ColId, highlight, waitForUser, "XWing on rows");
            var changesCol = XWingPatternEx(mattrix.Cols.Values, mattrix.Rows.Values, 2, (s) => s.RowId, highlight, waitForUser, "XWing on cols");
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
                                    highlight(single, algoDescription + " on " + num , swordFishSingles);
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

        private static IEnumerable<SimpleSKAlgo.HiddenPairs> LocatePairs(SinglesCollection collection)
        {
            if (collection.UnresolvedSinglesCount < 2)
                return Enumerable.Repeat(default(SimpleSKAlgo.HiddenPairs), 0);

            var hiddenVectorCandidates = SKMattrix.AllNumbers.Select(num =>
            {
                return
                    new
                    {
                        Num = num,
                        Singles = collection.Where(s => !s.IsNumberSet && s.Possible.Contains(num) && s.Possible.Count > 1),
                        Locations =
                            collection.Where(s => !s.IsNumberSet && s.Possible.Contains(num))
                                .Aggregate(string.Empty, (c, n) => c + "," + n.RowId)
                    };
                //}).Where(num => num.Singles.Any()).ToList();
            }).Where(num => !string.IsNullOrEmpty(num.Locations)).ToList();

            var vectors = SimpleSKAlgo.GetPowerSet(hiddenVectorCandidates).Where(set => set.Count() == 2);

            var v1 = vectors.Select(v =>
            {
                return new SimpleSKAlgo.HiddenPairs
                {
                    JoinSingles = v.SelectMany(n => n.Singles).Distinct(),
                    Numbers = v.Select(n => n.Num),
                    NumbersKey = v.Select(n => n.Num).Aggregate(string.Empty, (c, n) => c + "," + n)

                };
            }).ToList();

            var v2 = v1.Where(v => v.JoinSingles.Count() == 2 && v.JoinSingles.Select(s => s.CubeId).Distinct().Count() == 2);
            return v2;
        }
        public static IEnumerable<SKSingle> UniqueRegtangle(SKMattrix mattrix, Action<SKSingle, string, IEnumerable<SKSingle>> highlight, Action waitForUser)
        {
            // in cols
            var c1 = mattrix.Cols.SelectMany(c => LocatePairs(c.Value)).GroupBy(n => n.NumbersKey);
            var c2 = c1.Where(g => g.Count() == 2 && g.First().CreateRowsKey() == g.ElementAt(1).CreateRowsKey());
            foreach (var c3 in c2)
            {
                var collection = c3.SelectMany(c4 => c4.JoinSingles);
                string text = "Unique rectabgle on:" + c3.Key;
                highlight(collection.First(), text, collection);
                waitForUser();
            }

            // in cols
            var r1 = mattrix.Rows.SelectMany(c => LocatePairs(c.Value)).GroupBy(n => n.NumbersKey);
            var r2 = c1.Where(g => g.Count() == 2 && g.First().CreateRowsKey() == g.ElementAt(1).CreateColsKey());
            foreach (var r3 in r2)
            {
                var collection = r3.SelectMany(c4 => c4.JoinSingles);
                string text = "Unique rectabgle on:" + r3.Key;
                highlight(collection.First(), text, collection);
                waitForUser();
            }

            return Enumerable.Empty<SKSingle>();
        }
    }
}
