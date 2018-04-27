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
    public partial class DBConnections : Window
    {
        public DBConnections()
        {
            InitializeComponent();

            foreach (ConnectionStringSettings c in ConfigurationManager.ConnectionStrings)
            {
                lst_db.Items.Add(c.Name);
            }
        }

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            if (!lst_db.IsEnabled)
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config
                    .ConnectionStrings
                    .ConnectionStrings
                    .Add(new ConnectionStringSettings()
                    {
                        Name = txt_name.Text,
                        ConnectionString = txt_cnnString.Text
                    });
                config.Save();
            }

            if (!string.IsNullOrEmpty(txt_name.Text)
                && !string.IsNullOrEmpty(txt_cnnString.Text)
                && !string.IsNullOrWhiteSpace(txt_name.Text)
                && !string.IsNullOrWhiteSpace(txt_cnnString.Text))
            {
                YouCineLibrary.Config.InitializeConnection(
                    YouCineLibrary.DataAccess.ConnectionType.PostgreSQL, txt_cnnString.Text);
                this.DialogResult = true;
                this.Close();
            }
            else
                MessageBox.Show("Überprüfen Sie Ihre Eingaben!", "Fehler!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void lst_db_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lst_db.SelectedIndex >= 0)
            {
                ConnectionStringSettings s = ConfigurationManager.ConnectionStrings[lst_db.SelectedItem.ToString()];
                txt_name.Text = s.Name;
                txt_cnnString.Text = s.ConnectionString;
            }
        }

        private void Button_New_Click(object sender, RoutedEventArgs e)
        {
            txt_cnnString.Text = "";
            txt_name.Text = "";
            lst_db.IsEnabled = false;
            txt_cnnString.IsReadOnly = false;
            txt_name.IsReadOnly = false;
        }

        private void Button_Test_Click(object sender, RoutedEventArgs e)
        {
            // no net implementiert
            /// TODO - Implement test connection feature on DBConnections-Window
        }
    }
}
