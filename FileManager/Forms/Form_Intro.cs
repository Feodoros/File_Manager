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
    public partial class Intro : Form
    {
        public Intro()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Opacity += .03;
            if (Opacity == 1)
            {
                timer1.Stop();
            }
        }

        private void Intro_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }
    }
}
