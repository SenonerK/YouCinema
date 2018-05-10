using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace YouCineUI
{
    public partial class MovieViewWindow : Window
    {
        public MovieViewWindow(YouCineLibrary.Models.MovieModel movie)
        {
            InitializeComponent();
            txt_name.Text = movie.MovieName;
            txt_year.Text = movie.Published.Year.ToString();
            txt_description.Text = movie.MovieDescription;
            txt_charge_day.Text = movie.Price.ToString();

            lst_cast.DataContext = YouCineLibrary.Config.GetParticipationsByMovie(movie.ID);

            System.Drawing.Image i = YouCineLibrary.Config.MediaConnection.GetImage(movie.Image);
            if (i != null) { img.Source = Util.ImageToImageSource(i); } else { img.Source = new BitmapImage(new Uri("pack://siteoforigin:,,,/Resources/noposter.jpg")); }
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
