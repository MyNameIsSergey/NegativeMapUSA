using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP1
{
    struct DictionaryPage
    {
        public int QtWords { get => Words.First().Key.Length; }
        public Dictionary<string[], float> Words { get; set; }
        public float CheckSentence(Sentence sentence)
        {
            float result = 0;
            if (sentence.Length < QtWords)
            {
                return 0;
            }
            for (int i = 0; i + QtWords < sentence.Length; i++)
            {
                foreach (var word in Words)
                {
                    bool correct = true;
                    for (int j = 0; j < QtWords; j++)
                    {
                        if (sentence.Checked[i + j] || !(word.Key[j] == sentence[i + j]))
                        {
                            correct = false;
                            break;
                        }
                    }
                    if (correct)
                    {
                        for (int j = 0; j < QtWords; j++)
                        {
                            sentence.Checked[i + j] = true;
                        }
                        result += word.Value;
                    }
                }
            }
            return result;
        }
    }
}
