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
    public partial class Setting_Form : Form
    {
        /*SetX set = new SetX();   //For_XMLSerialization
        XmlSerializer xs;
        StreamWriter sw;*/

        BinaryFormatter BF = new BinaryFormatter();//For_BinarySerialization
        FileStream FS;

        Form f;

        public Setting_Form(Form f)
        {
            this.f = f;
            InitializeComponent();
            InitControls();

        }

        private void InitControls()
        {
            //Deserialization 

            //BinaryDeserialization
            FS = new FileStream(@"C:\Users\fzhil\source\repos\File_Manager\settings.txt", FileMode.Open, FileAccess.Read);
            SetX set1 = (SetX)BF.Deserialize(FS);
            textBox1.Text = set1.Width.ToString();
            textBox4.Text = set1.Height.ToString();         //---------------BinaryDeserialization--------------
            textBox2.Text = set1.color;
            textBox3.Text = set1.user;
            FS.Close();

            /*xs = new XmlSerializer(typeof(SetX)); //XMLDeserialization
            StreamReader sr = new StreamReader(@"C:\Users\fzhil\source\repos\File_Manager\SettingsXML.xml");
            SetX set = (SetX)xs.Deserialize(sr);
            textBox1.Text = set.Width.ToString();
            textBox4.Text = set.Height.ToString();
            textBox2.Text = set.Color;
            textBox3.Text = FromHash(set.User, "doberman");
            sr.Close();*/
        }


        private void button1_Click(object sender, EventArgs e)
        {
            //Serialization

            int w = int.Parse(textBox1.Text);
            int h = int.Parse(textBox4.Text);
            string u = textBox3.Text;                           //---------------BinarySerialization--------------
            string c = textBox2.Text;
            SetX set = new SetX(w, h, u, c);
            FS = new FileStream(@"C:\Users\fzhil\source\repos\File_Manager\settings.txt", FileMode.Create, FileAccess.Write);
            BF.Serialize(FS, set);
            FS.Flush();
            FS.Close();
            MessageBox.Show("Nice Serialization  (настройки сохранены) ");
            f.Width = Convert.ToInt32(textBox1.Text);
            f.Height = Convert.ToInt32(textBox4.Text);


            /*set.Width = Int16.Parse(textBox1.Text);  //XMLSerialization
            set.Height = Int16.Parse(textBox4.Text);
            set.User = GetHash(textBox3.Text, "doberman");
            set.Color = textBox2.Text;
            xs = new XmlSerializer(typeof(SetX));
            sw = new StreamWriter(@"C:\Users\fzhil\source\repos\File_Manager\SettingsXML.xml");
            xs.Serialize(sw, set);
            MessageBox.Show("Nice Serialization  (настройки сохранены) ");
            sw.Close();*/
        }
    }
}
