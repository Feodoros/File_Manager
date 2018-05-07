using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace FileManager
{
    public partial class User_Authentication : Form
    {
        public User_Authentication()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string s = textBox4.Text;
                byte[] b = Convert.FromBase64String(s);
                s = BitConverter.ToString(b);
                SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\fzhil\source\repos\File_Manager\FileManager\users.mdf; Integrated Security = True");
                SqlDataAdapter connection = new SqlDataAdapter("Select Count (*) From USERS where users = '" + textBox3.Text + "' and password = '" + s + "'", con);
                DataTable dataTable = new DataTable();
                connection.Fill(dataTable);
                if (dataTable.Rows[0][0].ToString() == "1")
                {
                    Form1 f = new Form1();
                    f.Show();
                    this.Hide();
                }
                else
                {
                    textBox3.Text = "";
                    textBox4.Text = "";
                    MessageBox.Show("Неправильный логин или пароль.");
                }
            }
            catch
            {
                textBox3.Text = "";
                textBox4.Text = "";
                MessageBox.Show("Неправильный логин или пароль.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
