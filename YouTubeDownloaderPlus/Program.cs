using System;
using System.Threading;
using System.Windows.Forms;

namespace YouTubeDownloaderPlus
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
            Application.ThreadException += HandleApplicationThreadException;
        }

        private static void HandleApplicationThreadException(object sender, ThreadExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message, "Free YouTube Downloader Ext", MessageBoxButtons.OK,
                            MessageBoxIcon.Hand);
        }
    }
}