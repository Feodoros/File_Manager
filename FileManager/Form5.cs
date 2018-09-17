using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
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
            /*SaveFileDialog sfd = new SaveFileDialog();                       
            sfd.Filter = "Zip files (*.zip)|*.zip";
            if (textBox1.Text != "" && sfd.ShowDialog() == DialogResult.OK)        *----------------------*
            {                                                                <==== *Старый метод архивации*
                ZipFile zf = new ZipFile(sfd.FileName);                            *----------------------*
                zf.AddDirectory(bd.SelectedPath);
                zf.Save();*/
            Archiving();
            MessageBox.Show("Архивация прошла успешно.", "Выполнено");
        }

        public void Archiving_One_By_One_File(string PathOfFile, string ArchivedFile)
        {
            using (FileStream Start = new FileStream(PathOfFile, FileMode.OpenOrCreate))
            {
                using (FileStream End = File.Create(ArchivedFile))
                {
                    using (GZipStream Archiving = new GZipStream(End, CompressionMode.Compress))
                    {
                        Start.CopyTo(Archiving);
                    }
                }
            }
        }//Арихвиация файлов по одному.

        public void Archiving()
        {
            Directory.CreateDirectory(textBox1.Text + ".Архив");

            DirectoryInfo directory = new DirectoryInfo(textBox1.Text);

            FileInfo[] files = directory.GetFiles();

            Parallel.ForEach(files, (currentFile) =>
            {
                string ArchivedFile = textBox1.Text + ".Архив" + "\\" + currentFile.ToString() + ".zip";
                string PathOfFile = currentFile.FullName;
                Archiving_One_By_One_File(PathOfFile, ArchivedFile);
            });
        }//Archiving

    }
}
