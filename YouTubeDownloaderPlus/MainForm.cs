using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using YouTubeDownloader;

namespace YouTubeDownloaderPlus
{
    public partial class MainForm : Form
    {
        private int count;
        private Dictionary<BackgroundWorker, ConversionTaskParameters> dictionary;
        private int finished;

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txbSaveFolder.Text = folderBrowserDialog1.SelectedPath;
                ApplicationSettings.Instance.DefaultDownloadFolder = folderBrowserDialog1.SelectedPath;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            foreach (ConversionOption option in ConversionOptionManager.ConversionOptions)
            {
                cmbConvertionOptions.Items.Add(option);
            }
            cmbConvertionOptions.SelectedIndex = 9;


            //this.cmbQuality.Items.AddRange(new object[]
            //    {
            //        manager.GetString("cmbQuality.Items"), manager.GetString("cmbQuality.Items1"),
            //        manager.GetString("cmbQuality.Items2"), manager.GetString("cmbQuality.Items3")
            //    });

            cmbQuality.SelectedIndex = 1;
#if DEBUG
            richTextBox1.Text =
                "https://www.youtube.com/watch?v=C_0VXPtZ58M\r\nhttps://www.youtube.com/watch?v=xQZTZHashKg";
            txbSaveFolder.Text = @"E:\Desktop";
#endif
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            dictionary = new Dictionary<BackgroundWorker, ConversionTaskParameters>();
            ApplicationSettings.Instance.DefaultDownloadFolder = txbSaveFolder.Text;

            string[] lines = richTextBox1.Text.Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);
            int index = 0;
            foreach (string line in lines)
            {
                var progressIndicator = new ProgressBar();
                var lblProcessState = new Label();
                var lbFileName = new Label();
                var lbNetSpeed = new Label();
                groupBox1.Controls.Add(lbNetSpeed);
                groupBox1.Controls.Add(lbFileName);
                groupBox1.Controls.Add(lblProcessState);
                groupBox1.Controls.Add(progressIndicator);
                int step = 30;
                progressIndicator.Location = new Point(500, 30 + index*step);
                progressIndicator.Size = new Size(160, 12);
                lblProcessState.AutoSize = true;
                lblProcessState.Location = new Point(345, 30 + index*step);
                lbFileName.AutoSize = true;
                lbFileName.Location = new Point(22, 30 + index*step);
                lbNetSpeed.AutoSize = true;
                lbNetSpeed.Location = new Point(281, 30 + index*step);
                index++;
                count++;
                BackgroundWorker backgroundWorker = InitWorker();

                var argument = new ConversionTaskParameters
                    {
                        ConversionProfile = cmbConvertionOptions.SelectedItem as ConversionOption,
                        OriginalFileLocation = line,
                        QualityIndex = cmbQuality.SelectedIndex,
                        IndirectConversion = ApplicationSettings.Instance.UseIndirectConversion,
                        BackgroundWorker = backgroundWorker,
                        lbFileName = lbFileName,
                        lbNetSpeed = lbNetSpeed,
                        lblProcessState = lblProcessState,
                        progressIndicator = progressIndicator
                    };

                dictionary.Add(backgroundWorker, argument);
#if DEBUG
                var e1 = new DoWorkEventArgs(argument);
                backgroundWorker_DoWork(null, e1);

#else

                backgroundWorker.RunWorkerAsync(argument);
#endif
            }
        }


        private BackgroundWorker InitWorker()
        {
            var backgroundWorker = new BackgroundWorker();
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.ProgressChanged += backgroundWorker_ProgressChanged;
            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
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
                finished++;
                toolStripStatusLabel1.Text = "总共" + count + "个,完成了" + finished + "个";
            }
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var argument = (ConversionTaskParameters) e.Argument;
            BackgroundWorker backgroundWorker = argument.BackgroundWorker;
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
                var result = new VideoResult();
                bool flag = false;
                if (
                    Regex.Match(argument.OriginalFileLocation,
                                @"(?:[Yy][Oo][Uu][Tt][Uu][Bb][Ee]\.[Cc][Oo][Mm]/((?:(?:(?:watch)|(?:watch_popup))(?:(?:\?|\#|\!|\&)?[\w]*=[\w]*)*(?:\?|\#|\!|\&)?v=(?<vid>-?[\w|-]*))|(?:v/(?<vid>-?[\w|-]*))))|(?:[Yy][Oo][Uu][Tt][Uu].[Bb][Ee]/(?<vid>-?[\w|-]*))")
                        .Success)
                {
                    string str4;
                    backgroundWorker.ReportProgress(0, "Determining location of the video stream");
                    var u = new YouTubeDownloader.YouTubeDownloader();
                    ResourceLocation location = u.ResolveVideoURL(argument.OriginalFileLocation, argument.QualityIndex,
                                                                  argument.ConversionProfile.PreferedType, out str4);
                    if ((location == null) || string.IsNullOrEmpty(location.URL))
                    {
                        throw new Exception("Unable to obtain initial information about this video");
                    }
                    flag = ((argument.ConversionProfile.PreferedType !=
                             YouTubeDownloader.YouTubeDownloader.VideoStreamTypes.Any) &&
                            !string.IsNullOrEmpty(argument.ConversionProfile.AlternativeConversionString)) &&
                           (location.StreamType != argument.ConversionProfile.PreferedType);
                    str3 = !flag
                               ? argument.ConversionProfile.ConversionStringTemplate
                               : argument.ConversionProfile.AlternativeConversionString;
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
                    str3 = !string.IsNullOrEmpty(argument.ConversionProfile.ConversionStringTemplate)
                               ? argument.ConversionProfile.ConversionStringTemplate
                               : argument.ConversionProfile.AlternativeConversionString;
                }
                backgroundWorker.ReportProgress(0, "***" + fileNameWithoutExtension);
                string str5 = string.Format("{0}.{1}", fileNameWithoutExtension,
                                            argument.ConversionProfile.OutputExtension);
                string str6 = DateTime.Now.Ticks.ToString();
                string targetTmpFile = Path.Combine(ApplicationSettings.Instance.DefaultDownloadFolder, str6);
                string targetFile = Path.Combine(ApplicationSettings.Instance.DefaultDownloadFolder, str5);
                long resultSize = 0L;
                result.ResultPath = targetFile;
                if (((argument.ConversionProfile.ConversionStringTemplate != null) || flag) &&
                    !argument.IndirectConversion)
                {
                    try
                    {
                        e.Cancel = DownloadHelper.DownloadAndConvert(backgroundWorker, str3, uRL, targetFile,
                                                                     targetTmpFile, out resultSize);
                    }
                    catch (Exception exception)
                    {
                        resultSize = 0L;
                        result.ResultException = exception;
                    }
                }
                else
                {
                    e.Cancel = DownloadHelper.InternalDownload(backgroundWorker, str3, uRL, targetFile, targetTmpFile,
                                                               out resultSize);
                }
                result.FileSize = resultSize;
                e.Result = result;
            }
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var bw = sender as BackgroundWorker;
            ConversionTaskParameters par = dictionary[bw];

            object userState = e.UserState;
            if (userState != null)
            {
                if (typeof (long).IsInstanceOfType(userState))
                {
                    par.lbNetSpeed.Text = string.Format("{0}kB/s", userState);
                }
                else if (typeof (int).IsInstanceOfType(userState))
                {
                    par.progressIndicator.Maximum = (int) userState;
                }
                else if (typeof (string).IsInstanceOfType(userState))
                {
                    var stateDescription = (string) userState;
                    if (!stateDescription.StartsWith("***"))
                    {
                        par.lblProcessState.Text = stateDescription;
                        Application.DoEvents();
                    }
                    else
                    {
                        par.lbFileName.Text = stateDescription.Substring(3);
                    }
                    Application.DoEvents();
                }
                else if (typeof (Exception).IsInstanceOfType(userState))
                {
                    MessageBox.Show(((Exception) userState).Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            if (e.ProgressPercentage <= par.progressIndicator.Maximum)
            {
                par.progressIndicator.Value = e.ProgressPercentage;
            }
            else
            {
                par.progressIndicator.Value = par.progressIndicator.Maximum;
            }
        }
    }
}