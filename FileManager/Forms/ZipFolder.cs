using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Forms
{
    class ZipFolder: Entry
    {
        public  void createFolder()
        {
            new ZipFolder(Path).CreateFolder("New_Folder");
        }

        public  void createFile()
        {
            new ZipFolder(Path).CreateFile("file.txt");
        }

        public ZipFolder(string path) : base(path)
        {

        }

        public List<string> GetAllFiles()
        {
            using (Ionic.Zip.ZipFile zip = Ionic.Zip.ZipFile.Read(Path))
            {
                zip.AlternateEncoding = Encoding.Default;
                List<string> fulList = zip.EntryFileNames.ToList();
                List<string> shortList = new List<string>();
                int i = 0;
                foreach (string elem in fulList)
                {
                    i = elem.IndexOf("/");
                    if (i != -1)
                        shortList.Add(elem.Substring(0, i) + "/");
                    else
                        shortList.Add(elem);
                }
                //List<string> gi = shortList.Distinct().ToList();

                return shortList.Distinct().ToList();
            }
        }

        public void CreateFolder(string directoryname)
        {

            int index = Path.LastIndexOf('\\');
            string name = Path.Remove(index, Path.Length - index);

            var dir = Directory.CreateDirectory(name + '\\' + directoryname);


            //  zp.AddDirectory(dir.FullName);
            using (Ionic.Zip.ZipFile zp = Ionic.Zip.ZipFile.Read(Path))
            {
                zp.AddItem(dir.FullName, directoryname);
                zp.Save();
            }
            dir.Delete();
        }

        public static Byte[] ReadBytes(string fileName)
        {
            using (var Memmory_Mapped_File = MemoryMappedFile.CreateFromFile(fileName, FileMode.Open))
            {
                using (var stream = Memmory_Mapped_File.CreateViewStream())
                {
                    using (BinaryReader binReader = new BinaryReader(stream))
                    {
                        return binReader.ReadBytes((int)stream.Length);
                    }
                }
            }
        }

        public void CreateFile(string filename)
        {

            MemoryStream memory = new MemoryStream();
            var bytes = ReadBytes(filename);
            memory.Write(bytes, 0, bytes.Length);

            using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile(Path))

            {
                ZipEntry e = zip.AddEntry(filename, memory);
                // zip.AddFile(filename);

                zip.Save();

            }

        }

        public List<string> GetLevelFiles(int Slesh, string name)
        {
            using (Ionic.Zip.ZipFile zip = Ionic.Zip.ZipFile.Read(Path))
            {
                int ZipPlace1 = name.IndexOf("//");
                if (ZipPlace1 > 0)
                { name = name.Remove(ZipPlace1, 1); }
                List<string> fulList = zip.EntryFileNames.ToList();
                string[] shortList = new string[fulList.Count];
                string[] newList = new string[fulList.Count];
                List<string> finalList = new List<string>();


                shortList = fulList.ToArray();
                int k = 0;
                for (int i = 0; i < fulList.Count; i++)
                {
                    if (shortList[i].Contains(name) && shortList[i].IndexOf(name) == 0)
                    {
                        newList[k] = shortList[i];
                        k++;
                    }

                }
                k = 0;
                int count = 0;
                while (k < newList.Length && newList[k] != null)
                {
                    count++;
                    k++;
                }


                string[] s = new string[count];
                k = 0;
                while (k < newList.Length && newList[k] != null)
                {
                    s[k] = newList[k];
                    k++;
                }

                for (int i = 1; i <= Slesh; i++)
                {
                    for (int j = 0; j < count; j++)
                    {
                        k = s[j].IndexOf("/");
                        s[j] = s[j].Substring(k + 1);
                    }
                }

                foreach (string elem in s)
                {
                    k = elem.IndexOf("/");
                    if (k != -1)
                        if (elem.Substring(0, k) != "")
                            finalList.Add(elem.Substring(0, k) + "/");
                        else { }
                    else
                       if (elem != "")
                        finalList.Add(elem);
                }
                return finalList.Distinct().ToList();
            }
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
