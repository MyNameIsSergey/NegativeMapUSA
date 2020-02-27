using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP1
{
    class Comparer : IComparer<DictionaryPage>
    {
        public int Compare(DictionaryPage x, DictionaryPage y)
        {
            return -x.QtWords + y.QtWords;
        }
    }
}
