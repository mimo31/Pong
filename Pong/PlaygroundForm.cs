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
        long[] LifeSpans;
        int PadlePosition;
        PointF BallPosition;
        Vector2F BallSpeed;
        const int PixelsPerMove = 2;

        public PlaygroundForm()
        {
            InitializeComponent();
        }

        private void PlaygroundForm_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
            Program.Control = new ControlForm();
            Program.Control.Show();
            Program.Control.UpdateValues(0);
            LifeSpans = new long[4];
            StartNewTest();
        }

        private void StartNewTest()
        {
            PadlePosition = 511;
            BallPosition = new PointF(511, 511);
            BallSpeed = new Vector2F();
            BallSpeed.X = (float)(Program.R.NextDouble() - 0.5);
            BallSpeed.Y = (float)(Program.R.NextDouble() - 0.5);
            BallSpeed = BallSpeed.Shrink(1);
        }

        private void MovePadle()
        {
            bool[] boolInputs = new bool[0];
            long[] numberInputs = new long[5];
            numberInputs[0] = (long)BallPosition.X;
            numberInputs[1] = (long)BallPosition.Y;
            numberInputs[2] = (long)BallSpeed.X;
            numberInputs[3] = (long)BallSpeed.Y;
            numberInputs[4] = PadlePosition;
            Program.CurrentSample.Genome.PushInputs(boolInputs, numberInputs);
            bool willMoveLeft = Program.CurrentSample.Genome.GetBoolOutput(0);
            bool willMoveRight = Program.CurrentSample.Genome.GetBoolOutput(1);
            if (!willMoveLeft == willMoveRight)
            {
                if (willMoveLeft && !(PadlePosition - 128 < PixelsPerMove))
                {
                    PadlePosition -= PixelsPerMove;
                }
                else if (willMoveRight && !(PadlePosition + 128 > 1024 - PixelsPerMove))
                {
                    PadlePosition += PixelsPerMove;
                }
            }
        }

        private void MoveBall()
        {
            BallPosition.X += BallSpeed.X;
            BallPosition.Y += BallSpeed.Y;
        }

        private void ResolveCollisions()
        {
            if (BallPosition.Y < 16)
            {
                BallSpeed.Y = -BallSpeed.Y;
                BallPosition.Y = 16 + 16 - BallPosition.Y;
            }
            else if (BallPosition.Y > 1024 - 16 - 16)
            {
                float distanceBelowPadle = (BallPosition.Y - (1024 - 16 - 16));
                float possibleInterceptionPosition = BallPosition.X - distanceBelowPadle * BallSpeed.X / BallSpeed.Y;
                if (possibleInterceptionPosition >= PadlePosition - 128 && possibleInterceptionPosition < PadlePosition + 128)
                {
                    BallSpeed.Y = -BallSpeed.Y;
                    BallPosition.Y = 1024 - 16 - 16 - distanceBelowPadle;
                }
                else
                {
                    if (TestStage == 3)
                    {
                        Program.CurrentSample.LifeSpan = CalculateAverageLifeSpan(LifeSpans);
                        LifeSpans = new long[4];
                        TestStage = 0;
                        if (Program.SampleNumber == 127)
                        {
                            Program.TakeNewGeneration();
                        }
                        else
                        {
                            Program.SampleNumber++;
                            Program.CurrentSample = Program.Generation[Program.SampleNumber];
                        }
                    }
                    else
                    {
                        TestStage++;
                    }
                    StartNewTest();
                    Program.Control.UpdateValues(TestStage);
                }
            }
            if (BallPosition.X < 16)
            {
                BallSpeed.X = -BallSpeed.X;
                BallPosition.X = 16 + 16 - BallPosition.X;
            }
            else if (BallPosition.X > 1024 - 16)
            {
                BallSpeed.X = -BallSpeed.X;
                BallPosition.X = 1024 - 16 - (BallPosition.X - (1024 - 16));
            }
        }

        private long CalculateAverageLifeSpan(long[] array)
        {
            array = array.OrderBy(val => val).ToArray();
            return (array[1] + array[2]) / 2;
        } 

        private void UpdateTimeAndBallSpeed()
        {
            LifeSpans[TestStage]++;
            BallSpeed = BallSpeed.Shrink(BallSpeed.GetMagnitude() + (float)0.015625);
        }

        private void UpdateScene()
        {
            MovePadle();
            MoveBall();
            ResolveCollisions();
            UpdateTimeAndBallSpeed();
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
