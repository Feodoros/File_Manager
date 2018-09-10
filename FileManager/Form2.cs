﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Threading;
using System.Diagnostics;

namespace FileManager
{
    public partial class Form2 : Form
    {
        string[] Arr_Of_Files_For_Status_Bar;
        int Number_Of_Cores;
        List<Thread> threads;
        static public int x = 0;
        int y = 0;
        private static int varthgCount;

        public Form2()
        {
            InitializeComponent();
            //textBox1.Text = "C:\\";
            //Output_Folders();
            string[] Disks = { "C:\\", "D:\\ " };
            listBox1.Items.AddRange(Disks);
            Number_Of_Cores = Environment.ProcessorCount;
            threads = new List<Thread>();

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            Number_Of_Cores = Environment.ProcessorCount;
            threads = new List<Thread>();
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
                if (s == "C:\\" || s == "D:\\")
                {
                    listBox1.Items.Clear();
                    string[] Disks = { "C:\\", "D:\\ " };
                    listBox1.Items.AddRange(Disks);
                    textBox1.Text = "";
                }
                else
                {
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

            }

            catch when (textBox1.Text == "")
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
            /*Thread thread = new Thread(Find_User_Data);
            thread.Start();*/
        }

        private void Find_User_Data()
        {
            string[] Files;
            List<string> Matched = new List<string>();
            string Path_To_Save = @"C:\Users\fzhil\Desktop\data.txt";

            Files = All_Files_From_This_Directory(textBox1.Text);
            Arr_Of_Files_For_Status_Bar = Files;

            List<List<string>> List = Break_Up_Files_Into_List(Files.ToList(), Files.Length / (Number_Of_Cores));

            Parallel.ForEach(Files, currentFile => Open_File_Then_Search_Matches(Files, Files.Length));

         /* foreach (List<string> i in List)
            {                 
                
                Thread thread = new Thread(() => Open_File_Then_Search_Matches(i.ToArray(), Files.Length));
                thread.Start();
            }*/
            Matched = Open_File_Then_Search_Matches(Files, Files.Length / (Number_Of_Cores));
            File.WriteAllLines(Path_To_Save, Matched);
            MessageBox.Show("Посмотри в папке 'data' что интересного нашлось.");
        }

        /*private int Count_Files()
        {
            bool fl = true;
            if (fl == true)
            {
                foreach (var file in Directory.EnumerateFiles(textBox1.Text, "*", SearchOption.AllDirectories))
                {
                    y++;
                }
                fl = false;
            }
            return y;
        }*/


        private string[] All_Files_From_This_Directory(string text)
        {
            try
            {
                string[] FilesName = Directory.GetFiles(text, "*.*", SearchOption.AllDirectories);
                return FilesName;
            }
            catch
            {
                MessageBox.Show("Зашел не туда.");
                return null;
            }
        }

        /*public static List<List<string>> Break_Up_Files_Into_List(List<string> Loc, int n)
        {
            var List = new List<List<string>>();

            for (int i = 0; i < Loc.Count; i += n)
            {
                List.Add(Loc.GetRange(i, Math.Min(n, Loc.Count - i)));
            }

            return List;
        }*/

        private List<string> Open_File_Then_Search_Matches(string[] Files, int Num)
        {
            var NumberFiles = Files.Length;
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
            UpdateStatusBar += Bar_Update;
            foreach (string file in Files)
            {
                try
                {
                    using (StreamReader sr = new StreamReader(file))
                    {
                        String line = sr.ReadToEnd();

                        foreach (string s in Reg)
                        {

                            if (Regex.Match(line, s).Success)
                            {
                                Match.Add(file + "   " + s + "   " + Regex.Match(line, s));
                            }
                        }
                    }
                }

                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }

                Interlocked.Increment(ref x);                
                //UpdateStatusBar();
            }

            return Match;
        }


        public event Action UpdateStatusBar;

       public void Bar_Update()
        {
             Action action = () =>
            {
                progressBar1.Maximum = Arr_Of_Files_For_Status_Bar.Length;
                progressBar1.Value = x;
            };

            Invoke(action);
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        static IEnumerable<ICollection<T>> Break_Up_Files_Into_List<T>(IEnumerable<T> src, int maxItems)
        {
            var list = new List<T>();
            foreach (var t in src)
            {
                list.Add(t);
                if (list.Count == 5)
                {
                    yield return list;
                    list = new List<T>();
                }
            }
            if (list.Count > 0)
                yield return list;
        }

        public static List<List<T>> Break_Up_Files_Into_List<T>(List<T> collection, int size)
        {
            var thg = new List<List<T>>();
            var thgCount = collection.Count() / size;

            if (collection.Count % size > 0)
                thgCount++;

            for (var i = 0; i < thgCount; i++)
                thg.Add(collection.Skip(i * size).Take(size).ToList());

            return thg;
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
