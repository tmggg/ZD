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
    [ValueConversion(typeof(int), typeof(Visibility))]
    public class Int2Visibility : IValueConverter
    {
        public static Int2Visibility Instance = new Int2Visibility();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Visibility.Visible;
            }
            switch ((int)value)
            {
                case 0:
                    return Visibility.Collapsed;
                case 1:
                    return Visibility.Collapsed;
                case 2:
                    return Visibility.Visible;
                default:
                    return Visibility.Visible;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    [ValueConversion(typeof(int), typeof(Visibility))]
    public class Int2Visibility_b : IValueConverter
    {

        public static Int2Visibility_b Instance = new Int2Visibility_b();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Visibility.Visible;
            }
            switch ((int)value)
            {
                case 0:
                    return Visibility.Collapsed;
                case 1:
                    return Visibility.Visible;
                case 2:
                    return Visibility.Visible;
                default:
                    return Visibility.Visible;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
