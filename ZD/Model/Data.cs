using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Windows.Media;
using System.Xml.Serialization;
using GalaSoft.MvvmLight;

namespace SgS.Model
{
    [Serializable]
    [XmlInclude(typeof(System.Windows.Media.SolidColorBrush))]
    [XmlInclude(typeof(System.Windows.Media.MatrixTransform))]
    public class Data:ObservableObject
    {
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

        private double _value;
        public double Value
        {
            get { return _value; }
            set
            {
                _value = value;
                RaisePropertyChanged(() => Value);
            }
        }

        private double _bigcleanValue;
        public double BigCleanValue
        {
            get { return _bigcleanValue; }
            set
            {
                _bigcleanValue = value;
                RaisePropertyChanged(() => BigCleanValue);
            }
        }

        private double _littlecleanValue;
        public double LittleCleanValue
        {
            get { return _littlecleanValue; }
            set
            {
                _littlecleanValue = value;
                RaisePropertyChanged(() => LittleCleanValue);
            }
        }

        private double _tagValue;
        public double TagValue
        {
            get { return _tagValue; }
            set
            {
                _tagValue = value;
                RaisePropertyChanged(() => TagValue);
            }
        }

        private bool? _enableTag;
        public bool? EnableTag
        {
            get { return _enableTag; }
            set
            {
                _enableTag = value;
                RaisePropertyChanged(() => EnableTag);
            }
        }

        private ushort _tagIndex;

        public ushort TagIndex
        {
            get { return _tagIndex; }
            set 
            { 
                _tagIndex = value;
                RaisePropertyChanged(() => TagIndex);
            }
        }

        private ushort _addIndex;

        public ushort AddIndex
        {
            get { return _addIndex; }
            set
            {
                _addIndex = value;
                RaisePropertyChanged(() => AddIndex);
            }
        }

        


        private Brush _color;
        public Brush Color
        {
            get { return _color; }
            set
            {
                _color = value;
                RaisePropertyChanged(() => Color);
            }
        }

        private bool _showClose;
        public bool ShowClose
        {
            get { return _showClose; }
            set
            {
                _showClose = value;
                RaisePropertyChanged(() => ShowClose);
            }
        }

        public override void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            base.RaisePropertyChanged(propertyExpression);
        }
    }
}
