using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SK
{
    public class SKRow : SinglesCollection
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
        public int UnresolvedSinglesCount
        {
            get { return Singles.Values.Count(s => !s.IsNumberSet); }
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
}