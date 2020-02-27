using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP1
{
    class Sentence
    {
        public string[] Words { get; private set; }
        public int Length { get => Words.Length; }
        public bool[] Checked { get; private set; }
        public string this[int index]
        {
            get { return Words[index]; }
        }
        public Sentence(string[] words)
        {
            Words = words;
            Checked = new bool[words.Length];
        }
    }
}
