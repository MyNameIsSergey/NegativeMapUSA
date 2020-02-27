using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOP1
{
    class MouseController
    {
        public event EventHandler<MatrixArgs> MatrixUpdated;
        public MatrixArgs MatrixArgs
        {
            get => new MatrixArgs() { Offset = offset, Scale = scale };
            set
            {
                scale = value.Scale;
                offset = value.Offset;
            }
        }
        public MouseController(Control control, PointF offset, float scale)
        {
            control.MouseDown += MouseDown;
            control.MouseUp += MouseUp;
            control.MouseMove += MouseMove;
            control.MouseWheel += MouseWheel;

            this.offset = offset;
            this.scale = scale;
        }
        private void MouseDown(object sender, MouseEventArgs e)
        {
            s = true;
            lastMouse = e.Location;
        }
        bool s;
        PointF lastMouse;
        PointF offset;
        float scale;
        private void MouseMove(object sender, MouseEventArgs e)
        {
            if (s)
            {
                float k = scale / (scale + 1 / scale) ;
                offset.X += (e.X - lastMouse.X) * k;
                offset.Y += (e.Y - lastMouse.Y) * k;
                lastMouse = e.Location;
                MatrixUpdated(this, new MatrixArgs() { Offset = offset, Scale = scale });


            }
        }
        private void MouseWheel(object sender, MouseEventArgs e)
        {
            if ((scale > 1 || e.Delta > 0) && (scale < 20 || e.Delta < 0))
                scale += (float)e.Delta / 1000.0f;
            MatrixUpdated(this, new MatrixArgs() { Offset = offset, Scale = scale });
        }

        private void MouseUp(object sender, MouseEventArgs e)
        {
            s = false;
        }
    }
    class MatrixArgs
    {
        public PointF Offset { get; set; }
        public float Scale { get; set; }

    }

}
