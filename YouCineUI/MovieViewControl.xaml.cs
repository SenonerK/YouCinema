﻿using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using YouCineLibrary.Models;

namespace YouCineUI
{
    public partial class MovieViewControl : UserControl
    {
        public event EventHandler Click;

        private MovieModel _movie;
        public MovieModel Movie
        {
            get { return _movie; }
            set
            {
                txt.Text = value.MovieName;
                img.Source = Util.ImageIDToBitmapImage(value.Image);
                _movie = value;
            }
        }

        public MovieViewControl()
        {
            InitializeComponent();
        }

        private void UserControl_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Click?.Invoke(_movie, e);
        }
    }
}
