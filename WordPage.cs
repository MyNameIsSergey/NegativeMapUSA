using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP1
{
    class WordPage : IDictionaryPage
    {
        public int QtWords => 1;
        Dictionary<string, float> dictionary = new Dictionary<string, float>(); 
        public void Add(string[] key, float value)
        {
            dictionary.Add(key[0], value);
        }

        public float CheckSentence(Sentence sentence)
        {
            float result = 0;
            for (int i = 0; i < sentence.Length; i++)
                if (!sentence.Checked[i] && dictionary.TryGetValue(sentence.Words[i], out float value)) 
                {
                    result += value;
                    sentence.Checked[i] = true;
                }
            return result;
        }
    }
}
