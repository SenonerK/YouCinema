using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;

namespace YouCineUI
{
    public class StringToProjectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            YouCineLibrary.Models.ProjectionModel tmp = YouCineLibrary.Config.GetProjectionById(value.ToString());
            return "Am " 
                + tmp.Date.ToString("dd/MM/yyyy") 
                + " um " 
                + tmp.Date.ToString("hh:mm")
                + " :  "
                + YouCineLibrary.Config.GetMovieById(tmp.Movie).MovieName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
