using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Meowscles.Services;
using Windows.Gaming.Input;
using Windows.Storage;
using WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO.Compression;
using System.Threading;
using System.Windows.Documents;

namespace Meowscles.Pages
{
    /// <summary>
    /// Interaction logic for Download.xaml
    /// </summary>
    public partial class Download : Page
    {
        private WebClient client;
        private ManifestFile manifest;
        private string version = "12.41";
        private string path;

        public Download()
        {
            InitializeComponent();

            var gpath = Vars.Path != "NONE" ? Vars.Path : UpdateINI.ReadValue("Auth", "Path");

            if (gpath != "NONE" && File.Exists(System.IO.Path.Combine(gpath, "FortniteGame\\Binaries\\Win64\\FortniteClient-Win64-Shipping.exe")))
            {
                this.btnDownload.Content = "Downloaded!";
                this.btnDownload.IsEnabled = false;
            }
        }

        public const string BASE_URL = "https://manifest.fnbuilds.services";
        private const int CHUNK_SIZE = 536870912 / 8;

        class ChunkedFile
        {
            public List<int> ChunksIds = new List<int>();
            public String File = String.Empty;
            public long FileSize = 0;
        }

        class ManifestFile
        {
            public String Name = String.Empty;
            public List<ChunkedFile> Chunks = new List<ChunkedFile>();
            public long Size = 0;
        }

        static string FormatBytesWithSuffix(long bytes)
        {
            string[] Suffix = { "B", "KB", "MB", "GB", "TB" };
            int i;
            double dblSByte = bytes;
            for (i = 0; i < Suffix.Length && bytes >= 1024; i++, bytes /= 1024)
            {
                dblSByte = bytes / 1024.0;
            }

            return String.Format("{0:0.##} {1}", dblSByte, Suffix[i]);
        }

        async void DownloadBuild()
        {
            long totalBytes = manifest.Size;
            long completedBytes = 0;

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            SemaphoreSlim semaphore = new SemaphoreSlim(Environment.ProcessorCount * 2);

            await Task.WhenAll(manifest.Chunks.Select(async chunkedFile =>
            {
                await semaphore.WaitAsync();

                try
                {
                    WebClient httpClient = new WebClient();

                    string outputFilePath = Path.Combine(path, chunkedFile.File);
                    var fileInfo = new FileInfo(outputFilePath);

                    if (File.Exists(outputFilePath) && fileInfo.Length == chunkedFile.FileSize)
                    {
                        completedBytes += chunkedFile.FileSize;
                        semaphore.Release();
                        return;
                    }

                    Directory.CreateDirectory(Path.GetDirectoryName(outputFilePath));

                    using (FileStream outputStream = File.OpenWrite(outputFilePath))
                    {
                        foreach (int chunkId in chunkedFile.ChunksIds)
                        {
                        retry:

                            try
                            {
                                string chunkUrl = BASE_URL + $"/{version}/" + chunkId + ".chunk";
                                var chunkData = await httpClient.DownloadDataTaskAsync(chunkUrl);

                                byte[] chunkDecompData = new byte[CHUNK_SIZE + 1];
                                int bytesRead;
                                long chunkCompletedBytes = 0;

                                MemoryStream memoryStream = new MemoryStream(chunkData);
                                GZipStream decompressionStream = new GZipStream(memoryStream, CompressionMode.Decompress);

                                while ((bytesRead = await decompressionStream.ReadAsync(chunkDecompData, 0, chunkDecompData.Length)) > 0)
                                {
                                    await outputStream.WriteAsync(chunkDecompData, 0, bytesRead);
                                    Interlocked.Add(ref completedBytes, bytesRead);
                                    Interlocked.Add(ref chunkCompletedBytes, bytesRead);

                                    double progress = (double)completedBytes / totalBytes * 100;
                                    System.Console.WriteLine($"{progress}");
                                    this.Dispatcher.Invoke(() =>
                                    {
                                        if ((string)lblStatus.Content != $"{progress:F2}%" && progress == 100)
                                        {
                                            progressBar.Value = progress;
                                            lblStatus.Content = $"{progress:F2}%";
                                        }
                                    });
                                }

                                memoryStream.Close();
                                decompressionStream.Close();
                            }
                            catch (Exception)
                            {
                                goto retry;
                            }
                        }
                    }
                }
                finally
                {
                    semaphore.Release();
                    progressBar.Value = 100;
                    lblStatus.Content = $"Done!";
                    UpdateINI.WriteToConfig("Auth", "Path", path); // Updates Live OMG!
                    Vars.Path = path;
                }
            }));
        }

    private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            var gpath = Vars.Path != "NONE" ? Vars.Path : UpdateINI.ReadValue("Auth", "Path");

            if (gpath != "NONE" && File.Exists(System.IO.Path.Combine(gpath, "FortniteGame\\Binaries\\Win64\\FortniteClient-Win64-Shipping.exe")))
            {
                this.btnDownload.Content = "Downloaded!";
                this.btnDownload.IsEnabled = false;
                return;
            }

            //SaveFileDialog saveFileDialog = new SaveFileDialog();
            //saveFileDialog.FileName = "12.41.zip";
            //saveFileDialog.Filter = "ZIP Archives (*.zip)|*.zip";
            CommonOpenFileDialog saveFileDialog = new CommonOpenFileDialog();
            saveFileDialog.IsFolderPicker = true;
            saveFileDialog.Title = "Select a folder for the build";
            saveFileDialog.Multiselect = false;
            if (saveFileDialog.ShowDialog() == CommonFileDialogResult.Ok /*true*/)
            {

                //MessageBox.Show("Download has started.");
                client = new WebClient();

                manifest = JsonConvert.DeserializeObject<ManifestFile>(client.DownloadString(BASE_URL + $"/12.41/12.41.manifest"));

                this.btnDownload.Content = "Downloading...";
                this.btnDownload.IsEnabled = false;

                path = saveFileDialog.FileName;

                var thread = new Thread(DownloadBuild);
                thread.Start();
            }
        }

        private void progressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
    }
}