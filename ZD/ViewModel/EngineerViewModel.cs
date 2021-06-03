using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using EasyNetVars;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using SgS.Helper;
using SgS.Model;
using SgS.View;
using Window = System.Windows.Window;

namespace SgS.ViewModel
{

    public class EngineerViewModel : ViewModelBase
    {
        private string _controlerIP;

        private Button _okButton;

        #region Attribute

        private int _z1_Sensitivity;

        public int Z1_Sensitivity
        {
            get { return _z1_Sensitivity; }
            set { _z1_Sensitivity = value; }
        }

        private int _z2_Sensitivity;

        public int Z2_Sensitivity
        {
            get { return _z2_Sensitivity; }
            set { _z2_Sensitivity = value; }
        }

        private Visibility _test;

        public Visibility Test
        {
            get { return _test; }
            set
            {
                _test = value;
                RaisePropertyChanged(() => Test);
            }
        }

        private int _pos_x;

        public int Pos_X
        {
            get { return _pos_x; }
            set
            {
                _pos_x = value;
                RaisePropertyChanged(() => Pos_X);
            }
        }

        private int _pos_y;

        public int Pos_Y
        {
            get { return _pos_y; }
            set
            {
                _pos_y = value;
                RaisePropertyChanged(() => Pos_Y);
            }
        }

        private int _pos_z1;

        public int Pos_Z1
        {
            get { return _pos_z1; }
            set
            {
                if (_pos_z1 != value)
                    _pos_z1 = value;
                else
                    return;
                _pos_z1 = value;
                if (_pos_z1 > 0 || _pos_z1 < -146000)
                {
                    SendCommand2Plc(2, false);
                    SendCommand2Plc(6, false);
                }
                RaisePropertyChanged(() => Pos_Z1);
            }
        }

        private int _pos_z2;

        public int Pos_Z2
        {
            get { return _pos_z2; }
            set
            {
                if (_pos_z2 != value)
                    _pos_z2 = value;
                else
                    return;
                if (_pos_z2 > 0 || _pos_z2 < -146000)
                {
                    SendCommand2Plc(3, false);
                    SendCommand2Plc(7, false);
                }
                RaisePropertyChanged(() => Pos_Z2);
            }
        }

        private int _sensor_z1;

        public int Sensor_Z1
        {
            get { return _sensor_z1; }
            set
            {
                _sensor_z1 = value;
                RaisePropertyChanged(() => Sensor_Z1);
            }
        }

        private int _sensor_z2;

        public int Sensor_Z2
        {
            get { return _sensor_z2; }
            set
            {
                _sensor_z2 = value;
                RaisePropertyChanged(() => Sensor_Z2);
            }
        }

        private int _z1_down;

        public int Z1_Down
        {
            get { return _z1_down; }
            set
            {
                _z1_down = value;
                RaisePropertyChanged(() => Z1_Down);
            }
        }

        private int _z2_down;

        public int Z2_Down
        {
            get { return _z2_down; }
            set
            {
                _z2_down = value;
                RaisePropertyChanged(() => Z2_Down);
            }
        }


        #endregion

        #region Command 
        public RelayCommand<object[]> OkCommand { get; set; }

        public RelayCommand<EngineerView> LoadedCommand { get; set; }

        public RelayCommand<Grid> CloseCommand { get; set; }

        public RelayCommand<Window> DragCommand { get; set; }

        public RelayCommand<MouseButtonEventArgs> MontorCommandDown { get; set; }

        public RelayCommand<MouseButtonEventArgs> MontorCommandUp { get; set; }

        public ObservableCollection<PositionData> PositionDatas { get; set; }

        public ObservableCollection<CompensateData> CompensateData1 { get; set; }

        public ObservableCollection<CompensateData> CompensateData2 { get; set; }

        public ObservableCollection<LiquidTypes> LiquidTypes { get; set; }

        public RelayCommand<Grid> TesetCommand { get; set; } = new RelayCommand<Grid>((e) =>
        {
            if (e.Width == 400)
            {
                DoubleAnimation doubleAnimation = new DoubleAnimation();
                doubleAnimation.From = 400;
                doubleAnimation.To = 100;
                doubleAnimation.Duration = TimeSpan.FromSeconds(0.5);
                Storyboard.SetTarget(doubleAnimation, e);
                Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("Width"));
                Storyboard storyboard = new Storyboard();
                storyboard.Children.Add(doubleAnimation);
                storyboard.Begin();
            }
            else
            {
                DoubleAnimation doubleAnimation = new DoubleAnimation();
                doubleAnimation.From = 100;
                doubleAnimation.To = 400;
                doubleAnimation.Duration = TimeSpan.FromSeconds(0.5);
                Storyboard.SetTarget(doubleAnimation, e);
                Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("Width"));
                Storyboard storyboard = new Storyboard();
                storyboard.Children.Add(doubleAnimation);
                storyboard.Begin();
            }
        });

        //public RelayCommand<Grid> CancelCommand { get; set; } = new RelayCommand<Grid>((e) =>
        //{
        //    AnimationHelper.CreateWidthChangedAnimation(e, 0, 1046, TimeSpan.FromSeconds(0.5));
        //});

        public RelayCommand<Grid> CancelCommand { get; set; }

        public RelayCommand CleanCommand { get; set; }

        public RelayCommand AddLiquidItemCommand { get; set; }

        public RelayCommand<object[]> ShowKeyPadCommand { get; private set; }


        #endregion

        public EngineerViewModel()
        {
            OkCommand = new RelayCommand<object[]>(CheckPasswd);
            LoadedCommand = new RelayCommand<EngineerView>(Loaded);
            CloseCommand = new RelayCommand<Grid>(CloseWindow);
            DragCommand = new RelayCommand<Window>(DragWindow);
            MontorCommandDown = new RelayCommand<MouseButtonEventArgs>(RunMotorDown);
            MontorCommandUp = new RelayCommand<MouseButtonEventArgs>(RunMotorUp);
            AddLiquidItemCommand = new RelayCommand(AddLiquidCommand);
            CancelCommand = new RelayCommand<Grid>(CancelAction);
            ShowKeyPadCommand = new RelayCommand<object[]>(ShowKeyPad);
            CleanCommand = new RelayCommand(() => { SendCommand2Plc(8, true); });

            PositionDatas = new ObservableCollection<PositionData>();
            Test = Visibility.Visible;
            CompensateData1 = new ObservableCollection<CompensateData>();
            CompensateData2 = new ObservableCollection<CompensateData>();
            LiquidTypes = new ObservableCollection<LiquidTypes>();
            PositionDatas = XmlReadWriter.LoadFromFile<PositionData>(AppDomain.CurrentDomain.BaseDirectory + @"\Settings\PositionData.xml");
            CompensateData1 = XmlReadWriter.LoadFromFile<CompensateData>(AppDomain.CurrentDomain.BaseDirectory + @"\Settings\Pump_A.xml");
            CompensateData2 = XmlReadWriter.LoadFromFile<CompensateData>(AppDomain.CurrentDomain.BaseDirectory + @"\Settings\Pump_B.xml");
            LiquidTypes = XmlReadWriter.LoadFromFile<LiquidTypes>(AppDomain.CurrentDomain.BaseDirectory + @"\Settings\LiquidTypes.xml");

            _controlerIP = MainViewModel._config.AppSettings.Settings["IPaddress"].Value == null ? "127.0.0.1" : MainViewModel._config.AppSettings
                .Settings["IPaddress"].Value;

            if (PositionDatas.Count < 1)
            {
                PositionDatas.Clear();
                //for (int i = 1; i < 5; i++)
                //{
                PositionDatas.Add(new PositionData() { Name = $"A区粗针开始位置", Note = "测试", Px = 0, Py = 0, Px2 = 0, Py2 = 0 });
                PositionDatas.Add(new PositionData() { Name = $"A区粗针结束位置", Note = "测试", Px = 0, Py = 0, Px2 = 0, Py2 = 0 });
                PositionDatas.Add(new PositionData() { Name = $"A区细针开始位置", Note = "测试", Px = 0, Py = 0, Px2 = 0, Py2 = 0 });
                PositionDatas.Add(new PositionData() { Name = $"A区细针结束位置", Note = "测试", Px = 0, Py = 0, Px2 = 0, Py2 = 0 });

                PositionDatas.Add(new PositionData() { Name = $"B区粗针开始位置", Note = "测试", Px = 0, Py = 0, Px2 = 0, Py2 = 0 });
                PositionDatas.Add(new PositionData() { Name = $"B区粗针结束位置", Note = "测试", Px = 0, Py = 0, Px2 = 0, Py2 = 0 });
                PositionDatas.Add(new PositionData() { Name = $"B区细针开始位置", Note = "测试", Px = 0, Py = 0, Px2 = 0, Py2 = 0 });
                PositionDatas.Add(new PositionData() { Name = $"B区细针结束位置", Note = "测试", Px = 0, Py = 0, Px2 = 0, Py2 = 0 });

                //}
                XmlReadWriter.SaveToFile(PositionDatas, AppDomain.CurrentDomain.BaseDirectory + @"\Settings\PositionData.xml");
            }

            if (CompensateData1.Count == 0 || CompensateData2.Count == 0)
            {
                for (int i = 0; i < 1; i++)
                {
                    CompensateData1.Add(new CompensateData() { Min = 0, Max = 0.1, A = 0, B = 0, C = 0, SelectMethod = 0 });
                    CompensateData1.Add(new CompensateData() { Min = 0.1, Max = 0.2, A = 0, B = 0, C = 0, SelectMethod = 0 });
                    CompensateData1.Add(new CompensateData() { Min = 0.2, Max = 0.3, A = 0, B = 0, C = 0, SelectMethod = 0 });
                    CompensateData1.Add(new CompensateData() { Min = 0.3, Max = 0.4, A = 0, B = 0, C = 0, SelectMethod = 0 });
                    CompensateData1.Add(new CompensateData() { Min = 0.4, Max = 1, A = 0, B = 0, C = 0, SelectMethod = 0 });
                    CompensateData2.Add(new CompensateData() { Min = 0, Max = 0.1, A = 0, B = 0, C = 0, SelectMethod = 0 });
                    CompensateData2.Add(new CompensateData() { Min = 0.1, Max = 0.2, A = 0, B = 0, C = 0, SelectMethod = 0 });
                    CompensateData2.Add(new CompensateData() { Min = 0.2, Max = 0.3, A = 0, B = 0, C = 0, SelectMethod = 0 });
                    CompensateData2.Add(new CompensateData() { Min = 0.3, Max = 0.4, A = 0, B = 0, C = 0, SelectMethod = 0 });
                    CompensateData2.Add(new CompensateData() { Min = 0.4, Max = 1, A = 0, B = 0, C = 0, SelectMethod = 0 });
                }
                XmlReadWriter.SaveToFile(CompensateData1, AppDomain.CurrentDomain.BaseDirectory + @"\Settings\Pump_A.xml");
                XmlReadWriter.SaveToFile(CompensateData2, AppDomain.CurrentDomain.BaseDirectory + @"\Settings\Pump_B.xml");
            }

            if (LiquidTypes.Count == 0)
            {
                LiquidTypes.Add(new LiquidTypes() { LiquidName = "乙酸乙酯", Z1Value = 31396300, Z2Value = 30763500 });
                LiquidTypes.Add(new LiquidTypes() { LiquidName = "二氯甲烷", Z1Value = 0, Z2Value = 0 });
                LiquidTypes.Add(new LiquidTypes() { LiquidName = "乙腈", Z1Value = 0, Z2Value = 0 });
                LiquidTypes.Add(new LiquidTypes() { LiquidName = "叔丁基甲醚", Z1Value = 0, Z2Value = 0 });
            }
            Z1_Sensitivity = MainViewModel._config.AppSettings.Settings["Z1_Sensitivity"].Value == null ? -5000 : int.Parse(MainViewModel._config.AppSettings.Settings["Z1_Sensitivity"].Value);
            Z2_Sensitivity = MainViewModel._config.AppSettings.Settings["Z2_Sensitivity"].Value == null ? -5000 : int.Parse(MainViewModel._config.AppSettings.Settings["Z2_Sensitivity"].Value);
            Z1_Down = MainViewModel._config.AppSettings.Settings["Z1_Down"].Value == null ? -95000 : int.Parse(MainViewModel._config.AppSettings.Settings["Z1_Down"].Value);
            Z2_Down = MainViewModel._config.AppSettings.Settings["Z2_Down"].Value == null ? -95000 : int.Parse(MainViewModel._config.AppSettings.Settings["Z2_Down"].Value);
        }

        /// <summary>
        /// 显示软键盘
        /// </summary>
        /// <param name="obj">参数与控件</param>
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
        /// 取消操作
        /// </summary>
        /// <param name="obj">列表控件</param>
        private void CancelAction(Grid obj)
        {
            //throw new NotImplementedException();
            obj.CreateWidthChangedAnimation(0, 1046, TimeSpan.FromSeconds(0.5));
            _okButton.IsDefault = true;
            _okButton.Focus();
        }

        /// <summary>
        /// 没用的函数
        /// </summary>
        private void CleanNeedle()
        {
            SendCommand2Plc(8, true);

        }

        /// <summary>
        /// 实现长按操作电机
        /// </summary>
        /// <param name="obj">参数</param>
        private void RunMotorUp(MouseButtonEventArgs obj)
        {
            //throw new NotImplementedException();
            if (obj.Source is Button)
            {
                Button temp = obj.Source as Button;
                switch (temp.Content)
                {
                    case "X 正转点动":
                        SendCommand2Plc(0, false);
                        break;
                    case "Y 正转点动":
                        SendCommand2Plc(1, false);
                        break;
                    case "Z1 正转点动":
                        SendCommand2Plc(2, false);
                        break;
                    case "Z2 正转点动":
                        SendCommand2Plc(3, false);
                        break;
                    case "X 反转点动":
                        SendCommand2Plc(4, false);
                        break;
                    case "Y 反转点动":
                        SendCommand2Plc(5, false);
                        break;
                    case "Z1 反转点动":
                        SendCommand2Plc(6, false);
                        break;
                    case "Z2 反转点动":
                        SendCommand2Plc(7, false);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 实现长按操作电机
        /// </summary>
        /// <param name="obj">参数</param>
        private void RunMotorDown(MouseButtonEventArgs obj)
        {
            if (obj.Source is Button)
            {
                Button temp = obj.Source as Button;
                switch (temp.Content)
                {
                    case "X 正转点动":
                        SendCommand2Plc(0, true);
                        break;
                    case "Y 正转点动":
                        SendCommand2Plc(1, true);
                        break;
                    case "Z1 正转点动":
                        SendCommand2Plc(2, true);
                        break;
                    case "Z2 正转点动":
                        SendCommand2Plc(3, true);
                        break;
                    case "X 反转点动":
                        SendCommand2Plc(4, true);
                        break;
                    case "Y 反转点动":
                        SendCommand2Plc(5, true);
                        break;
                    case "Z1 反转点动":
                        SendCommand2Plc(6, true);
                        break;
                    case "Z2 反转点动":
                        SendCommand2Plc(7, true);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 向下位机发送指令
        /// </summary>
        /// <param name="commandPos"></param>
        /// <param name="value"></param>
        private void SendCommand2Plc(int commandPos, bool value)
        {
            NetVar sender = new NetVar(_controlerIP, 99);
            List<CDataTypeCollection> sendercommand;
            sendercommand = new List<CDataTypeCollection>();
            for (int i = 0; i < 9; i++)
            {
                if (commandPos == i)
                    sendercommand.Add(new CDataTypeCollection(value, DataTypes.booltype));
                else
                    sendercommand.Add(new CDataTypeCollection(false, DataTypes.booltype));
            }
            sender.WriteData(sendercommand);
            sender.Disconnect();
        }

        /// <summary>
        /// 检查密码
        /// </summary>
        /// <param name="data">控件与参数</param>
        private void CheckPasswd(object[] data)
        {
            if (((HandyControl.Controls.PasswordBox)data[0]).Password == "8888")
            {
                AnimationHelper.CreateWidthChangedAnimation((Grid)data[1], ((Grid)data[1]).ActualWidth, 0, TimeSpan.FromSeconds(0.5));
                Growl.InfoGlobal("欢迎您，工程师!");
                ((HandyControl.Controls.PasswordBox)data[0]).Password = "";
                ((Button)data[2]).IsDefault = false;
                _okButton = ((Button)data[2]);
                if (MainViewModel._clientRead.ThreadReceive.IsAlive != true)
                    MainViewModel._clientRead.ThreadReceive.Start();
                MainViewModel._clientRead.EPosStatusCallBack += _clientRead_EPosStatusCallBack;
            }
            else
            {
                Growl.WarningGlobal("验证失败，密码错误！");
            }
        }

        /// <summary>
        /// 更新下位机，针头位置
        /// </summary>
        /// <param name="posData">下位机返回的针头参数集合</param>
        private void _clientRead_EPosStatusCallBack(Dictionary<string, int> posData)
        {
            Pos_X = posData["x"];
            Pos_Y = posData["y"];
            Pos_Z1 = posData["z1"];
            Pos_Z2 = posData["z2"];
            Sensor_Z1 = posData["z1sensor"];
            Sensor_Z2 = posData["z2sensor"];
        }

        /// <summary>
        /// 没用的函数
        /// </summary>
        /// <param name="w"></param>
        private void Loaded(EngineerView w)
        {

        }

        /// <summary>
        /// 参数保存
        /// </summary>
        /// <param name="w">列表控件</param>
        private void CloseWindow(Grid w)
        {
            if (w.ActualWidth == 0)
            {
                //w.CreateWidthChangedAnimation();
                AnimationHelper.CreateWidthChangedAnimation(w, 0, ((FrameworkElement)w.Parent).ActualWidth, TimeSpan.FromSeconds(0.5));
                XmlReadWriter.SaveToFile(PositionDatas, AppDomain.CurrentDomain.BaseDirectory + @"\Settings\PositionData.xml");
                XmlReadWriter.SaveToFile(CompensateData1, AppDomain.CurrentDomain.BaseDirectory + @"\Settings\Pump_A.xml");
                XmlReadWriter.SaveToFile(CompensateData2, AppDomain.CurrentDomain.BaseDirectory + @"\Settings\Pump_B.xml");
                XmlReadWriter.SaveToFile(LiquidTypes, AppDomain.CurrentDomain.BaseDirectory + @"\Settings\LiquidTypes.xml");
                MainViewModel._config.AppSettings.Settings["Z1_Sensitivity"].Value = Z1_Sensitivity.ToString();
                MainViewModel._config.AppSettings.Settings["Z2_Sensitivity"].Value = Z2_Sensitivity.ToString();
                MainViewModel._config.AppSettings.Settings["Z1_Down"].Value = Z1_Down.ToString();
                MainViewModel._config.AppSettings.Settings["Z2_Down"].Value = Z2_Down.ToString();
                MainViewModel._config.Save();
                MainViewModel._clientRead.EPosStatusCallBack -= _clientRead_EPosStatusCallBack;
                MessengerInstance.Send<ObservableCollection<LiquidTypes>, MainViewModel>(LiquidTypes);
                _okButton.IsDefault = true;
            }
            //w?.Close();
        }

        /// <summary>
        /// 实现窗口拖动
        /// </summary>
        /// <param name="w"></param>
        private void DragWindow(Window w)
        {
            w?.DragMove();
        }

        /// <summary>
        /// 添加新溶剂
        /// </summary>
        private void AddLiquidCommand()
        {
            AddLiquidItem w = new AddLiquidItem();
            w.ShowDialog();
            if (w.Z1_Value.Value == 0 && w.Z2_Value.Value == 0)
            {
                return;
            }
            LiquidTypes.Add(new LiquidTypes() { LiquidName = w.LiquidType.Text, Z1Value = w.Z1_Value.Value, Z2Value = w.Z2_Value.Value });

        }
    }

    static class AnimationExtension
    {
        public static void CreateWidthChangedAnimation(this FrameworkElement element, double form, double to, TimeSpan span)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = form;
            doubleAnimation.To = to;
            doubleAnimation.Duration = span;
            Storyboard.SetTarget(doubleAnimation, element);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("Width"));
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(doubleAnimation);
            storyboard.Begin();
        }
    }

}
