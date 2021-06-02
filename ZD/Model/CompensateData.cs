using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace SgS.Model
{
    public class CompensateData:ObservableObject
    {
        private double _min;

        public double Min
        {
            get { return _min; }
            set
            {
                _min = value;
                RaisePropertyChanged(() => Min);
            }
        }

        private double _max;

        public double Max
        {
            get { return _max; }
            set
            {
                _max = value;
                RaisePropertyChanged(() => Max);
            }
        }

        private double _a;

        public double A
        {
            get { return _a; }
            set
            {
                _a = value;
                RaisePropertyChanged(() => A);
            }
        }

        private double _b;

        public double B
        {
            get { return _b; }
            set
            {
                _b = value;
                RaisePropertyChanged(() => B);
            }
        }

        private double _c;

        public double C
        {
            get { return _c; }
            set
            {
                _c = value;
                RaisePropertyChanged(() => C);
            }
        }

        private double _selectMethod;

        public double SelectMethod
        {
            get { return _selectMethod; }
            set
            {
                _selectMethod = value;
                RaisePropertyChanged(() => SelectMethod);
            }
        }


    }
}
