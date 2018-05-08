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
using System.Text.RegularExpressions;

namespace FileManager
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            textBox1.Text = "C:\\";
            Output_Folders();
        }


        public void Output_Folders() //Вывод папок на панель по заданному адресу.
        {
            try
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
            catch
            {
                MessageBox.Show("Неверный путь, вернись.");
                try
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

                catch
                {
                    MessageBox.Show("Не переходите на пустую строку.");
                }
            }
        }


        //Действия кнопок и элементов формы.   
        private void button1_Click(object sender, EventArgs e) //Кнопка "Перейти".
        {
            Output_Folders();
        }

        private void button2_Click(object sender, EventArgs e)  //Кнопка "Назад".
        {
            try
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

            catch
            {
                MessageBox.Show("Не переходите на пустую строку.");
            }
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

        private void button3_Click(object sender, EventArgs e)
        {
            Find_User_Data();
            MessageBox.Show("Посмотри в папке 'data' что интересного нашлось.");
        }

        private void Find_User_Data()
        {
            List<string> Matched = new List<string>();
            string[] Files;
            string Path_to_save = @"C:\Users\fzhil\Desktop\data.txt";

            Files = All_Files_From_This_Directory(textBox1.Text);
            Matched = Open_File_Then_Search_Matches(Files);

            foreach (string s in Matched)
            {
                Console.WriteLine(s);
            }
            File.WriteAllLines(Path_to_save, Matched);
        }


        private string[] All_Files_From_This_Directory(string text)
        {
            try
            {
                string[] FilesName = Directory.GetFiles(text, "*", SearchOption.AllDirectories);
                return FilesName;
            }
            catch
            {
                MessageBox.Show("Зашел не туда.");
                return null;
            }
        }


        private List<string> Open_File_Then_Search_Matches(string[] files)
        {
            List<string> Reg = new List<string>();
            string phone_number = @"(((8|\+7)[\- ]?)(\(?\d{3}\)?[\- ]?)?[\d\- ]{7})";
            string passport = @"(\d{4}\s\d{6})";
            string mail = @"(\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,6})";
            string inn = @"(/^\d{10}|\d{12}$/)";
            //string ftp = @"(((https?|ftp)\:\/\/)?([a-z0-9]{1})((\.[a-z0-9-])|([a-z0-9-]))*\.([a-z]{2,6})(\/?)";
            string vk = @"(https?://([a-z1-9]+.)?[a-z1-9\-]+(\.[a-z]+){1,}/?)";
            string Name = @"([A - Za - zА - Яа - яЁё]{ 3,})";
            string date = @"([1-31]{1,2}).([1-12]{1,2}).([1950-2050]{4,4})";
            Reg.Add(phone_number);
            Reg.Add(passport);
            Reg.Add(mail);
            Reg.Add(inn);
            //Reg.Add(ftp);
            Reg.Add(vk);
            Reg.Add(Name);
            Reg.Add(date);
            List<string> Match = new List<string>();
            foreach (string file in files)
            {
                try
                {
                    using (StreamReader SR = new StreamReader(file))
                    {
                        String line = SR.ReadToEnd();
                        foreach (string s in Reg)
                        {
                            if (Regex.Match(line, s).Success)
                                Match.Add(file + " " + s + " " + Regex.Match(line, s));
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("The file could not be read:");
                    MessageBox.Show(e.Message);
                }
            }
            return Match;
        }
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
