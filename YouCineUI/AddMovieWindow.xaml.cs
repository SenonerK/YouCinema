using Microsoft.Win32;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using YouCineLibrary;
using YouCineLibrary.Models;
using System.Collections.Generic;

namespace YouCineUI
{
    public partial class AddMovieWindow : Window
    {
        public List<MovieParticipationModel> Participations { get; set; } = new List<MovieParticipationModel>();

        public AddMovieWindow()
        {
            InitializeComponent();
            cmb_actor.DataContext = Config.Cinema.Actors;
            lst_cast.DataContext = this.Participations;
        }

        private void Button_Image_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = "PNG|*.png",
                ValidateNames = true,
                Multiselect = false
            };

            if (ofd.ShowDialog().Value)
            {
                img.Source = new BitmapImage(new Uri(ofd.FileName));
                img.DataContext = ofd.FileName;
            }
        }

        private void txt_jahr_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(new Regex("^[0-9]+$").IsMatch(e.Text));
        }

        private void txt_charge_day_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(new Regex("^[0-9.,]+$").IsMatch(e.Text));
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e) => Close();

        private void Button_ok_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_name.Text) && !string.IsNullOrWhiteSpace(txt_name.Text)
                && !string.IsNullOrEmpty(txt_year.Text) && !string.IsNullOrWhiteSpace(txt_year.Text)
                && !string.IsNullOrEmpty(txt_charge_day.Text) && !string.IsNullOrWhiteSpace(txt_charge_day.Text)
                && !string.IsNullOrEmpty(txt_description.Text) && !string.IsNullOrWhiteSpace(txt_description.Text)
                && lst_cast.Items.Count > 0
                && img.Source != null
                && int.Parse(txt_year.Text)<2100 && int.Parse(txt_year.Text)>1700)
            {
                MovieModel movie = Config.Connection.CreateMovie(
                    txt_name.Text,
                    txt_description.Text,
                    new DateTime(int.Parse(txt_year.Text), 1, 1),
                    double.Parse(txt_charge_day.Text),
                    Util.FileToImage(img.DataContext as string)
                    );

                Config.Cinema.Movies.Add(movie);

                foreach (MovieParticipationModel m in Participations)
                {
                    Config.Cinema.MovieParticipations.Add(Config.Connection.CreateMovieParticipation(
                            movie.ID,
                            m.Actor,
                            m.Role
                        )
                    );
                }

                DialogResult = true;
                Close();
            }
            else
                MessageBox.Show("Überprüfen Sie Iher Eingaben", "Fehler!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void Button_Add_Actor_Click(object sender, RoutedEventArgs e)
        {
            if (cmb_actor.SelectedIndex > -1 && !string.IsNullOrWhiteSpace(txt_role.Text) && !string.IsNullOrEmpty(txt_role.Text))
            {
                Participations.Add(new MovieParticipationModel()
                {
                    Actor = (cmb_actor.SelectedItem as ActorModel).ID,
                    Movie = "",
                    Role = txt_role.Text
                });
            }
            else
                MessageBox.Show("Wählen Sie einen Schauspieler aus und geben Sie eine Rolle an." + Environment.NewLine + "Sie können auch einen neuen Schauspeiler hinzufügen indem Sie auf \"Neu\" klicken", "Fehler!", MessageBoxButton.OK, MessageBoxImage.Error);

            lst_cast.Items.Refresh();
        }

        private void Button_New_Actor_Click(object sender, RoutedEventArgs e)
        {
            new AddActorWindow().ShowDialog();
            cmb_actor.Items.Refresh();
        }
    }
}
