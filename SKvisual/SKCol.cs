using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SK
{
    public interface SinglesCollection : IEnumerable<SKSingle>
    {
        int UnresolvedSinglesCount { get; }
    }

    public class SKCol : SinglesCollection
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

        public int UnresolvedSinglesCount
        {
            get { return Singles.Values.Count(s => !s.IsNumberSet); }
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