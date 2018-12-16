using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileManager
{
    public partial class Loading_Data : Form
    {
        CancellationTokenSource token = new CancellationTokenSource();

        public Loading_Data()
        {
            InitializeComponent();

            textBox1.Height = 200;
            //textBox1.Width = 500;
            textBox1.Multiline = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            var result = DemoMethods.RunDownloadParallelSync();
            PrintResults(result);

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            textBox1.Text += $"Total execution time: {elapsedMs}";

        } //ParallelSync

        private async void button2_Click(object sender, EventArgs e)
        {
            Progress<ProgressReportModel> progress = new Progress<ProgressReportModel>();
            progress.ProgressChanged += ReportProgress;

            var watch = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                var result = await DemoMethods.RunDownloadAsync(progress, token.Token);
                PrintResults(result);
            }
            catch (OperationCanceledException)
            {
                textBox1.Text += $"The async download was cancelled {Environment.NewLine} ";
            }

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            MessageBox.Show("Асинхронная загрузка завершена");
            textBox1.Text += $"Total execution time: {elapsedMs}";
        } //Async

        private void ReportProgress(object sender, ProgressReportModel e)
        {
            progressBar1.Value = e.ProcentageComplete;
            PrintResults(e.SitesDownloaded);
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            Progress<ProgressReportModel> progress = new Progress<ProgressReportModel>();
            progress.ProgressChanged += ReportProgress;

            var watch = System.Diagnostics.Stopwatch.StartNew();

            var result = await DemoMethods.RunDownloadParallelAsync();
            PrintResults(result);

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            textBox1.Text += $"Total execution time: {elapsedMs}";
        } //ParallelAsync

        private void PrintResults(List<WebsiteDataModel> results)
        {
            textBox1.Text = "";
            foreach (var item in results)
            {
                textBox1.Text += $"{item.WebsiteUrl} downloaded: {item.WebsiteData.Length} characters.{Environment.NewLine}";
            }
        }

        private void button4_Click(object sender, EventArgs e) //Cancel
        {
            token.Cancel();
        }

    }
}
