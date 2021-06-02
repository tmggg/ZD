using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SgS.Coverter
{
    public class LiquidTypes2String: IValueConverter
    {
        public static LiquidTypes2String instance = new LiquidTypes2String();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
