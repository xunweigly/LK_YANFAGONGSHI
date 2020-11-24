using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UFIDA.U8.Portal.Framework.Actions;
using System.Windows.Forms;

namespace LKU8.shoukuan
{
    class NetSampleDelegate : IActionDelegate
    {
        #region IActionDelegate 成员

        //代理，和菜单项关联。
        void IActionDelegate.Run(IAction action)
        {
            //action.tag 把实例化的 user control 传过来了。
            //使用时，直接使用 action.tag就可以了。方法要使用public，才可以用。
            UserControl1 us = (UserControl1)action.Tag;
                switch (action.Id)
                {
                    case "add":
                        {
                            us.Add();
                            return;
                        }
                    case "del":
                        {
                            us.Del();

                            return;
                        }
                    case "Save":
                        {
                            us.Save();
                            //us.WaitJs();
                            return;
                        }
                    case "Checkall":
                        {
                            us.Checkall();
                            //us.WaitJs();
                            return;
                        }

                    //case "Copy":
                    //    {
                    //        us.Copy();
                    //        //us.WaitJs();
                    //        return;
                    //    }
                    case "ShenHe":
                        {
                            us.ShenHe();

                            return;
                        }

                    case "query":
                        {
                            us.Cx();


                            return;
                        }

                    case "QiShen":
                        {
                            us.QiShen();


                            return;
                        }
                    case "Excel":
                        {
                            us.Excel();


                            return;
                        }
                    case "ExcelIn":
                        {
                            //us.ExcelIn();


                            return;
                        }

                    case "savebuju":
                        {
                            us.SaveBuju();


                            return;
                        }
                    case "delbuju":
                        {
                            us.DelBuju();


                            return;
                        }
                   
                 
                }
                MessageBox.Show("press a Toolbar ID is '"+ action.Id+"'");
            
        }

        void IActionDelegate.SelectionChanged(IAction action, UFIDA.U8.Portal.Common.Core.ISelection selection)
        {
            //throw new NotImplementedException();
        }

        #endregion
    }
}
