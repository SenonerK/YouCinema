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
using YouCineLibrary;
using YouCineLibrary.Models;

namespace YouCineUI
{
    public partial class AddReservationWindow : Window
    {
        public AddReservationWindow()
        {
            InitializeComponent();
            cmb_kunde.DataContext = Config.Cinema.Customers;
            cmb_projection.DataContext = Config.Cinema.Projections;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(new System.Text.RegularExpressions.Regex("^[0-9]$").IsMatch(e.Text));
        }

        private void Button_Close_Click(object sender, RoutedEventArgs e) => Close();

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            if (cmb_kunde.SelectedIndex > -1 && cmb_projection.SelectedIndex > -1
                && !string.IsNullOrEmpty(txt_col.Text) && !string.IsNullOrWhiteSpace(txt_col.Text)
                && !string.IsNullOrEmpty(txt_row.Text) && !string.IsNullOrWhiteSpace(txt_row.Text))
            {
                int row = int.Parse(txt_row.Text);
                int col = int.Parse(txt_col.Text);
                if (Config.ReservationPositionExists((cmb_projection.SelectedItem as ProjectionModel).Auditorium,
                    col,row))
                {
                    if (!Config.ReservationPositionIsTaken((cmb_projection.SelectedItem as ProjectionModel).ID,
                        row, col))
                    {
                        Config.Cinema.Reservations.Add(
                            Config.Connection.CreateReservation(
                                (cmb_kunde.SelectedItem as CustomerModel).ID,
                                (cmb_projection.SelectedItem as ProjectionModel).ID,
                                int.Parse(txt_col.Text),
                                int.Parse(txt_row.Text)
                                ));

                        DialogResult = true;
                        Close();
                    }
                    else
                        MessageBox.Show("Dieser Sitzplatz wurden schon reserviert!", "Fehler!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                    MessageBox.Show("Den Sitz den Sie angegeben haben existiert nicht!", "Fehler!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
                MessageBox.Show("Überprüfen Sie bitte Ihre Eingabe!", "Fehler!", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
