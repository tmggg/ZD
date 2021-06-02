using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SgS.Coverter
{
    [ValueConversion(typeof(int), typeof(string))]
    public class CheckPosition : IValueConverter
    {
        public static CheckPosition Instance = new CheckPosition();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (int.Parse(value.ToString()) > 0 || int.Parse(value.ToString()) < -146000)
                return "位置超限!";
            return value;
            //throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(int), typeof(string))]
    public class CheckPositionZ2 : IValueConverter
    {
        public static CheckPositionZ2 Instance = new CheckPositionZ2();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (int.Parse(value.ToString()) > 0 || int.Parse(value.ToString()) < -146000)
                return "位置超限!";
            return value;
            //throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
