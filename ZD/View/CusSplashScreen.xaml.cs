using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SgS.View
{
    /// <summary>
    /// CusSplashScreen.xaml 的交互逻辑
    /// </summary>
    public partial class CusSplashScreen : Window
    {
        int loop = 0;
        DispatcherTimer timer = new DispatcherTimer();

        const string str = "软件加载中，请稍等。。。。。";
        public CusSplashScreen()
        {
            InitializeComponent();
            //myBorder.Focus();
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromMilliseconds(200);
            Start();
            this.Loaded += CusSplashScreen_Loaded;
        }

        private void CusSplashScreen_Loaded(object sender, RoutedEventArgs e)
        {
            var bw = new BackgroundWorker();

            bw.DoWork += (s, y) =>
            {
                // 长时间任务
                Thread.Sleep(5000);
            };

            bw.RunWorkerCompleted += (s, y) =>
            {
                timer.Stop();
                DeviceMain mView = new DeviceMain();
                App.Current.MainWindow = mView;
                mView.Show();
                LoadingCircle.Visibility = Visibility.Collapsed;
                TipMessage.Text = "软件加载完成。。。。";


                var closeAnimation = new DoubleAnimation
                {
                    From = this.Opacity,
                    To = 0,
                    Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                    EasingFunction = new BackEase { EasingMode = EasingMode.EaseOut }
                };
                closeAnimation.Completed += (s1, e1) =>
                {
                    this.Close();
                };
                //this.BeginAnimation(Window.OpacityProperty, closeAnimation);
                this.BeginAnimation(Window.OpacityProperty, closeAnimation);
            };
            bw.RunWorkerAsync();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TipMessage.Text += str[loop++];
            if (loop == str.Length)
            {
                TipMessage.Text = string.Empty;
                loop = 0;
                timer.Stop();
                Start();
            }
        }
        private void Start()
        {
            timer.Start();
        }
    }
}
