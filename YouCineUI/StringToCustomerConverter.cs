using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;

namespace YouCineUI
{
    public class StringToCustomerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
             YouCineLibrary.Models.CustomerModel tmp = YouCineLibrary.Config.GetCustomerById(value.ToString());
            return tmp.FirstName + " " + tmp.LastName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
