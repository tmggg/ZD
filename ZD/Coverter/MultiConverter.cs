using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using SgS.ViewModel;
using System;
using System.Globalization;
using System.Windows.Data;

namespace SgS.Coverter
{
    public class MultiConverter:IMultiValueConverter
    {
        public static MultiConverter Instance = new MultiConverter();
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null)
                return null;
            //MessengerInstance.Send<NotificationMessage, MainViewModel>(new NotificationMessage("我被复制了！"));
            return values.Clone();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
