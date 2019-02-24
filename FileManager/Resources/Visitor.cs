using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileManager.Resources
{

    class MD5
    {
        public static string CreateMD5(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }

    interface IVisitor
    {
        void Visit(Forms.File f);
        void Visit(Forms.Folder f);
    }

    class Md5Hash : IVisitor
    {
        public void Visit(Forms.File f)
        {
            string hashed = MD5.CreateMD5(f.GetTextFromFile());
            MessageBox.Show(hashed);
        }

        public void Visit(Forms.Folder f)
        {
            string[] mas = Forms.Folder.Get_ALL_Files_From_All_Inner_Dir(f.GetFullName());

            string dano = "";

            foreach (string i in mas)
            {
                string s = new Forms.File(i).GetTextFromFile();

                dano += s;
            }

            string hashed = MD5.CreateMD5(dano);
            MessageBox.Show(hashed);
        }
    }

    class Derypt : IVisitor
    {
        private int NumToCript;

        public Derypt()
        {
            NumToCript = 0;
        }

        public void setNum(int n)
        {
            NumToCript = n;
        }

        public void Visit(Forms.File f)
        {
            string wat = f.GetTextFromFile();
            wat = Decrypt(wat);
            var someStream = new Forms.File(f.GetFullName() + "crypt.txt").Create();

            byte[] info = new UTF8Encoding(true).GetBytes(wat);
            someStream.Write(info, 0, info.Length);
            f.Delete();
        }


        public void Visit(Forms.Folder f)
        {
            string[] mas = Forms.Folder.Get_ALL_Files_From_All_Inner_Dir(f.GetFullName());

            foreach (string i in mas)
            {
                Visit(new Forms.File(i));
            }
        }

        private string Decrypt(string a)
        {
            char[] mas = a.ToArray();
            string s = "";
            for (int i = 0; i < a.Length; i++)
            {
                mas[i] = (char)(mas[i] + NumToCript);
                s += mas[i];
            }
            return s;
        }

    }

    class Encrypt : IVisitor
    {
        Derypt c;
        int myNum;
        public Encrypt()
        {
            myNum = 0;
            c = new Derypt();
        }

        public void setKey(int a)
        {
            myNum = a;
        }


        public void Visit(Forms.File f)
        {
            myNum = -myNum;
            string wat = f.GetTextFromFile();
            wat = Encryption(wat);
            var someStream = new Forms.File(f.GetFullName().Remove(f.GetFullName().Length - 5, 5) + ".txt").Create();

            byte[] info = new UTF8Encoding(true).GetBytes(wat);
            someStream.Write(info, 0, info.Length);
            f.Delete();
        }


        public void Visit(Forms.Folder f)
        {
            string[] mas = Forms.Folder.Get_ALL_Files_From_All_Inner_Dir(f.GetFullName());

            foreach (string i in mas)
            {
                Visit(new Forms.File(i));
            }
        }

        private string Encryption(string a)
        {
            char[] mas = a.ToArray();
            string s = "";
            for (int i = 0; i < a.Length; i++)
            {
                mas[i] = (char)(mas[i] + myNum);
                s += mas[i];
            }
            return s;
        }

    }

}

