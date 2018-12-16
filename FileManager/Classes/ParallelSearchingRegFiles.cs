using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileManager
{
    class ParallelSearchingRegFiles
    {
        private Regex[] reg = null;
        private string DirectoryName;
        private ISearchHandler SearchHandler = null;

        public ParallelSearchingRegFiles(ISearchHandler handler)
        {
            SearchHandler = handler;
        }

        public void StartToSearch(string DirectoryName)
        {
            reg = SearchHandler.RegExamples();

            this.DirectoryName = DirectoryName;

            if (Directory.Exists(DirectoryName))
            {
                string FileWithResult = DirectoryName + "_Result" + @".txt";
                using (StreamWriter file = new StreamWriter(FileWithResult, false, Encoding.UTF8))
                {
                    Searching(DirectoryName, file);
                }
                MessageBox.Show("Поиск по файлам завершен.", "Поиск информации.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Searching(string path, StreamWriter file)
        {
            try
            {
                DirectoryInfo directory = new DirectoryInfo(path);
                DirectoryInfo[] directories = directory.GetDirectories();
                FileInfo[] files = directory.GetFiles();

                foreach (var dir in directories)
                {
                    Searching(dir.FullName, file);
                };

                foreach (var File in files)
                {
                    SearchHandler.CheckReg(file, reg, File);
                };
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Произошла ошибка.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }


    public interface ISearchHandler
    {
        Regex[] RegExamples();
        void CheckReg(StreamWriter whereToWrite, Regex[] r, FileInfo file);
    }


    public class SearchHandler : ISearchHandler
    {
        public Regex[] RegExamples()
        {
            Regex[] r = new Regex[5];
            r[0] = new Regex(@"[-a-f0-9_.]+@{1}[-0-9a-z]+\.[a-z]{2,5}");
            r[1] = new Regex(@"(\d{4}\s\d{6})");
            r[2] = new Regex(@"[a-zA-Z1-9\-\._]+@[a-z1-9]+(.[a-z1-9]+){1,}");
            r[3] = new Regex(@"(8|\+7)([\-\s])?(\(?\d{3}\)?[\-\s])?[\d\-\s]{7,20}");
            r[4] = new Regex(@"(https?://([a-z1-9]+.)?[a-z1-9\-]+(\.[a-z]+){1,}/?)");
            return r;
        }

        public void CheckReg(StreamWriter whereToWrite, Regex[] regularies, FileInfo file)
        {
            byte[] b = File.ReadAllBytes(file.FullName);
            UTF8Encoding temp = new UTF8Encoding(true);
            string str;
            for (int i = 0; i < 4; i++)
                foreach (Match m in regularies[i].Matches(temp.GetString(b)))
                {
                    str = m.ToString();
                    whereToWrite.WriteLine(str);
                }
        }
    }
}
