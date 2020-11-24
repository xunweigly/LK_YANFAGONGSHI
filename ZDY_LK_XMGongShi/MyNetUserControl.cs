using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UFIDA.U8.Portal.Framework.MainFrames;
using UFIDA.U8.Portal.Framework.Actions;
using UFIDA.U8.Portal.Proxy.editors;
using UFIDA.U8.Portal.Proxy.Actions;

namespace LKU8.shoukuan
{
    class MyNetUserControl : INetUserControl
    {
        #region INetUserControl 成员

        UserControl1 usercontrol = null;
        //private IEditorInput _editInput = null;
        //private IEditorPart _editPart = null;
        private string _title;
        public System.Windows.Forms.Control CreateControl(UFSoft.U8.Framework.Login.UI.clsLogin login, string MenuID, string Paramters)
        {
            usercontrol = new UserControl1();
            usercontrol.Name = "LKxsxunjia";
            return usercontrol;
            //throw new NotImplementedException();
        }

        public UFIDA.U8.Portal.Proxy.Actions.NetAction[] CreateToolbar(UFSoft.U8.Framework.Login.UI.clsLogin login)
        {


            IActionDelegate nsd = new NetSampleDelegate();
            ////string skey = "mynewcontrol";

            NetAction[] aclist;
            aclist = new NetAction[12];
            NetAction ac = new NetAction("add", nsd);
            ac.Text = "增行";
            ac.Tag = usercontrol;
            ac.Image = Properties.Resources.add;
            ac.ActionType = NetAction.NetActionType.Edit;
            ac.DisplayStyle = 1;
            ac.Style = 1;
            ac.SetGroup = "增行";
            ac.SetGroupRow = 1;
            ac.RowSpan = 3;
            aclist[0] = ac;

            ac = new NetAction("del", nsd);
            //aclist = new NetAction[1];
            ac.Text = "删行";
            ac.Tag = usercontrol;
            ac.Image = Properties.Resources.Adjust_write_off;
            ac.ActionType = NetAction.NetActionType.Edit;
            ac.DisplayStyle = 1;
            ac.Style = 1;
            ac.SetGroup = "删行";
            ac.SetGroupRow = 1;
            ac.RowSpan = 3;
            aclist[1] = ac;

           

             ac = new NetAction("add", nsd);
            ac.Text = "批量添加";
            ac.Tag = usercontrol;
            ac.Image = Properties.Resources.add;
            ac.ActionType = NetAction.NetActionType.Edit;
            ac.DisplayStyle = 1;
            ac.Style = 1;
            ac.SetGroup = "添加";
            ac.SetGroupRow = 1;
            ac.RowSpan = 3;
            ac.IsVisible = false;
            aclist[2] = ac;

            ac = new NetAction("Save", nsd);
            //aclist = new NetAction[1];
            ac.Text = "保存";
            ac.Tag = usercontrol;
            ac.Image = Properties.Resources.save;
            ac.ActionType = NetAction.NetActionType.Edit;
            ac.DisplayStyle = 1;
            ac.Style = 1;
            ac.SetGroup = "保存";
            ac.SetGroupRow = 1;
            ac.RowSpan = 3;
            aclist[3] = ac;

            ac = new NetAction("query", nsd);
            //aclist = new NetAction[1];
            ac.Text = "查询";
            ac.Tag = usercontrol;
            ac.Image = Properties.Resources.query;
            ac.ActionType = NetAction.NetActionType.Edit;
            ac.DisplayStyle = 1;
            ac.Style = 1;
            ac.SetGroup = "查询";
            ac.SetGroupRow = 1;
            ac.RowSpan = 3;
            aclist[4] = ac;

            ac = new NetAction("Checkall", nsd);
            //aclist = new NetAction[1];
            ac.Text = "全选/全消";
            ac.Tag = usercontrol;
            ac.Image = Properties.Resources.Select_all;
            ac.ActionType = NetAction.NetActionType.Edit;
            ac.DisplayStyle = 1;
            ac.Style = 1;
            ac.SetGroup = "审核";
            ac.SetGroupRow = 1;
            ac.RowSpan = 3;
            //ac.IsVisible = false;
            aclist[5] = ac;


            ac = new NetAction("ShenHe", nsd);
            //aclist = new NetAction[1];
            ac.Text = "审核";
            ac.Tag = usercontrol;
            ac.Image = Properties.Resources.Cancel;
            ac.ActionType = NetAction.NetActionType.Edit;
            ac.DisplayStyle = 1;
            ac.Style = 1;
            ac.SetGroup = "审核";
            ac.SetGroupRow = 1;
            ac.RowSpan = 3;
            //ac.IsVisible = false;
            aclist[6] = ac;

            ac = new NetAction("QiShen", nsd);
            //aclist = new NetAction[1];
            ac.Text = "弃审";
            ac.Tag = usercontrol;
            ac.Image = Properties.Resources.open;
            ac.ActionType = NetAction.NetActionType.Edit;
            ac.DisplayStyle = 1;
            ac.Style = 1;
            ac.SetGroup = "弃审";
            ac.SetGroupRow = 1;
            ac.RowSpan = 3;
            //ac.IsVisible = false;
            aclist[7] = ac;

            ac = new NetAction("Excel", nsd);
            //aclist = new NetAction[1];
            ac.Text = "导出EXCEL";
            ac.Tag = usercontrol;
            ac.Image = Properties.Resources.transfer;
            ac.ActionType = NetAction.NetActionType.Edit;
            ac.DisplayStyle = 1;
            ac.Style = 1;
            ac.SetGroup = "查询";
            ac.SetGroupRow = 1;
            ac.RowSpan = 3;
            //ac.IsVisible = false;
            aclist[8] = ac;

            ac = new NetAction("ExcelIn", nsd);
            //aclist = new NetAction[1];
            ac.Text = "导入数据";
            ac.Tag = usercontrol;
            ac.Image = Properties.Resources.import;
            ac.ActionType = NetAction.NetActionType.Edit;
            ac.DisplayStyle = 1;
            ac.Style = 1;
            ac.SetGroup = "查询";
            ac.SetGroupRow = 1;
            ac.RowSpan = 3;
            ac.IsVisible = false;
            aclist[9] = ac;


            ac = new NetAction("savebuju", nsd);
            //aclist = new NetAction[1];
            ac.Text = "保存布局";
            ac.Tag = usercontrol;
            ac.Image = Properties.Resources.import;
            ac.ActionType = NetAction.NetActionType.Edit;
            ac.DisplayStyle = 1;
            ac.Style = 1;
            ac.SetGroup = "保存布局";
            ac.SetGroupRow = 1;
            ac.RowSpan = 1;
            aclist[10] = ac;

            ac = new NetAction("delbuju", nsd);
            //aclist = new NetAction[1];
            ac.Text = "删除布局";
            ac.Tag = usercontrol;
            ac.Image = Properties.Resources.import;
            ac.ActionType = NetAction.NetActionType.Edit;
            ac.DisplayStyle = 1;
            ac.Style = 1;
            ac.SetGroup = "保存布局";
            ac.SetGroupRow = 1;
            ac.RowSpan = 1;
            aclist[11] = ac;

            return aclist;
            ////return null;
        }
        public bool CloseEvent()
        {
            //throw new Exception("The method or operation is not implemented.");
            return true;
        }
        #endregion



        IEditorInput INetUserControl.EditorInput
        {
            get;
            set;
        }

        IEditorPart INetUserControl.EditorPart
        {
            get;set;

        }

        string INetUserControl.Title
        {
            get
            {
                return this._title;
            }
            set
            {
                this._title = value;
            }
        }
       


    }


}
