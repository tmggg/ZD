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

    //方法参数
    public class Data:ObservableObject
    {
        /// <summary>
        /// 方法名
        /// </summary>
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

        /// <summary>
        /// 加液体积
        /// </summary>
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

        /// <summary>
        /// 粗针清洗体积
        /// </summary>
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

        /// <summary>
        /// 细针清洗体积
        /// </summary>
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

        /// <summary>
        /// 加标体积
        /// </summary>
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

        /// <summary>
        /// 启用加标
        /// </summary>
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

        /// <summary>
        /// 加标通道
        /// </summary>
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

        /// <summary>
        /// 加液通道
        /// </summary>
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
