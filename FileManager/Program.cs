using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileManager
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            /*Intro first = new Intro();
            DateTime end = DateTime.Now + TimeSpan.FromSeconds(6);
            first.Show();
            while(end > DateTime.Now)
            {
                Application.DoEvents();
            }
            first.Close();
            first.Dispose();*/
            Application.Run( new  /*User_Authentication()*/ Form1() );

        }
    }
}
