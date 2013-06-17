using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using YouTubeDownloader;

namespace YouTubeDownloaderExt
{
    public class YouTubeDownloader2 : YouTubeDownloader.YouTubeDownloader
    {
        private const string AGE_MESSAGE_EXPRESSIONS = "<div id=\"verify-age-actions\">(?<text>[\\w\\W]*?)</div>";
        private static readonly string[] TicketExpressions = new string[] { "(?:^|&amp;|\")url_encoded_fmt_stream_map=([^&]+)", "(?:^|amp;|\")url_encoded_fmt_stream_map=([^(&|\\\\)]+)" };
        //public const string URLPattern = @"(?:[Yy][Oo][Uu][Tt][Uu][Bb][Ee]\.[Cc][Oo][Mm]/((?:(?:(?:watch)|(?:watch_popup))(?:(?:\?|\#|\!|\&)?[\w]*=[\w]*)*(?:\?|\#|\!|\&)?v=(?<vid>-?[\w|-]*))|(?:v/(?<vid>-?[\w|-]*))))|(?:[Yy][Oo][Uu][Tt][Uu].[Bb][Ee]/(?<vid>-?[\w|-]*))";
        //public const string ValidUrlprefix = "http://www.youtube.com/watch?v={0}";

        private static string DecomposeUrl(string srcUrl)
        {
            string str = srcUrl;
            string str2 = TextUtil.JustAfter(srcUrl, "sparams=", "&");
            if (!string.IsNullOrEmpty(str2))
            {
                str = str.Replace("&sparams=" + str2, string.Empty);
            }
            return (str + "&range=13-1781759");
        }

        private ResourceLocation FindRequiredResourceQuality(VideoInfo videoInfo, int preferedQuality, YouTubeDownloader.YouTubeDownloader.VideoStreamTypes preferedType)
        {
            IList list = new ArrayList();
            Array.Sort<ResourceLocation>(videoInfo.Mirrors, new Comparison<ResourceLocation>(ResourceLocationComparer));
            if (preferedType == VideoStreamTypes.Any)
            {
                list = new ArrayList(videoInfo.Mirrors);
            }
            else
            {
                foreach (ResourceLocation location in videoInfo.Mirrors)
                {
                    if (location.StreamType == preferedType)
                    {
                        list.Add(location);
                    }
                }
            }
            switch (preferedQuality)
            {
                case 0:
                    foreach (ResourceLocation location2 in list)
                    {
                        if (location2.VideoStreamQuality <= 1)
                        {
                            return location2;
                        }
                    }
                    break;

                case 1:
                    foreach (ResourceLocation location3 in list)
                    {
                        if (location3.VideoStreamQuality <= 100)
                        {
                            return location3;
                        }
                    }
                    break;

                case 2:
                    foreach (ResourceLocation location4 in list)
                    {
                        if (location4.VideoStreamQuality <= 0x3e8)
                        {
                            return location4;
                        }
                    }
                    break;

                case 3:
                    foreach (ResourceLocation location5 in list)
                    {
                        if (location5.VideoStreamQuality <= 0x2710)
                        {
                            return location5;
                        }
                    }
                    break;
            }
            if (preferedType == VideoStreamTypes.FLV)
            {
                return null;
            }
            return this.FindRequiredResourceQuality(videoInfo, preferedQuality, VideoStreamTypes.FLV);
        }

        private static int FmtDecode(int fmtIndex)
        {
            switch (fmtIndex)
            {
                case 5:
                    return 5;

                case 6:
                    return 0x67;

                case 13:
                    return 1;

                case 0:
                    return 5;

                case 0x11:
                    return 2;

                case 0x12:
                    return 0x68;

                case 0x16:
                    return 0x3e8;

                case 0x22:
                    return 6;

                case 0x23:
                    return 0x69;

                case 0x25:
                    return 0x2710;
            }
            return 1;
        }

        private static int FmtDecode(int fmtIndex, out VideoStreamTypes streamType)
        {
            int num;
            switch (fmtIndex)
            {
                case 5:
                    num = 5;
                    streamType = VideoStreamTypes.FLV;
                    return num;

                case 6:
                    num = 0x67;
                    streamType = VideoStreamTypes.FLV;
                    return num;

                case 13:
                    num = 1;
                    streamType = VideoStreamTypes.ThreeGP;
                    return num;

                case 0:
                    num = 5;
                    streamType = VideoStreamTypes.FLV;
                    return num;

                case 0x11:
                    num = 2;
                    streamType = VideoStreamTypes.ThreeGP;
                    return num;

                case 0x12:
                    num = 0x68;
                    streamType = VideoStreamTypes.MP4;
                    return num;

                case 0x16:
                    num = 0x3e8;
                    streamType = VideoStreamTypes.MP4;
                    return num;

                case 0x22:
                    num = 6;
                    streamType = VideoStreamTypes.FLV;
                    return num;

                case 0x23:
                    num = 0x69;
                    streamType = VideoStreamTypes.FLV;
                    return num;

                case 0x25:
                    num = 0x2710;
                    streamType = VideoStreamTypes.MP4;
                    return num;

                case 0x26:
                    num = 0x2711;
                    streamType = VideoStreamTypes.MP4;
                    return num;
            }
            num = 1;
            streamType = VideoStreamTypes.FLV;
            return num;
        }

        public virtual string GetData(ResourceLocation rl)
        {
            string str2;
            Stream stream = HttpUtility.CreateStream(rl, 0L, 0L, out str2);
            string name = TextUtil.JustAfter(str2, "charset=", ";");
            Encoding encoding = null;
            try
            {
                encoding = Encoding.GetEncoding(name);
            }
            catch
            {
                encoding = null;
            }
            using (StreamReader reader = (encoding != null) ? new StreamReader(stream, encoding) : new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        protected string GetTitle(string pageData)
        {
            string str = TextUtil.JustAfter(pageData, "<meta name=\"title\" content=\"", "\">");
            if (string.IsNullOrEmpty(str))
            {
                str = TextUtil.JustAfter(pageData, "<title>", "</title>");
            }
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Replace("\n", string.Empty).Replace("\r", string.Empty);
            }
            return str;
        }

        public static string GetVideoId(string url)
        {
            Match match = Regex.Match(url, @"(?:[Yy][Oo][Uu][Tt][Uu][Bb][Ee]\.[Cc][Oo][Mm]/((?:(?:(?:watch)|(?:watch_popup))(?:(?:\?|\#|\!|\&)?[\w]*=[\w]*)*(?:\?|\#|\!|\&)?v=(?<vid>-?[\w|-]*))|(?:v/(?<vid>-?[\w|-]*))))|(?:[Yy][Oo][Uu][Tt][Uu].[Bb][Ee]/(?<vid>-?[\w|-]*))");
            if (match.Success)
            {
                return match.Groups["vid"].Value;
            }
            return null;
        }

        public static ResourceLocation ParseFmtURL(string fmtURL, string playerLocation)
        {
            VideoStreamTypes fLV;
            int num;
            string url = string.Empty;
            string str2 = string.Empty;
            string str3 = string.Empty;
            string str = string.Empty;
            foreach (string str5 in fmtURL.Split(new char[] { '&' }))
            {
                string[] strArray = str5.Split(new char[] { '=' });
                string str6 = strArray[0];
                string str8 = str6;
                if (str8 != null)
                {
                    if (!(str8 == "url"))
                    {
                        if (str8 == "quality")
                        {
                            goto Label_00B9;
                        }
                        if (str8 == "sig")
                        {
                            goto Label_00C0;
                        }
                        if (str8 == "type")
                        {
                            goto Label_00C7;
                        }
                        if (str8 == "itag")
                        {
                            goto Label_00CE;
                        }
                    }
                    else
                    {
                        url = Uri.UnescapeDataString(strArray[1]);
                    }
                }
                continue;
            Label_00B9:
                str2 = strArray[1];
                continue;
            Label_00C0:
                str3 = strArray[1];
                continue;
            Label_00C7:
                str = strArray[1];
                continue;
            Label_00CE:
                Convert.ToInt32(strArray[1]);
            }
            url = url + "&signature=" + str3;
            string str9 = TextUtil.JustAfter(str, "video%2F", "%3B");
            if (str9 != null)
            {
                if (!(str9 == "mp4"))
                {
                    if (str9 == "web")
                    {
                        fLV = VideoStreamTypes.WebM;
                        goto Label_0160;
                    }
                    if (str9 == "webm")
                    {
                        fLV = VideoStreamTypes.WebM;
                        goto Label_0160;
                    }
                    if (str9 == "x-flv")
                    {
                        fLV = VideoStreamTypes.FLV;
                        goto Label_0160;
                    }
                }
                else
                {
                    fLV = VideoStreamTypes.MP4;
                    goto Label_0160;
                }
            }
            fLV = VideoStreamTypes.FLV;
        Label_0160:
            switch (str2)
            {
                case "hd3072":
                    num = 0x3ed;
                    break;

                case "highres":
                    num = 0x3eb;
                    break;

                case "hd1080":
                    num = 0x2710;
                    break;

                case "hd720":
                    num = 0x3e8;
                    break;

                case "large":
                    num = 0x65;
                    break;

                case "medium":
                    num = 100;
                    break;

                default:
                    num = 5;
                    break;
            }
            return new ResourceLocation(url) { Referer = playerLocation, VideoStreamQuality = num, StreamType = fLV };
        }

        public VideoInfo ParseVideoInfo(string infoContent, string playerLocation)
        {
            VideoInfo info = new VideoInfo();
            if (!string.IsNullOrEmpty(infoContent))
            {
                ResourceLocation[] locationArray;
                if (!(TextUtil.JustAfter(infoContent, "status=", "&") != "fail"))
                {
                    throw new VideoServerException(TextUtil.HtmlToText(System.Web.HttpUtility.UrlDecode(TextUtil.JustAfter(infoContent, "reason=", "&"))));
                }
                string str2 = TextUtil.JustAfter(infoContent, "&url_encoded_fmt_stream_map=", "&");
                if (!string.IsNullOrEmpty(str2))
                {
                    string[] strArray = System.Web.HttpUtility.UrlDecode(str2).Split(new char[] { ',' });
                    locationArray = new ResourceLocation[strArray.Length];
                    for (int i = 0; i < strArray.Length; i++)
                    {
                        ResourceLocation location = ParseFmtURL(strArray[i], playerLocation);
                        if (((location != null) && (location.URL != null)) && !location.URL.Contains("rtmp"))
                        {
                            locationArray[i] = ParseFmtURL(strArray[i], playerLocation);
                        }
                    }
                }
                else
                {
                    locationArray = null;
                }
                info.Mirrors = locationArray;
                info.Titile = System.Web.HttpUtility.UrlDecode(TextUtil.JustAfter(infoContent, "title=", "&"));
            }
            return info;
        }

        public VideoInfo ParseVideoInfo(ResourceLocation infoURL, string playerLocation)
        {
            VideoInfo info = new VideoInfo();
            string data = this.GetData(infoURL);
            if (data != null)
            {
                info = this.ParseVideoInfo(data, playerLocation);
                if (infoURL != null)
                {
                    if ((info.MappedURL != null) && (info.MappedURL.Referer == null))
                    {
                        info.MappedURL.Referer = infoURL.Referer;
                    }
                    foreach (ResourceLocation location in info.Mirrors)
                    {
                        if ((location != null) && (location.Referer == null))
                        {
                            location.Referer = infoURL.Referer;
                        }
                    }
                }
                return info;
            }
            info.Mirrors = null;
            info.MappedURL = null;
            return info;
        }

        public ResourceLocation ResolveVideoURL(string url, int preferedQuality, VideoStreamTypes preferedStreamType, out string caption)
        {
            VideoInfo info;
            string videoId = GetVideoId(url);
            ResourceLocation rl = new ResourceLocation(string.Format("http://www.youtube.com/watch?v={0}", videoId));
            string domain = TextUtil.GetDomain(rl.URL);
            ResourceLocation infoURL = new ResourceLocation(string.Format("{0}/get_video_info?video_id={1}&hl=en&gl=US&ptk=vevo&el=detailpage", domain, videoId));
            string str4 = null;
            string str = null;
            try
            {
                str = HttpUtility2.GetPageData(rl);
                str4 = TextUtil.JustAfter(str, "swfConfig = {\"url\": \"", "\"");
            }
            catch
            {
            }
            if (!string.IsNullOrEmpty(str4))
            {
                str4 = str4.Replace(@"\/", "/");
                infoURL.Referer = str4;
            }
            else
            {
                string str6 = TextUtil.JustAfter(str, "var swf = \"", "\";");
                if (!string.IsNullOrEmpty(str6))
                {
                    str4 = TextUtil.JustAfter(str6, "name=\\\"movie\\\" value=\\\"", "\\\"");
                    if (!string.IsNullOrEmpty(str4))
                    {
                        str4 = System.Web.HttpUtility.HtmlDecode(str4).Replace(@"\/", @"\");
                    }
                }
            }
            try
            {
                info = this.ParseVideoInfo(infoURL, str4);
            }
            catch (VideoServerException exception)
            {
                Match match = new Regex(TicketExpressions[0]).Match(str);
                if (string.IsNullOrEmpty(match.Value))
                {
                    match = new Regex(TicketExpressions[1]).Match(str);
                    if (string.IsNullOrEmpty(match.Value))
                    {
                        Match match2 = new Regex("<div id=\"verify-age-actions\">(?<text>[\\w\\W]*?)</div>").Match(str);
                        if (match2.Success && !string.IsNullOrEmpty(match2.Groups["text"].Value))
                        {
                            throw new VideoServerException(TextUtil.HtmlToText(match2.Groups["text"].Value));
                        }
                        throw exception;
                    }
                }
                string str7 = match.Groups[1].Value;
                if (string.IsNullOrEmpty(str7))
                {
                    throw exception;
                }
                string[] strArray = System.Web.HttpUtility.UrlDecode(str7).Split(new char[] { ',' });
                info = new VideoInfo
                {
                    Mirrors = new ResourceLocation[strArray.Length]
                };
                int num = -1;
                for (int i = 0; i < strArray.Length; i++)
                {
                    ResourceLocation location3 = ParseFmtURL(strArray[i], str4);
                    if (((location3 != null) && (location3.URL != null)) && !location3.URL.Contains("rtmp"))
                    {
                        info.Mirrors[i] = location3;
                    }
                    if ((info.Mirrors[i] != null) && !string.IsNullOrEmpty(str4))
                    {
                        info.Mirrors[i].Referer = str4.Replace(@"\/", "/");
                    }
                    if ((info.Mirrors[i] != null) && ((info.Mirrors[i].VideoStreamQuality < num) || (num < 0)))
                    {
                        info.MappedURL = info.Mirrors[i];
                    }
                }
            }
            info.MappedURL = this.FindRequiredResourceQuality(info, preferedQuality, preferedStreamType);
            caption = !string.IsNullOrEmpty(info.Titile) ? info.Titile : this.GetTitle(str);
            return info.MappedURL;
        }

        private static int ResourceLocationComparer(ResourceLocation resource0, ResourceLocation resource1)
        {
            if ((resource0 != null) && (resource1 != null))
            {
                return (resource1.VideoStreamQuality - resource0.VideoStreamQuality);
            }
            if ((resource0 == null) && (resource1 == null))
            {
                return 0;
            }
            if (resource0 != null)
            {
                return 1;
            }
            return -1;
        }

      
    }
}
