using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using EasyNetVars;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using HandyControl.Controls;
using SgS.AttachProperty;
using SgS.Helper;
using SgS.Model;
using SgS.View;
using TabControl = HandyControl.Controls.TabControl;
using TabItem = HandyControl.Controls.TabItem;
using Window = System.Windows.Window;

namespace SgS.ViewModel
{

    public enum RunStatus
    {
        connecting,
        init,
        ready,
        start,
        pause,
        clean,
        clean_error,
        estop
    }

    public class MainViewModel : ViewModelBase
    {
        private int _tabItemCount;
        private bool _selectAll;
        private RunStatus _isRuning;
        private bool EStop;
        private bool _select;
        private int _starttag;
        private int _endtag;
        public static NetVar _clientRead;
        private string _controlerIP;
        private int _port;
        private Border Status_Light;
        public static Configuration _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        public static AllStatus _allStatus;
        public static string countpath;
        private System.Media.SoundPlayer musicPlayer = new System.Media.SoundPlayer();
        private readonly int[,] tubeTags;
        private Dictionary<int, int[]> zoneSelPoint;
        private Stack<RunStatus> _lastStatus = new Stack<RunStatus>();

        #region Attribute

        private int _todayCount;

        public int TodayCount
        {
            get { return _todayCount; }
            set
            {
                _todayCount = value;
                RaisePropertyChanged(() => TodayCount);
            }
        }


        private bool _startEnable;

        public bool StartEnable
        {
            get { return _startEnable; }
            set
            {
                _startEnable = value;
                RaisePropertyChanged(() => StartEnable);
            }
        }

        private bool _pauseEnable;

        public bool PauseEnable
        {
            get { return _pauseEnable; }
            set
            {
                _pauseEnable = value;
                RaisePropertyChanged(() => PauseEnable);
            }
        }

        private bool _stopEnable;

        public bool StopEnable
        {
            get { return _stopEnable; }
            set
            {
                _stopEnable = value;
                RaisePropertyChanged(() => StopEnable);
            }
        }

        private bool _settingEnable;

        public bool SettingEnable
        {
            get { return _settingEnable; }
            set
            {
                _settingEnable = value;
                RaisePropertyChanged(() => SettingEnable);
            }
        }

        private bool _isbusy;

        public bool IsBusy
        {
            get { return _isbusy; }
            set
            {
                _isbusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }

        private bool _mainEnable;

        public bool MainEnable
        {
            get { return _mainEnable; }
            set
            {
                _mainEnable = value;
                RaisePropertyChanged(() => MainEnable);
            }
        }

        private bool _resetEnable;

        public bool ReSetEnable
        {
            get { return _resetEnable; }
            set
            {
                _resetEnable = value;
                RaisePropertyChanged(() => ReSetEnable);
            }
        }

        private bool _cleanEnable;

        public bool CleanEnable
        {
            get { return _cleanEnable; }
            set
            {
                _cleanEnable = value;
                RaisePropertyChanged(() => CleanEnable);
            }
        }

        private string _busyContent;

        public string BusyContent
        {
            get { return _busyContent; }
            set
            {
                if (_busyContent != value)
                {
                    _busyContent = value;
                    RaisePropertyChanged(() => BusyContent);

                }
            }
        }

        private int _errorId;

        public int ErrorID
        {
            get { return _errorId; }
            set
            {
                if (value != _errorId)
                {
                    _errorId = value;
                    RaisePropertyChanged(() => ErrorID);
                }
            }
        }

        private int _runId;

        public int RunId
        {
            get { return _runId; }
            set
            {
                if (value != _runId)
                {
                    _runId = value;
                    RaisePropertyChanged(() => RunId);
                }
            }
        }

        private RunStatus _lightStatus;

        public RunStatus LightStatus
        {
            get { return _lightStatus; }
            set
            {
                if (Status_Light != null)
                {
                    switch (value)
                    {
                        case RunStatus.connecting:
                            BorderAttach.SetFlashing(Status_Light, false);
                            Status_Light.Background = new SolidColorBrush(Colors.WhiteSmoke);
                            BorderAttach.SetFlashing(Status_Light, true);
                            _lightStatus = value;
                            break;
                        case RunStatus.init:
                            BorderAttach.SetFlashing(Status_Light, false);
                            Status_Light.Background = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                            BorderAttach.SetFlashing(Status_Light, true);
                            _lightStatus = value;
                            break;
                        case RunStatus.ready:
                            Status_Light.Background = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                            BorderAttach.SetFlashing(Status_Light, false);
                            _lightStatus = value;
                            break;
                        case RunStatus.start:
                            Status_Light.Background = new SolidColorBrush(Color.FromRgb(0, 0, 255));
                            BorderAttach.SetFlashing(Status_Light, false);
                            _lightStatus = value;
                            break;
                        case RunStatus.pause:
                            BorderAttach.SetFlashing(Status_Light, false);
                            Status_Light.Background = new SolidColorBrush(Colors.Yellow);
                            BorderAttach.SetFlashing(Status_Light, true);
                            _lightStatus = value;
                            break;
                        //case RunStatus.stop:
                        //    BorderAttach.SetFlashing(Status_Light, false);
                        //    Status_Light.Background = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                        //    _lightStatus = value;
                        //    break;
                        case RunStatus.clean:
                            BorderAttach.SetFlashing(Status_Light, false);
                            Status_Light.Background = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                            BorderAttach.SetFlashing(Status_Light, true);
                            _lightStatus = value;
                            break;
                        case RunStatus.clean_error:
                            BorderAttach.SetFlashing(Status_Light, false);
                            Status_Light.Background = new SolidColorBrush(Colors.Red);
                            BorderAttach.SetFlashing(Status_Light, true);
                            _lightStatus = value;
                            break;
                        case RunStatus.estop:
                            BorderAttach.SetFlashing(Status_Light, false);
                            Status_Light.Background = new SolidColorBrush(Colors.Red);
                            BorderAttach.SetFlashing(Status_Light, true);
                            _lightStatus = value;
                            break;
                        default:
                            break;
                    }

                    _lightStatus = value;
                }
            }
        }

        private int _liquidSelectindex;

        public int LiquidSelectindex
        {
            get
            {
                return _liquidSelectindex;
            }
            set
            {
                _liquidSelectindex = value;
                RaisePropertyChanged(() => LiquidSelectindex);
            }
        }

        private ushort _runArea;

        public ushort RunArea
        {
            get { return _runArea; }
            set
            {
                _runArea = value; RaisePropertyChanged(() => RunArea);
            }
        }

        #endregion

        #region Command

        public ObservableCollection<TabItem> TabItems { get; private set; }
        public ObservableCollection<Data> MethodItems { get; private set; }
        public ObservableCollection<Data> AllData { get; private set; }
        public ObservableCollection<LiquidTypes> LiquidTypes { get; private set; }
        public ObservableCollection<bool> WatchLiquidTube { get; set; }
        public RelayCommand<TabControl> AddTabItemCommand { get; private set; }
        public RelayCommand<RoutedEventArgs> DelTabItemCommand { get; private set; }
        public RelayCommand<object[]> DeployMethodCommand { get; private set; }
        public RelayCommand<CancelEventArgs> WindowClosingCommand { get; private set; }
        public RelayCommand<Data> ItemCloseCommand { get; private set; }
        public RelayCommand<Window> OpenEngineerViewCommand { get; private set; }
        public RelayCommand<Window> OpenHelperViewCommand { get; private set; }
        public RelayCommand<Window> CloseCommand { get; private set; }
        public RelayCommand<Window> DragCommand { get; private set; }
        public RelayCommand<object[]> ActionCommand { get; private set; }

        public RelayCommand<NumericUpDown> UpValueCommand { get; private set; }
        public RelayCommand<NumericUpDown> DownValueCommand { get; private set; }

        public RelayCommand testCommand { get; private set; }

        public RelayCommand<object[]> SetRowCommand { get; private set; }

        public RelayCommand<object[]> SetColumnCommand { get; private set; }

        public RelayCommand<object[]> SelectAllCommand { get; private set; }

        public RelayCommand<object[]> ShowKeyPadCommand { get; private set; }

        public RelayCommand<Border> GetLightCommand { get; private set; }

        public RelayCommand<Window> MinCommand { get; private set; } = new RelayCommand<Window>((w) => { w.WindowState = WindowState.Minimized; });
        public RelayCommand<Window> MaxCommand { get; private set; } = new RelayCommand<Window>((w) => { w.WindowState = w.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal; });

        #endregion

        public MainViewModel()
        {
            MessengerInstance.Register<NotificationMessage>(this, NotifyMe);
            MessengerInstance.Register<LogViewViewModel>(this, DoSomeThing);
            MessengerInstance.Register<ObservableCollection<LiquidTypes>>(this, UpdateLiquidType);
            MessengerInstance.Register<bool>(this, "OffMainTab", OffMainTab);
            TabItems = new ObservableCollection<TabItem>();
            AllData = new ObservableCollection<Data>();
            MethodItems = new ObservableCollection<Data>();
            WatchLiquidTube = new ObservableCollection<bool>();
            AddTabItemCommand = new RelayCommand<TabControl>(Addtabitem);
            DeployMethodCommand = new RelayCommand<object[]>(DeployMethod);
            WindowClosingCommand = new RelayCommand<CancelEventArgs>(WindowClosing);
            ItemCloseCommand = new RelayCommand<Data>(ItemClose);
            OpenEngineerViewCommand = new RelayCommand<Window>(OpenEngineerView);
            CloseCommand = new RelayCommand<Window>(CloseWindow);
            DragCommand = new RelayCommand<Window>(DragWindow);
            OpenHelperViewCommand = new RelayCommand<Window>(OpenHelper);
            ActionCommand = new RelayCommand<object[]>(ActionDo);
            SetRowCommand = new RelayCommand<object[]>(SetRow);
            SetColumnCommand = new RelayCommand<object[]>(SetColums);
            SelectAllCommand = new RelayCommand<object[]>(SelectAll);
            UpValueCommand = new RelayCommand<NumericUpDown>(UpValue);
            DownValueCommand = new RelayCommand<NumericUpDown>(DownValue);
            ShowKeyPadCommand = new RelayCommand<object[]>(ShowKeyPad);
            GetLightCommand = new RelayCommand<Border>(GetLight);
            testCommand = new RelayCommand(() => { System.Windows.MessageBox.Show("检测到编辑"); });
            ReSetEnable = false;
            StartEnable = false;
            PauseEnable = false;
            StopEnable = false;
            CleanEnable = false;
            SettingEnable = true;
            MainEnable = true;
            IsBusy = false;
            EStop = false;
            _starttag = 0;
            _endtag = 0;
            _isRuning = RunStatus.ready;
            //BusyContent = "正在操作，请等待。。。。";
            _busyContent = "准备就绪，等待启动。。。。";
            string subPath = AppDomain.CurrentDomain.BaseDirectory + @"Logs";
            if (false == System.IO.Directory.Exists(subPath))
            {
                System.IO.Directory.CreateDirectory(subPath);
            }
            if (!System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"Logs\ZD-count-" +
                                                  DateTime.Now.ToString("yyyy-MM-dd") + ".log"))
            {
                System.IO.File.Create(AppDomain.CurrentDomain.BaseDirectory + @"Logs\ZD-count-" +
                                                          DateTime.Now.ToString("yyyy-MM-dd") + ".log").Close();
                countpath = AppDomain.CurrentDomain.BaseDirectory + @"Logs\ZD-count-" +
                                                          DateTime.Now.ToString("yyyy-MM-dd") + ".log";
            }
            countpath = AppDomain.CurrentDomain.BaseDirectory + @"Logs\ZD-count-" +
                                          DateTime.Now.ToString("yyyy-MM-dd") + ".log";
            if (!IsInDesignMode)
            {
                _controlerIP = _config.AppSettings.Settings["IPaddress"].Value == null ? "127.0.0.1" : _config.AppSettings
                    .Settings["IPaddress"].Value;
                _port = _config.AppSettings.Settings["Port"].Value == null ? 1231 : int.Parse(_config.AppSettings
                    .Settings["Port"].Value);
                _clientRead = new NetVar(_controlerIP, _port, 31);
                //_clientRead.ThreadReceive.Start();
                //_clientRead.EDataReceiveCallBack += _clientRead_EDataReceiveCallBack;
                //AllData = XmlReadWriter.LoadFromFile<Data>(AppDomain.CurrentDomain.BaseDirectory + @"Settings\AllData.xml");
                //foreach (var item in AllData)
                //{
                //    item.Color = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                //}
                MethodItems = XmlReadWriter.LoadFromFile<Data>(AppDomain.CurrentDomain.BaseDirectory + @"Settings\MethodData.xml");
                LiquidTypes = XmlReadWriter.LoadFromFile<LiquidTypes>(AppDomain.CurrentDomain.BaseDirectory + @"Settings\LiquidTypes.xml");
                _allStatus = AllStatus.GetAllStatus();
            }
            _tabItemCount = MethodItems.Count;

            tubeTags = new int[6, 6];
            int[] seeds = new int[] { 0, 6, 12, 18, 24, 30 };
            for (int i = 0; i < 6; i++)
            {
                for (int k = 0; k < 6; k++)
                {
                    tubeTags[k, i] = i + seeds[k];
                }
            }

            zoneSelPoint = new Dictionary<int, int[]>();

            if (AllData.Count == 0)
            {
                for (int i = 0; i < 36; i++)
                {
                    AllData.Add(new Data()
                    {
                        Name = $@"N",
                        BigCleanValue = 0,
                        LittleCleanValue = 0,
                        Value = 0,
                        TagIndex = 0,
                        TagValue = 0,
                        AddIndex = 0,
                        EnableTag = false,
                        Color = new SolidColorBrush(Color.FromRgb(128, 128, 128))
                    });
                }
                //for (int i = 1; i < 2; i++)
                //{
                //    MethodItems.Add(new Data() { Name = $@"S{i}", Value = 0, CleanValue = 0, Color = new SolidColorBrush(Color.FromRgb(48, 66, 88)), ShowClose = true });
                //}
                //MethodItems.Last().ShowClose = true;
            }

            //if (IsInDesignMode)
            //{
            //    // Code runs in Blend --> create design CleanValue data.
            //}
            //else
            //{
            //    // Code runs "for real"
            //}

            LogHelper.instance().Info("软件启动，数据初始化完成！");
            //Growl.InfoGlobal("已恢复上一次保存的数据!");
            System.Media.SoundPlayer player = new System.Media.SoundPlayer();
            player.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + @"Music\Done.wav";
            player.Load();
            player.Play();
            musicPlayer.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + @"Music\Error.wav";
            musicPlayer.LoadAsync();
            _clientRead = new NetVar(_controlerIP, _port, 31);
            _clientRead.ThreadReceive.Start();
            //_clientRead.EInitStepCallBack += _clientRead_EInitStepCallBack;
            _clientRead.ERunStatusCallBack += _clientRead_ERunStatusCallBack;
            _clientRead.EDeviceErrorCallBack += _clientRead_EDeviceErrorCallBack;
            _clientRead.EDeviceConnected += _clientRead_EDeviceConnected;
            //_clientRead.ESolventErrorCallBack += _clientRead_ESolventErrorCallBack;
            //_clientRead.ThreadReceive.Start();
            //_clientRead.EInitStepCallBack += _clientRead_EInitStepCallBack;
            //_clientRead.EDataReceiveCallBack += _clientRead_EDataReceiveCallBack;
            //_clientRead.ERunStatusCallBack += _clientRead_ERunStatusCallBack;
            //_clientRead.EErrorReceiveCallBack += _clientRead_EErrorReceiveCallBack;
            //_clientRead.ERuningStepCallBack += _clientRead_ERuningStepCallBack;
            string line;
            try
            {
                //Application.Current.Dispatcher.Invoke(() =>
                //{
                //    LogDatas.Clear();
                //});
                //IntPtr vHandle = _lopen(AppDomain.CurrentDomain.BaseDirectory + @"Logs\SGS-error-" +
                //    DateTime.Now.ToString("yyyy-MM-dd") + ".log", OF_READWRITE | OF_SHARE_DENY_NONE);
                //if(vHandle == HFILE_ERROR)
                //{
                //    CloseHandle(vHandle);
                //    Console.WriteLine("File Used!");
                //    continue;
                //}
                //CloseHandle(vHandle);
                List<string> temp = new List<string>();
                using (System.IO.StreamReader sr = new System.IO.StreamReader(countpath, System.Text.Encoding.UTF8))
                {
                    // 从文件读取并显示行，直到文件的末尾
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Contains("本次完成的样品数量："))
                        {
                            TodayCount += int.Parse(line.Split('：').GetValue(4).ToString());
                        }
                        if (line.Contains("今日完成的样品数量------"))
                            continue;
                        temp.Add(line);
                    }
                }
                System.IO.FileStream fs = new System.IO.FileStream(countpath, System.IO.FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                foreach (var data in temp)
                {
                    sw.WriteLine(data);
                }
                sw.Close();
                fs.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                TodayCount = 0;
                //throw;
            }
        }

        private void _clientRead_ESolventErrorCallBack(List<bool> SolventErrorList)
        {

        }

        private void OffMainTab(bool off)
        {
            MainEnable = false;
        }

        private void _clientRead_EDeviceConnected(bool isConnected)
        {
            if (Status_Light != null && isConnected == true)
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    LightStatus = RunStatus.ready;
                    ReSetEnable = true;
                    StopEnable = true;
                    SettingEnable = true;
                    _clientRead.EDeviceConnected -= _clientRead_EDeviceConnected;
                    _clientRead.EInitStepCallBack += _clientRead_EInitStepCallBack;
                    _clientRead.ERuningStepCallBack += _clientRead_ERuningStepCallBack;
                });
            }
            else
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    ReSetEnable = false;
                    StopEnable = false;
                    SettingEnable = false;
                });
            }
        }

        private void GetLight(Border obj)
        {
            Status_Light = obj;
            LightStatus = RunStatus.connecting;
        }

        /// <summary>
        /// 显示软件盘
        /// </summary>
        /// <param name="obj">命令参数数组</param>
        private void ShowKeyPad(object[] obj)
        {
            if (obj[0] is Window && obj[1] is NumericUpDown)
            {
                KeyPad.Keypad keypad = new KeyPad.Keypad(obj[0] as Window, ((NumericUpDown)obj[1]).Tag.ToString(), ((NumericUpDown)obj[1]).Minimum, ((NumericUpDown)obj[1]).Maximum, ((NumericUpDown)obj[1]).Value);
                keypad.ShowDialog();
                double temp = double.NaN;
                double.TryParse(keypad.Result, out temp);
                ((NumericUpDown)obj[1]).Value = temp;
            }
        }

        /// <summary>
        /// 将数值进行减法操作
        /// </summary>
        /// <param name="obj">被操作的控件</param>
        private void DownValue(NumericUpDown obj)
        {
            if (obj.Name == "N3" || obj.Name == "N4")
            {
                obj.Value -= 10;
                return;
            }
            obj.Value -= 0.1;
        }

        /// <summary>
        /// 将数值进行加法操作
        /// </summary>
        /// <param name="obj">被操作的控件</param>
        private void UpValue(NumericUpDown obj)
        {
            if (obj.Name == "N3" || obj.Name == "N4")
            {
                obj.Value += 10;
                return;
            }
            obj.Value += 0.1;
        }

        /// <summary>
        /// 全选操作
        /// </summary>
        /// <param name="obj">方法内容</param>
        private void SelectAll(object[] obj)
        {
            if (_isRuning != RunStatus.ready || obj[0] == null) return;
            //throw new NotImplementedException();
            if (obj[0] == null)
                return;
            else if (!_selectAll)
            {
                foreach (var item in AllData)
                {
                    item.Name = ((Data)obj[0]).Name;
                    item.Value = ((Data)obj[0]).Value;
                    item.BigCleanValue = ((Data)obj[0]).BigCleanValue;
                    item.LittleCleanValue = ((Data)obj[0]).LittleCleanValue;
                    item.TagValue = ((Data)obj[0]).TagValue;
                    item.EnableTag = ((Data)obj[0]).EnableTag;
                    item.TagIndex = ((Data)obj[0]).TagIndex;
                    item.AddIndex = ((Data)obj[0]).AddIndex;
                    item.Color = ((Data)obj[0]).Color;
                    item.Color = new SolidColorBrush(Color.FromRgb(3, 195, 175));
                }
                _selectAll = true;
            }
            else
            {
                foreach (var item in AllData)
                {
                    item.Name = "N";
                    item.Value = 0;
                    item.BigCleanValue = 0;
                    item.LittleCleanValue = 0;
                    item.TagValue = 0;
                    item.EnableTag = false;
                    item.TagIndex = 0;
                    item.AddIndex = 0;
                    item.Color = ((Data)obj[0]).Color;
                    item.Color = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                }
                _selectAll = false;
            }
            _starttag = 0;
            _endtag = 0;
            _select = false;
            zoneSelPoint.Clear();
        }

        /// <summary>
        /// 应用列方法
        /// </summary>
        /// <param name="obj">方法内容</param>
        private void SetRow(object[] obj)
        {
            if (_isRuning != RunStatus.ready || obj[1] == null) return;
            bool iscancel = false;
            int start = 0;
            switch (obj[0].ToString())
            {
                case "A":
                    start = 0;
                    break;
                case "B":
                    start = 6;
                    break;
                case "C":
                    start = 12;
                    break;
                case "D":
                    start = 18;
                    break;
                case "E":
                    start = 24;
                    break;
                case "F":
                    start = 30;
                    break;
                default:
                    return;
            }
            for (int i = start; i <= start + 5; i++)
            {
                if (AllData[i].Name == ((Data)obj[1]).Name)
                {
                    AllData[i].Name = "N";
                    AllData[i].Value = 0;
                    AllData[i].BigCleanValue = 0;
                    AllData[i].LittleCleanValue = 0;
                    AllData[i].TagValue = 0;
                    AllData[i].EnableTag = false;
                    AllData[i].TagIndex = 0;
                    AllData[i].AddIndex = 0;
                    AllData[i].Color = ((Data)obj[1]).Color;
                    AllData[i].Color = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                    _select = false;
                    iscancel = true;
                }
                else
                {
                    AllData[i].Name = ((Data)obj[1]).Name;
                    AllData[i].Value = ((Data)obj[1]).Value;
                    AllData[i].BigCleanValue = ((Data)obj[1]).BigCleanValue;
                    AllData[i].LittleCleanValue = ((Data)obj[1]).LittleCleanValue;
                    AllData[i].TagValue = ((Data)obj[1]).TagValue;
                    AllData[i].EnableTag = ((Data)obj[1]).EnableTag;
                    AllData[i].TagIndex = ((Data)obj[1]).TagIndex;
                    AllData[i].AddIndex = ((Data)obj[1]).AddIndex;
                    AllData[i].Color = ((Data)obj[1]).Color;
                    AllData[i].Color = new SolidColorBrush(Color.FromRgb(3, 195, 175));
                }
            }
            if (iscancel)
                Growl.InfoGlobal("样品方法取消成功！");
            else
                Growl.InfoGlobal("样品方法应用成功！");
            _select = false;
            zoneSelPoint.Clear();
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 应用行方法
        /// </summary>
        /// <param name="obj">方法内容</param>
        private void SetColums(object[] obj)
        {
            bool iscancel = false;
            if (_isRuning != RunStatus.ready || obj[1] == null) return;
            int selectColIndex = 0;
            switch (obj[0].ToString())
            {
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                    selectColIndex = obj[0].ToString()[0] - '1';
                    break;
                default:
                    return;
            }
            int rows = tubeTags.GetLength(0);
            for (int i = 0; i < rows; i++)
            {
                int tag = tubeTags[i, selectColIndex];
                if (AllData[tag].Name == ((Data)obj[1]).Name)
                {
                    AllData[tag].Name = "N";
                    AllData[tag].Value = 0;
                    AllData[tag].BigCleanValue = 0;
                    AllData[tag].LittleCleanValue = 0;
                    AllData[i].TagValue = 0;
                    AllData[i].EnableTag = false;
                    AllData[i].TagIndex = 0;
                    AllData[i].AddIndex = 0;
                    AllData[tag].Color = ((Data)obj[1]).Color;
                    AllData[tag].Color = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                    iscancel = true;
                }
                else
                {
                    AllData[tag].Name = ((Data)obj[1]).Name;
                    AllData[tag].Value = ((Data)obj[1]).Value;
                    AllData[tag].BigCleanValue = ((Data)obj[1]).BigCleanValue;
                    AllData[tag].LittleCleanValue = ((Data)obj[1]).LittleCleanValue;
                    AllData[i].TagValue = ((Data)obj[1]).TagValue;
                    AllData[i].EnableTag = ((Data)obj[1]).EnableTag;
                    AllData[i].TagIndex = ((Data)obj[1]).TagIndex;
                    AllData[i].AddIndex = ((Data)obj[1]).AddIndex;
                    AllData[tag].Color = ((Data)obj[1]).Color;
                    AllData[tag].Color = new SolidColorBrush(Color.FromRgb(3, 195, 175));
                    iscancel = false;
                }
            }
            if (iscancel)
                Growl.InfoGlobal("样品方法取消成功！");
            else
                Growl.InfoGlobal("样品方法应用成功！");
            _select = false;
            zoneSelPoint.Clear();
        }

        /// <summary>
        /// 接收从工程师界面发送过来的新溶剂类型消息处理
        /// </summary>
        /// <param name="obj">溶剂类型集合</param>
        private void UpdateLiquidType(ObservableCollection<LiquidTypes> obj)
        {
            LiquidTypes.Clear();
            foreach (var item in obj)
            {
                LiquidTypes.Add(item);
            }
            LiquidSelectindex = 0;
        }

        /// <summary>
        /// 没用的函数
        /// </summary>
        /// <param name="obj"></param>
        private void DoSomeThing(LogViewViewModel obj)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 没用的函数
        /// </summary>
        /// <param name="obj"></param>

        private void NotifyMe(NotificationMessage obj)
        {
        }

        /// <summary>
        /// 没用的函数
        /// </summary>
        /// <param name="obj"></param>
        private void OpenHelper(Window obj)
        {
            //Window w = new HelpView();
            //w.Owner = obj;
            //w.ShowDialog();
        }

        /// <summary>
        /// 根据命令执行对应函数
        /// </summary>
        /// <param name="obj">命令内容及参数</param>
        private void ActionDo(object[] obj)
        {
            switch (obj[1].ToString())
            {
                case "复位":
                    //Status_Light = ((DeviceMain)obj[0]).StatusLight;
                    //LightStatus = RunStatus.init;
                    SendCommand2Plc(RunStatus.init);
                    //_config.AppSettings.Settings["Z1_Sensitivity"].Value = ((LiquidTypes)obj[2])?.Z1Value.ToString();
                    //_config.AppSettings.Settings["Z2_Sensitivity"].Value = ((LiquidTypes)obj[2])?.Z2Value.ToString();
                    //SendData();
                    //_clientRead.ERunStatusCallBack += _clientRead_ERunStatusCallBack;
                    //_clientRead.EDeviceErrorCallBack += _clientRead_EDeviceErrorCallBack;
                    //_isRuning = RunStatus.init;
                    StartEnable = false;
                    PauseEnable = false;
                    StopEnable = true;
                    ReSetEnable = false;
                    CleanEnable = false;
                    SettingEnable = false;
                    //HelperEnable = true;
                    if (_clientRead.ThreadReceive.IsAlive != true)
                        _clientRead.ThreadReceive.Start();
                    LogHelper.instance().Info("上位机动作：设备进入初始化状态！");
                    break;
                case "开始":
                    if (_isRuning == RunStatus.clean_error)
                    {
                        System.Threading.Tasks.Task.Run(() =>
                        {
                            SendCommand2Plc(RunStatus.clean_error);
                            System.Threading.Thread.Sleep(1000);
                            //SendCommand2Plc(RunStatus.start);
                        });
                        //StartEnable = false;
                        //PauseEnable = true;
                        //ReSetEnable = false;
                        //LightStatus = RunStatus.start;
                        //_isRuning = RunStatus.start;
                        return;
                    }
                    if (_isRuning == RunStatus.pause)
                    {
                        SendCommand2Plc(RunStatus.start);
                        //StartEnable = false;
                        //PauseEnable = true;
                        //ReSetEnable = false;
                        //LightStatus = RunStatus.start;
                        //_isRuning = RunStatus.start;
                        return;
                    }
                    if (RunArea == 1)
                        foreach (var item in AllData)
                        {
                            if(item.Value > 15)
                            {
                                if (MessageBoxResult.Yes == System.Windows.MessageBox.Show("检测到运行区域为B区，且加液体积大于运行区域试管规定体积15ml，是否继续执行？", "运行确认", MessageBoxButton.YesNo, MessageBoxImage.Question))
                                    break;
                                else
                                    return;
                            }
                        }

                    LightStatus = RunStatus.start;
                    StartEnable = false;
                    PauseEnable = true;
                    //StopEnable = true;
                    ReSetEnable = false;
                    CleanEnable = false;
                    SettingEnable = false;
                    //HelperEnable = true;
                    _isRuning = RunStatus.start;
                    _config.AppSettings.Settings["Z1_Sensitivity"].Value = ((LiquidTypes)obj[2])?.Z1Value.ToString();
                    _config.AppSettings.Settings["Z2_Sensitivity"].Value = ((LiquidTypes)obj[2])?.Z2Value.ToString();
                    _config.Save();
                    System.Threading.Tasks.Task.Run(() =>
                    {
                        SendData();
                        Thread.Sleep(500);
                        SendCommand2Plc(RunStatus.start);
                    });
                    //SendData();
                    //SendCommand2Plc(RunStatus.start);
                    if (_clientRead.ThreadReceive.IsAlive != true)
                    {
                        _clientRead.ThreadReceive.Start();
                    }
                    //_clientRead.ERunStatusCallBack += _clientRead_ERunStatusCallBack;
                    //_clientRead.EDeviceErrorCallBack += _clientRead_EDeviceErrorCallBack;
                    _clientRead.ERuningStepCallBack += _clientRead_ERuningStepCallBack;
                    _clientRead.EDataReceiveCallBack += _clientRead_EDataReceiveCallBack;
                    Status_Light = ((DeviceMain)obj[0]).StatusLight;
                    LightStatus = RunStatus.start;
                    LogHelper.instance().Info("上位机动作：设备进入执行操作状态！");
                    CountLog(countpath, "本次动作开始！");

                    break;
                case "暂停":
                    StartEnable = true;
                    PauseEnable = false;
                    //StopEnable = true;
                    ReSetEnable = false;
                    CleanEnable = false;
                    SettingEnable = false;
                    //HelperEnable = true;
                    SendCommand2Plc(RunStatus.pause);
                    //_isRuning = RunStatus.pause;
                    LogHelper.instance().Info("上位机动作：设备进入暂停状态！");
                    break;
                case "急停":
                    Status_Light = ((DeviceMain)obj[0]).StatusLight;
                    StopEnable = false;
                    PauseEnable = false;
                    ReSetEnable = true;
                    StartEnable = false;
                    CleanEnable = false;
                    //((DeviceMain)obj[0]).StatusLight.Background = new SolidColorBrush(Colors.Gray);
                    //BorderAttach.SetFlashing(((DeviceMain)obj[0]).StatusLight, false);
                    SettingEnable = false;
                    //HelperEnable = true;
                    IsBusy = false;
                    _isRuning = RunStatus.estop;
                    _clientRead.ERuningStepCallBack -= _clientRead_ERuningStepCallBack;
                    //_clientRead.ERunStatusCallBack -= _clientRead_ERunStatusCallBack;
                    _clientRead.EDataReceiveCallBack -= _clientRead_EDataReceiveCallBack;
                    //_clientRead.EDeviceErrorCallBack -= _clientRead_EDeviceErrorCallBack;
                    //_clientRead.EInitStepCallBack -= _clientRead_EInitStepCallBack;
                    /*
                     if (_clientRead.ThreadReceive.IsAlive)
                    {
                        IsBusy = true;
                        _clientRead.stop = true;
                        //System.Threading.Tasks.Task.Run(
                        //    () =>
                        //    {
                        //        while (_clientRead.ThreadReceive.IsAlive == true)
                        //            ;
                        //        Application.Current.Dispatcher.Invoke(() =>
                        //        {
                        //            ((Border)obj[0]).Background = new SolidColorBrush(Colors.Gray);
                        //            BorderAttach.SetFlashing((Border)obj[0], false);
                        //            StartEnable = true;
                        //            SettingEnable = true;
                        //        });
                        //    });
                        await Task.Run(() =>
                        {
                            Thread.Sleep(1000);
                            _clientRead.stop = true;
                            while (_clientRead.ThreadReceive.IsAlive)
                            {
                            }
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                ((DeviceMain)obj[0]).StatusLight.Background = new SolidColorBrush(Colors.Gray);
                                BorderAttach.SetFlashing(((DeviceMain)obj[0]).StatusLight, false);
                                StartEnable = true;
                                SettingEnable = true;
                                HelperEnable = true;
                                IsBusy = false;
                                _isRuning = RunStatus.stop;
                            });
                        });
                    }
                    */

                    //Task.WaitAll(new Task(() =>
                    //{
                    //    _clientRead.EDataReceiveCallBack -= _clientRead_EDataReceiveCallBack;
                    //    _clientRead.stop = true;
                    //}));
                    SendCommand2Plc(RunStatus.ready);
                    LightStatus = RunStatus.estop;
                    Growl.ErrorGlobal("设备进入急停状态！");
                    LogHelper.instance().Error("上位机动作：设备进入急停状态，请重新初始化设备！");
                    break;
                case "排空":
                    StartEnable = false;
                    PauseEnable = true;
                    //StopEnable = true;
                    ReSetEnable = false;
                    CleanEnable = false;
                    SettingEnable = false;
                    //HelperEnable = true;
                    SendCommand2Plc(RunStatus.clean);
                    LightStatus = RunStatus.init;
                    _isRuning = RunStatus.clean;
                    LogHelper.instance().Info("上位机动作：设备进入排空状态！");
                    Growl.InfoGlobal("设备排空操作开始！");
                    if (_clientRead.ThreadReceive.IsAlive != true)
                    {
                        _clientRead.ThreadReceive.Start();
                    }
                    //_clientRead.ERunStatusCallBack += _clientRead_ERunStatusCallBack;
                    break;
            }
        }

        /// <summary>
        /// 向下位机发送参数以及配置
        /// </summary>
        private void SendData()
        {
            NetVar writeVar = new NetVar(_controlerIP, 34);
            List<CDataTypeCollection> dataTypeCollection;
            dataTypeCollection = new List<CDataTypeCollection>();
            List<CDataTypeCollection> PostionCollection;
            PostionCollection = new List<CDataTypeCollection>();

            ObservableCollection<PositionData> PositionDatas = new ObservableCollection<PositionData>();
            ObservableCollection<CompensateData> pump_A = new ObservableCollection<CompensateData>();
            ObservableCollection<CompensateData> pump_B = new ObservableCollection<CompensateData>();
            ObservableCollection<CompensateData> pump_C = new ObservableCollection<CompensateData>();


            PositionDatas = XmlReadWriter.LoadFromFile<PositionData>(AppDomain.CurrentDomain.BaseDirectory + @"\Settings\PositionData.xml");
            pump_A = XmlReadWriter.LoadFromFile<CompensateData>(AppDomain.CurrentDomain.BaseDirectory + @"\Settings\Pump_A.xml");
            pump_B = XmlReadWriter.LoadFromFile<CompensateData>(AppDomain.CurrentDomain.BaseDirectory + @"\Settings\Pump_B.xml");
            pump_C = XmlReadWriter.LoadFromFile<CompensateData>(AppDomain.CurrentDomain.BaseDirectory + @"\Settings\Pump_C.xml");

            foreach (var pos in PositionDatas)
            {
                PostionCollection.Add(new CDataTypeCollection(pos.Px, DataTypes.dinttype, "Px"));
                PostionCollection.Add(new CDataTypeCollection(pos.Py, DataTypes.dinttype, "Py"));
                //PostionCollection.Add(new CDataTypeCollection(pos.Px2, DataTypes.dinttype));
                //PostionCollection.Add(new CDataTypeCollection(pos.Py2, DataTypes.dinttype));
            }

            //for (int i = 0; i < 21; i = i + 6)
            //{
            //    PostionCollection.Add(new CDataTypeCollection(PositionDatas[i + 6].Px, DataTypes.dinttype));
            //    PostionCollection.Add(new CDataTypeCollection(PositionDatas[i + 6].Py, DataTypes.dinttype));
            //    PostionCollection.Add(new CDataTypeCollection(PositionDatas[i + 6].Px2, DataTypes.dinttype));
            //    PostionCollection.Add(new CDataTypeCollection(PositionDatas[i + 6].Py2, DataTypes.dinttype));
            //}

            dataTypeCollection.AddRange(PostionCollection);

            int loop = 0;
            foreach (var data in AllData)
            {
                if (data.Name != "N")
                {
                    double compensationA = 0.0;
                    double compensationB = 0.0;
                    double compensationC = 0.0;

                    double temp = data.Value % 25;

                    if (pump_A[0].Min < temp && temp <= pump_A[0].Max)
                    {
                        switch (pump_A[0].SelectMethod)
                        {
                            case 0:
                                compensationA = pump_A[0].A;
                                compensationA = Math.Round(compensationA, 2);
                                break;
                            case 1:
                                compensationA = Math.Round(pump_A[0].A * temp, 4) + pump_A[0].C;
                                compensationA = Math.Round(compensationA, 2);
                                break;
                            case 2:
                                compensationA = Math.Round(pump_A[0].A * Math.Pow(temp, 2), 4)
                                    + Math.Round(pump_A[0].B * temp, 4)
                                    + pump_A[0].C;
                                compensationA = Math.Round(compensationA, 2);
                                break;
                            default:
                                compensationA = Math.Round(compensationA, 2);
                                break;
                        }
                    }
                    else if (pump_A[1].Min < temp && temp <= pump_A[1].Max)
                    {
                        switch (pump_A[1].SelectMethod)
                        {
                            case 0:
                                compensationA = pump_A[1].A;
                                compensationA = Math.Round(compensationA, 2);
                                break;
                            case 1:
                                compensationA = Math.Round(pump_A[1].A * temp, 4) + pump_A[1].C;
                                compensationA = Math.Round(compensationA, 2);
                                break;
                            case 2:
                                compensationA = Math.Round(pump_A[1].A * Math.Pow(temp, 2), 4)
                                    + Math.Round(pump_A[1].B * temp, 4)
                                    + pump_A[1].C;
                                compensationA = Math.Round(compensationA, 2);
                                break;
                            default:
                                compensationA = Math.Round(compensationA, 2);
                                break;
                        }
                    }
                    else if (pump_A[2].Min < temp && temp <= pump_A[2].Max)
                    {
                        switch (pump_A[2].SelectMethod)
                        {
                            case 0:
                                compensationA = pump_A[2].A;
                                compensationA = Math.Round(compensationA, 2);
                                break;
                            case 1:
                                compensationA = Math.Round(pump_A[2].A * temp, 4) + pump_A[2].C;
                                compensationA = Math.Round(compensationA, 2);
                                break;
                            case 2:
                                compensationA = Math.Round(pump_A[2].A * Math.Pow(temp, 2), 4)
                                    + Math.Round(pump_A[2].B * temp, 4)
                                    + pump_A[2].C;
                                compensationA = Math.Round(compensationA, 2);
                                break;
                            default:
                                compensationA = Math.Round(compensationA, 2);
                                break;
                        }
                    }
                    else if (pump_A[3].Min < temp && temp <= pump_A[3].Max)
                    {
                        switch (pump_A[3].SelectMethod)
                        {
                            case 0:
                                compensationA = pump_A[3].A;
                                compensationA = Math.Round(compensationA, 2);
                                break;
                            case 1:
                                compensationA = Math.Round(pump_A[3].A * temp, 4) + pump_A[3].C;
                                compensationA = Math.Round(compensationA, 2);
                                break;
                            case 2:
                                compensationA = Math.Round(pump_A[3].A * Math.Pow(temp, 2), 4)
                                    + Math.Round(pump_A[3].B * temp, 4)
                                    + pump_A[3].C;
                                compensationA = Math.Round(compensationA, 2);
                                break;
                            default:
                                compensationA = Math.Round(compensationA, 2);
                                break;
                        }
                    }
                    else if (pump_A[4].Min < temp && temp <= pump_A[4].Max)
                    {
                        switch (pump_A[4].SelectMethod)
                        {
                            case 0:
                                compensationA = pump_A[4].A;
                                compensationA = Math.Round(compensationA, 2);
                                break;
                            case 1:
                                compensationA = Math.Round(pump_A[4].A * temp, 4) + pump_A[4].C;
                                compensationA = Math.Round(compensationA, 2);
                                break;
                            case 2:
                                compensationA = Math.Round(pump_A[4].A * Math.Pow(temp, 2), 4)
                                    + Math.Round(pump_A[4].B * temp, 4)
                                    + pump_A[4].C;
                                compensationA = Math.Round(compensationA, 2);
                                break;
                            default:
                                compensationA = Math.Round(compensationA, 2);
                                break;
                        }
                    }

                    if (pump_B[0].Min < temp && temp <= pump_B[0].Max)
                    {
                        switch (pump_B[0].SelectMethod)
                        {
                            case 0:
                                compensationB = pump_B[0].A;
                                compensationB = Math.Round(compensationB, 2);
                                break;
                            case 1:
                                compensationB = Math.Round(pump_B[0].A * temp, 4) + pump_B[0].C;
                                compensationB = Math.Round(compensationB, 2);
                                break;
                            case 2:
                                compensationB = Math.Round(pump_B[0].A * Math.Pow(temp, 2), 4)
                                    + Math.Round(pump_B[0].B * temp, 4)
                                    + pump_B[0].C;
                                compensationB = Math.Round(compensationB, 2);
                                break;
                            default:
                                compensationB = Math.Round(compensationB, 2);
                                break;
                        }
                    }
                    else if (pump_B[1].Min < temp && temp <= pump_B[1].Max)
                    {
                        switch (pump_B[1].SelectMethod)
                        {
                            case 0:
                                compensationB = pump_B[1].A;
                                compensationB = Math.Round(compensationB, 2);
                                break;
                            case 1:
                                compensationB = Math.Round(pump_B[1].A * temp, 4) + pump_B[1].C;
                                compensationB = Math.Round(compensationB, 2);
                                break;
                            case 2:
                                compensationB = Math.Round(pump_B[1].A * Math.Pow(temp, 2), 4)
                                    + Math.Round(pump_B[1].B * temp, 4)
                                    + pump_B[1].C;
                                compensationB = Math.Round(compensationB, 2);
                                break;
                            default:
                                compensationB = Math.Round(compensationB, 2);
                                break;
                        }
                    }
                    else if (pump_B[2].Min < temp && temp <= pump_B[2].Max)
                    {
                        switch (pump_B[2].SelectMethod)
                        {
                            case 0:
                                compensationB = pump_B[2].A;
                                compensationB = Math.Round(compensationB, 2);
                                break;
                            case 1:
                                compensationB = Math.Round(pump_B[2].A * temp, 4) + pump_B[2].C;
                                compensationB = Math.Round(compensationB, 2);
                                break;
                            case 2:
                                compensationB = Math.Round(pump_B[2].A * Math.Pow(temp, 2), 4)
                                    + Math.Round(pump_B[2].B * temp, 4)
                                    + pump_B[2].C;
                                compensationB = Math.Round(compensationB, 2);
                                break;
                            default:
                                compensationB = Math.Round(compensationB, 2);
                                break;
                        }
                    }
                    else if (pump_B[3].Min < temp && temp <= pump_B[3].Max)
                    {
                        switch (pump_B[3].SelectMethod)
                        {
                            case 0:
                                compensationB = pump_B[3].A;
                                compensationB = Math.Round(compensationB, 2);
                                break;
                            case 1:
                                compensationB = Math.Round(pump_B[3].A * temp, 4) + pump_B[3].C;
                                compensationB = Math.Round(compensationB, 2);
                                break;
                            case 2:
                                compensationB = Math.Round(pump_B[3].A * Math.Pow(temp, 2), 4)
                                    + Math.Round(pump_B[3].B * temp, 4)
                                    + pump_B[3].C;
                                compensationB = Math.Round(compensationB, 2);
                                break;
                            default:
                                compensationB = Math.Round(compensationB, 2);
                                break;
                        }
                    }
                    else if (pump_B[4].Min < temp && temp <= pump_B[4].Max)
                    {
                        switch (pump_B[4].SelectMethod)
                        {
                            case 0:
                                compensationB = pump_B[4].A;
                                compensationB = Math.Round(compensationB, 2);
                                break;
                            case 1:
                                compensationB = Math.Round(pump_B[4].A * temp, 4) + pump_B[4].C;
                                compensationB = Math.Round(compensationB, 2);
                                break;
                            case 2:
                                compensationB = Math.Round(pump_B[4].A * Math.Pow(temp, 2), 4)
                                    + Math.Round(pump_B[4].B * temp, 4)
                                    + pump_B[4].C;
                                compensationB = Math.Round(compensationB, 2);
                                break;
                            default:
                                compensationB = Math.Round(compensationB, 2);
                                break;
                        }
                    }

                    if (pump_C[0].Min < data.TagValue && data.TagValue <= pump_C[0].Max)
                    {
                        switch (pump_C[0].SelectMethod)
                        {
                            case 0:
                                compensationC = pump_C[0].A;
                                compensationC = Math.Round(compensationC, 2);
                                break;
                            case 1:
                                compensationC = Math.Round(pump_C[0].A * data.TagValue, 4) + pump_C[0].C;
                                compensationC = Math.Round(compensationC, 2);
                                break;
                            case 2:
                                compensationC = Math.Round(pump_C[0].A * Math.Pow(data.TagValue, 2), 4)
                                    + Math.Round(pump_C[0].B * data.TagValue, 4)
                                    + pump_C[0].C;
                                compensationC = Math.Round(compensationC, 2);
                                break;
                            default:
                                compensationC = Math.Round(compensationC, 2);
                                break;
                        }
                    }
                    else if (pump_C[1].Min < data.TagValue && data.TagValue <= pump_C[1].Max)
                    {
                        switch (pump_C[1].SelectMethod)
                        {
                            case 0:
                                compensationC = pump_C[1].A;
                                compensationC = Math.Round(compensationC, 2);
                                break;
                            case 1:
                                compensationC = Math.Round(pump_C[1].A * data.TagValue, 4) + pump_C[1].C;
                                compensationC = Math.Round(compensationC, 2);
                                break;
                            case 2:
                                compensationC = Math.Round(pump_C[1].A * Math.Pow(data.TagValue, 2), 4)
                                    + Math.Round(pump_C[1].B * data.TagValue, 4)
                                    + pump_C[1].C;
                                compensationC = Math.Round(compensationC, 2);
                                break;
                            default:
                                compensationC = Math.Round(compensationC, 2);
                                break;
                        }
                    }
                    else if (pump_C[2].Min < data.TagValue && data.TagValue <= pump_C[2].Max)
                    {
                        switch (pump_C[2].SelectMethod)
                        {
                            case 0:
                                compensationC = pump_C[2].A;
                                compensationC = Math.Round(compensationC, 2);
                                break;
                            case 1:
                                compensationC = Math.Round(pump_C[2].A * data.TagValue, 4) + pump_C[2].C;
                                compensationC = Math.Round(compensationC, 2);
                                break;
                            case 2:
                                compensationC = Math.Round(pump_C[2].A * Math.Pow(data.TagValue, 2), 4)
                                    + Math.Round(pump_C[2].B * data.TagValue, 4)
                                    + pump_C[2].C;
                                compensationC = Math.Round(compensationC, 2);
                                break;
                            default:
                                compensationC = Math.Round(compensationC, 2);
                                break;
                        }
                    }
                    else if (pump_C[3].Min < data.TagValue && data.TagValue <= pump_C[3].Max)
                    {
                        switch (pump_C[3].SelectMethod)
                        {
                            case 0:
                                compensationC = pump_C[3].A;
                                compensationC = Math.Round(compensationC, 2);
                                break;
                            case 1:
                                compensationC = Math.Round(pump_C[3].A * data.TagValue, 4) + pump_C[3].C;
                                compensationC = Math.Round(compensationC, 2);
                                break;
                            case 2:
                                compensationC = Math.Round(pump_C[3].A * Math.Pow(data.TagValue, 2), 4)
                                    + Math.Round(pump_C[3].B * data.TagValue, 4)
                                    + pump_C[3].C;
                                compensationC = Math.Round(compensationC, 2);
                                break;
                            default:
                                compensationC = Math.Round(compensationC, 2);
                                break;
                        }
                    }
                    else if (pump_C[4].Min < data.TagValue && data.TagValue <= pump_C[4].Max)
                    {
                        switch (pump_C[4].SelectMethod)
                        {
                            case 0:
                                compensationC = pump_C[4].A;
                                compensationC = Math.Round(compensationC, 2);
                                break;
                            case 1:
                                compensationC = Math.Round(pump_C[4].A * data.TagValue, 4) + pump_C[4].C;
                                compensationC = Math.Round(compensationC, 2);
                                break;
                            case 2:
                                compensationC = Math.Round(pump_C[4].A * Math.Pow(data.TagValue, 2), 4)
                                    + Math.Round(pump_C[4].B * data.TagValue, 4)
                                    + pump_C[4].C;
                                compensationC = Math.Round(compensationC, 2);
                                break;
                            default:
                                compensationC = Math.Round(compensationC, 2);
                                break;
                        }
                    }

                    dataTypeCollection.Add(new CDataTypeCollection(data.Value, DataTypes.realtype, "AddValue"));//加液值
                    //dataTypeCollection.Add(new CDataTypeCollection(data.BigCleanValue, DataTypes.realtype));
                    //dataTypeCollection.Add(new CDataTypeCollection(data.LittleCleanValue, DataTypes.realtype));
                    dataTypeCollection.Add(new CDataTypeCollection(data.TagValue, DataTypes.realtype, "TagValue"));//加标值
                    dataTypeCollection.Add(new CDataTypeCollection(data.EnableTag, DataTypes.realtype, "EnableTag"));//启用加标
                    dataTypeCollection.Add(new CDataTypeCollection(data.TagIndex + 1, DataTypes.realtype, "TagIndex"));//0->1
                    dataTypeCollection.Add(new CDataTypeCollection(data.AddIndex + 1, DataTypes.realtype, "AddIndex"));//0->1
                    dataTypeCollection.Add(new CDataTypeCollection(compensationA, DataTypes.realtype, "compensationA"));//补偿值A
                    dataTypeCollection.Add(new CDataTypeCollection(compensationB, DataTypes.realtype, "compensationB"));//补偿值B
                    dataTypeCollection.Add(new CDataTypeCollection(compensationC, DataTypes.realtype, "compensationC"));//加标补偿值C
                    loop++;
                }
                else
                {
                    dataTypeCollection.Add(new CDataTypeCollection(0, DataTypes.realtype, "AddValue"));//加液值
                    //dataTypeCollection.Add(new CDataTypeCollection(0, DataTypes.realtype));
                    //dataTypeCollection.Add(new CDataTypeCollection(0, DataTypes.realtype));
                    dataTypeCollection.Add(new CDataTypeCollection(0, DataTypes.realtype, "TagValue"));//加标值
                    dataTypeCollection.Add(new CDataTypeCollection(0, DataTypes.realtype, "EnableTag"));//启用加标
                    dataTypeCollection.Add(new CDataTypeCollection(1, DataTypes.realtype, "TagIndex"));//0->1
                    dataTypeCollection.Add(new CDataTypeCollection(1, DataTypes.realtype, "AddIndex"));//0->1
                    dataTypeCollection.Add(new CDataTypeCollection(0, DataTypes.realtype, "compensationA"));//补偿值A
                    dataTypeCollection.Add(new CDataTypeCollection(0, DataTypes.realtype, "compensationB"));//补偿值B
                    dataTypeCollection.Add(new CDataTypeCollection(0, DataTypes.realtype, "compensationC"));//加标补偿值C
                }
            }

            dataTypeCollection.Add(
                new CDataTypeCollection(_config.AppSettings.Settings["Z1_Sensitivity"].Value ==
                null ? -5000 : int.Parse(MainViewModel._config.AppSettings.Settings["Z1_Sensitivity"].Value), DataTypes.dinttype, "Z1_Sensitivity"));
            dataTypeCollection.Add(
                new CDataTypeCollection(_config.AppSettings.Settings["Z2_Sensitivity"].Value ==
                null ? -5000 : int.Parse(MainViewModel._config.AppSettings.Settings["Z2_Sensitivity"].Value), DataTypes.dinttype, "Z2_Sensitivity"));
            dataTypeCollection.Add(
                new CDataTypeCollection(_config.AppSettings.Settings["Z1_Down"].Value ==
                null ? -95000 : int.Parse(MainViewModel._config.AppSettings.Settings["Z1_Down"].Value), DataTypes.dinttype, "Z1_Down"));
            dataTypeCollection.Add(
                new CDataTypeCollection(_config.AppSettings.Settings["Z2_Down"].Value ==
                null ? -95000 : int.Parse(MainViewModel._config.AppSettings.Settings["Z2_Down"].Value), DataTypes.dinttype, "Z2_Down"));
            dataTypeCollection.Add(
                new CDataTypeCollection(_config.AppSettings.Settings["BigClean"].Value ==
                null ? 10 : int.Parse(MainViewModel._config.AppSettings.Settings["BigClean"].Value), DataTypes.realtype, "BigClean"));
            dataTypeCollection.Add(
                new CDataTypeCollection(_config.AppSettings.Settings["LittleClean"].Value ==
                null ? 200 : int.Parse(MainViewModel._config.AppSettings.Settings["LittleClean"].Value), DataTypes.realtype, "LittleClean"));
            dataTypeCollection.Add(
                new CDataTypeCollection(_config.AppSettings.Settings["FullA"].Value ==
                null ? 0 : double.Parse(MainViewModel._config.AppSettings.Settings["FullA"].Value), DataTypes.realtype, "FullA"));
            dataTypeCollection.Add(
                new CDataTypeCollection(_config.AppSettings.Settings["FullB"].Value ==
                null ? 0 : double.Parse(MainViewModel._config.AppSettings.Settings["FullB"].Value), DataTypes.realtype, "FullB"));

            dataTypeCollection.Add(new CDataTypeCollection(RunArea + 1, DataTypes.realtype, "RunArea"));

            writeVar.WriteData(dataTypeCollection);
            writeVar.Disconnect();

            App.Current.Dispatcher.Invoke(() =>
            {
                Growl.InfoGlobal("发送数据到下位机！");
            });

            #region 1000 个 DINT 测试 
            //writeVar = new NetVar(_controlerIP, 33);

            //List<CDataTypeCollection> temp;
            //temp = new List<CDataTypeCollection>();

            //for (int i = 0; i < 1000; i++)
            //{
            //    temp.Add(new CDataTypeCollection("Send Data Send Data Send Data Send Data Send Data Send Data Send Data Send Data ", DataTypes.stringtype));
            //}

            //writeVar.WriteData(temp);
            //writeVar.Disconnect();
            #endregion

        }

        /// <summary>
        /// 向下位机发送操作指令
        /// </summary>
        /// <param name="runStatus">操作指令</param>
        private void SendCommand2Plc(RunStatus runStatus)
        {
            NetVar sender = new NetVar(_controlerIP, 88);
            List<CDataTypeCollection> sendercommand;
            sendercommand = new List<CDataTypeCollection>();
            switch (runStatus)
            {
                case RunStatus.init:
                    sendercommand.Add(new CDataTypeCollection(true, DataTypes.booltype, "init"));
                    sendercommand.Add(new CDataTypeCollection(false, DataTypes.booltype, "start"));
                    sendercommand.Add(new CDataTypeCollection(false, DataTypes.booltype, "pause"));
                    sendercommand.Add(new CDataTypeCollection(false, DataTypes.booltype, "stop"));
                    sendercommand.Add(new CDataTypeCollection(false, DataTypes.booltype, "clean"));
                    sendercommand.Add(new CDataTypeCollection(false, DataTypes.booltype, "clean_error"));
                    #region 发送液位监控数据
                    WatchLiquidTube = WatchLiquidTube = XmlReadWriter.LoadFromFile<bool>(AppDomain.CurrentDomain.BaseDirectory + @"\Settings\WatchLiquidTube.xml");
                    if (WatchLiquidTube.Count != 0)
                    {
                        NetVar wireData = new NetVar(_controlerIP, 99);
                        List<CDataTypeCollection> senderdata;
                        senderdata = new List<CDataTypeCollection>();
                        for (int i = 0; i < 16; i++)
                        {
                            if (i >= 10)
                                senderdata.Add(new CDataTypeCollection(WatchLiquidTube?[i - 10], DataTypes.booltype));
                            else
                                senderdata.Add(new CDataTypeCollection(false, DataTypes.booltype));
                        }
                        wireData.WriteData(senderdata);
                        wireData.Disconnect();
                    }
                    #endregion
                    break;
                case RunStatus.start:
                    sendercommand.Add(new CDataTypeCollection(false, DataTypes.booltype, "init"));
                    sendercommand.Add(new CDataTypeCollection(true, DataTypes.booltype, "start"));
                    sendercommand.Add(new CDataTypeCollection(false, DataTypes.booltype, "pause"));
                    sendercommand.Add(new CDataTypeCollection(false, DataTypes.booltype, "stop"));
                    sendercommand.Add(new CDataTypeCollection(false, DataTypes.booltype, "clean"));
                    sendercommand.Add(new CDataTypeCollection(false, DataTypes.booltype, "clean_error"));
                    break;
                case RunStatus.pause:
                    sendercommand.Add(new CDataTypeCollection(false, DataTypes.booltype, "init"));
                    sendercommand.Add(new CDataTypeCollection(false, DataTypes.booltype, "start"));
                    sendercommand.Add(new CDataTypeCollection(true, DataTypes.booltype, "pause"));
                    sendercommand.Add(new CDataTypeCollection(false, DataTypes.booltype, "stop"));
                    sendercommand.Add(new CDataTypeCollection(false, DataTypes.booltype, "clean"));
                    sendercommand.Add(new CDataTypeCollection(false, DataTypes.booltype, "clean_error"));
                    break;
                case RunStatus.ready:
                    sendercommand.Add(new CDataTypeCollection(false, DataTypes.booltype, "init"));
                    sendercommand.Add(new CDataTypeCollection(false, DataTypes.booltype, "start"));
                    sendercommand.Add(new CDataTypeCollection(false, DataTypes.booltype, "pause"));
                    sendercommand.Add(new CDataTypeCollection(true, DataTypes.booltype, "stop"));
                    sendercommand.Add(new CDataTypeCollection(false, DataTypes.booltype, "clean"));
                    sendercommand.Add(new CDataTypeCollection(false, DataTypes.booltype, "clean_error"));
                    break;
                case RunStatus.clean:
                    sendercommand.Add(new CDataTypeCollection(false, DataTypes.booltype, "init"));
                    sendercommand.Add(new CDataTypeCollection(false, DataTypes.booltype, "start"));
                    sendercommand.Add(new CDataTypeCollection(false, DataTypes.booltype, "pause"));
                    sendercommand.Add(new CDataTypeCollection(false, DataTypes.booltype, "stop"));
                    sendercommand.Add(new CDataTypeCollection(true, DataTypes.booltype, "clean"));
                    sendercommand.Add(new CDataTypeCollection(false, DataTypes.booltype, "clean_error"));
                    break;
                case RunStatus.clean_error:
                    sendercommand.Add(new CDataTypeCollection(false, DataTypes.booltype, "init"));
                    sendercommand.Add(new CDataTypeCollection(false, DataTypes.booltype, "start"));
                    sendercommand.Add(new CDataTypeCollection(false, DataTypes.booltype, "pause"));
                    sendercommand.Add(new CDataTypeCollection(false, DataTypes.booltype, "stop"));
                    sendercommand.Add(new CDataTypeCollection(false, DataTypes.booltype, "clean"));
                    sendercommand.Add(new CDataTypeCollection(true, DataTypes.booltype, "clean_error"));
                    sender.WriteData(sendercommand);
                    //Task.Run(() =>
                    //{
                    //    sender.WriteData(sendercommand);
                    //    Thread.Sleep(5);
                    //    sendercommand[5].SendValue = false;
                    //    sender.WriteData(sendercommand);
                    //    sender.Disconnect();
                    //});
                    return;
                default:
                    break;
            }
            sender.WriteData(sendercommand);
            sender.Disconnect();
        }

        /// <summary>
        /// 更新下位机返回的样品区工作状态
        /// </summary>
        /// <param name="reStatus">下位机返回的样品状态集合</param>
        private void _clientRead_EDataReceiveCallBack(List<int> reStatus)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                int loop = 0;
                foreach (var status in reStatus)
                {
                    if (AllData[loop].Name != "N")
                    {
                        switch (status)
                        {
                            case 0:
                                AllData[loop].Color = new SolidColorBrush(Color.FromRgb(78, 180, 72));
                                break;
                            case 1:
                                AllData[loop].Color = new SolidColorBrush(Color.FromRgb(70, 134, 198));
                                break;
                            case 2:
                                AllData[loop].Color = new SolidColorBrush(Color.FromRgb(247, 148, 33));
                                break;
                            case 3:
                                AllData[loop].Color = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                                break;
                            default:
                                break;
                        }
                    }
                    loop++;
                }
            });
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 更新下位机返回的错误ID号
        /// </summary>
        /// <param name="device_error_id">下位机错误ID号</param>
        private void _clientRead_EDeviceErrorCallBack(int device_error_id)
        {
            ErrorID = device_error_id;
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 更新下位机初始化动作步骤内容
        /// </summary>
        /// <param name="init_step_id">下位机当前动作ID号</param>
        private void _clientRead_EInitStepCallBack(int init_step_id)
        {
            //throw new NotImplementedException();
            if (_allStatus.InitStatus.ContainsKey(init_step_id))
            {
                BusyContent = _allStatus.InitStatus[init_step_id];
            }
        }

        /// <summary>
        /// 更新下位机操作样品时的动作ID号
        /// </summary>
        /// <param name="run_step_id">下位机操作样品的动作ID号</param>
        private void _clientRead_ERuningStepCallBack(int run_step_id)
        {
            RunId = run_step_id;
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 更新下位机的工作状态
        /// </summary>
        /// <param name="runStatus">工作状态集合</param>
        private void _clientRead_ERunStatusCallBack(List<bool> runStatus)// 0:
        {
            //throw new NotImplementedException();
            //设备急停
            if (runStatus[6] == true && EStop == false && _isRuning != RunStatus.estop)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    StartEnable = false;
                    PauseEnable = false;
                    StopEnable = false;
                    ReSetEnable = false;
                    CleanEnable = false;
                    SettingEnable = false;
                    //HelperEnable = true;
                    LightStatus = RunStatus.estop;
                    _isRuning = RunStatus.estop;
                    //SendCommand2Plc(RunStatus.stop);
                    Growl.ErrorGlobal("设备急停按钮已按下，进入急停状态！");
                    LogHelper.instance().Error("设备急停按钮被按下！");
                    musicPlayer.PlayLooping();
                    EStop = true;
                    IsBusy = false;
                });
            }
            //设备急停，并弹出急停按钮
            if (runStatus[6] == false && EStop == true && _isRuning == RunStatus.estop)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    StartEnable = false;
                    PauseEnable = false;
                    StopEnable = true;
                    ReSetEnable = true;
                    CleanEnable = false;
                    SettingEnable = false;
                    //HelperEnable = true;
                    LightStatus = RunStatus.ready;
                    _isRuning = RunStatus.ready;
                    IsBusy = false;
                    //SendCommand2Plc(RunStatus.stop);
                    Growl.ErrorGlobal("设备急停按钮已弹出，请重新初始化设备！");
                    LogHelper.instance().Error("设备急停按钮已弹出！");
                    _clientRead.ERuningStepCallBack -= _clientRead_ERuningStepCallBack;
                    //_clientRead.ERunStatusCallBack -= _clientRead_ERunStatusCallBack;
                    _clientRead.EDataReceiveCallBack -= _clientRead_EDataReceiveCallBack;
                    //_clientRead.EDeviceErrorCallBack -= _clientRead_EDeviceErrorCallBack;
                    //_clientRead.EInitStepCallBack -= _clientRead_EInitStepCallBack;
                    EStop = false;
                    musicPlayer.Stop();
                });
            }
            //设备暂停
            if (runStatus[5] == true && (_isRuning == RunStatus.start | _isRuning == RunStatus.clean))//从运行状态暂停
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _lastStatus.Push(_isRuning);
                    _isRuning = RunStatus.pause;
                    LightStatus = RunStatus.pause;
                    _lastStatus.Push(_isRuning);
                    StartEnable = true;
                    PauseEnable = false;
                    //StopEnable = true;
                    ReSetEnable = false;
                    CleanEnable = false;
                    SettingEnable = false;
                    //HelperEnable = true;
                    LogHelper.instance().Info("设备动作：设备进入暂停状态！");
                    Growl.InfoGlobal("设备已暂停！");
                });
            }
            //设备恢复运行
            if (runStatus[5] == false && _isRuning == RunStatus.pause)//从暂停状态取消
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _lastStatus.Pop();
                    _isRuning = _lastStatus.Pop();
                    LightStatus = _isRuning;
                    StartEnable = false;
                    PauseEnable = true;
                    //StopEnable = true;
                    ReSetEnable = false;
                    CleanEnable = false;
                    SettingEnable = false;
                    //HelperEnable = true;
                    LogHelper.instance().Info("设备动作：设备恢复执行操作状态！");
                    Growl.InfoGlobal("设备恢复继续运行！");
                });
            }
            //设备复位完成
            if (runStatus[2] == true && _isRuning == RunStatus.init)//复位动作执行完成
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    LightStatus = RunStatus.ready;
                    StartEnable = true;
                    PauseEnable = false;
                    //StopEnable = true;
                    ReSetEnable = false;
                    CleanEnable = true;
                    SettingEnable = true;
                    //HelperEnable = true;
                    IsBusy = false;
                    //_clientRead.EInitStepCallBack -= _clientRead_EInitStepCallBack;
                    _clientRead.ERuningStepCallBack -= _clientRead_ERuningStepCallBack;
                    _isRuning = RunStatus.ready;
                    LogHelper.instance().Info("设备动作：设备初始化操作执行完成！");
                    Growl.InfoGlobal("设备复位成功！");
                });
                return;
            }
            //设备动作执行完成
            if (runStatus[0] == true && runStatus[3] == true && _isRuning == RunStatus.start)//工作流程执行完成
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    LightStatus = RunStatus.ready;
                    _clientRead.ERuningStepCallBack -= _clientRead_ERuningStepCallBack;
                    _clientRead.EDataReceiveCallBack -= _clientRead_EDataReceiveCallBack;
                    StartEnable = true;
                    PauseEnable = false;
                    //StopEnable = true;
                    ReSetEnable = false;
                    CleanEnable = true;
                    SettingEnable = true;
                    //HelperEnable = true;
                    IsBusy = false;
                    _isRuning = RunStatus.ready;
                    LogHelper.instance().Info("设备动作：设备所有动作执行完成，准备接收下一次执行参数！");
                    Growl.InfoGlobal("设备所有操作已完成！");
                    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"Music\Done.wav"))
                    {
                        System.Media.SoundPlayer player = new System.Media.SoundPlayer();
                        player.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + @"Music\Done.wav";
                        player.LoadAsync();
                        player.PlaySync();
                    }
                    int count = 0;
                    foreach (var item in AllData)
                    {
                        if (item.Color.ToString() == "#FFF79421")
                            count++;
                    }
                    CountLog(countpath, $"本次完成的样品数量：{count}");
                    CountLog(countpath, "本次动作结束！");
                    string checkpath = AppDomain.CurrentDomain.BaseDirectory + @"Logs\ZD-count-" +
                    DateTime.Now.ToString("yyyy-MM-dd") + ".log";
                    if (countpath != checkpath)
                    {
                        try
                        {
                            string path = countpath;//文件的路径，保证文件存在。
                            if (!File.Exists(path))
                            {
                                File.Create(path).Close();
                            }
                            string w2f = System.DateTime.Now.ToString("yyyy-MM-dd HH：mm：ss ：");
                            w2f += $"今日完成的样品数量------{TodayCount}";
                            TodayCount = 0;
                            countpath = checkpath;
                            System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Append);
                            StreamWriter sw = new StreamWriter(fs);
                            sw.WriteLine(w2f);
                            sw.Close();
                            fs.Close();
                        }
                        catch (Exception ex)
                        {
                            LogHelper.instance().Error("count文件写入错误{0}", ex.Message);
                            //throw;
                        }
                    }
                    TodayCount += count;
                });
                return;
            }
            //设备排空操作执行完成
            if (runStatus[7] == true && _isRuning == RunStatus.clean)//设备排空操作完成
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    LightStatus = RunStatus.ready;
                    StartEnable = true;
                    PauseEnable = false;
                    //StopEnable = true;
                    ReSetEnable = true;
                    CleanEnable = true;
                    SettingEnable = true;
                    //HelperEnable = true;
                    IsBusy = false;
                    //_clientRead.ERunStatusCallBack -= _clientRead_ERunStatusCallBack;
                    _isRuning = RunStatus.ready;
                    LogHelper.instance().Info("设备动作：设备排空操作执行完成！");
                    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"Music\Done.wav"))
                    {
                        System.Media.SoundPlayer player = new System.Media.SoundPlayer();
                        player.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + @"Music\Done.wav";
                        player.LoadAsync();
                        player.PlaySync();
                    }
                    Growl.InfoGlobal("设备排空成功！");
                });
            }
            //洗针操作完成
            if (runStatus[0] == true && runStatus[4] == true)
            {
                LogHelper.instance().Info("设备动作：全部洗针操作执行完成！");
                Growl.InfoGlobal("全部洗针成功！");
                MainEnable = true;
            }

        }

        /// <summary>
        /// 添加方法
        /// </summary>
        /// <param name="t">被操作的控件</param>
        private void Addtabitem(TabControl t)
        {
            //if (tabItemCount < 5)
            //{
            //    t.Items.Add(new TabItem()
            //    {
            //        Header = $@"方法-{tabItemCount + 1}", Content = new DataSet()
            //    });
            //    t.SelectedIndex = tabItemCount;
            //    tabItemCount++;
            //}
            //if (_tabItemCount < 5)
            //{
            //    if (TabItems.Count > 0)
            //    {
            //        TabItems.Last().ShowCloseButton = false;
            //    }
            //    TabItems.Add(new TabItem() { Header = $@"S{_tabItemCount + 1}", Content = new DataSet() });
            //    //AllData[0].Name = TabItems.Last().Header.ToString();
            //    _tabItemCount++;
            //    Debug.Print(TabItems.Count.ToString());
            //}
            //t.SelectedIndex = t.Items.Count - 1;
            //MethodItems.Add(new Data() { Name = $@"S{i}", Value = 0, CleanValue = 0, Color = new SolidColorBrush(Colors.Gray), ShowClose = false });
            if (_tabItemCount < 6)
            {
                if (MethodItems.Count > 0)
                {
                    MethodItems.Last().ShowClose = false;
                }
                //MethodItems.Add(new Data() { Name = $@"S{_tabItemCount + 1}", Value = 0, CleanValue = 0, Color = new SolidColorBrush(Color.FromRgb((byte)_random.Next(0,255), (byte)_random.Next(0, 255), (byte)_random.Next(0, 255))), ShowClose = true });
                MethodItems.Add(new Data() { Name = $@"S{_tabItemCount + 1}", Value = 0.6, BigCleanValue = 1.1, LittleCleanValue = 500, AddIndex = 0, EnableTag = false, TagIndex = 0, TagValue = 0, Color = new SolidColorBrush(Color.FromRgb(48, 66, 88)), ShowClose = true });
                _tabItemCount++;
                t.SelectedIndex++;
            }
            else
            {
                Growl.WarningGlobal("已达到最大方法数量！");
            }
        }

        /// <summary>
        /// 部署方法
        /// </summary>
        /// <param name="parm">控件及方法参数</param>
        private void DeployMethod(object[] parm)
        {
            bool iscancel = false;
            if (_isRuning != RunStatus.ready) return;

            if (!_select)
            {
                if (parm[0] == null || !(parm[1] is Border) || !(parm[2] is Grid)) return;
                _starttag = int.Parse(((Border)parm[1]).Tag.ToString());
                _select = true;
                //Growl.InfoGlobal("已选择开始样品，再次点击选择结束样品");
                AllData[_starttag].Color = new SolidColorBrush(Colors.Red);
                zoneSelPoint.Add(_starttag, new int[] { Grid.GetRow(parm[1] as Border), Grid.GetColumn(parm[1] as Border) });
            }
            else
            {
                if (parm[0] == null || !(parm[1] is Border)) return;
                _endtag = int.Parse(((Border)parm[1]).Tag.ToString());
                if (_starttag == _endtag)
                {
                    if (AllData[_endtag].Name == ((Data)parm[0]).Name)
                    {
                        AllData[_endtag].Name = "N";
                        AllData[_endtag].Value = 0;
                        AllData[_endtag].BigCleanValue = 0;
                        AllData[_endtag].LittleCleanValue = 0;
                        AllData[_endtag].TagValue = 0;
                        AllData[_endtag].EnableTag = false;
                        AllData[_endtag].TagIndex = 0;
                        AllData[_endtag].AddIndex = 0;
                        AllData[_endtag].Color = ((Data)parm[0]).Color;
                        AllData[_endtag].Color = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                        zoneSelPoint.Clear();
                        iscancel = true;
                    }
                    else
                    {
                        AllData[_endtag].Name = ((Data)parm[0]).Name;
                        AllData[_endtag].Value = ((Data)parm[0]).Value;
                        AllData[_endtag].BigCleanValue = ((Data)parm[0]).BigCleanValue;
                        AllData[_endtag].LittleCleanValue = ((Data)parm[0]).LittleCleanValue;
                        AllData[_endtag].TagValue = ((Data)parm[0]).TagValue;
                        AllData[_endtag].EnableTag = ((Data)parm[0]).EnableTag;
                        AllData[_endtag].TagIndex = ((Data)parm[0]).TagIndex;
                        AllData[_endtag].AddIndex = ((Data)parm[0]).AddIndex;
                        AllData[_endtag].Color = ((Data)parm[0]).Color;
                        AllData[_endtag].Color = new SolidColorBrush(Color.FromRgb(3, 195, 175));
                        zoneSelPoint.Clear();
                    }
                }
                else
                {
                    if (!zoneSelPoint.ContainsKey(_endtag))
                    {
                        zoneSelPoint.Add(_endtag, new int[] { Grid.GetRow(parm[1] as Border), Grid.GetColumn(parm[1] as Border) });
                        if (zoneSelPoint.Count >= 2)
                        {
                            int[] max = zoneSelPoint.ElementAt(0).Value;
                            int[] min = zoneSelPoint.ElementAt(1).Value;
                            zoneSelPoint.Clear();
                            int temp;
                            if (max[0] < min[0])
                            {
                                temp = max[0];
                                max[0] = min[0];
                                min[0] = temp;
                            }
                            if (max[1] < min[1])
                            {
                                temp = max[1];
                                max[1] = min[1];
                                min[1] = temp;
                            }

                            for (int i = min[0]; i <= max[0]; i++)
                            {
                                for (int j = min[1]; j <= max[1]; j++)
                                {
                                    if (AllData[tubeTags[i - 1, j - 1]].Name == ((Data)parm[0]).Name)
                                    {
                                        AllData[tubeTags[i - 1, j - 1]].Name = "N";
                                        AllData[tubeTags[i - 1, j - 1]].Value = 0;
                                        AllData[tubeTags[i - 1, j - 1]].BigCleanValue = 0;
                                        AllData[tubeTags[i - 1, j - 1]].LittleCleanValue = 0;
                                        AllData[tubeTags[i - 1, j - 1]].TagValue = 0;
                                        AllData[tubeTags[i - 1, j - 1]].EnableTag = false;
                                        AllData[tubeTags[i - 1, j - 1]].TagIndex = 0;
                                        AllData[tubeTags[i - 1, j - 1]].AddIndex = 0;
                                        AllData[tubeTags[i - 1, j - 1]].Color = ((Data)parm[0]).Color;
                                        AllData[tubeTags[i - 1, j - 1]].Color = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                                        iscancel = true;
                                    }
                                    else
                                    {
                                        AllData[tubeTags[i - 1, j - 1]].Name = ((Data)parm[0]).Name;
                                        AllData[tubeTags[i - 1, j - 1]].Value = ((Data)parm[0]).Value;
                                        AllData[tubeTags[i - 1, j - 1]].BigCleanValue = ((Data)parm[0]).BigCleanValue;
                                        AllData[tubeTags[i - 1, j - 1]].LittleCleanValue = ((Data)parm[0]).LittleCleanValue;
                                        AllData[tubeTags[i - 1, j - 1]].TagValue = ((Data)parm[0]).TagValue;
                                        AllData[tubeTags[i - 1, j - 1]].EnableTag = ((Data)parm[0]).EnableTag;
                                        AllData[tubeTags[i - 1, j - 1]].TagIndex = ((Data)parm[0]).TagIndex;
                                        AllData[tubeTags[i - 1, j - 1]].AddIndex = ((Data)parm[0]).AddIndex;
                                        AllData[tubeTags[i - 1, j - 1]].Color = ((Data)parm[0]).Color;
                                        AllData[tubeTags[i - 1, j - 1]].Color = new SolidColorBrush(Color.FromRgb(3, 195, 175));
                                    }
                                }
                            }
                        }
                    }
                }
                #region 旧打码
                /* 最初选择方式
if (_starttag > _endtag)
{
//for (int i = _starttag; i > _endtag - 1; i -= 2)
//{
//    if (AllData[i].Name == ((Data)parm[0]).Name)
//    {
//        AllData[i].Name = "N";
//        AllData[i].Value = 0;
//        AllData[i].CleanValue = 0;
//        AllData[i].Color = ((Data)parm[0]).Color;
//        AllData[i].Color = new SolidColorBrush(Color.FromRgb(128, 128, 128));
//        _select = false;
//        iscancel = true;
//    }
//}
//Growl.WarningGlobal("样品方法取消成功！");
Growl.WarningGlobal("开始样品位置大于结束样品位置，操作异常！");
if (AllData[_starttag].Name == "N")
AllData[_starttag].Color = new SolidColorBrush(Color.FromRgb(128, 128, 128));

else
AllData[_starttag].Color = new SolidColorBrush(Color.FromRgb(3, 195, 175));
return;
}



for (int i = _starttag; i < _endtag + 1; i += 1)
{
if (AllData[i].Name == ((Data)parm[0]).Name)
{
AllData[i].Name = "N";
AllData[i].Value = 0;
AllData[i].CleanValue = 0;
AllData[i].Color = ((Data)parm[0]).Color;
AllData[i].Color = new SolidColorBrush(Color.FromRgb(128, 128, 128));

_select = false;
iscancel = true;
}
else
{
AllData[i].Name = ((Data)parm[0]).Name;
AllData[i].Value = ((Data)parm[0]).Value;
AllData[i].CleanValue = ((Data)parm[0]).CleanValue;
AllData[i].Color = ((Data)parm[0]).Color;
AllData[i].Color = new SolidColorBrush(Color.FromRgb(3, 195, 175));
}
}
*/

                #endregion

                if (iscancel)
                    Growl.InfoGlobal("样品方法取消成功！");
                else
                    Growl.InfoGlobal("样品方法应用成功！");

                _starttag = 0;
                _endtag = 0;
                _select = false;
            }
        }

        /// <summary>
        /// 关闭窗口前执行数据统计操作
        /// </summary>
        /// <param name="e"></param>
        private void WindowClosing(CancelEventArgs e)
        {
            if (_isRuning != RunStatus.ready && _isRuning != RunStatus.estop)
            {
                Growl.ErrorGlobal("设备处于工作状态不能关闭软件！");
                e.Cancel = true;
                return;
            }
            CountLog(countpath, $"关闭软件统计,今日完成的样品数量------{TodayCount}");
            MessengerInstance.Send<bool, LogViewViewModel>(true);
            if (_starttag != _endtag)
            {
                if (_starttag != 0 && AllData[_starttag].Name == "N")
                    AllData[_starttag].Color = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                else
                {
                    AllData[_starttag].Color = new SolidColorBrush(Color.FromRgb(3, 195, 175));
                }
            }
            foreach (var item in AllData)
            {
                if (item.Name != "N")
                    item.Color = new SolidColorBrush(Color.FromRgb(3, 195, 175));
                else
                    item.Color = new SolidColorBrush(Color.FromRgb(128, 128, 128));
            }
            XmlReadWriter.SaveToFile(AllData, AppDomain.CurrentDomain.BaseDirectory + @"\Settings\AllData.xml");
            XmlReadWriter.SaveToFile(MethodItems, AppDomain.CurrentDomain.BaseDirectory + @"\Settings\MethodData.xml");
            _clientRead.stop = true;
        }

        /// <summary>
        /// 方法被删除时，同时重置在样品区被应用的方法
        /// </summary>
        /// <param name="data"></param>
        private void ItemClose(Data data)
        {
            MethodItems.Remove(data);
            if (MethodItems.Count != 0)
                MethodItems.Last().ShowClose = true;
            foreach (var item in AllData)
            {
                if (item.Name == data.Name)
                {
                    item.Name = "N";
                    item.Value = 0;
                    item.BigCleanValue = 0;
                    item.LittleCleanValue = 0;
                    item.TagValue = 0;
                    item.EnableTag = false;
                    item.TagIndex = 0;
                    item.AddIndex = 0;
                    item.Color = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                }
            }

            _tabItemCount--;
        }

        /// <summary>
        /// 没用的函数
        /// </summary>
        /// <param name="parent"></param>
        private void OpenEngineerView(Window parent)
        {
            //Window w = new EngineerView();
            //w.Owner = parent;
            //w.ShowDialog();
        }

        /// <summary>
        /// 没用的函数
        /// </summary>
        /// <param name="w"></param>
        private void CloseWindow(Window w)
        {
            if (w != null)
            {
                w.Close();
            }
        }

        /// <summary>
        /// 没用的函数
        /// </summary>
        /// <param name="w"></param>
        private void DragWindow(Window w)
        {
            if (w != null)
                w.DragMove();
        }

        /// <summary>
        /// 重写UI刷新响应
        /// </summary>
        /// <param name="propertyName"></param>
        public override void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.RaisePropertyChanged(propertyName);
            switch (propertyName)
            {
                case "ErrorID":
                    if (ErrorID == 0)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            if (_isRuning != RunStatus.clean_error)
                            {
                                musicPlayer.Stop();
                                return;
                            }
                            _lastStatus.Pop();
                            _isRuning = _lastStatus.Pop();
                            LightStatus = _isRuning;
                            StartEnable = false;
                            PauseEnable = true;
                            ReSetEnable = false;
                            CleanEnable = false;
                            //StopEnable = true;
                            LogHelper.instance().Info("设备动作：设备执行消错，恢复执行操作状态！");
                            Growl.InfoGlobal("设备错误已清除！");
                            musicPlayer.Stop();
                        });
                        return;
                    }
                    else if (ErrorID >= 15)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            StartEnable = false;
                            PauseEnable = false;
                            StopEnable = false;
                            ReSetEnable = false;
                            CleanEnable = false;
                            SettingEnable = false;
                            //HelperEnable = true;
                            LightStatus = RunStatus.estop;
                            _isRuning = RunStatus.ready;
                            _clientRead.ERuningStepCallBack -= _clientRead_ERuningStepCallBack;
                            //_clientRead.ERunStatusCallBack -= _clientRead_ERunStatusCallBack;
                            _clientRead.EDataReceiveCallBack -= _clientRead_EDataReceiveCallBack;
                            //_clientRead.EDeviceErrorCallBack -= _clientRead_EDeviceErrorCallBack;
                            //_clientRead.EInitStepCallBack -= _clientRead_EInitStepCallBack;
                            SendCommand2Plc(RunStatus.ready);
                            if (_allStatus.ErrorStatus.ContainsKey(ErrorID))
                            {
                                Growl.ErrorGlobal(_allStatus.ErrorStatus[ErrorID]);
                                LogHelper.instance().Error("设备错误：{0}", _allStatus.ErrorStatus[ErrorID]);
                            }
                            Growl.ErrorGlobal("设备出现不可恢复的错误，进入急停状态！");
                            musicPlayer.PlayLooping();
                            //BorderAttach.SetFlashing(Status_Light, false);
                            //Status_Light.Background = new SolidColorBrush(Colors.Red);
                            //BorderAttach.SetFlashing(Status_Light, true);
                        });
                        return;
                    }
                    if (_allStatus.ErrorStatus.ContainsKey(ErrorID))
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                      {
                          if (_lastStatus.Count > 0)
                          {
                              if (_lastStatus.Peek() != RunStatus.clean_error)
                              {
                                  _lastStatus.Push(_isRuning);
                                  _isRuning = RunStatus.clean_error;
                                  LightStatus = RunStatus.clean_error;
                                  _lastStatus.Push(_isRuning);
                              }
                          }
                          else
                          {
                              _lastStatus.Push(_isRuning);
                              _isRuning = RunStatus.clean_error;
                              LightStatus = RunStatus.clean_error;
                              _lastStatus.Push(_isRuning);
                          }
                          //BorderAttach.SetFlashing(Status_Light, false);
                          //Status_Light.Background = new SolidColorBrush(Colors.Red);
                          //BorderAttach.SetFlashing(Status_Light, true);
                          StartEnable = true;
                          PauseEnable = false;
                          ReSetEnable = false;
                          //StopEnable = true;
                          CleanEnable = false;
                          LogHelper.instance().Error("设备错误：{0}", _allStatus.ErrorStatus[ErrorID]);
                          Growl.ErrorGlobal("设备错误：" + _allStatus.ErrorStatus[ErrorID]);
                          musicPlayer.PlayLooping();
                      });
                    }
                    break;
                case "RunId":
                    if (_allStatus.SeparationStatus.ContainsKey(RunId))
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            //Growl.InfoGlobal(_allStatus.SeparationStatus[RunId]);
                            LogHelper.instance().Info("设备动作：{0}", _allStatus.SeparationStatus[RunId]);
                        });
                    }
                    break;
                case "BusyContent":
                    if ((_isRuning == RunStatus.ready || _isRuning == RunStatus.estop) && _busyContent != "准备就绪，等待启动。。。。")
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            //Growl.InfoGlobal(_allStatus.SeparationStatus[RunId]);
                            //_config.AppSettings.Settings["Z1_Sensitivity"].Value = ((LiquidTypes)obj[2])?.Z1Value.ToString();
                            //_config.AppSettings.Settings["Z2_Sensitivity"].Value = ((LiquidTypes)obj[2])?.Z2Value.ToString();
                            //SendData();
                            //_clientRead.ERunStatusCallBack += _clientRead_ERunStatusCallBack;
                            //_clientRead.EDeviceErrorCallBack += _clientRead_EDeviceErrorCallBack;
                            //_isRuning = RunStatus.init;
                            StartEnable = false;
                            PauseEnable = false;
                            StopEnable = true;
                            ReSetEnable = false;
                            CleanEnable = false;
                            SettingEnable = false;
                            IsBusy = true;
                            _isRuning = RunStatus.init;
                            LightStatus = RunStatus.init;
                            //HelperEnable = true;
                            if (_clientRead.ThreadReceive.IsAlive != true)
                                _clientRead.ThreadReceive.Start();
                            LogHelper.instance().Info("上位机动作：设备进入初始化状态！");
                        });

                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 写入统计数据
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="logstr"></param>
        /// <returns></returns>
        public bool CountLog(string filepath, string logstr)
        {
            try
            {
                //string path = AppDomain.CurrentDomain.BaseDirectory + @"Logs\ZD-count-" +
                //              DateTime.Now.ToString("yyyy-MM-dd") + ".log";//文件的路径，保证文件存在。
                //if (!File.Exists(path))
                //{
                //    File.Create(path).Close();
                //}
                string w2f = System.DateTime.Now.ToString("yyyy-MM-dd HH：mm：ss ：");
                w2f += logstr;
                System.IO.FileStream fs = new System.IO.FileStream(countpath, System.IO.FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(w2f);
                sw.Close();
                fs.Close();
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.instance().Error("count文件写入错误{0}", ex.Message);
                return false;
                //throw;
            }
        }

    }
}