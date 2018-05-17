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
using System.Windows.Navigation;
using System.Windows.Shapes;
using YouCineLibrary;
using YouCineLibrary.Models;

namespace YouCineUI
{
    public partial class Dashboard : Window
    {
        public Dashboard()
        {
            InitializeComponent();
            ShowDBUI();

        }

        private void ShowDBUI()
        {
            // Window anzeigen zur auswahl der Dantenbankverbindung
            if (new DBConnections().ShowDialog().Value && Config.Connection.TestConnection() && Config.MediaConnection.TestConnection())
                LoadUI();
            else
            {
                // Wenn vrebindug nicht erfolgreich war auswahl anzeigen
                MessageBoxResult r = MessageBox.Show(
                    "Die Datenbankverbindug konnte nicht aufgebaut werden"
                    + Environment.NewLine
                    + "Möchten Sie es nochmals versuchen?",
                    "Fehler!",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Error);

                if (r == MessageBoxResult.Yes)
                    ShowDBUI();
                else
                    this.Close();
            }
        }

        private void LoadUI()
        {
            Config.LoadCinema();

            LoadCustomers();
            LoadMovies();
            LoadAudits();
            loadBorrows();
        }

        #region ProjectionsTab

        private void Button_Projections_Search_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_projections_del_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Projections_Add_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region MovieTab

        private void LoadMovies()
        {
            wrap_movies.Children.Clear();
            foreach (MovieModel m in Config.Cinema.Movies)
            {
                MovieViewControl mv = new MovieViewControl()
                {
                    Movie = m
                };
                mv.Click += new EventHandler(Movie_Click);
                wrap_movies.Children.Add(mv);
            }
        }

        private void Movie_Click(object sender, EventArgs e)
        {
            new MovieViewWindow(sender as MovieModel).ShowDialog();
        }

        private void Button_Movie_Add_Click(object sender, RoutedEventArgs e)
        {
            new AddMovieWindow().ShowDialog();
            LoadMovies();
        }

        #endregion

        #region CustomersTab

        private void LoadCustomers()
        {
            dg_customers.DataContext = Config.Cinema.Customers;
            dg_customers.CanUserAddRows = false;
            dg_customers.CanUserDeleteRows = false;
            dg_customers.CanUserResizeRows = false;
            dg_customers.IsReadOnly = true;
        }

        private void UpdateCustomers()
        {
            dg_customers.Items.Refresh();
        }

        private void Button_Customers_Add_Click(object sender, RoutedEventArgs e)
        {
            new AddCustomerWindow().ShowDialog();
            UpdateCustomers();
        }

        private void Button_Customers_Del_Click(object sender, RoutedEventArgs e)
        {
            if (dg_customers.SelectedIndex > -1)
            {
                if ((dg_customers.SelectedItem as CustomerModel).Delete())
                    Config.Cinema.Customers.Remove(dg_customers.SelectedItem as CustomerModel);
                else
                    MessageBox.Show("Fehler auf Seiten der Dantenbank. Möglicherweise existieren irgendwo noch Einträge die mit diesem Kunden in verbindung stehen!");
            }
            else
                MessageBox.Show("Wählen Sie einen Kunden aus!");

            UpdateCustomers();
        }

        #endregion

        #region AuditoriumTab

        private void LoadAudits()
        {
            wrap_rooms.Children.Clear();
            foreach(AuditoriumModel m in Config.Cinema.Auditoriums)
            {
                wrap_rooms.Children.Add(new AuditViewControl() { Auditorium = m });
            }
        }

        private void Button_Audit_Add_Click(object sender, RoutedEventArgs e)
        {
            new AddAuditWindow().ShowDialog();
            LoadAudits();
        }

        private void Button_Audit_Del_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region ReservationsTab

        private void Button_Reservations_Search_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Reservations_Add_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Reservations_Del_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region BorrowTab

        private void loadBorrows()
        {
            dg_borrows.ItemsSource = Config.Cinema.Borrows;
            dg_borrows.CanUserAddRows = false;
            dg_borrows.CanUserDeleteRows = false;
            dg_borrows.CanUserResizeRows = false;
            dg_borrows.IsReadOnly = true;

            dg_borrows_log.ItemsSource = Config.Cinema.Borrows;
            dg_borrows_log.CanUserAddRows = false;
            dg_borrows_log.CanUserDeleteRows = false;
            dg_borrows_log.CanUserResizeRows = false;
            dg_borrows_log.IsReadOnly = true;
        }

        private void reloadBorrow()
        {
            dg_borrows.Items.Refresh();
            dg_borrows_log.Items.Refresh();
        }

        private void Button_Borrow_Add_Click(object sender, RoutedEventArgs e)
        {
            AddBorrwordMove abm = new AddBorrwordMove();
            abm.ShowDialog();
            if(abm.DialogResult.HasValue && abm.DialogResult.Value)
                reloadBorrow();
        }

        private void Button_Borrow_Del_Click(object sender, RoutedEventArgs e)
        {
            if (dg_borrows.SelectedIndex > -1)
            {
                if ((dg_borrows.SelectedItem as BorrowModel).Remove())
                    Config.Cinema.Borrows.Remove((dg_borrows.SelectedItem as BorrowModel));
                else
                    MessageBox.Show("Fehler auf Seiten der Dantenbank. Möglicherweise existieren irgendwo noch Einträge die mit diesem Kunden in verbindung stehen!");
            }
            else
                MessageBox.Show("Wählen Sie etwas aus!");

            reloadBorrow();
        }

        private void Button_Borrow_Search_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion        
    }
}