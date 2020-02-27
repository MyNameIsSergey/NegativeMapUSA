using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OOP1
{
    class StateReader
    {
        List<string> buff;
        public StateReader(string file)
        {
            System.IO.StreamReader reader = new System.IO.StreamReader(file);
            string s = reader.ReadToEnd();
            buff = s.Split('\"').ToList();
            buff.RemoveAt(0);
            reader.Close();
        }
        public State GetNextState()
        {
            if (buff.Count <= 1)
                return null;
            string name = buff.First();
            buff.RemoveAt(0);
            Regex regex = new Regex("-*[0-9]+[.,][0-9]+");
            string[] paths = buff.First().Replace("[[[", "b").Split('b');
            buff.RemoveAt(0);
            List<PointF[]> points = new List<PointF[]>();
            for (int j = 0; j < paths.Length; j++)
            {
                var mathces = regex.Matches(paths[j]);
                int count = mathces.Count / 2;
                if (count > 0)
                {
                    PointF[] array = new PointF[count];
                    for (int i = 0; i < count; i++)
                    {
                        array[i].Y = float.Parse(mathces[i * 2].Value.Replace('.', ','));
                        array[i].X = float.Parse(mathces[i * 2 + 1].Value.Replace('.', ','));
                    }
                    points.Add(array);
                }
            }
            return new State(name, points.ToArray());
        }
    }
}
