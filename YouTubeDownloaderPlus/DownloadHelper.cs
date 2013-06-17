using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using YouTubeDownloader;
using YouTubeDownloader.Properties;

namespace YouTubeDownloaderPlus
{
    internal class DownloadHelper
    {
        public static bool InternalDownload(BackgroundWorker backgroundWorker, string conversionStringTemplate,
                                            string videoFile, string targetFile, string targetTmpFile,
                                            out long resultSize)
        {
            bool flag = false;
            long num = 0L;
            Stream stream = null;
            string item = ApplicationSettings.Instance.UseIndirectConversion ? targetTmpFile : targetFile;
            try
            {
                RemoteFileInfo remoteResource = HttpUtility.GetRemoteResource(new ResourceLocation(videoFile),
                                                                              out stream);
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
                        var buffer = new byte[0x4000];
                        count = stream.Read(buffer, 0, 0x4000);
                        if (count > 0)
                        {
                            stream2.Write(buffer, 0, count);
                            num += count;
                            long num4 = (num*0x7fffffffL)/remoteResource.FileSize;
                            long num5 = DateTime.Now.Ticks - ticks;
                            if (num5 > 0L)
                            {
                                backgroundWorker.ReportProgress((num4 <= 0x7fffffffL) ? ((int) num4) : 0x7fffffff,
                                                                ((num*0x989680L)/num5)/0x400L);
                            }
                            else
                            {
                                backgroundWorker.ReportProgress((num4 <= 0x7fffffffL) ? ((int) num4) : 0x7fffffff, null);
                            }
                        }
                    } while (count > 0);
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
                var info2 = new ProcessStartInfo("ffmpeg.exe", arguments)
                    {
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        WorkingDirectory = Path.GetDirectoryName(Application.StartupPath),
                        RedirectStandardError = true,
                        RedirectStandardOutput = true
                    };
                var process = new Process
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
                                userState = (int) span.TotalSeconds;
                                backgroundWorker.ReportProgress(0, userState);
                            }
                        }
                        else
                        {
                            int num8;
                            if ((str.Contains("size=") && str.Contains("time=")) &&
                                int.TryParse(TextUtil.JustAfter(str, "time=", "."), out num8))
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
                    } while (!standardError.EndOfStream);
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
            resultSize = num/0x400L;
            return flag;
        }

        public static bool DownloadAndConvert(BackgroundWorker backgroundWorker, string conversionStringTemplate,
                                              string videoFile, string targetFile, string targetTmpFile,
                                              out long resultSize)
        {
            bool flag = false;
            long num = 0L;
            string arguments = string.Format(conversionStringTemplate, videoFile, targetTmpFile);
            var info = new ProcessStartInfo("ffmpeg.exe", arguments)
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = Path.GetDirectoryName(Application.StartupPath),
                    RedirectStandardError = true,
                    RedirectStandardOutput = true
                };
            var process = new Process
                {
                    StartInfo = info
                };
            string message = null;
            int exitCode = -1;
            int userState = 0;
            backgroundWorker.ReportProgress(0, "Downloading && Converting");
            FileGarbageCollector.Instance.Add(targetTmpFile);
            try
            {
                string str3;
                process.Start();
                long ticks = DateTime.Now.Ticks;
                StreamReader standardError = process.StandardError;
                long result = 0L;
                int percentProgress = 0;
                do
                {
                    long num6 = 0L;
                    Application.DoEvents();
                    str3 = standardError.ReadLine();
                    Application.DoEvents();
                    if (str3.Contains("Duration: "))
                    {
                        TimeSpan span;
                        if (TimeSpan.TryParse(TextUtil.JustAfter(str3, "Duration: ", ","), out span))
                        {
                            userState = (int) span.TotalSeconds;
                            backgroundWorker.ReportProgress(0, userState);
                        }
                    }
                    else if (str3.Contains("size=") && str3.Contains("time="))
                    {
                        int num8;
                        if (int.TryParse(TextUtil.JustAfter(str3, "time=", "."), out num8))
                        {
                            percentProgress = num8;
                            //percentProgress = (num8 <= progressIndicator.Maximum) ? num8 : progressIndicator.Maximum;
                        }
                        if (long.TryParse(TextUtil.JustAfter(str3, "size=", "kB"), out result))
                        {
                            num6 = result - num;
                            num = result;
                        }
                    }
                    long num9 = DateTime.Now.Ticks - ticks;
                    if (num9 > 0L)
                    {
                        try
                        {
                            backgroundWorker.ReportProgress(percentProgress, (num6*0x989680L)/num9);
                        }
                        catch
                        {
                        }
                    }
                    ticks = DateTime.Now.Ticks;
                    if (backgroundWorker.CancellationPending)
                    {
                        process.Kill();
                        process.Close();
                        process.Dispose();
                        flag = true;
                    }
                } while (!standardError.EndOfStream);
                process.WaitForExit();
                if (!backgroundWorker.CancellationPending)
                {
                    exitCode = process.ExitCode;
                    message = str3;
                    return flag;
                }
                return true;
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
                message = exception.Message;
            }
            finally
            {
                if (!backgroundWorker.CancellationPending)
                {
                    if ((exitCode != 0) && !string.IsNullOrEmpty(message))
                    {
                        if (message.Contains("Decoder (codec id 0) not found for input stream"))
                        {
                            throw new VideoConversionException(message);
                        }
                        throw new Exception(message);
                    }
                    if (File.Exists(targetFile))
                    {
                        try
                        {
                            File.Delete(targetFile);
                        }
                        catch
                        {
                        }
                    }
                    File.Move(targetTmpFile, targetFile);
                    FileGarbageCollector.Instance.Remove(targetTmpFile);
                    backgroundWorker.ReportProgress(userState, Messages.CompletedMessage);
                }
                resultSize = num;
            }
            return flag;
        }
    }
}