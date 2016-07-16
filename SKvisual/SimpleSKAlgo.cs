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

                highlight(s, "SinglePossiableNum:" + s.Number);
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

                        highlight(s, "SingleNumInCube:" + num);
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

                        highlight(s, "SingleNumInRow:" + num);
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
                highlight(vector.JoinSingles.First(), funcDescription + ":" + vector.NumbersKey, collection);
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

        public static IEnumerable<SKSingle> UniqueRegtangle(SKMattrix mattrix, Action<SKSingle, string, IEnumerable<SKSingle>> highlight, Action waitForUser)
        {
            // in cols
            var c1 = mattrix.Cols.SelectMany(c => LocatePairs(c.Value)).GroupBy(n => n.NumbersKey);
            var c2 = c1.Where(g=>g.Count()==2 && g.First().CreateRowsKey() == g.ElementAt(1).CreateRowsKey() );
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

        private static IEnumerable<HiddenPairs> LocatePairs(SinglesCollection collection)
        {
            if (collection.UnresolvedSinglesCount < 2)
                return Enumerable.Repeat(default(HiddenPairs), 0);

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

            var vectors = GetPowerSet(hiddenVectorCandidates).Where(set => set.Count() == 2);

            var v1 = vectors.Select(v =>
            {
                return new HiddenPairs
                {
                    JoinSingles = v.SelectMany(n => n.Singles).Distinct(),
                    Numbers = v.Select(n => n.Num),
                    NumbersKey = v.Select(n => n.Num).Aggregate(string.Empty, (c, n) => c + "," + n)

                };
            }).ToList();

            var v2 = v1.Where(v => v.JoinSingles.Count() == 2 && v.JoinSingles.Select(s => s.CubeId).Distinct().Count() == 2);
            return v2;
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

