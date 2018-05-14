using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class AddProjectionWindow : Window
    {
        public AddProjectionWindow()
        {
            InitializeComponent();
            cmb_audit.DataContext = YouCineLibrary.Config.Cinema.Auditoriums;
            cmb_movie.DataContext = YouCineLibrary.Config.Cinema.Movies;
        }

        private void Button_Close_Click(object sender, RoutedEventArgs e) => Close();

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            if (cmb_audit.SelectedIndex > -1
                && cmb_movie.SelectedIndex > -1
                && dp_date.SelectedDate.HasValue
                && !string.IsNullOrWhiteSpace(txt_time.Text) && !string.IsNullOrEmpty(txt_time.Text)
                && !string.IsNullOrWhiteSpace(txt_price.Text) && !string.IsNullOrEmpty(txt_price.Text)
                && new Regex("^[0-9]{0,2}:[0-9]{0,2}$").IsMatch(txt_time.Text))
            {
                YouCineLibrary.Config.Cinema.Projections.Add(
                    YouCineLibrary.Config.Connection.CreateProjection(
                        dp_date.SelectedDate.Value.AddMinutes(
                            (double.Parse(txt_time.Text.Split(':')[0]) * 60)
                            + (double.Parse(txt_time.Text.Split(':')[1]))
                            ),
                        double.Parse(txt_price.Text),
                        (cmb_movie.SelectedItem as YouCineLibrary.Models.MovieModel).ID,
                        (cmb_audit.SelectedItem as YouCineLibrary.Models.AuditoriumModel).ID
                        ));

                DialogResult = true;
                Close();
            }
            else
                MessageBox.Show("Ungültige Eingabe!", "Fehler!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void txt_price_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(new Regex("^[0-9,]$").IsMatch(e.Text));
        }

        private void txt_time_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(new Regex("^[0-9:]$").IsMatch(e.Text));
        }
    }
}
