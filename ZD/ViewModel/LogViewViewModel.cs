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

        public ObservableCollection<LogData> LogDatas { get; set; }

        public RelayCommand<Window> CloseCommand { get; set; }

        public RelayCommand<Window> DragCommand { get; set; }

        public RelayCommand RefreshCommand { get; set; }

        public LogViewViewModel()
        {
            LogDatas = new ObservableCollection<LogData>();

            if (!System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"Logs\ZD-error-" +
                                                              DateTime.Now.ToString("yyyy-MM-dd") + ".log"))
                System.IO.File.Create(AppDomain.CurrentDomain.BaseDirectory + @"Logs\ZD-error-" +
                                                                          DateTime.Now.ToString("yyyy-MM-dd") + ".log").Close();

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
            //RefreshCommand = new RelayCommand(RefreshLog);
            //MessengerInstance.Send(new NotificationMessage("I'm Done !"));
            //MessengerInstance.Send(this);
            MessengerInstance.Register<bool>(this, WindowClosing);
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
