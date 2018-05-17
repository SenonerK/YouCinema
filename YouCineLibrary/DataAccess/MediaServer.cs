using System.Text;
using System.Drawing;
using System.Net;
using System.IO;

namespace YouCineLibrary.DataAccess
{
    public class MediaServer
    {
        public const int IDLenght = 5;
        public string Mediaserver { get; set; }

        private readonly char[] alphabet = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        public MediaServer(string server)
        {
            Mediaserver = server;
        }

        public string GETRequest(string url)
        {
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Timeout = 30000;
                webRequest.Method = "GET";

                return new StreamReader(
                    webRequest
                    .GetResponse()
                    .GetResponseStream())
                    .ReadToEnd();
            }
            catch
            {
                return null;
            }
        }

        public bool TestConnection()
        {
            return GETRequest(Mediaserver + "?task=test")=="OK";
        }

        public string UploadImage(Image img)
        {
            try
            {
                string tmppath = Path.Combine(Path.GetTempPath(), "yctmpimg");
                img.Save(tmppath);

                WebClient _webClient = new WebClient();
                _webClient.Headers.Add("Content-Type", "binary/octet-stream");
                string res = Encoding.ASCII.GetString(_webClient.UploadFile(Mediaserver + "/?task=upload", "POST", tmppath));

                File.Delete(tmppath);

                return res=="ERROR" ? null : res;
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
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(Mediaserver + "/?task=download&imgID=" + ID);
                webRequest.Timeout = 30000;
                webRequest.AllowWriteStreamBuffering = true;

                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                Stream stream = webResponse.GetResponseStream();

                img = Image.FromStream(stream);

                webResponse.Close();
            }
            catch { }

            return img;
        }
    }
}
