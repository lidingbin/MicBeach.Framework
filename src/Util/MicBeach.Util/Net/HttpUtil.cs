using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Util.Extension;
using MicBeach.Util.Serialize;

namespace MicBeach.Util.Net
{
    /// <summary>
    /// http util
    /// </summary>
    public static class HttpUtil
    {
        #region Http call

        /// <summary>
        /// http call
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="parameters">parameters</param>
        /// <param name="files">files</param>
        /// <returns>response</returns>
        public static string HttpCall(string url, Dictionary<string, string> parameters = null, Dictionary<string, byte[]> files = null, string method = "POST", int timeOut = 60000, bool keepAlive = false)
        {
            #region verify args

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new Exception("Url Is NullOrEmpty");
            }

            #endregion

            string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "multipart/form-data; boundary=" + boundary;
            httpWebRequest.Method = method;
            httpWebRequest.KeepAlive = keepAlive;
            httpWebRequest.Timeout = timeOut;
            httpWebRequest.Credentials = System.Net.CredentialCache.DefaultCredentials;
            Stream memStream = new System.IO.MemoryStream();
            byte[] boundarybytes = System.Text.Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
            string formdataTemplate = "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\";\r\n\r\n{1}";

            #region request parameters

            if (parameters != null && parameters.Count > 0)
            {
                foreach (string key in parameters.Keys)
                {
                    string formitem = string.Format(formdataTemplate, key, parameters[key]);
                    byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                    memStream.Write(formitembytes, 0, formitembytes.Length);
                }
            }

            #endregion

            memStream.Write(boundarybytes, 0, boundarybytes.Length);
            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\";filename=\"{1}\"\r\n Content-Type: application/octet-stream\r\n\r\n";

            #region upload files

            int fileCount = 0;
            foreach (var item in files)
            {
                if (item.Value == null || item.Value.Length <= 0)
                {
                    continue;
                }
                string header = string.Format(headerTemplate, "file" + fileCount.ToString(), item.Key);
                byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
                memStream.Write(headerbytes, 0, headerbytes.Length);
                memStream.Write(item.Value, 0, item.Value.Length);
                memStream.Write(boundarybytes, 0, boundarybytes.Length);
                fileCount++;
            }

            #endregion

            httpWebRequest.ContentLength = memStream.Length;
            Stream requestStream = httpWebRequest.GetRequestStream();
            memStream.Position = 0;
            byte[] tempBuffer = new byte[memStream.Length];
            memStream.Read(tempBuffer, 0, tempBuffer.Length);
            memStream.Close();
            requestStream.Write(tempBuffer, 0, tempBuffer.Length);
            requestStream.Close();
            WebResponse webResponse = httpWebRequest.GetResponse();
            Stream stream = webResponse.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            string readString = reader.ReadToEnd();
            webResponse.Close();
            reader.Close();
            httpWebRequest = null;
            webResponse = null;
            return readString;
        }

        /// <summary>
        /// http call
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="parameters">parameters</param>
        /// <param name="files">files</param>
        /// <returns>response</returns>
        public static string HttpCall(string url, object parameters = null, string method = "POST", int timeOut = 60000, bool keepAlive = false)
        {
            Dictionary<string, string> parameterDic = null;
            if (parameters != null)
            {
                parameterDic = parameters.ObjectToDcitionary().Select(c => new KeyValuePair<string, string>(c.Key, c.Value.ToString())).ToDictionary(c => c.Key, c => c.Value);
            }
            return HttpCall(url, parameterDic, null, method, timeOut, keepAlive);
        }

        #endregion

        #region download file

        /// <summary>
        /// download file
        /// </summary>
        /// <param name="url">url</param>
        /// <returns>file byte</returns>
        public static byte[] DownLoadData(string url)
        {
            return new WebClient().DownloadData(url);
        }

        #endregion

        #region upload file

        /// <summary>
        /// upload file
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="files">files</param>
        /// <param name="parameters">parameters</param>
        /// <returns>upload result</returns>
        public static UploadResult Upload(string url, Dictionary<string, byte[]> files, Dictionary<string, string> parameters = null)
        {
            #region verify args

            if (url.IsNullOrEmpty())
            {
                return new UploadResult()
                {
                    Success = false,
                    ErrorMsg = "url is null or empty"
                };
            }
            if (files == null || files.Count <= 0)
            {
                return new UploadResult()
                {
                    Success = false,
                    ErrorMsg = "not set any files to upload"
                };
            }

            #endregion

            string responseVal = HttpCall(url, parameters, files);
            return JsonSerialize.JsonToObject<UploadResult>(responseVal);
        }

        /// <summary>
        /// upload file
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="file">file</param>
        /// <param name="parameters">parameters</param>
        /// <returns>upload result</returns>
        public static UploadResult Upload(string url, byte[] file, object parameters)
        {
            if (file == null || file.Length <= 0)
            {
                return new UploadResult()
                {
                    ErrorMsg = "not set any files to upload",
                    Success = false
                };
            }
            Dictionary<string, string> parameterDic = null;
            if (parameters != null)
            {
                parameterDic = parameters.ObjectToStringDcitionary();
            }
            return Upload(url, new Dictionary<string, byte[]>() { { "file1", file } }, parameterDic);
        }

        #endregion
    }
}
