using System;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;
using Ionic.Zip;

namespace FileManager
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }


        FolderBrowserDialog bd = new FolderBrowserDialog();

        private void button1_Click(object sender, EventArgs e)
        {
            if (bd.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = bd.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Zip files (*.zip)|*.zip";
            if (textBox1.Text != "" && sfd.ShowDialog() == DialogResult.OK)
            {
                ZipFile zf = new ZipFile(sfd.FileName);
                zf.AddDirectory(bd.SelectedPath);
                zf.Save();
                MessageBox.Show("Архивация прошла успешно.", "Выполнено");
            }
        }
    }
}
