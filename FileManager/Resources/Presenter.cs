using FileManager.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace FileManager.Resources
{
    class Presenter
    {
        private iView view;

        Md5Hash Cash = new Md5Hash();

        Derypt CesarCrypt;
        Encrypt CesarEncrypt;


        public Presenter(iView view)
        {
            this.view = view;
            CesarCrypt = new Derypt();
            CesarEncrypt = new Encrypt();
            view.Settings_btn += new EventHandler(Open_Settings);
            view.Download_btn += new EventHandler(Open_Downloader);
            view.Rename_btn += new EventHandler(Rename);
            view.UpdateListView += new EventHandler(UpdateList);
            view.DisksOutput += new EventHandler(OutputDisks);
            view.GoBack += new EventHandler(GoBack);
            view.Delete_btn += new EventHandler(Delete);
            view.Archiving_btn += new EventHandler(Archiving);
            view.Paste_btn += new EventHandler(Paste);
            view.Stat_btn += new EventHandler(Stat);
            view.DoubleClick_btn += new EventHandler(Open_Or_Go);
            view.DoubleClick_Right_ListView += new EventHandler(Click_ListView_Second);
            view.Search_btn += new EventHandler(Search);
            view.PatternSearch_box += new EventHandler(Search_By_Name);
            view.Hash_btn += new EventHandler(GetHash);
            view.Create_Folder += new EventHandler(Create_Folder);
            view.Create_File += new EventHandler(Create_File);
            view.Encrypt += new EventHandler(Encrypt);
            view.Decrypt += new EventHandler(Decrypt);

        }

        private void Create_Folder(object sender, EventArgs e)
        {
            var curSelection = Factory.Get(view.getTextBox1);
            if (curSelection is Folder)
            {
                new Folder(view.getTextBox1 + '\\' + "Новая папка").Create();
            }
            if (curSelection is ZipFolder)
            {
                new ZipFolder(view.getTextBox1).CreateFolder("Новая папка");
            }
        }

        private void Create_File(object sender, EventArgs e)
        {
            try
            {
                new Forms.File(view.getTextBox1 + "\\Новый текстовый документ.txt").Create_File();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void GoBack(object sender, EventArgs e)
        {
            try
            {
                string s = view.getTextBox1;
                if (s != "")
                {
                    if (s == "C:\\" || s == "D:\\" || s == "E:\\")
                    {
                        view.getTextBox1 = "";
                        OutputDisks(sender, e);
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

                        view.getTextBox1 = s;
                        UpdateList(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка.");
            }
        }

        private void Open_Settings(object sender, EventArgs e)
        {
            Setting_Form formX = new Setting_Form(view as Form_File_Manager);
            formX.ShowDialog();
        }

        private void Open_Downloader(object sender, EventArgs e)
        {
            Form_MORE_FUN formX = new Form_MORE_FUN();
            formX.ShowDialog();
        }

        private void Rename(object sender, EventArgs e)
        {
            string d = view.getTextRename;

            string s = view.getTextBox1 + "\\" + view.GetSetlistView.SelectedItems[0].Text;
            string path = view.getTextBox1 + "\\" + view.GetSetlistView.SelectedItems[0].Text;

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
            if (new Forms.File(path).Existing())
                new Forms.File(path).Move(s + d + ext);
            else
                new Forms.Folder(path).Move(s + d + ext);
        }

        private void UpdateList(object sender, EventArgs e)
        {
            try
            {
                view.GetSetlistView.Items.Clear();

                ImageList images = new ImageList();
                images.ImageSize = new Size(25, 25);

                images.Images.Add(Properties.Resources.imageFile);
                images.Images.Add(Properties.Resources.imageFolder);

                view.GetSetlistView.LargeImageList = images;

                var Directories = new Forms.Folder(view.getTextBox1).GetDirectories();
                var files = new Forms.Folder(view.getTextBox1).Get_Files_In_Selected_Folder();

                foreach (var dir in Directories)
                {
                    ListViewItem listview = new ListViewItem();
                    listview.ImageIndex = 1;
                    var name_Of_File_Or_Directory = new Forms.Folder(dir.ToString()).GetName();
                    listview.Text = name_Of_File_Or_Directory.ToString();
                    listview.Tag = "directory";
                    view.GetSetlistView.Items.Add(listview);
                }

                foreach (var file in files)
                {
                    ListViewItem listView = new ListViewItem();
                    listView.ImageIndex = 0;
                    var name_Of_File_Or_Directory = new Forms.Folder(file.ToString()).GetName();
                    listView.Text = name_Of_File_Or_Directory.ToString();
                    listView.Tag = "file";
                    view.GetSetlistView.Items.Add(listView);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка.");
                view.getTextBox1 = "";
                OutputDisks(sender, e);
            }
        }

        private void OutputDisks(object sender, EventArgs e)
        {
            try
            {
                view.GetSetlistView.Items.Clear();

                ImageList images = new ImageList();
                images.ImageSize = new Size(25, 25);

                images.Images.Add(Properties.Resources.imageFolder);

                view.GetSetlistView.LargeImageList = images;

                var Drives = new Folder(" ").GetDriveName();

                foreach (var drive in Drives)
                {                    
                    ListViewItem listview = new ListViewItem();                    
                    listview.ImageIndex = 0;
                    listview.Tag = "drive";
                    listview.Text = drive.ToString();
                    view.GetSetlistView.Items.Add(listview);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка.");
            }
        }

        private void Delete(object sender, EventArgs e)
        {
            string path = view.getTextBox1 + "\\" + view.GetSetlistView.SelectedItems[0].Text;

            var Exist_Dir = new Folder(path).Existing();
            var Exist_File = new File(path).Existing();

            if (Exist_Dir)
                new Folder(path).Delete(Exist_Dir);

            if (Exist_File)
                new File(path).Delete();

            MessageBox.Show("Удаление завершено.");
        }

        private void Archiving(object sender, EventArgs e)
        {
            ArchivingFile file = new ArchivingFile();

            string Path = view.getTextBox1 + "\\" + view.GetSetlistView.SelectedItems[0].Text;

            file.Pack(Path);
            MessageBox.Show("Архивация прошла успешно.");
        }

        private void Paste(object sender, EventArgs e)
        {
            string d = view.getTextBox1 + "\\" + view.GetSetlistView.SelectedItems[0].Text;
            bool Copy_Or_Move = view.copy_or_move;
            string s = view.FromDir;
            if (Copy_Or_Move)
            {
                try
                {
                    if (new Forms.File(view.FromDir).Existing())
                        new Forms.File(view.FromDir).Copy(d + "/" + new Forms.File(view.FromDir).GetName(), true);
                    else
                        CopyDir(view.FromDir, d + "/" + new Forms.Folder(view.FromDir).GetName());
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
                    if (new Forms.File(view.FromDir).Existing())
                        new Forms.File(view.FromDir).Move(d + "/" + new Forms.File(view.FromDir).GetName());
                    else
                        new Forms.Folder(view.FromDir).Move(d + "/" + new Forms.File(view.FromDir).GetName());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            MessageBox.Show("Сделано.");
        }

        public void CopyDir(string FromDir, string ToDir)
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

        public void Stat(object sender, EventArgs e)
        {
            string path = view.getTextBox1 + "\\" + view.GetSetlistView.SelectedItems[0].Text;

            if (new Forms.File(path).Existing())
            {
                try
                {
                    var WordsLength = Int16.Parse(view.getLengthofWord);
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

        public void Open_Or_Go(object sender, EventArgs e)
        {
            var Path_Selected_Item = new Forms.Folder("").Combine(view.getTextBox1, view.GetSetlistView.SelectedItems[0].Text);
            var Ext_Selected_Item = new Forms.Folder(Path_Selected_Item).GetExtension();

            if (Ext_Selected_Item != ".zip")
            {
                try
                {
                    if (Ext_Selected_Item == "")
                    {
                        view.getTextBox1 = Path_Selected_Item;
                        UpdateList(sender, e);
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
            else
            {
                Forms.ZipFolder zipFolder = new Forms.ZipFolder(Path_Selected_Item);
                var files = zipFolder.GetAllFiles();

                foreach (var file in files)
                {
                    ListViewItem listView = new ListViewItem();
                    listView.ImageIndex = 0;
                    var name_Of_File_Or_Directory = new Forms.Folder(file.ToString()).GetName();
                    listView.Text = name_Of_File_Or_Directory.ToString();
                    listView.Tag = "file";
                    view.GetSetlistView.Items.Add(listView);
                }
            }
        }

        public void Click_ListView_Second(object sender, EventArgs e)
        {
            var Path_Selected_Item = new Forms.Folder("").Combine(view.getTextPattern, view.GetlistViewSecond.SelectedItems[0].Text);

            try
            {
                new Forms.File("").Start(Path_Selected_Item);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Ошибка.");
            }
        }

        public void Search(object sender, EventArgs e)
        {
            try
            {
                string Name = view.getTextBox1 + "\\" + view.GetSetlistView.SelectedItems[0].Text;
                (new ParallelSearchingRegFiles(new SearchHandler())).StartToSearch(Name);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        public void Search_By_Name(object sender, EventArgs e)
        {
            try
            {
                ImageList images = new ImageList();
                images.ImageSize = new Size(25, 25);

                images.Images.Add(Properties.Resources.imageFile);

                view.GetlistViewSecond.LargeImageList = images;

                string[] AllFilesInThisDir1 = new Forms.Folder(view.getTextBox1).Get_Files_With_Filter(view.getTextPattern);

                string[] AllFilesInThisDir = new Forms.Folder(view.getTextBox1).Get_ALL_Files_From_All_Inner_Dir1();
                List<string> List_AllFilesInThisDir = AllFilesInThisDir.ToList().FindAll(i => i.Contains(view.getTextPattern) == true);

                if (AllFilesInThisDir1.Length != 0)
                    foreach (string sd in AllFilesInThisDir1)
                    {
                        ListViewItem listview = new ListViewItem();
                        listview.ImageIndex = 0;
                        listview.Text = sd;
                        listview.Tag = "file";
                        view.GetlistViewSecond.Items.Add(listview);
                    }

                else
                    foreach (string sd in List_AllFilesInThisDir)
                    {
                        ListViewItem listview = new ListViewItem();
                        listview.ImageIndex = 0;
                        listview.Text = sd;
                        listview.Tag = "file";
                        view.GetlistViewSecond.Items.Add(listview);
                    }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        public void GetHash(object sender, EventArgs e)
        {
            string path = view.getTextBox1 + "\\" + view.GetSetlistView.SelectedItems[0].Text;
            var file = new Forms.File(path);

            file.Accept(Cash);
        }

        private void Encrypt(object sender, EventArgs e)
        {
            if (view.GetSetlistView.SelectedItems.Count > 0)
            {
                if (view.GetSetlistView.SelectedItems[0].Tag.ToString() == "file")
                {
                    Forms.File m = new Forms.File(view.getTextBox1 + "\\"+view.GetSetlistView.SelectedItems[0].Text);
                    CesarCrypt.setNum(int.Parse(view.getTextCryptBox));
                    m.Accept(CesarCrypt);
                }
                else
                {
                    Forms.Folder m = new Forms.Folder(view.getTextBox1 + "\\" + view.GetSetlistView.SelectedItems[0].Text);
                    CesarCrypt.setNum(int.Parse(view.getTextCryptBox));
                    m.Accept(CesarCrypt);
                }
            }
        }

        private void Decrypt(object sender, EventArgs e)
        {
            if (view.GetSetlistView.SelectedItems.Count > 0)
            {
                if (view.GetSetlistView.SelectedItems[0].Tag.ToString() == "file")
                {
                    Forms.File m = new Forms.File(view.getTextBox1 + "\\" + view.GetSetlistView.SelectedItems[0].Text);
                    CesarEncrypt.setKey(int.Parse(view.getTextCryptBox));
                    m.Accept(CesarEncrypt);
                }
                else
                {
                    Forms.Folder m = new Forms.Folder(view.getTextBox1 + "\\" + view.GetSetlistView.SelectedItems[0].Text);
                    CesarEncrypt.setKey(int.Parse(view.getTextCryptBox));
                    m.Accept(CesarEncrypt);
                }
            }
        }
    }

    interface iView
    {
        event EventHandler DisksOutput;
        event EventHandler GoBack;
        event EventHandler Settings_btn;
        event EventHandler Download_btn;
        event EventHandler UpdateListView;
        event EventHandler Rename_btn;
        event EventHandler Paste_btn;
        event EventHandler Archiving_btn;
        event EventHandler Delete_btn;
        event EventHandler Stat_btn;
        event EventHandler Hash_btn;
        event EventHandler Create_Folder;
        event EventHandler Create_File;
        event EventHandler Search_btn;
        event EventHandler DoubleClick_btn;
        event EventHandler DoubleClick_Right_ListView;
        event EventHandler PatternSearch_box;
        event EventHandler Encrypt;
        event EventHandler Decrypt;


        ListView GetSetlistView { get; set; }
        ListView GetlistViewSecond { get; set; }
        string getTextRename { get; set; }
        string getTextPattern { get; set; }
        string getTextBox1 { get; set; }
        string getLengthofWord { get; set; }
        bool copy_or_move { get; set; }
        string FromDir { get; set; }
        string getTextCryptBox { get; set; }


    }
}
