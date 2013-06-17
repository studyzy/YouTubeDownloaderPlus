using System;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Web;
using YouTubeDownloader;


namespace YouTubeDownloaderExt
{
    public class HttpUtility2
    {
        private const string HEADER_CONTENT_ENCODING = "Content-Encoding";
        private const string HEADER_CONTENT_GZIP = "gzip";
        private const string HEADER_CONTENT_DEFLATE = "deflate";

        public static string GetPageData(ResourceLocation rl)
        {
            string contentType;
            Stream stream = CreateStream(rl, 0L, 0L, out contentType);
            string name = TextUtil.JustAfter(contentType, "charset=", ";");
            Encoding encoding;
            try
            {
                encoding = Encoding.GetEncoding(name);
            }
            catch
            {
                encoding = (Encoding)null;
            }
            string str;
            using (StreamReader streamReader = encoding != null ? new StreamReader(stream, encoding) : new StreamReader(stream))
                str = streamReader.ReadToEnd();
            if (string.IsNullOrEmpty(str))
                throw new DownloaderException("Unable to obtain initial info");
            else
                return str;
        }

        public static Stream CreateStream(ResourceLocation rl, long initialPosition, long endPosition)
        {
            string contentType;
            return CreateStream(rl, initialPosition, endPosition, out contentType);
        }

        public static Stream CreateStream(ResourceLocation rl, long initialPosition, long endPosition, out string contentType)
        {
            HttpWebRequest request = (HttpWebRequest)GetRequest(rl);
            FillCredentials(request, rl);
            FillHeaders(request, rl);
            FillExtraData(request, rl);
            request.AllowAutoRedirect = true;
            if (initialPosition != 0L)
            {
                if (endPosition <= 0L)
                    request.AddRange((int)initialPosition);
                else
                    request.AddRange("bytes", (int)initialPosition, (int)endPosition);
            }
            try
            {
                WebResponse response = request.GetResponse();
                string str = ((NameValueCollection)response.Headers)["Content-Encoding"];
                Stream stream = str == null || !str.ToLower().Contains("gzip") ? (str == null || !str.ToLower().Contains("deflate") ? response.GetResponseStream() : (Stream)new DeflateStream(response.GetResponseStream(), CompressionMode.Decompress)) : (Stream)new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
                contentType = response.ContentType;
                return stream;
            }
            catch (Exception ex)
            {
                contentType = (string)null;
                return (Stream)new MemoryStream();
            }
        }

        private static void FillCredentials(HttpWebRequest request, ResourceLocation rl)
        {
            if (!rl.Authenticate)
                return;
            string userName = rl.Login;
            string str = string.Empty;
            int length = userName.IndexOf('\\');
            if (length >= 0)
            {
                str = userName.Substring(0, length);
                userName = userName.Substring(length + 1);
            }
            request.Credentials = (ICredentials)new NetworkCredential(userName, rl.Password)
            {
                Domain = str
            };
        }

        private static void FillHeaders(HttpWebRequest request, ResourceLocation rl)
        {
            if (rl.Headers == null || rl.Headers.Count <= 0)
                return;
            foreach (string name in rl.Headers.Keys)
                ((NameValueCollection)request.Headers).Add(name, rl.Headers[name]);
        }

        private static void FillExtraData(HttpWebRequest request, ResourceLocation rl)
        {
            request.Timeout = 10000;
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; MDDC)";
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
            request.ContentType = "application/x-www-form-urlencoded";
            if (!string.IsNullOrEmpty(rl.Referer))
                request.Referer = rl.Referer;
            byte[] buffer = (byte[])null;
            if (rl.PostDataRequest != null)
                buffer = Encoding.ASCII.GetBytes("request=" + System.Web.HttpUtility.UrlEncode(rl.PostDataRequest).Replace("_", "%5F"));
            else if (rl.PostData != null)
                buffer = Encoding.ASCII.GetBytes(rl.PostData);
            if (buffer == null || buffer.Length <= 0)
                return;
            request.Method = "POST";
            request.ContentLength = (long)buffer.Length;
            Stream requestStream = ((WebRequest)request).GetRequestStream();
            requestStream.Write(buffer, 0, buffer.Length);
            requestStream.Close();
        }

        public static WebRequest GetRequest(ResourceLocation location)
        {
            WebRequest request = WebRequest.Create(location.URL);
            request.Timeout = 30000;
            SetProxy(request);
            return request;
        }

        protected static void SetProxy(WebRequest request)
        {
        }

        public static RemoteFileInfo GetRemoteResource(ResourceLocation rl, out Stream stream)
        {
            HttpWebRequest request = (HttpWebRequest)GetRequest(rl);
            FillCredentials(request, rl);
            FillHeaders(request, rl);
            FillExtraData(request, rl);
            HttpWebResponse httpWebResponse = (HttpWebResponse)request.GetResponse();
            RemoteFileInfo remoteFileInfo = new RemoteFileInfo();
            remoteFileInfo.MimeType = httpWebResponse.ContentType;
            remoteFileInfo.LastModified = httpWebResponse.LastModified;
            remoteFileInfo.FileSize = httpWebResponse.ContentLength;
            remoteFileInfo.AcceptRanges = false;
            try
            {
                string str = ((NameValueCollection)httpWebResponse.Headers)["Content-Encoding"];
                Stream stream1 = str == null || !str.ToLower().Contains("gzip") ? (str == null || !str.ToLower().Contains("deflate") ? httpWebResponse.GetResponseStream() : (Stream)new DeflateStream(httpWebResponse.GetResponseStream(), CompressionMode.Decompress)) : (Stream)new GZipStream(httpWebResponse.GetResponseStream(), CompressionMode.Decompress);
                stream = stream1;
            }
            catch
            {
                stream = (Stream)new MemoryStream();
            }
            return remoteFileInfo;
        }
    }
}
