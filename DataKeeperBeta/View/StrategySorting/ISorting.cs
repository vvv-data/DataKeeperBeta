using System.Collections.Generic;



namespace View
{
    interface ISorting
    {
        Dictionary<int, string> Sort(Dictionary<int, string> notes);
    }
}
