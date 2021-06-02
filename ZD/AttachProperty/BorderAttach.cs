using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight;

namespace SgS.AttachProperty
{
    public class BorderAttach:DependencyObject
    { 
        public static bool GetFlashing(DependencyObject obj)
        {
            return (bool)obj.GetValue(FlashingProperty);
        }

        public static void SetFlashing(DependencyObject obj, bool value)
        {
            obj.SetValue(FlashingProperty, value);
        }

        public static readonly DependencyProperty FlashingProperty =
            DependencyProperty.RegisterAttached("Flashing", typeof(bool), typeof(BorderAttach), new UIPropertyMetadata(false));
    }
}
