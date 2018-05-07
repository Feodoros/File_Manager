using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace FileManager
{
    public partial class Form6 : Form
    {
        SetX set = new SetX();//For_XMLSerialization
        XmlSerializer xs;
        StreamWriter sw;

        BinaryFormatter BF = new BinaryFormatter();//For_BinarySerialization
        FileStream FS;

        Form f;

        public Form6(Form f)
        {
            this.f = f;
            InitializeComponent();
            InitControls();

        }

        private void InitControls()
        {
            //Deserialization 

            //BinaryDeserialization
            /*FS = new FileStream(@"C:\Users\fzhil\source\repos\File_Manager\settings.txt", FileMode.Open, FileAccess.Read);
            SetX set1 = (SetX)BF.Deserialize(FS);
            textBox1.Text = set1.Width.ToString();
            textBox4.Text = set1.Height.ToString();         ---------------BinaryDeserialization--------------
            textBox2.Text = set1.color;
            textBox3.Text = set1.user;
            FS.Close();*///....

            xs = new XmlSerializer(typeof(SetX)); //XMLDeserialization
            StreamReader sr = new StreamReader(@"C:\Users\fzhil\source\repos\File_Manager\SettingsXML.xml");
            SetX set = (SetX)xs.Deserialize(sr);
            textBox1.Text = set.Width.ToString();
            textBox4.Text = set.Height.ToString();
            textBox2.Text = set.Color;
            textBox3.Text = FromHash(set.User, "doberman");
            sr.Close();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            //Serialization

            /*int w = int.Parse(textBox1.Text);
            int h = int.Parse(textBox4.Text);
            string u = textBox3.Text;                           ---------------BinarySerialization--------------
            string c = textBox2.Text;
            SetX set = new SetX(w, h, u, c);
            FS = new FileStream(@"C:\Users\fzhil\source\repos\File_Manager\settings.txt", FileMode.Create, FileAccess.Write);
            BF.Serialize(FS, set);
            FS.Flush();
            FS.Close();
            MessageBox.Show("Nice Serialization");                
            f.Width = Convert.ToInt32(textBox1.Text);
            f.Height = Convert.ToInt32(textBox4.Text);
            */

            set.Width = Int16.Parse(textBox1.Text);  //XMLSerialization
            set.Height = Int16.Parse(textBox4.Text);
            set.User = GetHash(textBox3.Text, "doberman");
            set.Color = textBox2.Text;
            xs = new XmlSerializer(typeof(SetX));
            sw = new StreamWriter(@"C:\Users\fzhil\source\repos\File_Manager\SettingsXML.xml");
            xs.Serialize(sw, set);
            MessageBox.Show("Nice Serialization  (настройки сохранены) ");
            sw.Close();
        }


        //Encrypting & Decrypting
        public static string GetHash(string ishText, string pass,
       string sol = "doberman", string cryptographicAlgorithm = "SHA1",
       int passIter = 2, string initVec = "a8doSuDitOz1hZe#",
       int keySize = 256)
        {
            if (string.IsNullOrEmpty(ishText))
                return "";

            byte[] initVecB = Encoding.ASCII.GetBytes(initVec);
            byte[] solB = Encoding.ASCII.GetBytes(sol);
            byte[] ishTextB = Encoding.UTF8.GetBytes(ishText);

            PasswordDeriveBytes derivPass = new PasswordDeriveBytes(pass, solB, cryptographicAlgorithm, passIter);
            byte[] keyBytes = derivPass.GetBytes(keySize / 8);
            RijndaelManaged symmK = new RijndaelManaged();
            symmK.Mode = CipherMode.CBC;

            byte[] cipherTextBytes = null;

            using (ICryptoTransform encryptor = symmK.CreateEncryptor(keyBytes, initVecB))
            {
                using (MemoryStream memStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(ishTextB, 0, ishTextB.Length);
                        cryptoStream.FlushFinalBlock();
                        cipherTextBytes = memStream.ToArray();
                        memStream.Close();
                        cryptoStream.Close();
                    }
                }
            }

            symmK.Clear();
            return Convert.ToBase64String(cipherTextBytes);
        }

        public static string FromHash(string ciphText, string pass,
               string sol = "doberman", string cryptographicAlgorithm = "SHA1",
               int passIter = 2, string initVec = "a8doSuDitOz1hZe#",
               int keySize = 256)
        {
            if (string.IsNullOrEmpty(ciphText))
                return "";

            byte[] initVecB = Encoding.ASCII.GetBytes(initVec);
            byte[] solB = Encoding.ASCII.GetBytes(sol);
            byte[] cipherTextBytes = Convert.FromBase64String(ciphText);

            PasswordDeriveBytes derivPass = new PasswordDeriveBytes(pass, solB, cryptographicAlgorithm, passIter);
            byte[] keyBytes = derivPass.GetBytes(keySize / 8);

            RijndaelManaged symmK = new RijndaelManaged();
            symmK.Mode = CipherMode.CBC;

            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int byteCount = 0;

            using (ICryptoTransform decryptor = symmK.CreateDecryptor(keyBytes, initVecB))
            {
                using (MemoryStream mSt = new MemoryStream(cipherTextBytes))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(mSt, decryptor, CryptoStreamMode.Read))
                    {
                        byteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                        mSt.Close();
                        cryptoStream.Close();
                    }
                }
            }

            symmK.Clear();
            return Encoding.UTF8.GetString(plainTextBytes, 0, byteCount);
        }
    }
}
