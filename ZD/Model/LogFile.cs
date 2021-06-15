#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2021  TMGG 保留所有权利。
 * CLR版本：4.0.30319.42000
 * 机器名称：WTG
 * 公司名称：
 * 命名空间：SgS.Model
 * 唯一标识：1cce3449-6000-4143-a152-45642786bbcc
 * 文件名：LogFile
 * 当前用户域：WTG
 * 
 * 创建者：TMGG
 * 电子邮箱：tm574378328@gmail.com
 * 创建时间：2021/6/15 9:18:26
 * 版本：V1.0.0
 * 描述：
 *
 * ----------------------------------------------------------------
 * 修改人：
 * 时间：
 * 修改说明：
 *
 * 版本：V1.0.1
 *----------------------------------------------------------------*/
#endregion << 版 本 注 释 >>

using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SgS.Model
{
    /// <summary>
    /// LogFile 的摘要说明
    /// </summary>
    public class LogFile
    {
        #region <常量>
        #endregion <常量>

        #region <变量>
        #endregion <变量>

        #region <属性>
        public string FileName { get; set; }

        public string FilePath { get; set; }

        #endregion <属性>

        #region <构造方法和析构方法>
        #endregion <构造方法和析构方法>

        #region <方法>
        public override string ToString()
        {
            return FileName;
        }
        #endregion <方法>

        #region <事件>
        #endregion <事件>
    }
}