using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using YouTubeDownloader;

namespace YouTubeDownloaderPlus
{
    struct ConversionTaskParameters
    {
        public string OriginalFileLocation;
        public ConversionOption ConversionProfile;
        public int QualityIndex;
        public bool IndirectConversion;
        public BackgroundWorker BackgroundWorker;

        public Label lbFileName, lbNetSpeed, lblProcessState;
        public ProgressBar progressIndicator;
    }
}
