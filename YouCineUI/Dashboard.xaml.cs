﻿using System;
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
using System.Windows.Threading;

namespace YouCineUI
{
    public partial class Dashboard : Window
    {
        DispatcherTimer AuditTimer, GeneralTimer;

        public Dashboard()
        {
            AuditTimer = new DispatcherTimer();
            AuditTimer.Interval = TimeSpan.FromSeconds(5);
            AuditTimer.Tick += new EventHandler(LoadAudits);

            InitializeComponent();

            loading.Visibility = Visibility.Visible;
            lbl_version.Text = "Version: " + System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).ProductVersion;

            ShowDBUI();

            GeneralTimer = new DispatcherTimer();
            GeneralTimer.Interval = TimeSpan.FromMinutes(2);
            GeneralTimer.Tick += new EventHandler(LoadUI);
            GeneralTimer.Start();
        }

        private async void ShowDBUI()
        {
            // Window anzeigen zur auswahl der Dantenbankverbindung
            if (new DBConnections().ShowDialog().Value)
            {
                if (await Task.Run(() => Config.Connection.TestConnection()) && await Task.Run(() => Config.MediaConnection.TestConnection()))
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
            else
                this.Close();
        }

        private async void LoadUI(object s = null, EventArgs e = null)
        {
            if (s != null && !chk_refresh.IsChecked.Value)
                return;

            loading.Visibility = Visibility.Visible;

            AuditTimer.Stop();

            if (await Task.Run(() => Config.LoadCinema()))
            {
                loading.Visibility = Visibility.Hidden;

                // Alle vergangene Vorführungen und deren Reservierungen löschen
                Config.Connection.ClearProjections();

                LoadCustomers();
                LoadMovies();
                LoadAudits();
                LoadProjections();
                LoadReservations();
                LoadBorrows();

                AuditTimer.Start();
            }
            else
            {
                MessageBox.Show("Die Verbindug zur Datenbank steht nicht mehr!", "Fehler!", MessageBoxButton.OK, MessageBoxImage.Error);
                ShowDBUI();
            }
        }

        private void MenuItem_Reload_Click(object sender, RoutedEventArgs e) => LoadUI();

        private void MenuItem_Close_Click(object sender, RoutedEventArgs e) => Close();

        #region ProjectionsTab

        private void LoadProjections()
        {
            dg_projections.DataContext = Config.Cinema.Projections;
            dg_projections.Items.Refresh();
        }

        private void Button_Projections_Search_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).Content.ToString() == "Suche"
                && dte_von_projections.SelectedDate.HasValue
                && dte_bis_projections.SelectedDate.HasValue
                && dte_bis_projections.SelectedDate.Value >= dte_von_projections.SelectedDate.Value
                && dte_bis_projections.SelectedDate.Value != dte_von_projections.SelectedDate.Value)
            {
                (sender as Button).Content = "Alle";
                dte_von_projections.IsEnabled = false;
                dte_bis_projections.IsEnabled = false;
                dg_projections.DataContext = Config.SearchProjectionByDate(
                    dte_von_projections.SelectedDate.Value,
                    dte_bis_projections.SelectedDate.Value
                    );
            }
            else if ((sender as Button).Content.ToString() == "Alle")
            {
                (sender as Button).Content = "Suche";
                dte_von_projections.IsEnabled = true;
                dte_bis_projections.IsEnabled = true;
                LoadProjections();
            }
            else
                MessageBox.Show("Wählen Sie bitte den Zeitabschnitt!", "Fehler!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void btn_projections_del_Click(object sender, RoutedEventArgs e)
        {
            if (dg_projections.SelectedIndex > -1)
            {
                if (Config.Connection.DeleteProjection((dg_projections.SelectedItem as ProjectionModel).ID))
                    Config.Cinema.Projections.Remove(dg_projections.SelectedItem as ProjectionModel);
                else
                    MessageBox.Show("Fehler auf Seiten der Dantenbank. Möglicherweise existieren noch Reservierungen für diese Vorführung!");
            }
            LoadProjections();
        }

        private void Button_Projections_Add_Click(object sender, RoutedEventArgs e)
        {
            new AddProjectionWindow().ShowDialog();
            LoadProjections();
        }

        private void Loading_MediaEnded(object sender, RoutedEventArgs e)
        {
            (sender as MediaElement).Position = TimeSpan.FromMilliseconds(1);
            (sender as MediaElement).Play();
        }

        private void MenuItem_Info_Click(object sender, RoutedEventArgs e)
        {
            new InfoWindow().Show();
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
            Cursor = Cursors.Wait;
            new MovieViewWindow(sender as MovieModel).ShowDialog();
            Cursor = Cursors.Arrow;
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
                if (Config.Connection.DeleteCustomer((dg_customers.SelectedItem as CustomerModel).ID))
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

        private void LoadAudits(object sender = null, EventArgs e = null)
        {
            wrap_rooms.Children.Clear();
            foreach (AuditoriumModel m in Config.Cinema.Auditoriums)
            {
                wrap_rooms.Children.Add(new AuditViewControl() { Auditorium = m });
            }
        }

        private void Button_Audit_Add_Click(object sender, RoutedEventArgs e)
        {
            new AddAuditWindow().ShowDialog();
            LoadAudits();
        }

        #endregion

        #region ReservationsTab

        private void LoadReservations()
        {
            dg_reservations.DataContext = Config.Cinema.Reservations;
            dg_reservations.Items.Refresh();
        }

        private void Button_Reservations_Search_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).Content.ToString() == "Suche"
                && dte_von_reservations.SelectedDate.HasValue
                && dte_bis_reservations.SelectedDate.HasValue
                && dte_bis_reservations.SelectedDate.Value >= dte_von_reservations.SelectedDate.Value
                && dte_bis_reservations.SelectedDate.Value != dte_von_reservations.SelectedDate.Value)
            {
                (sender as Button).Content = "Alle";
                dte_von_reservations.IsEnabled = false;
                dte_bis_reservations.IsEnabled = false;
                dg_reservations.DataContext = Config.SearchReservationByDate(
                    dte_von_reservations.SelectedDate.Value,
                    dte_bis_reservations.SelectedDate.Value);
            }
            else if ((sender as Button).Content.ToString() == "Alle")
            {
                (sender as Button).Content = "Suche";
                dte_von_reservations.IsEnabled = true;
                dte_bis_reservations.IsEnabled = true;
                LoadReservations();
            }
            else
                MessageBox.Show("Wählen Sie bitte den Zeitabschnitt!", "Fehler!", MessageBoxButton.OK, MessageBoxImage.Error);

        }

        private void Button_Reservations_Add_Click(object sender, RoutedEventArgs e)
        {
            new AddReservationWindow().ShowDialog();
            LoadReservations();
        }

        private void Button_Reservations_Del_Click(object sender, RoutedEventArgs e)
        {
            if (dg_reservations.SelectedIndex > -1)
            {
                if (Config.Connection.DeleteReservation((dg_reservations.SelectedItem as ReservationModel).ID))
                    Config.Cinema.Reservations.Remove(dg_reservations.SelectedItem as ReservationModel);
                else
                    MessageBox.Show("Fehler auf Seiten der Dantenbank!", "Fehler!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
                MessageBox.Show("Wählen Sie einen Kunden aus!");

            LoadReservations();
        }

        #endregion

        #region BorrowTab

        private void LoadBorrows()
        {
            dg_borrows.DataContext = Config.Cinema.Borrows;
            dg_borrows.Items.Refresh();

            Config.LoadBorrowLog();
            dg_borrows_log.DataContext = Config.Cinema.BorrowLog;
            dg_borrows_log.Items.Refresh();
        }

        private void Button_Borrow_Add_Click(object sender, RoutedEventArgs e)
        {
            new AddBorrwordMove().ShowDialog();
            LoadBorrows();
            dg_customers.Items.Refresh();
        }

        private void Button_Borrow_Del_Click(object sender, RoutedEventArgs e)
        {
            if (dg_borrows.SelectedIndex > -1)
            {
                BorrowModel b = (dg_borrows.SelectedItem as BorrowModel);
                if (b.BringBackDate < DateTime.Now)
                {
                    double betrag = Convert.ToInt32((DateTime.Now - b.BringBackDate).TotalDays) * Config.GetMovieById(b.Movie).Price;
                    int cst = Config.Cinema.Customers.IndexOf(Config.GetCustomerById(b.Customer));

                    Config.Cinema.Customers[cst].Credit -= betrag;
                    Config.Cinema.Customers[cst].Update();
                    dg_customers.Items.Refresh();

                    MessageBox.Show("Der Film wurde zu spät abgegeben! Der Betrag von " + betrag + " € wurde zusätzlich vom Konto des Kunden abgezogen!", "Info", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    
                }

                if (b.Remove())
                    Config.Cinema.Borrows.Remove(b);
                else
                    MessageBox.Show("Fehler auf Seiten der Dantenbank!");
            }
            else
                MessageBox.Show("Wählen Sie etwas aus!");

            LoadBorrows();
        }

        private void Button_Borrow_Search_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).Content.ToString() == "Suche"
                && dte_von_borrows.SelectedDate.HasValue
                && dte_bis_borrows.SelectedDate.HasValue
                && dte_bis_borrows.SelectedDate.Value >= dte_von_borrows.SelectedDate.Value
                && dte_bis_borrows.SelectedDate.Value != dte_von_borrows.SelectedDate.Value)
            {
                (sender as Button).Content = "Alle";
                dte_von_borrows.IsEnabled = false;
                dte_bis_borrows.IsEnabled = false;
                dg_borrows_log.DataContext = Config.SearchBorrowLogByDate(
                    dte_von_borrows.SelectedDate.Value,
                    dte_bis_borrows.SelectedDate.Value
                    );
            }
            else if ((sender as Button).Content.ToString() == "Alle")
            {
                (sender as Button).Content = "Suche";
                dte_von_borrows.IsEnabled = true;
                dte_bis_borrows.IsEnabled = true;
                LoadBorrows();
            }
            else
                MessageBox.Show("Wählen Sie bitte den Zeitabschnitt!", "Fehler!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        #endregion        
    }
}
