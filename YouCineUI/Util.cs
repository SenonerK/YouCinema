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
        public static System.Windows.Media.ImageSource ByteToImageSource(byte[] img)
        {
            try
            {
                using (var ms = new MemoryStream(img))
                {
                    var bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = ms;
                    bitmapImage.EndInit();

                    return bitmapImage;
                }
            } catch (Exception ex) { return null; }
        }

        public static byte[] FileToByte(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            return br.ReadBytes((int)fs.Length);
        }
    }
}
