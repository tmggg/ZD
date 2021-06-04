using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace SgS.Model
{
    /// <summary>
    /// 日志类
    /// </summary>
    public class LogData:ObservableObject
    {
        private string _time;

        public string Time
        {
            get { return _time; }
            set
            {
                _time = value;
                RaisePropertyChanged(() => Time);
            }
        }

        private string _level;

        public string Level
        {
            get { return _level; }
            set
            {
                _level = value;
                RaisePropertyChanged(() => Level);
            }
        }

        private string _data;

        public string Data
        {
            get { return _data; }
            set
            {
                _data = value;
                RaisePropertyChanged(() => Data);
            }
        }

    }
}
