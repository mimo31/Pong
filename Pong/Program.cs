using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using UniGenome;

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

        [STAThread]
        static void Main()
        {
            InitalizeData();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PlaygroundForm());
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
