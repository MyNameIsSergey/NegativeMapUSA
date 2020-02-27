using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP1
{
    class State
    {
        public string Name { get; set; }
        public float PositiveLevel { get; set; } = 0;
        public PointF[][] Coords { get; private set; }


        public State(string name, PointF[][] points)
        {
            Coords = points;
            Name = name;
        }
        public bool Belong(PointF point)
        {
            for (int i = 0; i < Coords.Length; i++)
                if (InPoly(Coords[i], point))
                    return true;
            return false;
        }

        public void CorrectPositiveLevel(float level)
        {
            PositiveLevel += level;
        }

        private static bool InPoly(PointF[] points, PointF point)
        {
            bool c = false;
            for (int i = 0, j = points.Length - 1; i < points.Length; j = i++)
            {
                if ((((points[i].Y <= point.Y) && (point.Y < points[j].Y)) || ((points[j].Y <= point.Y) && (point.Y < points[i].Y))) &&
                  (((points[j].Y - points[i].Y) != 0) && (point.X > ((points[j].X - points[i].X) * (point.Y - points[i].Y) / (points[j].Y - points[i].Y) + points[i].X))))
                    c = !c;
            }
            return c;
        }
    }

}
