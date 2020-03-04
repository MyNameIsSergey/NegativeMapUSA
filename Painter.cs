using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOP1
{
    class Painter
    {
        private Graphics g1;
        private Bitmap bitmap;
        private PictureBox pictureBox;
        private Font font;
        private DateTime startTime;
        public Painter(PictureBox pictureBox)
        {
            this.pictureBox = pictureBox;
            bitmap = new Bitmap(pictureBox.Width, pictureBox.Height);
            g1 = Graphics.FromImage(bitmap);
            font = new Font(new FontFamily("Comic Sans MS"), 10, FontStyle.Regular);
            startTime = DateTime.Now;
        }
        public void Draw(StateEventArgs stateCollection, MatrixArgs e)
        {
            lock (g1)
            {
                g1.Clear(Color.White);
                Draw(stateCollection.States, e, g1, 0, stateCollection.States.Length);
                g1.DrawString(stateCollection.ProcessedMessage.ToString(), font, new SolidBrush(Color.Black), 0, 0);
                g1.DrawString((DateTime.Now - startTime).ToString(), font, new SolidBrush(Color.Black), 0, 15);
            }
            pictureBox.Image = bitmap;

        }
        public void Resize(int x, int y)
        {
            lock (bitmap)
            {
                if (pictureBox.Width > 0)
                {
                    bitmap = new Bitmap(pictureBox.Width, pictureBox.Height);
                    g1 = Graphics.FromImage(bitmap);
                    g2 = Graphics.FromImage(bitmap);
                    g1.Clear(Color.White);
                }
            }
        }
        private void Draw(State[] stateCollection, MatrixArgs e, Graphics g, int min, int max)
        {
            for (int k = min; k < max; k++)
            {
                PointF[][] points = new PointF[stateCollection[k].Coords.Length][];
                int delta = 5 * Convert.ToInt32(stateCollection[k].PositiveLevel);
                delta = delta > 127 ? 127 : delta;
                delta = delta < -127 ? -127 : delta;
                int red = 128 - delta;
                int green = 128 + delta;
                for (int j = 0; j < stateCollection[k].Coords.Length; j++)
                {
                    points[j] = new PointF[stateCollection[k].Coords[j].Length];
                    for (int i = 0; i < points[j].Length; i++)
                    {
                        points[j][i] = new PointF(e.Scale * stateCollection[k].Coords[j][i].Y + e.Offset.X, -e.Scale * stateCollection[k].Coords[j][i].X + e.Offset.Y);
                    }
                    g.FillPolygon(new SolidBrush(Color.FromArgb(red, green, 0)), points[j]);
                    g.DrawPolygon(new Pen(Color.Black), points[j]);
                }
            }
        }

    }

}
