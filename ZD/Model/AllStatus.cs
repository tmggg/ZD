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

        /// <summary>
        /// 初始化动作集合
        /// </summary>
        public Dictionary<int, string> InitStatus;
        /// <summary>
        /// 操作动作集合
        /// </summary>
        public Dictionary<int, string> SeparationStatus;
        /// <summary>
        /// 错误代码集合
        /// </summary>
        public Dictionary<int, string> ErrorStatus;

        private AllStatus()
        {
            InitStatus = new Dictionary<int, string>();
            SeparationStatus = new Dictionary<int, string>();
            ErrorStatus = new Dictionary<int, string>();

            InitStatus.Add(0, "准备就绪，等待启动。。。。");
            InitStatus.Add(1, "FX、FY轴回原点。。。。");
            InitStatus.Add(2, "FZ1、FZ2轴与各选择阀回原点。。。。");
            InitStatus.Add(3, "FX、FY轴走到清洗槽排废位置，泵A、泵C回原点。。。。");
            InitStatus.Add(4, "泵A、泵C管路预填充并清洗FZ双轴导电针内壁，泵B回原点。。。。");
            InitStatus.Add(5, "泵B、泵C管路预填充并清洗FZ双轴导电针内壁。。。。");
            InitStatus.Add(6, "FZ1轴导电针针浸泡外壁。。。。");
            InitStatus.Add(7, "FZ2轴导电针针浸泡外壁。。。。");

            SeparationStatus.Add(20, "第一次工作前洗针");
            SeparationStatus.Add(21, "更换加标液或加液溶剂，并洗针");
            SeparationStatus.Add(22, "加标 走至标液瓶");
            SeparationStatus.Add(23, "加标 吸取加标液");
            SeparationStatus.Add(24, "加标 去对应的试管上方等待加标");
            SeparationStatus.Add(25, "加标 打出标液");
            SeparationStatus.Add(26, "加标 加标结束");
            SeparationStatus.Add(27, "加液 FZ1轴走至试管上方等待加液");
            SeparationStatus.Add(28, "加液 正在加液");
            SeparationStatus.Add(29, "加液 加液完成");

            SeparationStatus.Add(40, "排空 FZ双轴下降，等待排空");
            SeparationStatus.Add(41, "排空 A泵、C泵正在排空");
            SeparationStatus.Add(42, "排空 B泵正在排空");
            SeparationStatus.Add(43, "排空 FZ1轴导电针针浸泡外壁");
            SeparationStatus.Add(44, "排空 FZ2轴导电针针浸泡外壁");
            SeparationStatus.Add(100, "各轴回到初始位置");
            SeparationStatus.Add(101, "暂停中");


            ErrorStatus.Add(0, "无错误");
            ErrorStatus.Add(1, "加标容积设置范围错误");
            ErrorStatus.Add(2, "加标瓶选择范围错误");
            ErrorStatus.Add(3, "加液溶剂选择范围错误");
            ErrorStatus.Add(4, "加液容积设置范围错误");
            ErrorStatus.Add(5, "加液导电针FZ1轴清洗容积设置范围错误");
            ErrorStatus.Add(6, "加标导电针FZ2轴清洗容积设置范围错误");
            ErrorStatus.Add(7, "1号溶剂瓶液面太低");
            ErrorStatus.Add(8, "2号溶剂瓶液面太低");
            ErrorStatus.Add(9, "3号溶剂瓶液面太低");
            ErrorStatus.Add(10, "4号溶剂瓶液面太低");
            ErrorStatus.Add(11, "5号溶剂瓶液面太低");
            ErrorStatus.Add(12, "排废瓶液面过高");
            ErrorStatus.Add(16, "有电机没有走到指定位置");
            ErrorStatus.Add(17, "第一个设备板ECAT联系不上");
            ErrorStatus.Add(18, "第二个设备板ECAT联系不上"); 
            ErrorStatus.Add(19, "第三个设备板ECAT联系不上"); 
            ErrorStatus.Add(20, "第四个设备板ECAT联系不上"); 
            ErrorStatus.Add(21, "第五个设备板ECAT联系不上");
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
