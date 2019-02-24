using FileManager.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Windows.Forms;


namespace FileManager
{
    //class Factory
    //{
    //    public static Forms.Entry Get(string path)
    //    {
    //        if (!path.Contains(".zip"))
    //        {
    //            if (Forms == "")
    //                return new FolderMethods(path);
    //            return new FileMethods(path);
    //        }
    //        else
    //        {
    //            if (Entry.getExstention(path) == "")
    //                return new ZipFolder(path);
    //            return new ZipFolder(path);
    //        }

    //    }
    //}

    public partial class Form_File_Manager : MaterialForm, Resources.iView
    {
        bool Clear_TextBox = false;
        string From_Dir = "";
        public bool Copy_Or_Move = true;


        public event EventHandler Settings_btn;
        public event EventHandler Download_btn;
        public event EventHandler Rename_btn;
        public event EventHandler Paste_btn;
        public event EventHandler Archiving_btn;
        public event EventHandler Delete_btn;
        public event EventHandler Stat_btn;
        public event EventHandler Hash_btn;
        public event EventHandler Search_btn;
        public event EventHandler DoubleClick_btn;
        public event EventHandler UpdateListView;
        public event EventHandler DisksOutput;
        public event EventHandler PatternSearch_box;
        public event EventHandler GoBack;
        public event EventHandler DoubleClick_Right_ListView;
        public event EventHandler Create_Folder;
        public event EventHandler Create_File;
        public event EventHandler Encrypt;
        public event EventHandler Decrypt;

        public ListView GetSetlistView { get { return listView1; } set { } }
        public ListView GetlistViewSecond { get { return listView2; } set { } }
        public string getTextRename { get { return toolStripTextBox1.Text; } set { } }
        public string getTextPattern { get { return textBox2.Text; } set { } }
        public string getTextBox1 { get { return textBox1.Text; } set { textBox1.Text = value; } }
        public bool copy_or_move { get { return Copy_Or_Move; } set { Copy_Or_Move = value; } }
        public string FromDir { get { return From_Dir; } set { From_Dir = value; } }
        public string getLengthofWord { get { return toolStripTextBox2.Text; } set { } }
        public string getTextCryptBox { get { return key_crypt.Text; } set { } }

        public Form_File_Manager()
        {
            InitializeComponent();

            MaterialSkinManager materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;

            materialSkinManager.ColorScheme = new ColorScheme(
                Primary.Blue700, Primary.Blue900,
                Primary.BlueGrey500, Accent.LightBlue200,
                TextShade.WHITE
                );



            Resources.Presenter presenter = new Resources.Presenter(this);

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            DisksOutput(sender, e);
        }



        //----Действия кнопок и элементов формы----//   
        private void materialFlatButton1_Click(object sender, EventArgs e) //Кнопка "Перейти".
        {
            UpdateListView(sender, e);
            if (textBox1.Text == "")
                DisksOutput(sender, e);
        }

        private void materialFlatButton2_Click(object sender, EventArgs e)  //Кнопка "Назад".
        {
            GoBack(sender, e);
        }

        private void listView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                Delete_btn(sender, e);
                UpdateListView(sender, e);
            }
        }//Клавиша Delete

        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DoubleClick_btn(sender, e);
            }
        }//Клавиша Enter              

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            DoubleClick_btn(sender, e);
        }//Двойной щелчок по элементу в listView'е 1.       

        private void listView2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            DoubleClick_Right_ListView(sender, e);
        }//Двойной щелчок по элементу в listView'е 2.

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (Clear_TextBox != false)
            {
                listView2.Items.Clear();
                PatternSearch_box(sender, e);
            }
            Clear_TextBox = true;
        }//При изменении текста в текстбоксе будет выполняться метод снова + чистка листбокса для дальнейшего вывода на "чистое".

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

                        toolStripMenuItem3.Text = "Hash файла.";
                    }
                    else
                    {
                        toolStripTextBox1.Text = listView1.SelectedItems[0].Text;
                        toolStripMenuItem1.Visible = false;
                        toolStripSeparator3.Visible = false;
                        toolStripTextBox2.Visible = false;

                        toolStripMenuItem3.Text = "Hash папки.";
                    }
                    Context_Menu.Show(MousePosition, ToolStripDropDownDirection.Right);

                }
                else
                {
                    if (e.Button == MouseButtons.XButton1)
                    {
                        GoBack(sender, e);

                    }
                }
            }
            catch (Exception ex)
            {
                contextMenuStrip2.Show(MousePosition, ToolStripDropDownDirection.Right);
                MessageBox.Show(ex.Message);
            }
        } //Открытие контекстного меню
          //----Действия кнопок и элементов формы----//



        //----Действия кнопок контекстного меню----//        
        private void удалитьToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Delete_btn(sender, e);

            UpdateListView(sender, e);
        }

        private void копироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Copy_Or_Move = true;
            From_Dir = textBox1.Text + "\\" + listView1.SelectedItems[0].Text;
        }

        private void архивацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Archiving_btn(sender, e);

            UpdateListView(sender, e);
        }

        private void переименовать_Click(object sender, EventArgs e)
        {
            Rename_btn(sender, e);

            UpdateListView(sender, e);
        }

        private void вставитьtoolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Paste_btn(sender, e);
            UpdateListView(sender, e);
        }

        private void вырезатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Copy_Or_Move = false;
            From_Dir = textBox1.Text + "\\" + listView1.SelectedItems[0].Text;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Stat_btn(sender, e);

        }//Статистика текстового файла.

        private void toolStripMenuItem1_MouseEnter(object sender, EventArgs e)
        {
            toolStripTextBox2.Visible = true;
        }

        private void toolStripTextBox2_Click(object sender, EventArgs e)
        {
            toolStripTextBox2.Text = "";
        }

        private void менеджерЗагрузкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Download_btn(sender, e);
        }

        private void настройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings_btn(sender, e);
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Hash_btn(sender, e);
        }

        private void Search_Files_Strip_Click(object sender, EventArgs e)
        {
            Search_btn(sender, e);
        }

        private void CreateDir_Strip_Click(object sender, EventArgs e)
        {
            Create_Folder(sender, e);
            UpdateListView(sender, e);
        }

        private void Create_File_Strip_Click(object sender, EventArgs e)
        {
            Create_File(sender, e);
            UpdateListView(sender, e);
        }

        private void расшифрованиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Decrypt(sender, e);
        }

        private void key_crypt_Click(object sender, EventArgs e)
        {
            key_crypt.Text = "";
        }

        private void шифрованиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Encrypt(sender, e);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        //----Действия кнопок контекстного меню----//

    }

   
}

