﻿using System;
using System.Windows.Controls;
using YouCineLibrary.Models;

namespace YouCineUI
{
    public partial class MovieViewControl : UserControl
    {
        private MovieModel _movie;
        public MovieModel Movie
        {
            get { return _movie; }
            set
            {
                txt.Text = value.MovieName;
                img.Source = Util.ByteToImageSource(value.Image);
                _movie = value;
            }
        }

        public MovieViewControl()
        {
            InitializeComponent();
        }
    }
}
