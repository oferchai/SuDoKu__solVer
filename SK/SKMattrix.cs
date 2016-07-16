using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SK
{

    public class SKMattrix
    {
        public static int[] LocationIds = { 0, 1, 2, 3, 4, 5, 6, 7, 8 };

        public static int[] AllNumbers = { 1, 2, 3, 4, 5, 6, 7, 8 ,9};

        public SKMattrix(IEnumerable<SKSingle> singles)
        {
            Cols = new Dictionary<int, SKCol>();
            Rows = new Dictionary<int, SKRow>();
            Cubes = new Dictionary<int, SKCube>();

            foreach (int x in LocationIds)
            {
                Cols.Add(x, new SKCol(x));
                Rows.Add(x, new SKRow(x));
                Cubes.Add(x, new SKCube());

            }

            foreach (int col in LocationIds)
            {
                foreach (int row in LocationIds)
                {
                    // get or build a single
                    var single = singles.FirstOrDefault(s => s.RowId == row && s.ColId == col) ?? new SKSingle(row, col, null);
                    single.SetLocation(Rows[row], Cols[col], Cubes[single.CubeId]);
                    int cubeId = ((row/3)*3) + (col/3);
                    Cols[col].InsertSingle(single);
                    Rows[row].InsertSingle(single);
                    Cubes[cubeId].InsertSingle(single);


                }
            }

            foreach(var s in AllSingles)
            {
                s.SetPossiableNumbers();
            }

        }

        public IEnumerable<SKSingle> AllSingles { get { return Cols.Values.SelectMany(c => c); } }

        public bool IsSolved
        {
            get { return AllSingles.All(s => s.IsNumberSet); }
        }

        public Dictionary<int, SKCol> Cols;
        public Dictionary<int, SKRow> Rows;
        public Dictionary<int, SKCube> Cubes;

        public void PrintMattrix()
        {
            string mattrix = Rows.Aggregate(string.Empty,
                (c, n) =>
                    c + (n.Key % 3 == 0 ? "|---|---|---|" + Environment.NewLine : string.Empty) +
                    n.Value.Aggregate(string.Empty, (cc, nn) => cc + (nn.ColId % 3 == 0 ? "|" : string.Empty) + nn.NumberStr) + "|" + Environment.NewLine);
            Console.WriteLine(mattrix);
        }
    }

    public class SKCol : IEnumerable<SKSingle>
    {
        public SKCol(int colId)
        {
            Singles = new Dictionary<int, SKSingle>();
            ColId = colId;
        }

        int ColId;
        public SKCol NextLine { get; private set; }
        public SKCol PrevLine { get; private set; }
        public Dictionary<int, SKSingle> Singles { get; private set; }
        public void InsertSingle(SKSingle single)
        {
            if (single.ColId == ColId)
            {
                if (Singles.ContainsKey(single.RowId))
                    Singles[single.RowId] = single;
                else
                    Singles.Add(single.RowId, single);
            }
        }
        public IEnumerator<SKSingle> GetEnumerator()
        {
            return Singles.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public string ToString()
        {
            return Singles.Values.Aggregate(string.Empty, (s, v) => s + v.ToString() + ",");
        }

    }

    public class SKRow : IEnumerable<SKSingle>
    {

        public SKRow(int rowId)
        {
            Singles = new Dictionary<int, SKSingle>();
            RowId = rowId;
        }
        int RowId;

        public SKRow NextRow { get; private set; }
        public SKRow PrevRow { get; private set; }

        public Dictionary<int, SKSingle> Singles { get; private set; }
        public void InsertSingle(SKSingle single)
        {
            if (single.RowId == RowId)
            {
                if (Singles.ContainsKey(single.ColId))
                    Singles[single.ColId] = single;
                else
                    Singles.Add(single.ColId, single);
            }
        }

        public IEnumerator<SKSingle> GetEnumerator()
        {
            return Singles.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public string ToString()
        {
            return Singles.Values.Aggregate(string.Empty, (s, v) => s + v.ToString() + ",");
        }
    }

    public class SKCube : IEnumerable<SKSingle>
    {
        public SKCube()
        {
            Singles = new Dictionary<string, SKSingle>(9);
        }
        int CubeId;
        public Dictionary<string, SKSingle> Singles { get; private set; }

        public void InsertSingle(SKSingle single)
        {
            string key = single.GetUniqueKey();
            if (Singles.ContainsKey(key))
                Singles[key] = single;
            else
                Singles.Add(key, single);

        }

        public string ToString()
        {
            return Singles.Values.Aggregate(string.Empty, (s, v) => s + v.ToString() + ",");
        }
        
        public IEnumerator<SKSingle> GetEnumerator()
        {
            return Singles.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


    }

    public class SKSingle
    {

        public SKSingle(int row, int col, int? number)
        {
            RowId = row;
            ColId = col;
            if (number.HasValue)
                Number = number;
        }
        public List<int> Possible = new List<int>();
        public int? Number;
        public int RowId;
        public int ColId;
        public SKCube Cube { get; private set; }

        public string GetUniqueKey()
        {
            return CreateUniqueKey(ColId, RowId);
        }

        public string ToString()
        {
            return string.Format("[{0},{1}]:{2} ", RowId, ColId, IsNumberSet ? Number + "" : "?");
        }
        public int CubeId
        {
            get
            {
                return ((RowId / 3) * 3) + (ColId / 3);
            }
        }

        public string NumberStr { get { return Number.HasValue ? Number + "" : "?"; } }
        public void SetPossiableNumbers()
        {
            if (IsNumberSet)
            {
                Possible.Clear();
                return;
            }


            // if number exists in row
            var rowNumbers = SkRow.Where(r => r.IsNumberSet).Select(s=>s.Number.Value);
            var colNumbers = SkCol.Where(r => r.IsNumberSet).Select(s => s.Number.Value);
            var cubeNumbers = SkCube.Where(r => r.IsNumberSet).Select(s => s.Number.Value);
            var combineSet = rowNumbers.Union(colNumbers).Union(cubeNumbers);
            
            Possible = new List<int>( SKMattrix.AllNumbers.Select(i => i).Except(combineSet) );


        }

        public static string CreateUniqueKey(int col,int row)
        {
            return "R:" + row + ",C:" + col;
        }

      
        public void SetNumber(int num)
        {
            Number = num;
            SetPossiableNumbers();

            foreach(var s in SkCube)
                s.SetPossiableNumbers();
            foreach (var s in SkCol)
                s.SetPossiableNumbers();
            foreach (var s in SkRow)
                s.SetPossiableNumbers();

        }
        public bool IsNumberSet { get { return Number.HasValue; } }

        public void SetLocation(SKRow skRow, SKCol skCol, SKCube skCube)
        {
            SkRow = skRow;
            SkCol = skCol;
            SkCube = skCube;
        }

        public SKCube SkCube { get; set; }

        public SKCol SkCol { get; set; }

        public SKRow SkRow { get; set; }
    }

}
