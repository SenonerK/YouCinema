using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace YouCineLibrary.DataAccess
{
    public class MediaServer
    {
        public string Mediaserver { get; set; }

        public MediaServer(string server)
        {
            Mediaserver = server;
        }

        public bool TestConnection()
        {
            // Überprüfen ob Server verfügbar ist
            return true;
        }

        public string UploadImage(Image img)
        {
            // Bild hochladen und dessen ID zurückgeben
            return null;
        }

        public Image GetImage(string ID)
        {
            // Bild herunterladen und zurückgeben
            return null;
        }
    }
}
