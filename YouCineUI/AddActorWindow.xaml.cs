using System.Windows;
using System.Windows.Input;

namespace YouCineUI
{
    public partial class AddActorWindow : Window
    {
        public AddActorWindow()
        {
            InitializeComponent();
        }

        private void Button_Close_Click(object sender, RoutedEventArgs e) => Close();

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txt_fname.Text) && !string.IsNullOrWhiteSpace(txt_fname.Text)
                    && !string.IsNullOrEmpty(txt_lname.Text) && !string.IsNullOrWhiteSpace(txt_lname.Text)
                    && dp_birthday.SelectedDate != null
                    && new System.Text.RegularExpressions.Regex("^[0-9]+,[0-9]+$").IsMatch(txt_rating.Text))
                {
                    YouCineLibrary.Config.Cinema.Actors.Add(
                        YouCineLibrary.Config.Connection.CreateActor(
                            txt_fname.Text,
                            txt_lname.Text,
                            dp_birthday.SelectedDate.Value,
                            double.Parse(txt_rating.Text)));

                    DialogResult = true;
                    Close();
                }
                else
                    MessageBox.Show("Überprüfen Sie Ihre Eingaben", "Fehler!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch { MessageBox.Show("Überprüfen Sie Ihre Eingaben", "Fehler!", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void Text_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(new System.Text.RegularExpressions.Regex("^[a-zA-Z ]+$").IsMatch(e.Text));
        }

        private void Number_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(new System.Text.RegularExpressions.Regex("^[0-9,]+$").IsMatch(e.Text));
        }
    }
}
