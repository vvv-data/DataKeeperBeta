using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace View
{
    internal class SortingByNameAsc : ISorting
    {
        public Dictionary<int, string> Sort(Dictionary<int, string> dictionary)
        {
            return dictionary.OrderBy(r => r.Value).ToDictionary(c => c.Key, d => d.Value);
        }
    }
}
