using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace YouCineUI
{
    public static class Util
    {
        public static BitmapImage ImageIDToBitmapImage(string image)
        {
            Image i = YouCineLibrary.Config.MediaConnection.GetImage(image);
            string path = string.Empty;

            if (i == null)
                path = "pack://siteoforigin:,,,/Resources/noposter.jpg";
            else
            {
                if(!Directory.Exists(Path.Combine(Path.GetTempPath(), "youcine")))
                    Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), "youcine"));

                string tmpp = Path.Combine(Path.GetTempPath(),"youcine", image);

                if (!File.Exists(tmpp))
                    i.Save(tmpp);
                path = tmpp;
            }

            return new BitmapImage(new Uri(path));
        }

        public static Image FileToImage(string path)
        {
            return Image.FromFile(path);
        }
    }
}
