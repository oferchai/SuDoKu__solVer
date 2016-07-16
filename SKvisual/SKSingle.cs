using System;
using System.Collections.Generic;
using System.Linq;

namespace SK
{
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
        

        public string GetUniqueKey()
        {
            return CreateUniqueKey(ColId, RowId);
        }

        public string ToString()
        {
            return string.Format("[{0},{1}]:{2} ", RowId, ColId, IsNumberSet ? Number + "" : "[" + PossibleKey + "]");
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

            //if (RowId == 0 && ColId == 6)
            //    int x = 1;
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
            if(IsNumberSet)
                throw new Exception("Single " + ToString() + " is soled, cannot set new number");
            Number = num;
            SetPossiableNumbers();

            foreach(var s in SkCube)
                s.RemoveFromPossiable(Enumerable.Repeat(num,1));
            foreach (var s in SkCol)
                s.RemoveFromPossiable(Enumerable.Repeat(num, 1));
            foreach (var s in SkRow)
                s.RemoveFromPossiable(Enumerable.Repeat(num, 1));

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

        public string PossibleKey
        {
            get { return Possible.Aggregate(String.Empty, (c, v) => c + "," + v); }
        }
        public bool ExistsInPossiable(IEnumerable<int> numbers)
        {
            foreach (var num in numbers)
            {
                if( Possible.Contains(num))
                    return true;
            }
            return false;
        }

        public bool RemoveFromPossiable(IEnumerable<int> numbers)
        {
            bool res = false;
            foreach (var num in numbers)
            {
                res |= Possible.Remove(num);
            }
            return res;
        }

        public bool RemoveAllExcept(IEnumerable<int> numbers)
        {
            return RemoveFromPossiable( Possible.Except(numbers).ToList() );
        }

       
    }

    public class SKSingleEqualityComparer : IEqualityComparer<SKSingle>
    {
        public bool Equals(SKSingle x, SKSingle y)
        {
            if (x.RowId != y.RowId)
                return false;
            if (y.ColId == x.ColId)
                return false;
            return true;
        }

        public int GetHashCode(SKSingle obj)
        {
            return obj.ColId.GetHashCode() ^ obj.RowId.GetHashCode();
        }
    }
}