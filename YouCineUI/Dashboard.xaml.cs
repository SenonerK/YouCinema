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
            if (new DBConnections().ShowDialog().Value && Config.Connection.TestConnection())
            {
                LoadUI();
            }
        }

        private void LoadUI()
        {
            // daten von datenbank einlesen ... 
            // (dazu brachen wir zierst amol die gonzn models und struktur ...)
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

    }
}