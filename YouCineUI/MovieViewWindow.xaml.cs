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
            txt_duration.Text = movie.Duration.ToString("hh:mm:ss");

            lst_cast.DataContext = YouCineLibrary.Config.GetParticipationsByMovie(movie.ID);

            img.Source = Util.ImageIDToBitmapImage(movie.Image);
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
