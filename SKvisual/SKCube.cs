using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SK
{
    public class SKCube : SinglesCollection
    {
        public readonly int CubeId;

        public SKCube(int cubeId)
        {
            CubeId = cubeId;
            Singles = new Dictionary<string, SKSingle>(9);
        }

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


    }
}