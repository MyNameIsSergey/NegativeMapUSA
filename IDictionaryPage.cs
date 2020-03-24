using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP1
{
    interface IDictionaryPage
    {
        float CheckSentence(Sentence sentence);
        int QtWords { get; }
        void Add(string[] key, float value);
    }
}
