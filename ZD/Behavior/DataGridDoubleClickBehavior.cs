#region <<版本注释>>
/*----------------------------------------------------------------
 * 版权所有 (c) 2021  NJRN 保留所有权利。
 * CLR版本：4.0.30319.42000
 * 机器名称：WTG
 * 公司名称：
 * 命名空间：SgS.Behavior
 * 唯一标识：369586e8-810b-4a2b-894b-eff5bb05453b
 * 文件名：DataGridDoubleClickBehavior
 * 当前用户域：WTG
 * 
 * 创建者：TMGG
 * 电子邮箱：tm574378328@gmail.com
 * 创建时间：2021/5/25 16:53:57
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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace SgS.Behavior
{
    public class DataGridDoubleClickBehavior:Behavior<DataGrid>
    {
        protected override void OnAttached()
        {
            AssociatedObject.PreparingCellForEdit += AssociatedObject_PreparingCellForEdit; ;
            base.OnAttached();
        }

        private void AssociatedObject_TouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            var cellInfo = grid.SelectedCells[grid.CurrentColumn.DisplayIndex];
            var content = cellInfo.Column.GetCellContent(cellInfo.Item);
            if (content is TextBox)
            {
                KeyPad.Keypad keypad = new KeyPad.Keypad(null, grid.CurrentColumn.Header.ToString());
                keypad.ShowDialog();
                ((TextBox)content).Text = keypad.Result;
            }
        }

        protected override void OnDetaching()
        {
            AssociatedObject.PreparingCellForEdit -= AssociatedObject_PreparingCellForEdit;
            base.OnDetaching();
        }


        private void AssociatedObject_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            var cellInfo = grid.SelectedCells[grid.CurrentColumn.DisplayIndex];
            var content = cellInfo.Column.GetCellContent(cellInfo.Item);
            if(content is TextBox)
            {
                KeyPad.Keypad keypad = new KeyPad.Keypad(null, grid.CurrentColumn.Header.ToString(),0,0,double.Parse(((TextBox)content).Text));
                keypad.ShowDialog();
                ((TextBox)content).Text = keypad.Result;
            }
        }

    }
}
