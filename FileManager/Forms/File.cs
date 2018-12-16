using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Forms
{
    class File : Entry
    {


        public File(string path) : base(path)
        {

        }


        public bool Serialize<T>(T user)
        {
            try
            {
                FileStream fs = new FileStream(Path, FileMode.OpenOrCreate);

                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, user);

                fs.Close();
                return true;
            }
            catch { return false; }
        }


        public T DeSerialize<T>()
        {
            try
            {
                FileStream fs = new FileStream("Users.xls", FileMode.Open);

                BinaryFormatter formatter = new BinaryFormatter();

                T user = (T)formatter.Deserialize(fs);

                fs.Close();
                return user;
            }
            catch
            {
                return default(T);
            }
        }


        public override bool Existing()
        {
            if (System.IO.File.Exists(Path) == true)
                return true;
            else
                return false;
        }


        public FileStream Create()
        {
            var file = System.IO.File.Create(Path);
            return file;
        }


        public void Copy(string newPath, bool owerwrite)
        {
            System.IO.File.Copy(Path, newPath, owerwrite);
        }


        public void Delete()
        {
            System.IO.File.Delete(Path);
        }


        public override string GetName()
        {
            string name = System.IO.Path.GetFileName(Path);
            return name;
        }


        public override string GetFullName(string path)
        {
            string fullname = System.IO.Path.GetFullPath(path);

            return path;
        }


        public void AppendText(string contain, Encoding encoding)
        {
            System.IO.File.AppendAllText(Path, contain, encoding);
        }


        public void WriteText(string contain, Encoding encoding)
        {
            System.IO.File.WriteAllText(Path, contain, encoding);
        }


        public StreamReader Open_Text(string path)
        {
            return System.IO.File.OpenText(path);
        }


        public byte[] Read_All_Bytes(string path)
        {
            return System.IO.File.ReadAllBytes(path);
        }


        public void WriteAllLineS(List<string> content)
        {
            System.IO.File.WriteAllLines(Path, content);
        }


        public void Move(string newpath)
        {
            System.IO.File.Move(Path, newpath);
        }


        public void Start(string path)
        {
            Process.Start(path);
        }


        public string ReadText()
        {
            var text = System.IO.File.ReadAllText(Path, Encoding.GetEncoding(1251));
            return text;
        }

    }
}
