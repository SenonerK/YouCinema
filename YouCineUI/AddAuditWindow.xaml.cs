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
    public partial class AddAuditWindow : Window
    {
        public AddAuditWindow()
        {
            InitializeComponent();
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e) => Close();

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_name.Text) && !string.IsNullOrWhiteSpace(txt_name.Text)
                && !string.IsNullOrEmpty(txt_row.Text) && !string.IsNullOrWhiteSpace(txt_row.Text)
                && !string.IsNullOrEmpty(txt_col.Text) && !string.IsNullOrWhiteSpace(txt_col.Text))
            {
                YouCineLibrary.Config.Cinema.Auditoriums.Add(
                    YouCineLibrary.Config.Connection.CreateAuditorium(txt_name.Text, int.Parse(txt_col.Text), int.Parse(txt_row.Text)));

                DialogResult = true;
                Close();
            }
            else
                MessageBox.Show("Überprfüfen Sie Ihre Eingaben", "Fehler!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void Num_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(new System.Text.RegularExpressions.Regex("^[0-9]$").IsMatch(e.Text));
        }
    }
}
