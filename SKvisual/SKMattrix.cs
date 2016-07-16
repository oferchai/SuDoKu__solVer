using System;
using System.Collections.Generic;
using System.Linq;

namespace SK
{

    public class SKMattrix
    {
        public static int[] LocationIds = { 0, 1, 2, 3, 4, 5, 6, 7, 8 };

        public static int[] AllNumbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };


        public SKMattrix(IEnumerable<SKSingle> singles)
        {
            Cols = new Dictionary<int, SKCol>();
            Rows = new Dictionary<int, SKRow>();
            Cubes = new Dictionary<int, SKCube>();

            foreach (int x in LocationIds)
            {
                Cols.Add(x, new SKCol(x));
                Rows.Add(x, new SKRow(x));
                Cubes.Add(x, new SKCube(x));

            }

            foreach (int col in LocationIds)
            {
                foreach (int row in LocationIds)
                {
                    // get or build a single
                    var single = singles.FirstOrDefault(s => s.RowId == row && s.ColId == col) ?? new SKSingle(row, col, null);
                    single.SetLocation(Rows[row], Cols[col], Cubes[single.CubeId]);
                    int cubeId = ((row / 3) * 3) + (col / 3);
                    Cols[col].InsertSingle(single);
                    Rows[row].InsertSingle(single);
                    Cubes[cubeId].InsertSingle(single);

                }
            }

            foreach (var s in AllSingles)
            {
                s.SetPossiableNumbers();
            }

        }

        public SKMattrix HardCopy()
        {
            return new SKMattrix(AllSingles.Where(s => s.IsNumberSet));
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

        public bool ValidateMattrix(ref string result)
        {
            result = string.Empty;

            // test all cubes 
            foreach (var cube in Cubes)
            {
                var doubleNumbers = cube.Value.Where(c => c.IsNumberSet).GroupBy(s => s.Number.Value).Where(g => g.Count() > 1);
                if (doubleNumbers.Any())
                {
                    result += Environment.NewLine + "Number:" + doubleNumbers.First().Key + " Appear " +
                              doubleNumbers.First().Count() + "times in cube " + cube.Key;
                    return false;
                }

                var numbersDistinct = cube.Value.SelectMany(
                    s => s.IsNumberSet ? Enumerable.Repeat(s.Number.Value, 1) : s.Possible).Distinct();

                if (numbersDistinct.Count() < 9)
                {
                    result += Environment.NewLine + "cube:" + cube.Key + " missing:"
                    + (9-numbersDistinct.Count()) +
                    " numbers";
                    return false;

                }


            }
            // test all rows 
            foreach (var row in Rows)
            {
                var doubleNumbers = row.Value.Where(c => c.IsNumberSet).GroupBy(s => s.Number.Value).Where(g => g.Count() > 1);
                if (doubleNumbers.Any())
                {
                    result += Environment.NewLine + "Number:" + doubleNumbers.First().Key + " Appear " +
                              doubleNumbers.First().Count() + "times in row " + row.Key;
                    return false;
                }

                var numbersDistinct = row.Value.SelectMany(
    s => s.IsNumberSet ? Enumerable.Repeat(s.Number.Value, 1) : s.Possible).Distinct();

                if (numbersDistinct.Count() < 9)
                {
                    result += Environment.NewLine + "row:" + row.Key + " missing:"
                    + (9-numbersDistinct.Count()) +
                    " numbers";
                    return false;

                }


            }
            // test all cols 
            foreach (var col in Cols)
            {
                var doubleNumbers = col.Value.Where(c => c.IsNumberSet).GroupBy(s => s.Number.Value).Where(g => g.Count() > 1);
                if (doubleNumbers.Any())
                {
                    result += Environment.NewLine + "Number:" + doubleNumbers.First().Key + " Appear " +
                              doubleNumbers.First().Count() + "times in col " + col.Key;
                    return false;
                }

                var numbersDistinct = col.Value.SelectMany(
                    s => s.IsNumberSet ? Enumerable.Repeat(s.Number.Value, 1) : s.Possible).Distinct();

                if (numbersDistinct.Count() < 9)
                {
                    result += Environment.NewLine + "col:" + col.Key + " missing:"
                    + (9-numbersDistinct.Count()) +
                    " numbers";
                    return false;

                }


            }

            // test possible array 
            foreach (var single in AllSingles.Where(s => !s.IsNumberSet).Where(s => !s.Possible.Any()))
            {
                result += Environment.NewLine + "Single " + single.ToString() + " is not set but has no possiables ";
                return false;

            }

            return true;

        }
    }
}
