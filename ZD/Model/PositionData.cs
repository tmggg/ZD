using GalaSoft.MvvmLight;

namespace SgS.Model
{
    public class PositionData : ObservableObject
    {
        private int _x;

        public int Px
        {
            get { return _x; }
            set
            {
                _x = value;
                RaisePropertyChanged(() => Px);
            }
        }

        private int _y;

        public int Py
        {
            get { return _y; }
            set
            {
                _y = value;
                RaisePropertyChanged(() => Py);
            }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        private string _note;

        public string Note
        {
            get { return _note; }
            set
            {
                _note = value;
                RaisePropertyChanged(() => Note);
            }
        }

        private int _x2;

        public int Px2
        {
            get { return _x2; }
            set
            {
                _x2 = value;
                RaisePropertyChanged(() => Px2);
            }
        }

        private int _y2;

        public int Py2
        {
            get { return _y2; }
            set
            {
                _y2 = value;
                RaisePropertyChanged(() => Py2);
            }
        }
    }
}