using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP1
{
    class Dictionary
    {
        private List<DictionaryPage> pages = new List<DictionaryPage>();

        public Dictionary(string file)
        {
            System.IO.StreamReader reader = new System.IO.StreamReader(file);
            string s = reader.ReadToEnd();
            reader.Close();
            List<string> buff = s.Split('\n', ',').ToList();
            buff.RemoveAll((t) => t == string.Empty);
            for (int i = 0; i < buff.Count; i += 2)
            {
                string[] words = buff[i].ToLower().Split(' ');
                int index = pages.FindIndex((page) => page.QtWords == words.Length);
                if (index >= 0)
                {
                    pages[index].Words.Add(words, float.Parse(buff[i + 1].Replace('.', ',')));
                }
                else
                {
                    pages.Add(new DictionaryPage() { Words = new Dictionary<string[], float>() });
                    pages.Last().Words.Add(words, float.Parse(buff[i + 1].Replace('.', ',')));
                }
            }
            pages.Sort(new Comparer());
        }

        public float CheckSentence(string[] s)
        {
            Sentence sentence = new Sentence(s);
            float positiveLevel = 0;
            for (int i = 0; i < pages.Count; i++)
            {
                positiveLevel += pages[i].CheckSentence(sentence);
            }
            return positiveLevel;
        }
    }

}
