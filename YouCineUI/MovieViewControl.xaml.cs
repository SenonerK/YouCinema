using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using YouCineLibrary.Models;

namespace YouCineUI
{
    public partial class MovieViewControl : UserControl
    {
        public event EventHandler Click;

        private MovieModel _movie;
        public MovieModel Movie
        {
            get { return _movie; }
            set
            {
                System.Drawing.Image i =YouCineLibrary.Config.MediaConnection.GetImage(value.Image);
                txt.Text = value.MovieName;
                if(i!=null) { img.Source = Util.ImageToImageSource(i); } else { img.Source = new BitmapImage(new Uri("pack://siteoforigin:,,,/Resources/noposter.jpg")); }
                _movie = value;
            }
        }

        public MovieViewControl()
        {
            InitializeComponent();
        }

        private void UserControl_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Click?.Invoke(_movie, e);
        }
    }
}
