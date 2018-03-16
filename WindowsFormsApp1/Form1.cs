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

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        static int index = 0;
        public Form1()
        {
            InitializeComponent();
            index++;
            Text = index.ToString() + ":" +
                Thread.CurrentThread.ManagedThreadId + ":" +
                Thread.CurrentThread.IsBackground.ToString();
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Thread th = new Thread(Function);
            th.IsBackground = true;
            th.Start();
        }
        private void Function()
        {
            Application.Run(new Form1());
        }
    }
}
