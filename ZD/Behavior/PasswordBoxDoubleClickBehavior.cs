#region <<版本注释>>
/*----------------------------------------------------------------
 * 版权所有 (c) 2021  NJRN 保留所有权利。
 * CLR版本：4.0.30319.42000
 * 机器名称：WTG
 * 公司名称：
 * 命名空间：SgS.Behavior
 * 唯一标识：10d85605-2f0f-4c82-afb6-7207ae66d6da
 * 文件名：PasswordBoxDoubleClickBehavior
 * 当前用户域：WTG
 * 
 * 创建者：TMGG
 * 电子邮箱：tm574378328@gmail.com
 * 创建时间：2021/5/26 11:11:23
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

using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interactivity;

namespace SgS.Behavior
{
    public class PasswordBoxDoubleClickBehavior:Behavior<PasswordBox>
    {
                protected override void OnAttached()
        {
            AssociatedObject.TouchDown += AssociatedObject_TouchDown;
            base.OnAttached();
        }

        private void AssociatedObject_TouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            if (sender is PasswordBox)
            {
                KeyPad.Keypad keypad = new KeyPad.Keypad(null, ((PasswordBox)sender).Tag?.ToString());
                keypad.ShowDialog();
                ((PasswordBox)sender).Password = keypad.Result;
            }
        }

        protected override void OnDetaching()
        {
            AssociatedObject.TouchDown -= AssociatedObject_TouchDown;
            base.OnDetaching();
        }


        private void AssociatedObject_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(sender is PasswordBox)
            {
                KeyPad.Keypad keypad = new KeyPad.Keypad(null, ((PasswordBox)sender).Tag?.ToString());
                keypad.ShowDialog();
                ((PasswordBox)sender).Password = keypad.Result;
            }
        }

    }
}
