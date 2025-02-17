﻿#region <<版本注释>>
/*----------------------------------------------------------------
 * 版权所有 (c) 2021  NJRN 保留所有权利。
 * CLR版本：4.0.30319.42000
 * 机器名称：WTG
 * 公司名称：
 * 命名空间：SgS.Behavior
 * 唯一标识：7f9f4245-efdc-4e50-ab44-b1e32313d86c
 * 文件名：TextBoxShowVirtualKeyboard
 * 当前用户域：WTG
 * 
 * 创建者：TMGG
 * 电子邮箱：tm574378328@gmail.com
 * 创建时间：2021/5/26 12:53:52
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
#endregion <<版本注释>>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace SgS.Behavior
{
    public class TextBoxShowVirtualKeyboard:Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            AssociatedObject.TouchDown += AssociatedObject_TouchDown;
            base.OnAttached();
        }

        private void AssociatedObject_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is TextBox)
            {
                KeyPad.VirtualKeyboard keypad = new KeyPad.VirtualKeyboard(null, null, ((TextBox)sender).Text);
                keypad.ShowDialog();
                ((TextBox)sender).Text = keypad.Result;
            }
        }

        private void AssociatedObject_TouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            if (sender is TextBox)
            {
                KeyPad.VirtualKeyboard keypad = new KeyPad.VirtualKeyboard(null,null,((TextBox)sender).Text);
                keypad.ShowDialog();
                ((TextBox)sender).Text = keypad.Result;
            }
        }

        protected override void OnDetaching()
        {
            AssociatedObject.TouchDown -= AssociatedObject_TouchDown;
            base.OnDetaching();
        }
    }
}
