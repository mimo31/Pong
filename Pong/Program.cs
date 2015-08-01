using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using UniGenome;
using Vector2d;

namespace Pong
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        const String RootDirectory = "";
        public static SampleData[] Generation;
        public static int GenerationNumber;
        public static int SampleNumber;
        public static int Speed = 1;
        public static SampleData CurrentSample;
        public static Random R = new Random();
        public static ControlForm Control;

        [STAThread]
        static void Main()
        {
            InitalizeData();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PlaygroundForm());
        }

        public static void TakeNewGeneration()
        {
            Generation = Generation.OrderByDescending<SampleData, long>((SampleData sample) => sample.LifeSpan).ToArray();
            int[] unluckyIndexes = Enumerable.Range(0, 64).OrderBy(val => R.NextDouble()).Take(8).ToArray();
            int[] luckyIndexes = Enumerable.Range(64, 64).OrderBy(val => R.NextDouble()).Take(8).ToArray();
            SampleData[] survivors = new SampleData[64];
            for (int i = 0; i < 64; i++)
            {
                if (unluckyIndexes.Contains(i))
                {
                    survivors[i] = Generation[luckyIndexes[Array.IndexOf(unluckyIndexes, i)]];
                }
                else
                {
                    survivors[i] = Generation[i];
                }
            }
            for (int i = 0; i < 64; i++)
            {
                Generation[i * 2] = new SampleData() {
                    Genome = survivors[i].Genome.GetMutation()
                };
                Generation[i * 2 + 1] = new SampleData()
                {
                    Genome = survivors[i].Genome.GetMutation()
                };
            }
            GenerationNumber++;
            SampleNumber = 0;
            CurrentSample = Generation[SampleNumber];
        }

        static void InitalizeData()
        {
            if (Directory.Exists(RootDirectory))
            {

            }
            else
            {
                Generation = new SampleData[128];
                for (int i = 0; i < Generation.Length; i++)
                {
                    Generation[i] = new SampleData();
                    Generation[i].Genome = new Genome(0, 2, 5, 0, R);
                }
                GenerationNumber = 0;
                SampleNumber = 0;
                CurrentSample = Generation[0];
            }
        }
    }
}
