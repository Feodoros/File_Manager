using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Threading;

namespace FileManager
{
    public partial class Form_MORE_FUN : Form
    {
        Random r = new Random();
        WebClient webload = new WebClient();
        CancellationTokenSource Cancel_Token = new CancellationTokenSource();

        // Thread thread = new Thread(Download_Speed);

        public Form_MORE_FUN()
        {
            InitializeComponent();
        }

        string Game_Name = "";

        public void Download_Speed()
        {

            webload.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
            webload.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);

            int caseSwitch = r.Next(1, 4);
            switch (caseSwitch)
            {
                case 1:
                    webload.DownloadFileAsync(new Uri("http://files.myplaycity.ru/files/cityracing_setup.exe"), "cityracing_setup.exe");
                    Game_Name = "Уличные Гонки";
                    break;
                case 2:
                    webload.DownloadFileAsync(new Uri("http://files.myplaycity.ru/files/bigtruck_setup.exe"), "bigtruck_setup.exe");
                    Game_Name = "Гонки 4*4 на монстер карах";
                    break;
                case 3:
                    webload.DownloadFileAsync(new Uri("http://files.myplaycity.ru/files/motoracing2_setup.exe"), "motoracing2_setup.exe");
                    Game_Name = "МОТОРАЙСИНГ 2";
                    break;
                case 4:
                    webload.DownloadFileAsync(new Uri("http://files.myplaycity.ru/files/furiousbiker_setup.exe"), "furiousbiker_setup.exe");
                    Game_Name = "СУМАСШЕДШИЙ БАЙКЕР";
                    break;
                default:
                    MessageBox.Show("Ошибка :(");
                    break;
            }
        }

        private void Download_Action()
        {
           
            webload.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
            webload.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);

            int caseSwitch = r.Next(1, 3);
            switch (caseSwitch)
            {
                case 1:
                    webload.DownloadFileAsync(new Uri("http://files.myplaycity.ru/files/starconflict_setup.exe"), "starconflict_setup.exe");
                    Game_Name = "Звездный десант";
                    break;
                case 2:
                    webload.DownloadFileAsync(new Uri("http://files.myplaycity.ru/files/bitefight_setup.exe"), "bitefight_setup.exe");
                    Game_Name = "Битва Зверей";
                    break;
                case 3:
                    webload.DownloadFileAsync(new Uri("http://files.myplaycity.ru/files/warofclans_setup.exe"), "warofclans_setup.exe");
                    Game_Name = "Война Кланов";
                    break;
                default:
                    MessageBox.Show("Ошибка :(");
                    break;
            }
        }

        private void Download_Logical()
        {
            
            webload.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
            webload.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);

            int caseSwitch = r.Next(1, 3);
            switch (caseSwitch)
            {
                case 1:
                    webload.DownloadFileAsync(new Uri("http://files.myplaycity.ru/files/seekers_setup.exe"), "seekers_setup.exe");
                    Game_Name = "Seekers";
                    break;
                case 2:
                    webload.DownloadFileAsync(new Uri("http://files.myplaycity.ru/files/cradleofpersia_setup.exe"), "cradleofpersia_setup.exe");
                    Game_Name = "Колыбель Персии";
                    break;
                case 3:
                    webload.DownloadFileAsync(new Uri("http://files.myplaycity.ru/files/sudoku_setup.exe"), "sudoku_setup.exe");
                    Game_Name = "Судоку";
                    break;
                default:
                    MessageBox.Show("Ошибка :(");
                    break;
            }
        }

        public void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (Cancel_Token.IsCancellationRequested)
                webload.CancelAsync();
            else
            {
                Action action = () =>
                {
                    progressBar1.Value = e.ProgressPercentage;
                    textBox1.Text = "Загружено байт: " + e.BytesReceived + progressBar1.Value;
                };

                Invoke(action);
            }
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }

           else
            {
                Action action = () =>
                {
                    textBox1.Text = "Скачивание завершено";
                    MessageBox.Show("Игра " + Game_Name + " скачена.");
                };

                Invoke(action);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(Download_Speed);
            thread.Start();            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(Download_Action);
            thread.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(Download_Logical);
            thread.Start();            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Cancel_Token.Cancel();
            MessageBox.Show("Скачивание отменено :(");
            // System.Diagnostics.Process.GetCurrentProcess().Kill();           //thread.Abort();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form formX = new Form7();
            formX.ShowDialog();
        }
    }
}
