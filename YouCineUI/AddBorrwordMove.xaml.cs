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
    public partial class AddBorrwordMove : Window
    {
        public AddBorrwordMove()
        {
            InitializeComponent();
            cb_cli.ItemsSource = Config.Cinema.Customers;
            cb_mov.ItemsSource = Config.Cinema.Movies;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dp_ABM.SelectedDate.HasValue
                    && cb_cli.SelectedIndex > -1
                    && cb_mov.SelectedIndex > -1
                    && dp_ABM.SelectedDate.Value > DateTime.Now)
                {
                    CustomerModel cst = (cb_cli.SelectedItem as CustomerModel);
                    MovieModel mv = (cb_mov.SelectedItem as MovieModel);

                    int tmp = Config.Cinema.Customers.IndexOf(cst);
                    double tmp_betrag = Convert.ToInt32((dp_ABM.SelectedDate.Value - DateTime.Now).TotalDays) + 1 * mv.Price;

                    if ((cst.Credit - tmp_betrag) < 0)
                    {
                        if (MessageBox.Show("Durch diese Operation geht der Kontostand ins Minus." + Environment.NewLine + "Möchten Sie Geld überweisen?", "Achtung!", MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.Yes)
                        {
                            new ChangeBalance(tmp).ShowDialog();
                        }
                        return;
                    }

                    if (MessageBox.Show("Soll der Betrag von " + tmp_betrag.ToString() + "€ vom Konto des Kunden abgezogen werden?", "Bestätigen", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.No)
                    {
                        return;
                    }

                    cst.Credit -= tmp_betrag;
                    if (cst.Update())
                    {
                        Config.Cinema.Borrows.Add(Config.Connection.AddBorrowedMovie(dp_ABM.SelectedDate.Value, cst.ID, mv.ID));
                        Config.Cinema.Customers[tmp] = cst;

                        DialogResult = true;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Fehler bei der kommuniktion mit der Datenbank versuchen Sie es später nochmals", "Fehler!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                    MessageBox.Show("Überprüfen Sie Ihre Eingaben. Das Rückbringdatum darf nicht in der Vergangen heit sein!", "Fehler!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch
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
