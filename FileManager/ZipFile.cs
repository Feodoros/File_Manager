using FileManager.Forms;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    class ZipFiles : Forms.Entry
    {
        public void createFolder()
        {
            new ZipFolder(Path).CreateFolder("new folder");
        }
        public void createFile()
        {
            new ZipFolder(Path).CreateFile("newfile.txt");
        }
        public ZipFiles(string path) : base(path)
        {

        }
        public void Delete(string name)
        {
            using (Ionic.Zip.ZipFile zip = Ionic.Zip.ZipFile.Read(Path))
            {
                List<string> fullist = zip.EntryFileNames.ToList();
                if (name[name.Length - 1] == '/')
                    name = name.Remove(name.Length - 1, 1) + "\\";
                zip.RemoveEntry(name.Replace('\\', '/'));

                zip.Save();
            }
        }
        public void OpenFile(string name)
        {
            int ZipPlace = Path.IndexOf(".zip");
            string path1 = Path.Substring(0, ZipPlace + 4);
            string ArchiveWay = Path.Substring(ZipPlace + 4);
            using (Ionic.Zip.ZipFile zip = Ionic.Zip.ZipFile.Read(path1))
            {
                while (path1[path1.Length - 1] != '\\')
                {
                    path1 = path1.Remove(path1.Length - 1, 1);
                }
                foreach (ZipEntry e in zip)
                {
                    if (e.FileName == ArchiveWay.Replace('\\', '/') + name)
                        e.Extract(path1, ExtractExistingFileAction.DoNotOverwrite);
                }

            }
        }
        public static void OpenZipFile(string DirectoryPath, string FilePath)
        {
            new ZipFiles(DirectoryPath).OpenFile(FilePath);
            int ZipPlace = DirectoryPath.IndexOf(".zip\\");
            string name = FilePath;
            string path1 = DirectoryPath.Substring(0, ZipPlace + 4);
            string ArchiveWay = DirectoryPath.Substring(ZipPlace + 5);
            while (path1[path1.Length - 1] != '\\')
            {
                path1 = path1.Remove(path1.Length - 1, 1);
            }

            while (DirectoryPath[DirectoryPath.Length - 1] != '\\')
            {
                DirectoryPath = DirectoryPath.Remove(DirectoryPath.Length - 1, 1);
            }
            Process.Start(path1 + name);
        }

        public override bool Existing()
        {
            throw new NotImplementedException();
        }

        public override string GetFullName(string name)
        {
            throw new NotImplementedException();
        }

        public override string GetName()
        {
            throw new NotImplementedException();
        }
    }

}
