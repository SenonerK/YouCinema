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
using YouCineLibrary.DataAccess;

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
            if (new DBConnections().ShowDialog().Value && Config.Connection.TestConnection())
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

        private void Button_Movie_Prev_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Movie_Next_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Movie_Add_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region CustomersTab

        private void Button_Customers_Add_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Customers_Del_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region AuditoriumTab

        private void Button_Audit_Add_Click(object sender, RoutedEventArgs e)
        {

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

        private void Button_Borrow_Add_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Borrow_Del_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Borrow_Search_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion
    }
}
