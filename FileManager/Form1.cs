using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileManager
{
    public partial class Form1 : Form
    {
        public Form1()
        {                       
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form formX = new Form2();
            formX.ShowDialog();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Form formX = new Form3();
            formX.ShowDialog();
        }       

        private void button3_Click_1(object sender, EventArgs e)
        {
            Form formX = new Form4();
            formX.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form formX = new Form5();
            formX.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form formX = new Form6(this);
            formX.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form formX = new Form_MORE_FUN();
            formX.ShowDialog();
        }
    }
}
