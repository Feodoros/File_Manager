using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileManager
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            textBox1.Text = "C:/";
            Output_Folders();
        }


        public void Output_Folders() //Вывод папок на панель по заданному адресу.
        {
            listBox1.Items.Clear();

            DirectoryInfo directory = new DirectoryInfo(textBox1.Text);

            DirectoryInfo[] directoryS = directory.GetDirectories();

            foreach (DirectoryInfo directoryX in directoryS)
            {
                listBox1.Items.Add(directoryX);
            }

            FileInfo[] Files = directory.GetFiles();

            foreach (FileInfo FileX in Files)
            {
                listBox1.Items.Add(FileX);
            }
        }


        //Действия кнопок и элементов формы.   
        private void button1_Click(object sender, EventArgs e) //Кнопка "Перейти".
        {
            Output_Folders();
        }

        private void button2_Click(object sender, EventArgs e)  //Кнопка "Назад".
        {
            string s = textBox1.Text;
            if (s[s.Length - 1] == '\\')
            {
                s = s.Remove(s.Length - 1, 1);
                while (s[s.Length - 1] != '\\')
                {
                    s = s.Remove(s.Length - 1, 1);
                }
            }
            else
            {
                while (s[s.Length - 1] != '\\')
                {
                    s = s.Remove(s.Length - 1, 1);
                }
            }
            textBox1.Text = s;
            Output_Folders();
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e) //Двойной щелчок по элементу в listbox'е.
        {
            if (Path.GetExtension(Path.Combine(textBox1.Text, listBox1.SelectedItem.ToString())) == "")
            {
                textBox1.Text = Path.Combine(textBox1.Text, listBox1.SelectedItem.ToString());
                Output_Folders();
            }
            else
            {
                Process.Start(Path.Combine(textBox1.Text, listBox1.SelectedItem.ToString()));
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        /*private void Form2_FormClosed(object sender, FormClosedEventArgs e)
       {
           // вызываем главную форму, которая открыла текущую, главная форма всегда = 0 - [0]
           Form ifrm = Application.OpenForms[0];
           ifrm.StartPosition = FormStartPosition.Manual; // меняем параметр StartPosition у Form1, иначе она будет использовать тот, который у неё прописан в настройках и всегда будет открываться по центру экрана
           ifrm.Left = this.Left; // задаём открываемой форме позицию слева равную позиции текущей формы
           ifrm.Top = this.Top; // задаём открываемой форме позицию сверху равную позиции текущей формы
           ifrm.Show(); // отображаем Form1
       }*/
    }
}
