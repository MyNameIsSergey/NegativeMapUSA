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
            control.MouseWheel += MouseWheel;
            timer = new Timer
            {
                Interval = 16
            };
            timer.Enabled = false;
            timer.Tick += MouseMove;
            this.offset = offset;
            this.scale = scale;
        }
        Timer timer;
        private void MouseDown(object sender, MouseEventArgs e)
        {
            lastMouse = Cursor.Position;
            timer.Enabled = true;
        }
        PointF lastMouse;
        PointF offset;
        float scale;
        private void MouseMove(object sender, EventArgs args)
        {
            float k = scale / (scale + 1 / scale);
            offset.X += (Cursor.Position.X - lastMouse.X) * k;
            offset.Y += (Cursor.Position.Y - lastMouse.Y) * k;
            lastMouse = Cursor.Position;
            MatrixUpdated(this, new MatrixArgs() { Offset = offset, Scale = scale });
        }
        private void MouseWheel(object sender, MouseEventArgs e)
        {
            if ((scale > 1 || e.Delta > 0) && (scale < 20 || e.Delta < 0))
                scale += e.Delta / 1000.0f;
            MatrixUpdated(this, new MatrixArgs() { Offset = offset, Scale = scale });
        }

        private void MouseUp(object sender, MouseEventArgs e)
        {
            timer.Enabled = false;
        }
    }
    class MatrixArgs
    {
        public PointF Offset { get; set; }
        public float Scale { get; set; }

    }

}
