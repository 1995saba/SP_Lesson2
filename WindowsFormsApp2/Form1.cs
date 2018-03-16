using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        private Random rand;
        private bool isStarted;
        private Thread thread1;
        private Mutex mutex1;
        public Form1()
        {
            InitializeComponent();
            rand = new Random();
            isStarted = false;
            button.Text = "Start";
            mutex1 = new Mutex(false, "MyMutex", out bool IsCreated);
            if (IsCreated)
                Text = "MyMutex is created";
            else
                Text = "MyMutex is opened";
        }
        
        private void Button_Click(object sender, EventArgs e)
        {
            isStarted = !isStarted;
            if (isStarted)
            {
                Rectangle rect = new Rectangle(10, 10, 200, 200);
                button.Text = "Pause";
                thread1 = new Thread(ThreadFunction);
                thread1.IsBackground = true;
                thread1.Start(rect);
            }
            else
            {
                if (thread1.ThreadState != ThreadState.Suspended)
                {
                    button.Text = "Resume";
                    mutex1.WaitOne();
                    //thread1.Suspend();
                }
                else
                {
                    button.Text = "Pause";
                    mutex1.ReleaseMutex();
                    //thread1.Resume();
                }
            }
        }

        private void ThreadFunction(object obj)
        {
            //подготовка к синхронизации потока
            Rectangle rect = (Rectangle)obj;
            DrawLines(rect);
            //Завершение потока
        }

        private void DrawLines(Rectangle rect)
        {
            while (true)
            {
                Graphics gr = CreateGraphics();
                int red = rand.Next(0, 255);
                int green = rand.Next(0, 255);
                int blue = rand.Next(0, 255);
                Color color = Color.FromArgb(red, green, blue);
                float width = rand.Next(1, 10);
                Pen pen = new Pen(color, width);
                float x1 = rect.X;
                float y1 = rand.Next(rect.Top, rect.Bottom);
                float x2 = rand.Next(rect.X, rect.X + rect.Width);
                float y2 = rect.Y;
                float x3 = rect.X + rect.Width;
                float y4 = rect.Bottom;
                mutex1.WaitOne();
                gr.DrawLine(pen, x1, y1, x2, y2);
                gr.DrawLine(pen, x2, y2, x3, y1);
                gr.DrawLine(pen, x3, y1, x2, y4);
                gr.DrawLine(pen, x2, y4, x1, y1);
                mutex1.ReleaseMutex();
                Thread.Sleep(100);
            }
        }
    }
}
