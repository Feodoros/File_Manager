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
using Ionic.Zip;

namespace FileManager
{
    public partial class Form2 : Form
    {
        string[] Arr_Of_Files_For_Status_Bar;
        int Number_Of_Cores;
        List<Thread> threads;
        static public int x = 0;
        int y = 0;
        //string[] Disks = { "C:\\", "D:\\ " };
        string s = "";

        public Form2()
        {
            InitializeComponent();

            //listBox1.Items.AddRange(Disks);
            OutputDisks();
            Number_Of_Cores = Environment.ProcessorCount;
            threads = new List<Thread>();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            Number_Of_Cores = Environment.ProcessorCount;
            threads = new List<Thread>();

            listView1.View = View.Details;
        }


        public void OutputDisks()
        {
            try
            {
                listView1.Items.Clear();

                ImageList images = new ImageList();
                images.ImageSize = new Size(25, 25);

                images.Images.Add(Bitmap.FromFile("C:/Users/fzhil/source/repos/File_Manager/Images/2.png"));

                listView1.LargeImageList = images;

                DriveInfo[] Drives = DriveInfo.GetDrives();

                foreach (DriveInfo drive in Drives)
                {
                    ListViewItem listview = new ListViewItem();
                    listview.ImageIndex = 0;
                    listview.Text = drive.Name;
                    listview.Tag = "drive";
                    listView1.Items.Add(listview);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка.");
            }
        }//Вывод дисков в окно ListView.

        public void UpdateList()
        {
            try
            {
                listView1.Items.Clear();

                ImageList images = new ImageList();
                images.ImageSize = new Size(25, 25);

                images.Images.Add(Bitmap.FromFile("C:/Users/fzhil/source/repos/File_Manager/Images/1.png"));
                images.Images.Add(Bitmap.FromFile("C:/Users/fzhil/source/repos/File_Manager/Images/2.png"));

                listView1.LargeImageList = images;
                DirectoryInfo dir = new DirectoryInfo(textBox1.Text);
                DirectoryInfo[] Directories = dir.GetDirectories();
                FileInfo[] files = dir.GetFiles();

                foreach (DirectoryInfo dirs in Directories)
                {
                    ListViewItem listview = new ListViewItem();
                    listview.ImageIndex = 1;
                    listview.Text = dirs.Name;
                    listview.Tag = "directory";
                    listView1.Items.Add(listview);
                }

                foreach (FileInfo i in files)
                {
                    ListViewItem listView = new ListViewItem();
                    listView.ImageIndex = 0;
                    listView.Text = i.Name;
                    listView.Tag = "file";
                    listView1.Items.Add(listView);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка.");
                textBox1.Text = "";
                OutputDisks();
            }
        }//Вывод папок и файлов в окно ListView.



        /*public void Output_Folders() 
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
                    //MessageBox.Show("Не переходите на пустую строку.");
                    listBox1.Items.AddRange(Disks);
                    //OutputDisks();
                }
            }
        }*/


        //Действия кнопок и элементов формы.   
        private void button1_Click(object sender, EventArgs e) //Кнопка "Перейти".
        {
            //Output_Folders();
            UpdateList();
            if (textBox1.Text == "")
                OutputDisks();
        }

        private void button2_Click(object sender, EventArgs e)  //Кнопка "Назад".
        {
            try
            {
                string s = textBox1.Text;
                if (s != "")
                {
                    if (s == "C:\\" || s == "D:\\" || s == "E:\\")
                    {
                        // listBox1.Items.Clear();
                        //string[] Disks = { "C:\\", "D:\\ " };
                        // listBox1.Items.AddRange(Disks);
                        textBox1.Text = "";
                        OutputDisks();
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
                        //Output_Folders();
                        UpdateList();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка.");
            }
        }

        private void listView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (File.Exists(Path.Combine(textBox1.Text + "\\" + listView1.SelectedItems[0].Text)))
                    File.Delete(Path.Combine(textBox1.Text + "\\" + listView1.SelectedItems[0].Text));
                else
                    Directory.Delete(Path.Combine(textBox1.Text + "\\" + listView1.SelectedItems[0].Text), true);
                MessageBox.Show("Удаление завершено.");
                UpdateList();
            }
        }//Клавиша Delete

        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    if (Path.GetExtension(Path.Combine(textBox1.Text, listView1.SelectedItems[0].Text)) == "")
                    {
                        textBox1.Text = Path.Combine(textBox1.Text, listView1.SelectedItems[0].Text);
                        UpdateList();
                    }
                    else
                    {
                        Process.Start(Path.Combine(textBox1.Text, listView1.SelectedItems[0].Text));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }//Клавиша Enter

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (Path.GetExtension(Path.Combine(textBox1.Text, listView1.SelectedItems[0].Text)) == "" || Path.GetExtension(Path.Combine(textBox1.Text, listView1.SelectedItems[0].Text)) == ".Архив")
                {
                    textBox1.Text = Path.Combine(textBox1.Text, listView1.SelectedItems[0].Text);
                    UpdateList();
                }
                else
                {
                    Process.Start(Path.Combine(textBox1.Text, listView1.SelectedItems[0].Text));
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Ошибка.");
            }
        }//Двойной щелчок по элементу в listView'е 1.       

        private void listView2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                Process.Start(Path.Combine(textBox1.Text, listView2.SelectedItems[0].Text));
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Ошибка.");
            }
        }//Двойной щелчок по элементу в listView'е 2.

        /*private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e) //Двойной щелчок по элементу в listbox'е.
        {
            try
            {
                if (Path.GetExtension(Path.Combine(textBox1.Text, listBox1.SelectedItem.ToString())) == "")
                {
                    textBox1.Text = Path.Combine(textBox1.Text, listBox1.SelectedItem.ToString());
                    //Output_Folders();
                }
                else
                {
                    Process.Start(Path.Combine(textBox1.Text, listBox1.SelectedItem.ToString()));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }*/

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


        private string[] All_Files_From_This_Directory(string text)
        {
            try
            {
                string[] FilesName = Directory.GetFiles(text, "*.*", SearchOption.AllDirectories);
                return FilesName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка.");
                return null;
            }
        }

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
            //var watch = System.Diagnostics.Stopwatch.StartNew();

            {
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
                    UpdateStatusBar();
                }
            }

            return Match;
        }

        public event Action UpdateStatusBar;

        public void Bar_Update()
        {
            var watch = Stopwatch.StartNew();
            Action action = () =>
           {
               progressBar1.Maximum = 500; //Arr_Of_Files_For_Status_Bar.Length;  
               if (progressBar1.Value == 500)
               {
                   MessageBox.Show("Посмотри в папке 'data' что интересного нашлось.");

               }
               if (progressBar1.Value < x && x < 500)
                   progressBar1.Value = x;
               if (watch.ElapsedMilliseconds >= 7000)
               {
                   MessageBox.Show("Посмотри в папке 'data' что интересного нашлось.");
                   progressBar1.Value = 500;
               }
           };

            Invoke(action);
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

        //Ищем файлы по названию (частичному совпадению) в данной директории и в дочерних директориях (используем FindAll и фильтрацию search pattern GetFiles). 
        private void Search_By_Name()
        {

            Action action = () =>
            {
                try
                {
                    ImageList images = new ImageList();
                    images.ImageSize = new Size(25, 25);

                    images.Images.Add(Bitmap.FromFile("C:/Users/fzhil/source/repos/File_Manager/Images/1.png"));

                    listView2.LargeImageList = images;

                    string[] AllFilesInThisDir1 = Directory.GetFiles(textBox1.Text, textBox2.Text, SearchOption.AllDirectories);

                    string[] AllFilesInThisDir = All_Files_From_This_Directory(textBox1.Text);
                    List<string> s1 = AllFilesInThisDir.ToList().FindAll(i => i.Contains(textBox2.Text) == true);

                    foreach (string d in AllFilesInThisDir1)
                    {

                    }

                    if (AllFilesInThisDir1.Length != 0)
                        foreach (string sd in AllFilesInThisDir1)
                        {
                            ListViewItem listview = new ListViewItem();
                            listview.ImageIndex = 0;
                            listview.Text = sd;
                            listview.Tag = "file";
                            listView2.Items.Add(listview);
                        }

                    else
                        foreach (string sd in s1)
                        {
                            ListViewItem listview = new ListViewItem();
                            listview.ImageIndex = 0;
                            listview.Text = sd;
                            listview.Tag = "file";
                            listView2.Items.Add(listview);
                        }
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.ToString());
                }
            };
            Invoke(action);
        }

        //При изменении текста в текстбоксе будет выполняться метод снова + чистка листбокса для дальнейшего вывода на "чистое".
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (y != 0)
            {
                listView2.Items.Clear();
                Thread thread = new Thread(Search_By_Name);
                thread.Start();
                //Search_By_Name();
            }
            y = 1;
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
        }

        /*private void listBox1_RightClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (File.Exists(textBox1.Text + "\\" + listBox1.SelectedItem.ToString()))
                {
                    string s = listBox1.SelectedItem.ToString();

                    while (s[s.Length - 1] != '.')
                    {
                        s = s.Remove(s.Length - 1, 1);
                    }
                    s = s.Remove(s.Length - 1, 1);
                    toolStripTextBox1.Text = s;
                }
                else
                    toolStripTextBox1.Text = listBox1.SelectedItem.ToString();

                contextMenuStrip1.Show(MousePosition, ToolStripDropDownDirection.Right);
            }
        }*/


        private void listView1_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (File.Exists(textBox1.Text + "\\" + listView1.SelectedItems[0].Text))
                    {
                        string s = listView1.SelectedItems[0].Text;

                        while (s[s.Length - 1] != '.')
                        {
                            s = s.Remove(s.Length - 1, 1);
                        }
                        s = s.Remove(s.Length - 1, 1);
                        toolStripTextBox1.Text = s;
                    }
                    else
                        toolStripTextBox1.Text = listView1.SelectedItems[0].Text;

                    contextMenuStrip1.Show(MousePosition, ToolStripDropDownDirection.Right);
                }
                else
                {
                    if (e.Button == MouseButtons.XButton1)
                    {
                        string s = textBox1.Text;
                        if (s != "")
                        {
                            if (s == "C:\\" || s == "D:\\" || s == "E:\\")
                            {
                                // listBox1.Items.Clear();
                                //string[] Disks = { "C:\\", "D:\\ " };
                                // listBox1.Items.AddRange(Disks);
                                textBox1.Text = "";
                                OutputDisks();
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
                                //Output_Folders();
                                UpdateList();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        } //Открытие контекстного меню

        bool Copy_Or_Move = true;

        //Действия кнопок контекстного меню.
        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {
            // toolStripTextBox1.Text = "";            
        }

        private void удалитьToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (File.Exists(Path.Combine(textBox1.Text + "\\" + listView1.SelectedItems[0].Text)))
                File.Delete(Path.Combine(textBox1.Text + "\\" + listView1.SelectedItems[0].Text));
            else
                Directory.Delete(Path.Combine(textBox1.Text + "\\" + listView1.SelectedItems[0].Text), true);
            MessageBox.Show("Удаление завершено.");
            //Output_Folders();
            UpdateList();
        }

        private void копироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Copy_Or_Move = true;
            s = textBox1.Text + "\\" + listView1.SelectedItems[0].Text;
        }

        private void архивацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Archiving();
            MessageBox.Show("Архивация прошла успешно.");
            // Output_Folders();
            UpdateList();
        }

        private void переименовать_Click(object sender, EventArgs e)
        {
            string d = toolStripTextBox1.Text;
            string s = textBox1.Text + "\\" + listView1.SelectedItems[0].Text;
            string ext = Path.GetExtension(s);

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
            if (File.Exists(Path.Combine(textBox1.Text + "\\" + listView1.SelectedItems[0].Text)))
                File.Move(textBox1.Text + "\\" + listView1.SelectedItems[0].Text, s + d + ext);
            else
                Directory.Move(textBox1.Text + "\\" + listView1.SelectedItems[0].Text, s + d);

            // Output_Folders();
            UpdateList();
        }

        private void вставитьtoolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string d = textBox1.Text + "\\" + listView1.SelectedItems[0].Text;
            if (Copy_Or_Move)
            {
                try
                {
                    if (File.Exists(s))
                        File.Copy(s, d + "/" + Path.GetFileName(s));
                    else
                        CopyDir(s, d + "/" + Path.GetFileName(s));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                try
                {
                    if (File.Exists(s))
                        File.Move(s, d + "/" + Path.GetFileName(s));
                    else
                        Directory.Move(s, d + "/" + Path.GetFileName(s));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            MessageBox.Show("Сделано.");
            //Output_Folders();
            UpdateList();
        }

        private void вырезатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Copy_Or_Move = false;
            s = textBox1.Text + "\\" + listView1.SelectedItems[0].Text;
        }



        void CopyDir(string FromDir, string ToDir)
        {
            Directory.CreateDirectory(ToDir);
            foreach (string s1 in Directory.GetFiles(FromDir))
            {
                string s2 = ToDir + "\\" + Path.GetFileName(s1);
                File.Copy(s1, s2);
            }
            foreach (string s in Directory.GetDirectories(FromDir))
            {
                CopyDir(s, ToDir + "\\" + Path.GetFileName(s));
            }
        }//Копирование каталога со всеми файлами.

        public void Archiving()
        {
            if (File.Exists(listView1.SelectedItems[0].Text))
            {
                string ArchivedFile = textBox1.Text + "\\" + listView1.SelectedItems[0].Text + ".zip";
                string PathOfFile = listView1.SelectedItems[0].Text;
                Archiving_One_By_One_File(PathOfFile, ArchivedFile);
            }

            else
            {
                Directory.CreateDirectory(textBox1.Text + "\\" + listView1.SelectedItems[0].Text + ".Архив");

                DirectoryInfo directory = new DirectoryInfo(textBox1.Text + "\\" + listView1.SelectedItems[0].Text);

                FileInfo[] files = directory.GetFiles("*", SearchOption.AllDirectories);
                string s = listView1.SelectedItems[0].Text;

                Parallel.ForEach(files, (currentFile) =>
                {
                    string ArchivedFile = textBox1.Text + "\\" + s + ".Архив" + "\\" + currentFile.ToString() + ".zip";
                    string PathOfFile = currentFile.FullName;
                    Archiving_One_By_One_File(PathOfFile, ArchivedFile);
                });
            }

        }//Archiving

        public void Archiving_One_By_One_File(string PathOfFile, string ArchivedFile)
        {
            using (FileStream Start = new FileStream(PathOfFile, FileMode.OpenOrCreate))
            {
                using (FileStream End = File.Create(ArchivedFile))
                {
                    using (GZipStream Archiving = new GZipStream(End, CompressionMode.Compress))
                    {
                        Start.CopyTo(Archiving);
                    }
                }
            }
        }//Арихвиация файлов по одному.        





        /*private void Form2_FormClosed(object sender, FormClosedEventArgs e)
              {
              C:\Users\fzhil\Documents\проверка
                  // вызываем главную форму, которая открыла текущую, главная форма всегда = 0 - [0]
                  Form ifrm = Application.OpenForms[0];
                  ifrm.StartPosition = FormStartPosition.Manual; // меняем параметр StartPosition у Form1, иначе она будет использовать тот, который у неё прописан в настройках и всегда будет открываться по центру экрана
                  ifrm.Left = this.Left; // задаём открываемой форме позицию слева равную позиции текущей формы
                  ifrm.Top = this.Top; // задаём открываемой форме позицию сверху равную позиции текущей формы
                  ifrm.Show(); // отображаем Form1
              }*/

    }
}

