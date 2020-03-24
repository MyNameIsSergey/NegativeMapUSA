using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP1
{
    class Comparer : IComparer<IDictionaryPage>
    {
        public int Compare(IDictionaryPage x, IDictionaryPage y)
        {
            return x.QtWords + y.QtWords;
        }
    }
}
