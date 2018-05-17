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
    public partial class ChangeBalance : Window
    {
        public ChangeBalance(int customerindex=-1)
        {
            InitializeComponent();
            cmb_kunde.DataContext = YouCineLibrary.Config.Cinema.Customers;
            cmb_kunde.SelectedIndex = customerindex;
        }

        private void txt_in_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(new System.Text.RegularExpressions.Regex("^[0-9,]$").IsMatch(e.Text));
        }

        private void Button_Close_Click(object sender, RoutedEventArgs e) => Close();

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            if(cmb_kunde.SelectedIndex > -1 && double.TryParse(txt_in.Text, out double b))
            {
                (cmb_kunde.SelectedItem as YouCineLibrary.Models.CustomerModel).Credit += b;
                if ((cmb_kunde.SelectedItem as YouCineLibrary.Models.CustomerModel).Update())
                {

                    DialogResult = true;
                    Close();
                }
                else
                    MessageBox.Show("Fehler bei speichern der Daten auf der Datenbank!", "Fehler!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("Wählen Sie einen Kunden aus und geben Sie einen Betrag ein!", "Fehler!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
