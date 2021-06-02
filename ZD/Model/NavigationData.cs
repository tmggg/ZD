using System.Windows.Controls;
using GalaSoft.MvvmLight;

namespace SgS.Model
{
    public class NavigationData:ObservableObject
    {
        private string _titleName;
        private string _imgPath;
        private ContentControl _userControl;
        private bool _enable;

        public string TitleName
        {
            get
            {
                return _titleName;
            }
            set
            {
                if (_titleName != value)
                {
                    _titleName = value;
                }
            }
        }

        public bool Enable
        {
            get
            {
                return _enable;
            }
            set
            {
                if (_enable != value)
                {
                    _enable = value;
                }
            }
        }

        public string ImgPath
        {
            get
            {
                return _imgPath;
            }
            set
            {
                if (_imgPath != value)
                {
                    _imgPath = value;
                }
            }
        }

        public ContentControl Content
        {
            get
            {
                return _userControl;
                //TransitioningContentControl tc = new TransitioningContentControl();
                //tc.Content = _userControl;
                ////Random r = new Random();
                ////tc.TransitionMode = (HandyControl.Data.TransitionMode)r.Next(0, 9);
                //tc.TransitionMode = HandyControl.Data.TransitionMode.Fade;
                //return tc;
            }
            set
            {
                if (_userControl != value)
                {
                    _userControl = value;
                }
            }
        }

        private string _pathdata;

        public string PathData
        {
            get { return _pathdata; }
            set
            {
                _pathdata = value;
                RaisePropertyChanged(() => PathData);
            }
        }


    }
}
