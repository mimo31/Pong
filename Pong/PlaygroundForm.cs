using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vector2d;

namespace Pong
{
    public partial class PlaygroundForm : Form
    {
        int TestStage = 0;
        int LifeSpanSum = 0;
        float PadlePosition = 512;
        PointF BallPosition = new PointF(511, 511);
        Vector2F BallSpeed;

        public PlaygroundForm()
        {
            InitializeComponent();
        }

        private void PlaygroundForm_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
            BallSpeed = new Vector2F();
            BallSpeed.X = (float)Program.R.NextDouble();
            BallSpeed.Y = (float)Program.R.NextDouble();
            BallSpeed = BallSpeed.Shrink(1);
        }

        private void UpdateScene()
        {

        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < Program.Speed; i++)
            {
                UpdateScene();
            }
            this.Refresh();
        }

        #region Painting

        private void PlaygroundForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.FillRectangle(Brushes.Blue, ToScreenCoors(new RectangleF(PadlePosition - 128, 1024 - 256 / 16, 256, 256 / 16)));
            graphics.FillEllipse(Brushes.Red, ToScreenCoors(new RectangleF(BallPosition.X - 16, BallPosition.Y - 16, 32, 32)));
        }

        private PointF ToScreenCoors(PointF point)
        {
            return new PointF(point.X / 1024 * this.ClientSize.Width, point.Y / 1024 * this.ClientSize.Height);
        }

        private RectangleF ToScreenCoors(RectangleF rect)
        {
            SizeF newSize = new SizeF(rect.Width / 1024 * this.ClientSize.Width, rect.Height / 1024 * this.ClientSize.Height);
            return new RectangleF(this.ToScreenCoors(rect.Location), newSize);
        }

        #endregion
    }
}
