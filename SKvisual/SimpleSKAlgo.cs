using System;
using System.Collections.Generic;
using System.Linq;

namespace SK
{
    public static class SimpleSKAlgo
    {
        public static IEnumerable<SKSingle> SinglePossiableNumber(SKMattrix mattrix, Action<SKSingle, string> highlight, Action waitForUser)
        {
            List<SKSingle> changed = new List<SKSingle>();
            foreach (var s in mattrix.AllSingles.Where(s => !s.IsNumberSet && s.Possible.Count == 1))
            {

                //highlight(s, "SinglePossiableNum:" + s.Number);
                changed.Add(s);
                waitForUser();
                s.SetNumber(s.Possible[0]);
            }
            return changed;
        }

        public static IEnumerable<SKSingle> SingleNumberInCube(SKMattrix mattrix, Action<SKSingle, string> highlight, Action waitForUser)
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

                        //highlight(s, "SingleNumInCube:" + num);
                        changed.Add(s);
                        waitForUser();
                        s.SetNumber(num);
                    }
                }
            }

            return changed;

        }

        public static IEnumerable<SKSingle> SingleNumberInRow(SKMattrix mattrix, Action<SKSingle, string> highlight, Action waitForUser)
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

                        //highlight(s, "SingleNumInRow:" + num);
                        changed.Add(s);
                        waitForUser();
                        s.SetNumber(num);
                    }
                }
            }

            return changed;

        }



        #region Naked/Hidden vectors

        public static void NakedVectorInCollection(string funcDescription, SinglesCollection collection, int vectorLen, Action<SKSingle, string, IEnumerable<SKSingle>> highlight, Action waitForUser, List<SKSingle> changed)
        {
            // optimization: if the number of unresolved singles is less or equal to vector length, ignore iteration            
            if (collection.UnresolvedSinglesCount <= vectorLen)
                return;

            // STEP 1: get all numbers , with that apears 'vectorLen' time in the collection
            var hiddenVectorCandidates = SKMattrix.AllNumbers.Select(num =>
            {
                return
                    new
                    {
                        Num = num,
                        Singles = collection.Where(s => !s.IsNumberSet && s.Possible.Contains(num) ),
                        Locations =
                            collection.Where(s => !s.IsNumberSet && s.Possible.Contains(num))
                                .Aggregate(string.Empty, (c, n) => c + "," + n.RowId)
                    };
            //}).Where(num => num.Singles.Any()).ToList();
            }).Where(num => !string.IsNullOrEmpty(num.Locations)).ToList();

            // STEP 2: Create a powerset and filter only the powerset og 'vectorLen' size sets
            var vectors = GetPowerSet(hiddenVectorCandidates).Where(set => set.Count() == vectorLen);

            // STEP 3: for every set, create a new object that pre-process and select all single in one collection
            var v1 = vectors.Select(v =>
            {
                return new
                {
                    JoinSingles = v.SelectMany(n => n.Singles).Distinct(),
                    Numbers = v.Select(n => n.Num),
                    NumbersKey = v.Select(n => n.Num).Aggregate(string.Empty, (c, n) => c + "," + n)

                };
            }).ToList();

            // STEP 4: Filter all sets that contains 'vectorLen' numbers in 'vectorLen' locations... this is the final Hidden/naked vectors in a collection!
            var v2 = v1.Where(v => v.JoinSingles.Count() == vectorLen);

            foreach (var vector in v2)
            {
                //highlight(vector.JoinSingles.First(), funcDescription + ":" + vector.NumbersKey, collection);
                waitForUser();

                foreach (var single in vector.JoinSingles)
                {
                    changed.Add(single);                    
                    if (!single.RemoveAllExcept(vector.Numbers))
                        changed.RemoveAt(changed.Count - 1);

                }
                foreach (var single in collection.Where(s => !s.IsNumberSet && !vector.JoinSingles.Contains(s)))
                {
                    //highlight(single, funcDescription + ":" + vector.NumbersKey, collection);
                    changed.Add(single);
                    //waitForUser();
                    if (!single.RemoveFromPossiable(vector.Numbers))
                        changed.RemoveAt(changed.Count - 1);
                }

            }
        }

        public static IEnumerable<SKSingle> HiddenVectorInCube(SKMattrix mattrix, Action<SKSingle, string, IEnumerable<SKSingle>> highlight, Action waitForUser)
        {
            List<SKSingle> changed = new List<SKSingle>();
            foreach (int vectorLen in new[] { 2, 3, 4 })
            {
                foreach (var cube in mattrix.Cubes.Values)
                {

                    NakedVectorInCollection("HiddenVectorInCube", cube, vectorLen, highlight, waitForUser, changed);
                }
            }


            return changed;

        }

        public static IEnumerable<SKSingle> HiddenVectorInRow(SKMattrix mattrix, Action<SKSingle, string, IEnumerable<SKSingle>> highlight, Action waitForUser)
        {
            List<SKSingle> changed = new List<SKSingle>();
            foreach (int vectorLen in new[] { 2, 3, 4 })
            {
                foreach (var row in mattrix.Rows.Values)
                {
                    NakedVectorInCollection("HiddenVectorInRow", row, vectorLen, highlight, waitForUser, changed);

                }
            }


            return changed;

        }

        public static IEnumerable<SKSingle> HiddenVectorInCol(SKMattrix mattrix, Action<SKSingle, string, IEnumerable<SKSingle>> highlight, Action waitForUser)
        {
            List<SKSingle> changed = new List<SKSingle>();
            foreach (int vectorLen in new[] { 2, 3, 4 })
            {
                foreach (var col in mattrix.Cols.Values)
                {
                    NakedVectorInCollection("HiddenVectorInCol", col, vectorLen, highlight, waitForUser, changed);

                }
            }


            return changed;

        }
        #endregion

       

        public struct HiddenPairs
        {
            public IEnumerable<SKSingle> JoinSingles;
            public IEnumerable<int> Numbers;
            public string NumbersKey;

            public string CreateColsKey()
            {
                return JoinSingles.Aggregate(string.Empty, (c, n) => c + "," + n.ColId);
            }
            public string CreateRowsKey()
            {
                return JoinSingles.Aggregate(string.Empty, (c, n) => c + "," + n.RowId);
            }

        }

        public static IEnumerable<SKSingle> BoxLineReduction(SKMattrix mattrix,
            Action<SKSingle, string, IEnumerable<SKSingle>> highlight, Action waitForUser)
        {
            List<SKSingle> changes = new List<SKSingle>();
            foreach(var r in mattrix.Rows.Values)
                changes.AddRange(BoxLineReductionInCollection(r,mattrix.Cubes,highlight,waitForUser));
            foreach (var r in mattrix.Cols.Values)
                changes.AddRange(BoxLineReductionInCollection(r, mattrix.Cubes, highlight, waitForUser));
            return changes;
        }

        private static IEnumerable<SKSingle> BoxLineReductionInCollection(SinglesCollection vector, Dictionary<int,SKCube> cubes,
            Action<SKSingle, string, IEnumerable<SKSingle>> highlight, Action waitForUser )
        {
            List<SKSingle> changes = new List<SKSingle>();

            foreach (int num in SKMattrix.AllNumbers)
            {
                var allNumsInvectorGroup =
                    vector.Where(s => !s.IsNumberSet & s.Possible.Contains(num)).GroupBy(s => s.CubeId);
                if (allNumsInvectorGroup.Count() == 1)
                {
                    // remove num from all other singles in cube
                    foreach (var single in cubes[allNumsInvectorGroup.First().Key].Except(allNumsInvectorGroup.First()))
                    {
                        //highlight(single, funcDescription + ":" + vector.NumbersKey, collection);
                        //waitForUser();
                        if (single.RemoveFromPossiable(Enumerable.Range(num,1)))
                            changes.Add(single);

                    }
                }
            }

            return changes;
        }



        public static IEnumerable<IEnumerable<T>> GetPowerSet<T>(List<T> list)
        {
            return from m in Enumerable.Range(0, 1 << list.Count)
                   select
                       from i in Enumerable.Range(0, list.Count)
                       where (m & (1 << i)) != 0
                       select list[i];
        }
    }
}

