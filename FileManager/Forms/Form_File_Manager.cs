using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Compression;
using System.Threading;
using System.Diagnostics;

namespace FileManager
{
    public partial class Form_File_Manager : Form
    {                   
        bool Clear_TextBox = false;
        string From_Dir = "";

        public Form_File_Manager()
        {
            InitializeComponent();

            OutputDisks();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            listView1.View = View.Details;
        }



        //----Методы формы----//
        public void OutputDisks()
        {
            try
            {
                listView1.Items.Clear();

                ImageList images = new ImageList();
                images.ImageSize = new Size(25, 25);

                images.Images.Add(Bitmap.FromFile("C:/Users/fzhil/source/repos/File_Manager/Images/2.png"));

                listView1.LargeImageList = images;

                var Drives = new Forms.Folder(" ").GetDriveName();

                foreach (var drive in Drives)
                {
                    ListViewItem listview = new ListViewItem();
                    listview.ImageIndex = 0;
                    listview.Tag = "drive";
                    listview.Text = drive.ToString();
                    listView1.Items.Add(listview);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка.");
            }
        }//Вывод дисков в окно ListView.

        public void UpdateListView()
        {
            try
            {
                listView1.Items.Clear();

                ImageList images = new ImageList();
                images.ImageSize = new Size(25, 25);

                images.Images.Add(Bitmap.FromFile("C:/Users/fzhil/source/repos/File_Manager/Images/1.png"));
                images.Images.Add(Bitmap.FromFile("C:/Users/fzhil/source/repos/File_Manager/Images/2.png"));

                listView1.LargeImageList = images;

                var Directories = new Forms.Folder(textBox1.Text).GetDirectories();
                var files = new Forms.Folder(textBox1.Text).Get_Files_In_Selected_Folder();

                foreach (var dir in Directories)
                {
                    ListViewItem listview = new ListViewItem();
                    listview.ImageIndex = 1;
                    var name_Of_File_Or_Directory = new Forms.Folder(dir.ToString()).GetName();
                    listview.Text = name_Of_File_Or_Directory.ToString();
                    listview.Tag = "directory";
                    listView1.Items.Add(listview);
                }

                foreach (var file in files)
                {
                    ListViewItem listView = new ListViewItem();
                    listView.ImageIndex = 0;
                    var name_Of_File_Or_Directory = new Forms.Folder(file.ToString()).GetName();
                    listView.Text = name_Of_File_Or_Directory.ToString();
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

        public void Back_To_Future()
        {
            try
            {
                string s = textBox1.Text;
                if (s != "")
                {
                    if (s == "C:\\" || s == "D:\\" || s == "E:\\")
                    {
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
                        UpdateListView();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка.");
            }
        }//Вернуться обратно.             

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

                    string[] AllFilesInThisDir1 = new Forms.Folder(textBox1.Text).Get_Files_With_Filter(textBox2.Text);

                    string[] AllFilesInThisDir = new Forms.Folder(textBox1.Text).Get_ALL_Files_From_All_Inner_Dir();
                    List<string> List_AllFilesInThisDir = AllFilesInThisDir.ToList().FindAll(i => i.Contains(textBox2.Text) == true);

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
                        foreach (string sd in List_AllFilesInThisDir)
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

        void CopyDir(string FromDir, string ToDir)
        {
            new Forms.Folder("").Create_Directory(ToDir);

            foreach (string s1 in new Forms.Folder(FromDir).Get_Files_In_Selected_Folder())
            {
                string s2 = ToDir + "\\" + new Forms.Folder(s1.ToString()).GetName().ToString();
                new Forms.File(s1).Copy(s2, true);
            }
            foreach (string s in new Forms.Folder(FromDir).GetDirectories())
            {
                CopyDir(s, ToDir + "\\" + new Forms.Folder(s.ToString()).GetName().ToString());
            }
        }//Копирование каталога со всеми файлами.

        public void Archiving()
        {
            if (new Forms.File(listView1.SelectedItems[0].Text).Existing())
            {
                string ArchivedFile = textBox1.Text + "\\" + listView1.SelectedItems[0].Text + ".zip";
                string PathOfFile = listView1.SelectedItems[0].Text;
                Archiving_One_By_One_File(PathOfFile, ArchivedFile);
            }

            else
            {
                new Forms.Folder("").Create_Directory(textBox1.Text + "\\" + listView1.SelectedItems[0].Text + ".Архив");

                var files = new Forms.Folder(textBox1.Text + "\\" + listView1.SelectedItems[0].Text).Get_ALL_Files_From_All_Inner_Dir();
                string s = listView1.SelectedItems[0].Text;

                Parallel.ForEach(files, (currentFile) =>
                {
                    string ArchivedFile = textBox1.Text + "\\" + s + ".Архив" + "\\" + currentFile.ToString() + ".zip";
                    var PathOfFile = new Forms.Folder("").GetFullName(currentFile.ToString());
                    Archiving_One_By_One_File(PathOfFile, ArchivedFile);
                });
            }

        }//Archiving

        public void Archiving_One_By_One_File(string PathOfFile, string ArchivedFile)
        {
            using (var Start = new Forms.File(PathOfFile).FileStreamCreate())
            {
                using (var End = new Forms.File(ArchivedFile).Create())
                {
                    using (GZipStream Archiving = new GZipStream(End, CompressionMode.Compress))
                    {
                        Start.CopyTo(Archiving);
                    }
                }
            }
        }//Арихвиация файлов по одному.        
        //----Методы формы----//



        //----Действия кнопок и элементов формы----//   
        private void button1_Click(object sender, EventArgs e) //Кнопка "Перейти".
        {
            UpdateListView();
            if (textBox1.Text == "")
                OutputDisks();
        }

        private void button2_Click(object sender, EventArgs e)  //Кнопка "Назад".
        {
            Back_To_Future();
        }

        private void listView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                var Exist_Dir = new Forms.Folder(textBox1.Text + "\\" + listView1.SelectedItems[0].Text).Existing();
                var Exist_File = new Forms.File(textBox1.Text + "\\" + listView1.SelectedItems[0].Text).Existing();

                if (Exist_Dir)
                    new Forms.Folder(textBox1.Text + "\\" + listView1.SelectedItems[0].Text).Delete(Exist_Dir);

                if (Exist_File)
                    new Forms.File(textBox1.Text + "\\" + listView1.SelectedItems[0].Text).Delete();

                MessageBox.Show("Удаление завершено.");
                UpdateListView();
            }
        }//Клавиша Delete

        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    var Path_Selected_Item = new Forms.Folder("").Combine(textBox1.Text, listView1.SelectedItems[0].Text);
                    var Ext_Selected_Item = new Forms.Folder(Path_Selected_Item).GetExtension();

                    if (Ext_Selected_Item == "")
                    {
                        textBox1.Text = Path_Selected_Item;
                        UpdateListView();
                    }
                    else
                    {
                        new Forms.File("").Start(Path_Selected_Item);
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
                var Path_Selected_Item = new Forms.Folder("").Combine(textBox1.Text, listView1.SelectedItems[0].Text);
                var Ext_Selected_Item = new Forms.Folder(Path_Selected_Item).GetExtension();

                if (Ext_Selected_Item == "")
                {
                    textBox1.Text = Path_Selected_Item;
                    UpdateListView();
                }
                else
                {
                    new Forms.File("").Start(Path_Selected_Item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }//Двойной щелчок по элементу в listView'е 1.       

        private void listView2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var Path_Selected_Item = new Forms.Folder("").Combine(textBox1.Text, listView1.SelectedItems[0].Text);

            try
            {
                new Forms.File("").Start(Path_Selected_Item);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Ошибка.");
            }
        }//Двойной щелчок по элементу в listView'е 2.

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string Name = textBox1.Text + "\\" + listView1.SelectedItems[0].Text;
                (new ParallelSearchingRegFiles(new SearchHandler())).StartToSearch(Name);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }//Поиск информации

        //При изменении текста в текстбоксе будет выполняться метод снова + чистка листбокса для дальнейшего вывода на "чистое".
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (Clear_TextBox != false)
            {
                listView2.Items.Clear();
                Thread thread = new Thread(Search_By_Name);
                thread.Start();
            }
            Clear_TextBox = true;
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
        }

        private void listView1_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (new Forms.File(textBox1.Text + "\\" + listView1.SelectedItems[0].Text).Existing())
                    {
                        string s = listView1.SelectedItems[0].Text;

                        while (s[s.Length - 1] != '.')
                        {
                            s = s.Remove(s.Length - 1, 1);
                        }
                        s = s.Remove(s.Length - 1, 1);
                        toolStripTextBox1.Text = s;
                        toolStripMenuItem1.Visible = true; ;
                        toolStripTextBox2.Visible = false;
                        toolStripSeparator3.Visible = true;
                        toolStripTextBox2.Text = "Длина слова: ";
                    }
                    else
                    {
                        toolStripTextBox1.Text = listView1.SelectedItems[0].Text;
                        toolStripMenuItem1.Visible = false;
                        toolStripSeparator3.Visible = false;
                        toolStripTextBox2.Visible = false;
                    }
                    contextMenuStrip1.Show(MousePosition, ToolStripDropDownDirection.Right);

                }
                else
                {
                    if (e.Button == MouseButtons.XButton1)
                    {
                        Back_To_Future();

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        } //Открытие контекстного меню
        //----Действия кнопок и элементов формы----//



        //----Действия кнопок контекстного меню----//
        bool Copy_Or_Move = true;

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {
            // toolStripTextBox1.Text = "";            
        }

        private void удалитьToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            var Exist_Dir = new Forms.Folder(textBox1.Text + "\\" + listView1.SelectedItems[0].Text).Existing();
            var Exist_File = new Forms.File(textBox1.Text + "\\" + listView1.SelectedItems[0].Text).Existing();

            if (Exist_Dir)
                new Forms.Folder(textBox1.Text + "\\" + listView1.SelectedItems[0].Text).Delete(Exist_Dir);

            if (Exist_File)
                new Forms.File(textBox1.Text + "\\" + listView1.SelectedItems[0].Text).Delete();

            MessageBox.Show("Удаление завершено.");
            UpdateListView();
        }

        private void копироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Copy_Or_Move = true;
            From_Dir = textBox1.Text + "\\" + listView1.SelectedItems[0].Text;
        }

        private void архивацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ArchivingFile file = new ArchivingFile();

            string Path = textBox1.Text + "\\" + listView1.SelectedItems[0].Text;

            file.Pack(Path);
            MessageBox.Show("Архивация прошла успешно.");
            UpdateListView();
        }

        private void переименовать_Click(object sender, EventArgs e)
        {
            string d = toolStripTextBox1.Text;
            string s = textBox1.Text + "\\" + listView1.SelectedItems[0].Text;
            var ext = new Forms.File(s).GetExtension();

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
            if (new Forms.File(textBox1.Text + "\\" + listView1.SelectedItems[0].Text).Existing())
                new Forms.File(textBox1.Text + "\\" + listView1.SelectedItems[0].Text).Move(s + d + ext);
            else
                new Forms.Folder(textBox1.Text + "\\" + listView1.SelectedItems[0].Text).Move(s + d + ext);

            UpdateListView();
        }

        private void вставитьtoolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string d = textBox1.Text + "\\" + listView1.SelectedItems[0].Text;
            if (Copy_Or_Move)
            {
                try
                {
                    if (new Forms.File(From_Dir).Existing())
                        new Forms.File(From_Dir).Copy(d + "/" + new Forms.File(From_Dir).GetName(), true);
                    else
                        CopyDir(From_Dir, d + "/" + new Forms.Folder(From_Dir).GetName());
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
                    if (new Forms.File(From_Dir).Existing())
                        new Forms.File(From_Dir).Move(d + "/" + new Forms.File(From_Dir).GetName());
                    else
                        new Forms.Folder(From_Dir).Move(d + "/" + new Forms.File(From_Dir).GetName());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            MessageBox.Show("Сделано.");
            UpdateListView();
        }

        private void вырезатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Copy_Or_Move = false;
            From_Dir = textBox1.Text + "\\" + listView1.SelectedItems[0].Text;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

            string path = textBox1.Text + "\\" + listView1.SelectedItems[0].Text;

            if (new Forms.File(path).Existing())
            {
                try
                {
                    var WordsLength = Int16.Parse(toolStripTextBox2.Text);
                    string Final = "";
                    var CountLines = 0;
                    var CountUnic = 0;


                    Stopwatch stopwatch = Stopwatch.StartNew();


                    Task taskLines = Task.Run(() =>
                    {
                        using (var reader = new Forms.File("").Open_Text(path))
                        {
                            while (reader.ReadLine() != null)
                                CountLines++;
                        }
                        Final += "Количество строк: " + CountLines + "\n";
                    });


                    Task taskWords = Task.Run(() =>
                    {
                        int counter = 1;
                        byte[] bytesInText = new Forms.File("").Read_All_Bytes(path);
                        string ChangedTextInFile = Encoding.Default.GetString(bytesInText).ToLower().Replace(",", "").Replace(".", "").Replace("(", "").Replace(")", "").Replace("-", "");
                        string[] Words = ChangedTextInFile.Split();

                        Final += "Количество слов: " + Words.Length + "\n";


                        CountUnic = (from word in Words.AsParallel() select word).Distinct().Count();
                        Final += "Количество уникальных слов: " + CountUnic + "\n";


                        var WordGroups = Words.GroupBy(s => s).Where(g => g.Count() > 1).OrderByDescending(g => g.Count()).Select(g => g.Key).ToList();
                        WordGroups.Remove("");

                        var ListWords = (from word in WordGroups where word.Length > WordsLength select word);
                        var topTenWords = ListWords.Take(10);

                        Final += "Самые популярные слова длины большей " + WordsLength + ":\n";


                        foreach (var word in topTenWords)
                        {
                            Final += counter + ": " + word + "\n";
                            counter++;
                        }
                    });


                    var finalTask = Task.Factory.ContinueWhenAll(new Task[] { taskLines, taskWords }, ant =>
                    {
                        stopwatch.Stop();
                        Final += "Время работы: " + stopwatch.Elapsed.TotalSeconds + " секунд.";
                        MessageBox.Show(Final, "Текстовая статистика.", MessageBoxButtons.OK);
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Может быть выбран только текстовый файл.", "Предупреждение.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }//Статистика текстового файла.

        private void toolStripMenuItem1_MouseEnter(object sender, EventArgs e)
        {
            toolStripTextBox2.Visible = true;
        }

        private void toolStripTextBox2_Click(object sender, EventArgs e)
        {
            toolStripTextBox2.Text = "";
        }

        private void менеджерЗагрузкиToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form formX = new Form_MORE_FUN();
            formX.ShowDialog();
        }

        private void настройкиToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form formX = new Setting_Form(this);
            formX.ShowDialog();
        }
        //----Действия кнопок контекстного меню----//

    }
}

