using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using EasyNetVars;
using HandyControl.Controls;
using SgS.Helper;

namespace SgS.Model
{
    public class NetVar
    {
        private readonly NetVars _client;
        private readonly List<int> _reTubeStatus;
        private  int _step_id;
        private  int _error_Id;
        private readonly List<bool> _status;
        private readonly Dictionary<string, int> _posData;
        private readonly int _sleepTime;
        public Thread ThreadReceive;
        public bool stop;

        /// <summary>
        /// 试管状态事件
        /// </summary>
        /// <param name="reStatus">返回的42个试管状态</param>
        public delegate void DataReceiveCallBack(List<int> reStatus);
        public event DataReceiveCallBack EDataReceiveCallBack;

        //public delegate void ErrorReceiveCallBack(int errorCode);
        //public event ErrorReceiveCallBack EErrorReceiveCallBack;

        /// <summary>
        /// 设备状态事件
        /// </summary>
        /// <param name="runStatus">设备状态集</param>
        public delegate void RunStatusCallBack(List<bool> runStatus);
        public event RunStatusCallBack ERunStatusCallBack;

        /// <summary>
        /// 针头X,Y,Z轴位置以及液面探测传感器数据事件
        /// </summary>
        /// <param name="posData">针头X,Y,Z轴位置以及液面探测传感器数据集合</param>
        public delegate void PosStatusCallBack(Dictionary<string, int> posData);
        public event PosStatusCallBack EPosStatusCallBack;

        /// <summary>
        /// 初始化动作执行步骤ID
        /// </summary>
        /// <param name="init_step_id">动作ID号</param>
        public delegate void InitStepCallBack(int init_step_id);
        public event InitStepCallBack EInitStepCallBack;

        /// <summary>
        /// 设备动作执行步骤ID
        /// </summary>
        /// <param name="run_step_id">设备运行动作ID号</param>
        public delegate void RuningStepCallBack(int run_step_id);
        public event RuningStepCallBack ERuningStepCallBack;

        /// <summary>
        /// 设备错误ID号
        /// </summary>
        /// <param name="device_error_id">设备错误ID号</param>
        public delegate void DeviceErrorCallBack(int device_error_id);
        public event DeviceErrorCallBack EDeviceErrorCallBack;
        public NetVar(string ipaddress, int port, int id, int readtotal = 42, int sleeptime = 100)
        {
            _reTubeStatus = new List<int>();
            _status = new List<bool>();
            //_status = new Dictionary<string, bool>() {
            //    { "busy_status",false},
            //    { "init_status",false},
            //    { "work_status",false},
            //    { "clean_status",false},
            //    { "pause_status",false},
            //    { "stop_status",false},
            //    { "clean_status",false},
            //};
            _posData = new Dictionary<string, int>();
            _sleepTime = sleeptime;
            stop = false;
            _client = new NetVars
            {
                CobID = id,
                IPAdress = ipaddress,
                Port = port
            };
            int i = 0;
            while (i < readtotal)
            {
                _client.dataTypeCollection.Add(new CDataTypeCollection(DataTypes.bytetype));
                i++;
            }
            _client.dataTypeCollection.Add(new CDataTypeCollection(DataTypes.inttype));
            _client.dataTypeCollection.Add(new CDataTypeCollection(DataTypes.booltype));
            _client.dataTypeCollection.Add(new CDataTypeCollection(DataTypes.booltype));
            _client.dataTypeCollection.Add(new CDataTypeCollection(DataTypes.inttype));
            _client.dataTypeCollection.Add(new CDataTypeCollection(DataTypes.booltype));
            _client.dataTypeCollection.Add(new CDataTypeCollection(DataTypes.booltype));
            _client.dataTypeCollection.Add(new CDataTypeCollection(DataTypes.booltype));
            _client.dataTypeCollection.Add(new CDataTypeCollection(DataTypes.booltype));
            _client.dataTypeCollection.Add(new CDataTypeCollection(DataTypes.booltype));
            _client.dataTypeCollection.Add(new CDataTypeCollection(DataTypes.booltype));
            _client.dataTypeCollection.Add(new CDataTypeCollection(DataTypes.dinttype));//X位置
            _client.dataTypeCollection.Add(new CDataTypeCollection(DataTypes.dinttype));//Y位置
            _client.dataTypeCollection.Add(new CDataTypeCollection(DataTypes.dinttype));//Z1位置
            _client.dataTypeCollection.Add(new CDataTypeCollection(DataTypes.dinttype));//Z2位置
            _client.dataTypeCollection.Add(new CDataTypeCollection(DataTypes.udinttype));//Z1传感器数据
            _client.dataTypeCollection.Add(new CDataTypeCollection(DataTypes.udinttype));//Z2传感器数据
            ThreadReceive = new Thread(ReadData) { IsBackground = true };
        }

        /// <summary>
        /// 下位机写入实例
        /// </summary>
        /// <param name="ipaddress"></param>
        /// <param name="id"></param>
        public NetVar(string ipaddress,int id)
        {
            _client = new NetVars { IPAdress = ipaddress, CobID = id };
        }

        public void Disconnect()
        {
            _client.disconnect();
        }

        private void ReadData()
        {
            Console.WriteLine(@"UDP Thread Start!");
            while (!stop)
            {
                try
                {
                    int loop = 0;
                    Thread.Sleep(_sleepTime);
                    _status.Clear();
                    _reTubeStatus.Clear();
                    _posData.Clear();
                    ArrayList dataTable = _client.ReadValues();
                    if (dataTable.Count == 1)
                    {
                        //Console.WriteLine($"数据错误：返回变量数量不匹配，返回数目：{dataTable[0]}");
                        //ShowGrowlwarningMessage($"数据错误：返回变量数量不匹配，返回数目：{dataTable[0]}");
                        //LogHelper.instance().Error($"数据错误：返回变量数量不匹配，返回数目：{dataTable[0]}");
                        continue;
                    }
                    foreach (var item in dataTable)
                    {
                        if(loop < 42)
                            _reTubeStatus.Add(int.Parse(item.ToString()));//试管状态
                        if (loop == 42)
                            _step_id = int.Parse(item.ToString());//进行的动作ID
                        if (loop == 43)
                            _status.Add(bool.Parse(item.ToString()));//BUSY信号 0,
                        if (loop == 44)
                            _status.Add(bool.Parse(item.ToString()));//设备ERROR信号 1,
                        if(loop == 45)
                        {
                            _error_Id = int.Parse(item.ToString());//设备错误ID
                            EDeviceErrorCallBack?.Invoke(_error_Id);
                        }
                        if (loop > 45 && loop < 52)
                            _status.Add(bool.Parse(item.ToString()));// 2 初始化状态,3 工作完成状态,4 洗针状态,5 暂停状态,6 急停状态,7 排空状态
                        if (loop > 51 && loop <= 53)
                        {
                            _posData.Add(string.Format("{0}",(char)('x'+ loop - 52)), int.Parse(item.ToString()));
                        }
                        if(loop > 53 && loop <= 55)
                        {
                            _posData.Add(string.Format("z{0}", loop - 53), int.Parse(item.ToString()));
                        }
                        if (loop > 55 && loop <= 57)
                        {
                            _posData.Add(string.Format("z{0}sensor", loop - 55), int.Parse(item.ToString()));
                        }
                        loop++;
                    }
                    EDataReceiveCallBack?.Invoke(_reTubeStatus);
                    ERunStatusCallBack?.Invoke(_status);
                    if (_step_id < 20)
                        EInitStepCallBack?.Invoke(_step_id);
                    else
                        ERuningStepCallBack?.Invoke(_step_id);
                    EPosStatusCallBack?.Invoke(_posData);
                    //Console.WriteLine(_resetValue.ToArray());
                    //Thread.Sleep(_sleepTime);
                }
                catch (SocketException socketError)
                {
                    Console.WriteLine(socketError.Message);
                    switch (socketError.ErrorCode)
                    {
                        case (int)SocketError.ConnectionRefused:
                            //EErrorReceiveCallBack?.Invoke((int)SocketError.ConnectionRefused);
                            ShowGrowlErrorMessage("服务器拒绝UDP客户端连接!");
                            LogHelper.instance().Error("UDP客户端错误：{0}", "服务器拒绝UDP客户端连接!", socketError);
                            return;
                        case (int)SocketError.NotConnected:
                            _client.disconnect();
                            _client.connect();
                            //EErrorReceiveCallBack?.Invoke((int)SocketError.NotConnected); 
                            ShowGrowlwarningMessage("UDP客户端重连!");
                            LogHelper.instance().Error("UDP客户端错误：{0}", "UDP客户端重连!", socketError);
                            break;
                        case (int)SocketError.Interrupted:
                            _client.disconnect();
#if LOCAL
                            MessageBox.Show(string.Format("UDP接收错误：{0}", socketError));
#endif
                            ShowGrowlErrorMessage("UDP客户端连接断开！");
                            LogHelper.instance().Error("UDP客户端错误：{0}", "UDP客户端错误!", socketError);
                            return;
                        case (int)SocketError.TimedOut:
                            ShowGrowlwarningMessage("UDP客户端连接超时!");
                            LogHelper.instance().Error("UDP客户端错误：{0}", "UDP客户端连接超时!");
                            break;
                    }
                }
                catch (ThreadAbortException ex)
                {
                    _client.disconnect();
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(@"UDP Client Stop !");
#if LCOAL
                    MessageBox.Show(string.Format("UDP接收错误：{0}", ex.Message));
#endif
                    ShowGrowlinfoMessage("线程-UDP客户端连接断开");
                    LogHelper.instance().Info("UDP客户端信息：{0}", "UDP客户端断开连接!", ex);

                    return;
                }
                catch (Exception ex)
                {
                    _client.disconnect();
                    Console.WriteLine(ex.Message);
#if LOCAL
                    MessageBox.Show(string.Format("UDP接收错误：{0}", ex.Message));
#endif
                    ShowGrowlErrorMessage($"线程-UDP客户端未知错误：{ex.Message}");
                    LogHelper.instance().Error("UDP客户端警告：{0}", "UDP客户端未知错误!", ex.Message);
                    return;
                }
            }
            Application.Current?.Dispatcher.Invoke(() =>
            {
                Growl.InfoGlobal("UDP客户端已停止！");
            });
            _client.disconnect();
            Console.WriteLine("UDP Thread Stop!");
        }

        public void WriteData(List<CDataTypeCollection> dataTypeCollection)
        {
            try
            {
                _client.dataTypeCollection = dataTypeCollection;
                _client.SendValues();

                _client.CreateGVLFile(AppDomain.CurrentDomain.BaseDirectory + "\\" + string.Format("{0}.GVL",_client.CobID));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ShowGrowlErrorMessage(ex.Message);
#if LOCAL
                MessageBox.Show(string.Format("UDP发送错误：{0}", ex.Message));
#endif
            }
        }

        private static void ShowGrowlinfoMessage(string message)
        {
            Application.Current?.Dispatcher.Invoke(() =>
            {
                Growl.InfoGlobal(message);
            });
        }

        private static void ShowGrowlErrorMessage(string message)
        {
            Application.Current?.Dispatcher.Invoke(() =>
            {
                Growl.ErrorGlobal(message);
            });
        }

        private static void ShowGrowlwarningMessage(string message)
        {
            Application.Current?.Dispatcher.Invoke(() =>
            {
                Growl.WarningGlobal(message);
            });
        }

    }
}
