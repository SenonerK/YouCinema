using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// <summary>
    /// Interaktionslogik für AddBorrwordMove.xaml
    /// </summary>
    public partial class AddBorrwordMove : Window
    {
        List<MovieModel> Movies { get; set; } = new List<MovieModel>(Config.Cinema.Movies);
        List<CustomerModel> Customers { get; set; } = new List<CustomerModel>(Config.Cinema.Customers);
        public AddBorrwordMove()
        {
            InitializeComponent();
            cb_cli.ItemsSource = Customers;
            cb_mov.ItemsSource = Movies;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BorrowModel borrow = Config.Connection.AddBorrowedMovie(
                   DateTime.ParseExact(dp_ABM.SelectedDate.Value.Date.ToString("yyyy-MM-dd"), "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    (cb_cli.SelectedItem as CustomerModel).ID,
                    (cb_mov.SelectedItem as MovieModel).ID
                    );
                Config.Cinema.Borrows.Add(borrow);
                MessageBoxResult dr = MessageBox.Show("Der Film wurde ausgeliehen", "Alles Ok", MessageBoxButton.OK, MessageBoxImage.Information);
                if (dr == MessageBoxResult.OK)
                {
                    DialogResult = true;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBoxResult dr = MessageBox.Show("Bitte überprüfen Sie Ihre eingaben. Möchten Sie es erneut versuchen?", "Fehler", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (dr == MessageBoxResult.No)
                {
                    this.Close();
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
