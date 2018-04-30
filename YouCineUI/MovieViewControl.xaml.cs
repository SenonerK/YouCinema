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
