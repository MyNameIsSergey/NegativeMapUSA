using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOP1
{
    public partial class Form1 : Form
    {
        StateCollection stateCollection;
        MouseController mouse;
        MatrixArgs stdArgs;
        Dictionary dictionary;
        Painter painter;
        public Form1()
        {
            InitializeComponent();
            StateReader fileReader = new StateReader("states.json");
            dictionary = new Dictionary("sentiments.csv");
            State state;
            List<State> states = new List<State>(50);
            while ((state = fileReader.GetNextState()) != null)
                states.Add(state);
            stateCollection = new StateCollection(states.ToArray()) { Update = 500 };
            stateCollection.OnNewData += StateCollection_OnNewData;
            stateCollection.OnEnd += StateCollection_OnEnd;
            stdArgs = new MatrixArgs() { Offset = new PointF(2000, 900), Scale = 10 };
            mouse = new MouseController(pictureBox, stdArgs.Offset, stdArgs.Scale);
            mouse.MatrixUpdated += Mouse_MatrixUpdated;
        }

        private void StateCollection_OnEnd(object sender, StateEventArgs e)
        {
            lock (painter)
            {
                painter.Draw(e, mouse.MatrixArgs);
            }
            MessageBox.Show("End");
        }

        private void StateCollection_OnNewData(object sender, StateEventArgs e)
        {
            lock (painter)
            {
                painter.Draw(e, mouse.MatrixArgs);
            }
        }
        private void Mouse_MatrixUpdated(object sender, MatrixArgs e)
        {
            lock(painter)
            {
                painter.Draw(new StateEventArgs() { ProcessedMessage = stateCollection.ProcessedMessages, States = stateCollection.States }, mouse.MatrixArgs);
            }

        }

        
        private void Form1_DoubleClick(object sender, EventArgs e)
        {
            mouse.MatrixArgs = stdArgs;
            Mouse_MatrixUpdated(this, stdArgs);
        }

        MessageReader messageReader;

        
        System.Threading.Thread[] threads;

        private void Init()
        {
            IniFile file = new IniFile("info.ini");
            int qtThreads = int.Parse(file.ReadINI("Threads", "threads"));
            int buff = int.Parse(file.ReadINI("Threads", "buff"));
            messageReader = new MessageReader(file.ReadINI("File", "name")) { BuffSize = buff };
            threads = new System.Threading.Thread[qtThreads];
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Init();
            painter = new Painter(pictureBox);
            Mouse_MatrixUpdated(this, stdArgs);
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new System.Threading.Thread(() => stateCollection.CalculatePositiveLevel(dictionary, messageReader));
                threads[i].Start();
            }
            //Task.Run(() => stateCollection.CalculatePositiveLevel(dictionary, messageReader));
        }

        private void PictureBox_Resize(object sender, EventArgs e)
        {
            painter?.Resize(pictureBox.Width, pictureBox.Height);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Abort();
            }
            messageReader.Dispose();
        }
    }

} 