using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pong
{
    public partial class ControlForm : Form
    {
        public ControlForm()
        {
            InitializeComponent();
        }

        public long GetLongestLifeSpan()
        {
            long longestFound = 0;
            for (int i = 0; i < Program.Generation.Length; i++)
            {
                long currentLifeSpan = Program.Generation[i].LifeSpan;
                if (currentLifeSpan == 0)
                {
                    break;
                }
                else
                {
                    if (currentLifeSpan > longestFound)
                    {
                        longestFound = currentLifeSpan;
                    }
                }
            }
            return longestFound;
        }

        public float GetAverageLifeSpan()
        {
            long sum = 0;
            int numberOfSamples = 0;
            for (int i = 0; i < Program.Generation.Length; i++)
            {
                long currentLifeSpan = Program.Generation[i].LifeSpan;
                if (currentLifeSpan == 0)
                {
                    numberOfSamples = i;
                    break;
                }
                else
                {
                    if (i == 127)
                    {
                        numberOfSamples = 128;
                    }
                    sum += currentLifeSpan;
                }
            }
            if (numberOfSamples != 0)
            {
                return sum / (float)numberOfSamples;
            }
            else
            {
                return 0;
            }
        }

        public void UpdateValues(int stage)
        {
            this.label3.Text = "Current generation: " + Program.GenerationNumber.ToString();
            this.label4.Text = "Average lifespan: " + this.GetAverageLifeSpan().ToString("0.00");
            this.label5.Text = "Longest lifespan: " + this.GetLongestLifeSpan().ToString();
            this.label7.Text = "Current sample: " + Program.SampleNumber.ToString();
            this.label8.Text = "Current stage: " + stage.ToString();
        }

        private void UpdateSpeedLabel()
        {
            this.label6.Text = "Speed: " + Program.Speed.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Program.Speed *= 2;
            UpdateSpeedLabel();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Program.Speed /= 2;
            UpdateSpeedLabel();
        }
    }
}
