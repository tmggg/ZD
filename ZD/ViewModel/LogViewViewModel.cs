using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using SgS.Model;


namespace SgS.ViewModel
{
    public class LogViewViewModel : ViewModelBase
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr _lopen(string lpPathName, int iReadWrite);
        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);

        public const int OF_READWRITE = 2;
        public const int OF_SHARE_DENY_NONE = 0x40;
        public readonly IntPtr HFILE_ERROR = new IntPtr(-1);
        private Thread readThread;
        private bool stop;
        private object Loglock = new object();

        public ObservableCollection<LogData> LogDatas { get; private set; }
        public ObservableCollection<LogFile> LogList { get; private set; }

        public RelayCommand<Window> CloseCommand { get; private set; }

        public RelayCommand<Window> DragCommand { get; private set; }

        public RelayCommand RefreshCommand { get; private set; }

        public RelayCommand<LogFile> LoadLogCommand { get; private set; }

        public RelayCommand RealTimeLogCommand { get; private set; }

        public LogViewViewModel()
        {
            LogDatas = new ObservableCollection<LogData>();
            LogList = new ObservableCollection<LogFile>();
            if (!System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"Logs\ZD-error-" +
                                                              DateTime.Now.ToString("yyyy-MM-dd") + ".log"))
                System.IO.File.Create(AppDomain.CurrentDomain.BaseDirectory + @"Logs\ZD-error-" +
                                                                          DateTime.Now.ToString("yyyy-MM-dd") + ".log").Close();

            string[] Filepath = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + @"Logs", "*error*");
            foreach (var f in Filepath)
            {
                LogList.Add(new LogFile() { FileName = Path.GetFileName(f), FilePath = f });
            }


            //using (System.IO.StreamReader sr = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"Logs\SGS-error-" +
            //                                                              DateTime.Now.ToString("yyyy-MM-dd") + ".log", Encoding.Default))
            //{
            //    // 从文件读取并显示行，直到文件的末尾
            //    while ((line = sr.ReadLine()) != null)
            //    {
            //        List<string> data = line.Split('|').ToList();
            //        data.RemoveAt(2);
            //        //Console.WriteLine(line);
            //        //strData = line;
            //        Application.Current.Dispatcher.Invoke(() =>
            //        {
            //            LogDatas.Add(new LogData() { Time = data[0], Level = data[1], Data = data[2] });
            //        });
            //    }
            //}

            stop = false;
            readThread = new Thread(ReadLogFile);
            readThread.Start();

            CloseCommand = new RelayCommand<Window>(CloseWindow);
            DragCommand = new RelayCommand<Window>(DragWindow);
            LoadLogCommand = new RelayCommand<LogFile>(LoadLog);
            RealTimeLogCommand = new RelayCommand(RealTimeLog);
            //RefreshCommand = new RelayCommand(RefreshLog);
            //MessengerInstance.Send(new NotificationMessage("I'm Done !"));
            //MessengerInstance.Send(this);
            MessengerInstance.Register<bool>(this, WindowClosing);
        }

        private void RealTimeLog()
        {
            if (!readThread.IsAlive)
            {
                LogDatas.Clear();
                stop = false;
                readThread = new Thread(ReadLogFile);
                readThread.Start();
            }
        }

        private void LoadLog(LogFile obj)
        {
            stop = true;
            string line;

            //LogDatas = null;
            LogDatas.Clear();
            Thread thread = new Thread(
                () =>
                {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(obj.FilePath, Encoding.Default))
                {
                    List<string> data = new List<string>();
                    // 从文件读取并显示行，直到文件的末尾
                    lock (Loglock)
                    {
                            while ((line = sr.ReadLine()) != null)
                            {
                                data = line.Split('|').ToList();
                                data.RemoveAt(2);
                                //Console.WriteLine(line);
                                //strData = line;
                                Application.Current.Dispatcher.Invoke(() =>
                            {
                                LogDatas.Add(new LogData() { Time = data[0], Level = data[1], Data = data[2] });
                            });
                            }
                        }
                    }
                });
            thread.Start();
        }

        private async void RefreshLog()
        {
            LogDatas.Clear();
            string line;
            using (System.IO.StreamReader sr = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"Logs\ZD-error-" +
                                                                          DateTime.Now.ToString("yyyy-MM-dd") + ".log", Encoding.Default))
            {
                // 从文件读取并显示行，直到文件的末尾
                while ((line = await sr.ReadLineAsync()) != null)
                {
                    List<string> data = line.Split('|').ToList();
                    data.RemoveAt(2);
                    //Console.WriteLine(line);
                    //strData = line;
                    LogDatas.Add(new LogData() { Time = data[0], Level = data[1], Data = data[2] });
                }
            }
        }

        private void WindowClosing(bool obj)
        {
            stop = true;
        }

        private void CloseWindow(Window w)
        {
            w?.Close();
        }

        private void DragWindow(Window w)
        {
            w?.DragMove();
        }

        private void ReadLogFile()
        {
            while (!stop)
            {
                string line;
                try
                {
                    int readCount = 0;
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
                    lock (Loglock)
                    {
                        if (stop)
                            return;
                        if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"Logs\ZD-error-" +
                            DateTime.Now.ToString("yyyy-MM-dd") + ".log"))
                            continue;
                        using (System.IO.StreamReader sr = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"Logs\ZD-error-" +
                            DateTime.Now.ToString("yyyy-MM-dd") + ".log", Encoding.Default))
                        {
                            List<string> data = new List<string>();
                            // 从文件读取并显示行，直到文件的末尾
                            while ((line = sr.ReadLine()) != null)
                            {
                                data = line.Split('|').ToList();
                                data.RemoveAt(2);
                                //Console.WriteLine(line);
                                //strData = line;
                                readCount++;
                                if (readCount > LogDatas.Count)
                                {
                                    Application.Current.Dispatcher.Invoke(() =>
                                    {
                                        LogDatas.Add(new LogData() { Time = data[0], Level = data[1], Data = data[2] });
                                    });
                                }
                            }
                        }
                    }
                    Thread.Sleep(1000);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    //throw;
                }
            }
        }
    }
}
