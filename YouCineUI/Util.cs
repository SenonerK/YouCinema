using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace YouCineUI
{
    public static class Util
    {
        public static System.Windows.Media.ImageSource ImageToImageSource(System.Drawing.Image image)
        {
            using (var ms = new MemoryStream())
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();

                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                ms.Seek(0, SeekOrigin.Begin);
                bitmap.StreamSource = ms;
                bitmap.EndInit();
                return bitmap;
            }
        }

        public static System.Drawing.Image FileToImage(string path)
        {
            return System.Drawing.Image.FromFile(path);
        }
    }
}
