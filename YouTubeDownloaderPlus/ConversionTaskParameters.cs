using System.ComponentModel;
using System.Windows.Forms;
using YouTubeDownloader;

namespace YouTubeDownloaderPlus
{
    internal struct ConversionTaskParameters
    {
        public BackgroundWorker BackgroundWorker;
        public ConversionOption ConversionProfile;
        public bool IndirectConversion;
        public string OriginalFileLocation;
        public int QualityIndex;

        public Label lbFileName, lbNetSpeed, lblProcessState;
        public ProgressBar progressIndicator;
    }
}