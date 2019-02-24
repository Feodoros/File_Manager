using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileManager
{
    public class ArchivingFile : Archiving
    {

        public void Pack(string path)
        {
            Archive(path);
        }

        protected override void CompressFile(string pathfile, string compressfile)
        {

            try
            {
                Directory.CreateDirectory(pathfile + "_ZIP");
                File.Copy(pathfile, pathfile + "_ZIP" + Path.DirectorySeparatorChar + Path.GetFileName(pathfile));
                ZipFile.CreateFromDirectory(pathfile + "_ZIP", compressfile);
                Directory.Delete(pathfile + "_ZIP", true);
                //MessageBox.Show("Архивация завершена.", "Архивация.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }

    public abstract class Archiving
    {

        protected void Archive(string path)
        {
            if (File.Exists(path))
            {
                CompressFile(path , Path.GetDirectoryName(path) + "\\" + Path.GetFileName(path) + ".zip");

            }

            else
            {
                path = path.Replace("[", "").Replace("]", "");

                Directory.CreateDirectory(path + "_archived");

                DirectoryInfo di = new DirectoryInfo(path);
                FileInfo[] files = di.GetFiles();

                Parallel.ForEach(files, (currentFile) =>
                {
                    string pathFile = currentFile.FullName;
                    string compressfile = path + "_archived" + "\\" + currentFile.ToString() + ".zip";
                    CompressFile(pathFile, compressfile);
                });
            }
        }

        protected abstract void CompressFile(string pathfile, string compressfile);
    }
}