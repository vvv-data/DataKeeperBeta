using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace View
{
    internal class SortingByDateDesc : ISorting
    {
        public Dictionary<int, string> Sort(Dictionary<int, string> dictionary)
        {
            return dictionary.OrderByDescending(r => r.Key).ToDictionary(c => c.Key, d => d.Value);

        }
    }
}
