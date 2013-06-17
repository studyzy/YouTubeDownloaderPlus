using YouTubeDownloader;

namespace YouTubeDownloaderPlus
{
    internal class ConversionOptionManager
    {
        private static readonly ConversionOption[] _conversionOptions = new[]
            {
                new ConversionOption("iPod/iPhone video (Apple QuickTime MOV)",
                                     "-threads 2 -y -i \"{0}\" -f mov -crf 25 -maxrate 1M -vcodec libxvid -bufsize 8M -qmin 3 -qmax 5 -s qvga -strict experimental -acodec aac -ab 128k \"{1}\"",
                                     "mov", YouTubeDownloader.YouTubeDownloader.VideoStreamTypes.Any),
                new ConversionOption("iPad video (H.264 MP4)",
                                     "-threads 2 -y -i \"{0}\" -f mp4 -vcodec libx264 -b 1200k -flags +loop+mv4 -cmp 256 -subq 7 -trellis 1 -refs 5 -coder 0 -me_range 16 -keyint_min 25 -sc_threshold 40 -i_qfactor 0.71 -crf 25 -bt 1.2M -maxrate 1.2M -bufsize 2.4M -rc_eq 'blurCplx^(1-qComp)' -qcomp 0.6 -qmin 10 -qmax 51 -qdiff 4 -level 30 -s hd480 -r 30 -g 90 -async 2 \"{1}\"",
                                     "mp4", YouTubeDownloader.YouTubeDownloader.VideoStreamTypes.Any),
                new ConversionOption("iPhone4 video (H.264 MP4)",
                                     "-threads 2 -y -i \"{0}\" -f mp4 -vcodec libx264 -flags +loop -cmp 2 -subq 7 -trellis 1 -refs 6 -coder 0 -me_range 16 -keyint_min 25 -sc_threshold 40 -i_qfactor 0.71 -crf 22 -bt 1M -minrate 500k -maxrate 1.8M -bufsize 3M -rc_eq 'blurCplx^(1-qComp)' -qcomp 0.6 -qmin 10 -qmax 51 -qdiff 4 -level 31 -s hd720 -strict experimental -acodec aac -ar 48000 -ab 128k \"{1}\"",
                                     "mp4", YouTubeDownloader.YouTubeDownloader.VideoStreamTypes.Any),
                new ConversionOption("PSP video (MPEG-4 MP4)",
                                     "-threads 2 -y -i \"{0}\" -b 2.6M -s 320x240 -ab 200k -f psp \"{1}\"", "mp4",
                                     YouTubeDownloader.YouTubeDownloader.VideoStreamTypes.Any),
                new ConversionOption("Mobile phone (H.263 3GP)",
                                     "-threads 2 -y -i \"{0}\" -f 3gp -s vga -r 24 -b 750k -vcodec mpeg4 -strict experimental -acodec aac -ar 44100 -ab 32k \"{1}\"",
                                     "3gp", YouTubeDownloader.YouTubeDownloader.VideoStreamTypes.Any),
                new ConversionOption("Windows Media Video (V.7 WMV)",
                                     "-threads 1 -vsync 1 -y -i \"{0}\" -vcodec wmv2 -crf 24 -b 1.3M -qmin 3 -qmax 28 -acodec wmav2 -ab 128k -ar 44100 -ac 2 -f asf \"{1}\"",
                                     "wmv", YouTubeDownloader.YouTubeDownloader.VideoStreamTypes.Any),
                new ConversionOption("XVid MPEG-4 (AVI)",
                                     "-threads 2 -vsync 1 -y -i \"{0}\" -vcodec mpeg4 -b 3100k -acodec libmp3lame -ab 128k -ar 44100 -ac 2 -f avi \"{1}\"",
                                     "avi", YouTubeDownloader.YouTubeDownloader.VideoStreamTypes.Any),
                new ConversionOption("MP3-player (audio only)",
                                     "-threads 2 -vsync 1 -y -i \"{0}\" -vn -acodec libmp3lame -ab 128k -ar 44100 -ac 2 -f mp3 \"{1}\"",
                                     "mp3", YouTubeDownloader.YouTubeDownloader.VideoStreamTypes.Any),
                new ConversionOption("No conversion (FLV)", null, "flv",
                                     YouTubeDownloader.YouTubeDownloader.VideoStreamTypes.FLV),
                new ConversionOption("No conversion (MP4)", null, "mp4",
                                     YouTubeDownloader.YouTubeDownloader.VideoStreamTypes.MP4,
                                     "-threads 2 -vsync 1 -y -i \"{0}\" -vcodec mpeg4 -vtag xvid -crf 18 -b 1.5M -minrate 300k -maxrate 1.8M -bufsize 10M -qmin 5 -qmax 28 -acodec libmp3lame -ab 128k -ar 44100 -ac 2 -f avi \"{1}\"")
            };

        public static ConversionOption[] ConversionOptions
        {
            get { return _conversionOptions; }
        }
    }
}