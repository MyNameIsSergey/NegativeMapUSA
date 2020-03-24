using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP1
{
    class DictionaryPage : IDictionaryPage
    {
        public int QtWords { get; private set; }
        private Dictionary<string, float> Words { get; set; }
        public DictionaryPage(int qt)
        {
            QtWords = qt;
            Words = new Dictionary<string, float>();
        }
        public void Add(string[] key, float value)
        {
            
            Words.Add(string.Join(" ", key), value);
        }
        public float CheckSentence(Sentence sentence)
        {
            float result = 0;
            if (sentence.Length < QtWords)
            {
                return 0;
            }
            for (int i = 0; i + QtWords < sentence.Length; i++)
            {
                string s = sentence.Words[i];
                for(int j = 1; j < QtWords; j++)
                {
                    s += ' ' + sentence.Words[i + j];
                }
                if (Words.ContainsKey(s))
                    result += Words[s];
            }
            return result;
        }
    }
}
