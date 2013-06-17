using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace YouTubeDownloaderExt
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
            Application.ThreadException += new ThreadExceptionEventHandler(HandleApplicationThreadException);
        }
        private static void HandleApplicationThreadException(object sender, ThreadExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message, "Free YouTube Downloader Ext", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }

    }
}
