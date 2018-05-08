using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using YouCineLibrary;
using YouCineLibrary.Models;
using System.Windows.Data;
using System.Globalization;

namespace YouCineUI
{
    public class StringToActorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            foreach (ActorModel m in Config.Cinema.Actors)
                if (m.ID == value as string)
                    return m.FirstName + ", " + m.LastName;

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value as ActorModel).ID;
        }
    }
}
