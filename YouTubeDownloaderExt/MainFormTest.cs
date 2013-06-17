using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace YouTubeDownloaderExt
{
    using System.Diagnostics;
    using System.IO;
    using System.Text.RegularExpressions;
    using YouTubeDownloader;
    using YouTubeDownloader.Properties;

    struct ConversionTaskParameters
    {
        public string OriginalFileLocation;
        public ConversionOption ConversionProfile;
        public int QualityIndex;
        public bool IndirectConversion;
        public BackgroundWorker BackgroundWorker;
    }

    

    public partial class MainFormTest : Form
    {
        public MainFormTest()
        {
            InitializeComponent();
            InitData();
#if DEBUG
            richTextBox1.Text = "https://www.youtube.com/watch?v=LTviZhQ5DBM";
            cmbQuality.SelectedIndex = 1;
#endif
        }

        private void InitData()
        {
            foreach (ConversionOption option in ConversionOptionManager.ConversionOptions)
            {
                this.cmbConvertionOptions.Items.Add(option);
            }
            this.cmbConvertionOptions.SelectedIndex = 9;

            ComponentResourceManager manager = new ComponentResourceManager(typeof (frmMain));
            this.cmbQuality.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbQuality.FormattingEnabled = true;
            this.cmbQuality.Items.AddRange(new object[]
                {
                    manager.GetString("cmbQuality.Items"), manager.GetString("cmbQuality.Items1"),
                    manager.GetString("cmbQuality.Items2"), manager.GetString("cmbQuality.Items3")
                });
            manager.ApplyResources(this.cmbQuality, "cmbQuality");
            this.cmbQuality.Name = "cmbQuality";
        }


        private void btnDownload_Click(object sender, EventArgs e)
        {
            BackgroundWorker backgroundWorker = InitWorker();
            ConversionTaskParameters argument = new ConversionTaskParameters
            {
                ConversionProfile = this.cmbConvertionOptions.SelectedItem as ConversionOption,
                OriginalFileLocation = richTextBox1.Text,
                QualityIndex = this.cmbQuality.SelectedIndex,
                IndirectConversion = ApplicationSettings.Instance.UseIndirectConversion,
                BackgroundWorker = backgroundWorker
            };
#if DEBUG
            DoWorkEventArgs e1=new DoWorkEventArgs(argument);
            backgroundWorker_DoWork(null, e1);
            MessageBox.Show("OK");
#else

            backgroundWorker.RunWorkerAsync(argument);
#endif
        }
        private BackgroundWorker InitWorker()
        {
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(this.backgroundWorker_ProgressChanged);
            backgroundWorker.DoWork += new DoWorkEventHandler(this.backgroundWorker_DoWork);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            return backgroundWorker;
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else
            {

                MessageBox.Show("OK");
            }
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            ConversionTaskParameters argument = (ConversionTaskParameters)e.Argument;
            var backgroundWorker = argument.BackgroundWorker;
            //this.CreateDownloadFolder();
            if (argument.ConversionProfile != null)
            {
                string fileNameWithoutExtension;
                string uRL;
                string str3;
                if (argument.OriginalFileLocation == null)
                {
                    throw new Exception("Incorect video location specified");
                }
                VideoResult result = new VideoResult();
                bool flag = false;
                if (Regex.Match(argument.OriginalFileLocation, @"(?:[Yy][Oo][Uu][Tt][Uu][Bb][Ee]\.[Cc][Oo][Mm]/((?:(?:(?:watch)|(?:watch_popup))(?:(?:\?|\#|\!|\&)?[\w]*=[\w]*)*(?:\?|\#|\!|\&)?v=(?<vid>-?[\w|-]*))|(?:v/(?<vid>-?[\w|-]*))))|(?:[Yy][Oo][Uu][Tt][Uu].[Bb][Ee]/(?<vid>-?[\w|-]*))").Success)
                {
                    string str4;
                    backgroundWorker.ReportProgress(0, "Determining location of the video stream");
                    var u = new YouTubeDownloader();
                    ResourceLocation location = u.ResolveVideoURL(argument.OriginalFileLocation, argument.QualityIndex, argument.ConversionProfile.PreferedType, out str4);
                    if ((location == null) || string.IsNullOrEmpty(location.URL))
                    {
                        throw new Exception("Unable to obtain initial information about this video");
                    }
                    flag = ((argument.ConversionProfile.PreferedType != YouTubeDownloader.VideoStreamTypes.Any) && !string.IsNullOrEmpty(argument.ConversionProfile.AlternativeConversionString)) && (location.StreamType != argument.ConversionProfile.PreferedType);
                    str3 = !flag ? argument.ConversionProfile.ConversionStringTemplate : argument.ConversionProfile.AlternativeConversionString;
                    uRL = location.URL;
                    fileNameWithoutExtension = TextUtil.FormatFileName(str4);
                    result.Title = str4;
                }
                else
                {
                    fileNameWithoutExtension = Path.GetFileNameWithoutExtension(argument.OriginalFileLocation);
                    uRL = argument.OriginalFileLocation;
                    result.Title = fileNameWithoutExtension;
                    result.FileSize = new FileInfo(argument.OriginalFileLocation).Length;
                    str3 = !string.IsNullOrEmpty(argument.ConversionProfile.ConversionStringTemplate) ? argument.ConversionProfile.ConversionStringTemplate : argument.ConversionProfile.AlternativeConversionString;
                }
                backgroundWorker.ReportProgress(0, "***" + fileNameWithoutExtension);
                string str5 = string.Format("{0}.{1}", fileNameWithoutExtension, argument.ConversionProfile.OutputExtension);
                string str6 = DateTime.Now.Ticks.ToString();
                string targetTmpFile = Path.Combine(ApplicationSettings.Instance.DefaultDownloadFolder, str6);
                string targetFile = Path.Combine(ApplicationSettings.Instance.DefaultDownloadFolder, str5);
                long resultSize = 0L;
                result.ResultPath = targetFile;
                if (((argument.ConversionProfile.ConversionStringTemplate != null) || flag) && !argument.IndirectConversion)
                {
                    //try
                    //{
                    //    e.Cancel = this.DownloadAndConvert(str3, uRL, targetFile, targetTmpFile, out resultSize);
                    //}
                    //catch (Exception exception)
                    //{
                    //    resultSize = 0L;
                    //    result.ResultException = exception;
                    //}
                }
                else
                {
                    e.Cancel = this.InternalDownload(backgroundWorker,str3, uRL, targetFile, targetTmpFile, out resultSize);
                }
                result.FileSize = resultSize;
                e.Result = result;
            }
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
         Debug.WriteLine(e.UserState.ToString());
        }


        private bool InternalDownload(BackgroundWorker backgroundWorker, string conversionStringTemplate, string videoFile, string targetFile, string targetTmpFile, out long resultSize)
        {
            bool flag = false;
            long num = 0L;
            Stream stream = null;
            string item = ApplicationSettings.Instance.UseIndirectConversion ? targetTmpFile : targetFile;
            try
            {
                RemoteFileInfo remoteResource = HttpUtility.GetRemoteResource(new ResourceLocation(videoFile), out stream);
                backgroundWorker.ReportProgress(0, 0x7fffffff);
                FileGarbageCollector.Instance.Add(item);
                long ticks = DateTime.Now.Ticks;
                backgroundWorker.ReportProgress(0, "Downloading");
                using (Stream stream2 = new FileStream(item, FileMode.Create))
                {
                    int count = 0;
                    do
                    {
                        Application.DoEvents();
                        if (backgroundWorker.CancellationPending)
                        {
                            resultSize = num;
                            return true;
                        }
                        byte[] buffer = new byte[0x4000];
                        count = stream.Read(buffer, 0, 0x4000);
                        if (count > 0)
                        {
                            stream2.Write(buffer, 0, count);
                            num += count;
                            long num4 = (num * 0x7fffffffL) / remoteResource.FileSize;
                            long num5 = DateTime.Now.Ticks - ticks;
                            if (num5 > 0L)
                            {
                                backgroundWorker.ReportProgress((num4 <= 0x7fffffffL) ? ((int)num4) : 0x7fffffff, ((num * 0x989680L) / num5) / 0x400L);
                            }
                            else
                            {
                                backgroundWorker.ReportProgress((num4 <= 0x7fffffffL) ? ((int)num4) : 0x7fffffff, null);
                            }
                        }
                    }
                    while (count > 0);
                }
                resultSize = num;
                FileGarbageCollector.Instance.Remove(item);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                }
                backgroundWorker.ReportProgress(0x7fffffff, Messages.DownloadCompleted);
            }
            if ((conversionStringTemplate != null) && ApplicationSettings.Instance.UseIndirectConversion)
            {
                FileGarbageCollector.Instance.Add(item);
                string str2 = item + ".tmp";
                FileGarbageCollector.Instance.Add(str2);
                string arguments = string.Format(conversionStringTemplate, item, str2);
                ProcessStartInfo info2 = new ProcessStartInfo("ffmpeg.exe", arguments)
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = Path.GetDirectoryName(Application.StartupPath),
                    RedirectStandardError = true,
                    RedirectStandardOutput = true
                };
                Process process = new Process
                {
                    StartInfo = info2
                };
                int userState = 0;
                FileGarbageCollector.Instance.Add(targetFile);
                try
                {
                    backgroundWorker.ReportProgress(0, "Converting");
                    process.Start();
                    long num1 = DateTime.Now.Ticks;
                    StreamReader standardError = process.StandardError;
                    int percentProgress = 0;
                    do
                    {
                        Application.DoEvents();
                        string str = standardError.ReadLine();
                        Application.DoEvents();
                        if (str.Contains("Duration: "))
                        {
                            TimeSpan span;
                            if (TimeSpan.TryParse(TextUtil.JustAfter(str, "Duration: ", ","), out span))
                            {
                                userState = (int)span.TotalSeconds;
                                backgroundWorker.ReportProgress(0, userState);
                            }
                        }
                        else
                        {
                            int num8;
                            if ((str.Contains("size=") && str.Contains("time=")) && int.TryParse(TextUtil.JustAfter(str, "time=", "."), out num8))
                            {
                                //percentProgress = (num8 <= this.progressIndicator.Maximum) ? num8 : this.progressIndicator.Maximum;
                            }
                        }
                        backgroundWorker.ReportProgress(percentProgress, userState);
                        if (backgroundWorker.CancellationPending)
                        {
                            process.Kill();
                            process.Close();
                            process.Dispose();
                            flag = true;
                        }
                    }
                    while (!standardError.EndOfStream);
                    process.WaitForExit();
                    if (!backgroundWorker.CancellationPending)
                    {
                        int exitCode = process.ExitCode;
                    }
                    else
                    {
                        flag = true;
                    }
                    File.Move(str2, targetFile);
                    FileGarbageCollector.Instance.Remove(targetFile);
                }
                catch (Exception exception)
                {
                    try
                    {
                        process.Kill();
                    }
                    catch
                    {
                    }
                    try
                    {
                        process.Close();
                    }
                    catch
                    {
                    }
                    try
                    {
                        process.Dispose();
                    }
                    catch
                    {
                    }
                    string message = exception.Message;
                }
            }
            resultSize = num / 0x400L;
            return flag;
        }

    }
}
