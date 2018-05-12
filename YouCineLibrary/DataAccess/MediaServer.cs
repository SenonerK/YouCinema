using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Net;
using System.IO;

namespace YouCineLibrary.DataAccess
{
    public class MediaServer
    {
        public string Mediaserver { get; set; }
        private WebClient _webClient;
        private Random rnd = new Random();
        private readonly string _serverUrl, tempFolder = Path.GetTempPath();
        private HttpWebRequest _webRequest, webRequest;
        private HttpWebResponse _webResponse;
        private StreamReader _streamReader;
        private Stream _stream, _streamAwn;
        private char[] alphabet = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        public MediaServer(string server)
        {
            Mediaserver = server;
        }

        public bool TestConnection()
        {
            try
            {
                _webRequest = (HttpWebRequest)WebRequest.Create(Mediaserver);
                byte[] buffer = Encoding.ASCII.GetBytes("&task=test");
                _webRequest.Timeout = 30000;
                _webRequest.Method = "POST";
                _webRequest.ContentType = "application/x-www-form-urlencoded";
                _webRequest.ContentLength = buffer.Length;
                _stream = _webRequest.GetRequestStream();
                _stream.Write(buffer, 0, buffer.Length);
                _stream.Close();
                _webResponse = (HttpWebResponse)_webRequest.GetResponse();
                _streamAwn = _webResponse.GetResponseStream();
                _streamReader = new StreamReader(_streamAwn);
                return _streamReader.ReadToEnd() == "OK" ? true : false;
            }
            catch
            {
                return false;
            }
        }

        public string UploadImage(Image img)
        {
            try
            {
                string ID = "";
                for (var i = 0; i < 5; i++)
                {
                    ID += alphabet[rnd.Next(0, alphabet.Length)];
                }
                img.Save(Path.Combine(tempFolder, ID));
                _webClient = new WebClient();
                _webClient.Headers.Add("Content-Type", "binary/octet-stream");
                byte[] result = _webClient.UploadFile(_serverUrl + "/upload.php", "POST", Path.Combine(tempFolder, ID));
                return ID;
            }
            catch
            {
                return null;
            }
        }

        public Image GetImage(string ID)
        {
            Image img = null;
            try
            {
                webRequest = (HttpWebRequest)WebRequest.Create(_serverUrl + "download.php?imgID=" + ID);
                webRequest.Timeout = 30000;
                webRequest.AllowWriteStreamBuffering = true;
                _webResponse = (HttpWebResponse)webRequest.GetResponse();
                _stream = _webResponse.GetResponseStream();
                img = Image.FromStream(_stream);
                _webResponse.Close();
            }
            catch
            {
                return null;
            }

            return img;
        }
    }
}
