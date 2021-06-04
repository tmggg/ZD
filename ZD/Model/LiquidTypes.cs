using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgS.Model
{
    /// <summary>
    /// 溶剂的导电针数值类
    /// </summary>
    public class LiquidTypes: ObservableObject
    {
        private string _liquidName;

        public string LiquidName
        {
            get { return _liquidName; }
            set 
            { 
                _liquidName = value;
                RaisePropertyChanged(() => LiquidName);
            }
        }

        private double _z1Value;

        public double Z1Value
        {
            get { return _z1Value; }
            set 
            { 
                _z1Value = value;
                RaisePropertyChanged(() => Z1Value);
            }
        }

        private double _z2Value;

        public double Z2Value
        {
            get { return _z2Value; }
            set 
            { 
                _z2Value = value;
                RaisePropertyChanged(() => Z2Value);
            }
        }
    }
}
