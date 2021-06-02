
namespace SgS.View
{
    /// <summary>
    /// DeviceMain.xaml 的交互逻辑
    /// </summary>
    public partial class DeviceMain
    {
        public DeviceMain()
        {
            InitializeComponent();
            // + 14因为有边框操作距离,如果resizemode是noresize则不需要
            this.MaxHeight = System.Windows.Forms.SystemInformation.WorkingArea.Height;
            this.WindowState = System.Windows.WindowState.Maximized;
        }
    }
}
