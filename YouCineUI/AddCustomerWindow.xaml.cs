using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace YouCineUI
{
    public partial class AddCustomerWindow : Window
    {
        public AddCustomerWindow()
        {
            InitializeComponent();
        }

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            // Überprüfe ob eingaben nicht leer sind und die email eine echte email ist
            if (!string.IsNullOrEmpty(txt_fname.Text) && !string.IsNullOrWhiteSpace(txt_fname.Text)
                && !string.IsNullOrEmpty(txt_lname.Text) && !string.IsNullOrWhiteSpace(txt_lname.Text)
                && !string.IsNullOrEmpty(txt_email.Text) && !string.IsNullOrWhiteSpace(txt_email.Text)
                && new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$").IsMatch(txt_email.Text))
            {
                // Neuen customer in der datnbank erstellen, seine id kriegen und neues objekt local abspeichern
                YouCineLibrary.Config.Cinema.Customers.Add(
                    YouCineLibrary.Config.Connection.CreateCustomer(txt_fname.Text, txt_lname.Text, txt_email.Text)
                    );

                DialogResult = true;
                Close();
            }
            else
                MessageBox.Show("Überprüfen Sie Ihre Eingabe!", "Fehler!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void Button_Abbr_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Name_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Nur eingeben wenn eingabe Buchstaben und leerzeichen sind
            e.Handled = !(new Regex("^[a-zA-Z ]+$").IsMatch(e.Text));
        }

        private void Email_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // nur eingeben wenn eingabe Buchstaben, Zahlen, @ oder . sind
            e.Handled = !(new Regex("^[a-zA-Z0-9@.]+$").IsMatch(e.Text));
        }
    }
}
