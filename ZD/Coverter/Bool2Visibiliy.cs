using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace SgS.Coverter
{
    public class Bool2Visibiliy : IValueConverter
    {
        public static Bool2Visibiliy instance = new Bool2Visibiliy();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool)) return Visibility.Collapsed;
            if ((bool)value) 
                return Visibility.Visible;
            else 
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
