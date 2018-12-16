using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Forms
{
    class Folder : Entry

    {
        public Folder(string path) : base(path)
        {

        }


        public DirectoryInfo FolderWork()
        {
            var folder = new DirectoryInfo(Path);
            return folder;
        }


        public IEnumerator<DirectoryInfo> GetCollect()
        {
            DirectoryInfo dir = new DirectoryInfo(Path);

            IEnumerable<DirectoryInfo> directoryInfo = dir.EnumerateDirectories("*", SearchOption.TopDirectoryOnly);
            var enumerator = directoryInfo.GetEnumerator();
            return enumerator;
        }


        public string[] Get_Files_In_Selected_Folder()
        {
            string[] files = Directory.GetFiles(Path);
            return files;
        }


        public string[] Get_Files_With_Filter(string pattern)
        {
            string[] files = Directory.GetFiles(Path, pattern, SearchOption.AllDirectories);
            return files;
        }


        public string[] Get_ALL_Files_From_All_Inner_Dir()
        {
            string[] files = Directory.GetFiles(Path, "*.*", SearchOption.AllDirectories);
            return files;
        }


        public string[] GetDirectories()
        {
            string[] directories = Directory.GetDirectories(Path);

            return directories;
        }


        public override bool Existing()
        {
            if (Directory.Exists(Path) == true)
                return true;
            else
                return false;
        }


        public void Move(string newpath)
        {
            Directory.Move(Path, newpath);
        }


        public DirectoryInfo Create()
        {
            var dir = Directory.CreateDirectory(Path);
            return dir;
        }


        public void Create_Directory(string path)
        {
            var dir = Directory.CreateDirectory(path);            
        }


        public void Delete(bool recursive)
        {
            Directory.Delete(Path, recursive);
        }


        public override string GetFullName(string name)
        {
            var dir = new DirectoryInfo(Path);
            string fullname = dir.FullName;
            return fullname;
        }


        public override string GetName()
        {
            var dir = new DirectoryInfo(Path);
            string name = dir.Name;
            return name;
        }


        public string CurrentDirectory()
        {
            string pass = Directory.GetCurrentDirectory();
            return pass;
        }
    }
}
