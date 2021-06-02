using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgS.Model
{
    public class AllStatus
    {
        private static AllStatus allstatus = null;

        public Dictionary<int, string> InitStatus;
        public Dictionary<int, string> SeparationStatus;
        public Dictionary<int, string> ErrorStatus;

        private AllStatus()
        {
            InitStatus = new Dictionary<int, string>();
            SeparationStatus = new Dictionary<int, string>();
            ErrorStatus = new Dictionary<int, string>();

            InitStatus.Add(0, "准备就绪，等待启动。。。。");
            InitStatus.Add(1, "X，Y轴回原点。。。。");
            InitStatus.Add(2, "X，Y轴回原点。。。。");
            InitStatus.Add(3, "X，Y轴走到清洗槽排废位置，注射泵准备回零准备。。。。");
            InitStatus.Add(4, "PumpA，PumpB回原点。。。。");
            InitStatus.Add(5, "管路预填充并清洗FZ双轴导电针内壁。。。。");
            InitStatus.Add(6, "FZ双轴导电针针浸泡外壁。。。。");

            SeparationStatus.Add(20, "第一次分取前洗针");
            SeparationStatus.Add(21, "走至50ml试管上方");
            SeparationStatus.Add(22, "50ml试管液面探测");
            SeparationStatus.Add(23, "在50ml试管吸液并液面追随");
            SeparationStatus.Add(24, "FZ轴运动消除挂液");
            SeparationStatus.Add(25, "走到色谱瓶上方");
            SeparationStatus.Add(26, "FZ轴下降加液");
            SeparationStatus.Add(27, "在色谱瓶中液面探测以消除挂液");
            SeparationStatus.Add(28, "洗针");
            SeparationStatus.Add(100, "各轴回到初始位置");
            SeparationStatus.Add(101, "暂停中");
            SeparationStatus.Add(110, "参数设置错误，已退出动作");

            ErrorStatus.Add(0, "无错误");
            ErrorStatus.Add(1, "FZ1轴导电针传感器异常");
            ErrorStatus.Add(2, "FZ2轴导电针传感器异常");
            ErrorStatus.Add(3, "FZ1和FZ2轴导电针传感器都异常");
            ErrorStatus.Add(4, "还没有进行初始化操作");
            ErrorStatus.Add(5, "有电机没有走到指定位置");
            ErrorStatus.Add(6, "分取体积参数设置错误");
            ErrorStatus.Add(7, "洗针体积参数设置错误");
            ErrorStatus.Add(8, "FZ1轴液面探测报错");
            ErrorStatus.Add(9, "FZ2轴液面探测报错");
            ErrorStatus.Add(10, "FZ1和FZ2轴都液面探测报错");
            ErrorStatus.Add(11, "FZ1轴液面跟追太低报错");
            ErrorStatus.Add(12, "FZ2轴液面跟追太低报错");
            ErrorStatus.Add(13, "FZ1和FZ2轴都液面跟追太低报错");

        }

        public static AllStatus GetAllStatus()
        {
            if(allstatus == null)
            {
                allstatus = new AllStatus();
            }
            return allstatus;
        }


    }
}
