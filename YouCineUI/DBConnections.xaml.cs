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
using System.Configuration;

namespace YouCineUI
{
    /// TODO - Combobox einbauen damit man zwischen (PostgreSQL, MySQL, ...) wählen kann
    public partial class DBConnections : Window
    {
        public DBConnections()
        {
            InitializeComponent();

            Load();
        }

        private void Load()
        {
            lst_db.Items.Clear();
            // Jeden connection string der schon existiert in die Auswahl laden
            foreach (ConnectionStringSettings c in ConfigurationManager.ConnectionStrings)
            {
                lst_db.Items.Add(c.Name);
            }
        }

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            // Wenn "Hinzufügen" gedrückt wurde
            if (!lst_db.IsEnabled)
            {
                // Neue connnection string erstellen
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config
                    .ConnectionStrings
                    .ConnectionStrings
                    .Add(new ConnectionStringSettings()
                    {
                        Name = txt_name.Text,
                        ConnectionString = txt_cnnString.Text,
                        ProviderName = txt_mserver.Text
                    });
                // Configuration speichern
                config.Save();
            }

            // Wenn eingaben nich leer... sind
            if (hasValidInput())
            {
                // Die connection string in der config laden und fenster schliessen
                YouCineLibrary.Config.InitializeConnection(
                    YouCineLibrary.DataAccess.ConnectionType.PostgreSQL, txt_cnnString.Text);

                YouCineLibrary.Config.InitializeMediaConnection(txt_mserver.Text);

                this.DialogResult = true;
                this.Close();
            }
            else
                MessageBox.Show("Überprüfen Sie Ihre Eingaben!", "Fehler!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private bool hasValidInput()
        {
            if (!string.IsNullOrEmpty(txt_name.Text)
                && !string.IsNullOrEmpty(txt_cnnString.Text)
                && !string.IsNullOrEmpty(txt_mserver.Text)
                && !string.IsNullOrWhiteSpace(txt_name.Text)
                && !string.IsNullOrWhiteSpace(txt_cnnString.Text)
                && !string.IsNullOrWhiteSpace(txt_mserver.Text))
                return true;

            return false;
        }

        private void lst_db_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Config aus liste laden
            if (lst_db.SelectedIndex >= 0)
            {
                ConnectionStringSettings s = ConfigurationManager.ConnectionStrings[lst_db.SelectedItem.ToString()];
                txt_name.Text = s.Name;
                txt_cnnString.Text = s.ConnectionString;
                txt_mserver.Text = s.ProviderName;
            }
        }

        private void Button_New_Click(object sender, RoutedEventArgs e)
        {
            // Listbox deaktivieren damit nicht etwas anders ausgewählt werden kann
            // Eingabefelder aktivieren um eingabe zu ermöglichen

            txt_cnnString.Text = "";
            txt_name.Text = "";
            lst_db.IsEnabled = false;
            txt_cnnString.IsReadOnly = false;
            txt_name.IsReadOnly = false;
            txt_mserver.IsReadOnly = false;
        }

        private void Button_Test_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            if (hasValidInput())
            {
                // In die config dieser app laden
                YouCineLibrary.Config.InitializeConnection(YouCineLibrary.DataAccess.ConnectionType.PostgreSQL, txt_cnnString.Text);
                YouCineLibrary.Config.InitializeMediaConnection(txt_mserver.Text);

                // Verbindug testen
                if (YouCineLibrary.Config.Connection.TestConnection() && YouCineLibrary.Config.MediaConnection.TestConnection())
                    MessageBox.Show("Verbindugn konnte aufgebaut werden!", "YouCinema", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Es konnte keine Verbindug hergestellt werden!", "Fehler!", MessageBoxButton.OK, MessageBoxImage.Error);

                // Verbindug wieder entfernen
                YouCineLibrary.Config.RemoveConnection();
            }
            this.Cursor = Cursors.Arrow;
        }

        private void Button_Clear_Click(object sender, RoutedEventArgs e)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.ConnectionStrings.ConnectionStrings.Clear();
            config.Save();

            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }
    }
}
