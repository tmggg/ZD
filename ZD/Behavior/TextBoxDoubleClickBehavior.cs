#region <<版本注释>>
/*----------------------------------------------------------------
 * 版权所有 (c) 2021  NJRN 保留所有权利。
 * CLR版本：4.0.30319.42000
 * 机器名称：WTG
 * 公司名称：
 * 命名空间：SgS.Behavior
 * 唯一标识：37c99ab9-3ac2-43ea-8cdc-05486ffb4981
 * 文件名：TextBoxDoubleClickBehavior
 * 当前用户域：WTG
 * 
 * 创建者：TMGG
 * 电子邮箱：tm574378328@gmail.com
 * 创建时间：2021/5/26 10:21:51
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
    public class TextBoxDoubleClickBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            AssociatedObject.TouchUp += AssociatedObject_TouchUp; ;
            base.OnAttached();
        }

        private void AssociatedObject_TouchUp(object sender, System.Windows.Input.TouchEventArgs e)
        {
            if (sender is TextBox)
            {
                double result = double.NaN;
                double.TryParse(((TextBox)sender).Text, out result);
                KeyPad.Keypad keypad = new KeyPad.Keypad(null, ((TextBox)sender).Tag?.ToString(), 0, 0, result);
                keypad.ShowDialog();
                if (double.TryParse(keypad.Result, out result))
                    ((TextBox)sender).Text = result.ToString();
            }
        }

        protected override void OnDetaching()
        {
            AssociatedObject.TouchDown -= AssociatedObject_TouchUp;
            base.OnDetaching();
        }


        private void AssociatedObject_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is TextBox)
            {
                KeyPad.Keypad keypad = new KeyPad.Keypad(null, ((TextBox)sender).Tag?.ToString());
                keypad.ShowDialog();
                double result = double.NaN;
                if (double.TryParse(keypad.Result, out result))
                    ((TextBox)sender).Text = keypad.Result;
            }
        }

    }
}
