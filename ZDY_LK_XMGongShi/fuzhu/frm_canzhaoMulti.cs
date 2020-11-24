using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace fuzhu
{
    public partial class frm_canzhaoMulti : Form
    {

         DataTable dts,dt2;//数据源
         string CZname;


         public DataRow[] drxz { get; set; }

        public frm_canzhaoMulti()
        {
            InitializeComponent();
        }


        #region 重载窗口
        /// <summary>
/// 参照窗体
/// </summary>
/// <param name="dt">参照数据源</param>
/// <param name="canZhaoTxt">参照，输入的文本</param>
/// <param name="canZhaoMing">参照名称</param>
/// <param name="cflag"></param>
        public frm_canzhaoMulti(string canZhaoTxt)
        {
            
            InitializeComponent();
            this.Text = "项目人员选择";
            textEdit1.Text = canZhaoTxt;
        }
      

        private void frm_canzhao_Load(object sender, EventArgs e)
        {
            DevExpress.Accessibility.AccLocalizer.Active = new DevExpress.LocalizationCHS.DevExpressUtilsLocalizationCHS();
            DevExpress.XtraEditors.Controls.Localizer.Active = new DevExpress.LocalizationCHS.DevExpressXtraEditorsLocalizationCHS();
            DevExpress.XtraGrid.Localization.GridLocalizer.Active = new DevExpress.LocalizationCHS.DevExpressXtraGridLocalizationCHS();
            DevExpress.XtraLayout.Localization.LayoutLocalizer.Active = new DevExpress.LocalizationCHS.DevExpressXtraLayoutLocalizationCHS();
            //DevExpress.XtraNavBar.NavBarLocalizer.Active = new DevExpress.LocalizationCHS.DevExpressXtraNavBarLocalizationCHS();
            DevExpress.XtraPrinting.Localization.PreviewLocalizer.Active = new DevExpress.LocalizationCHS.DevExpressXtraPrintingLocalizationCHS();
            
            //查询项目编码
            Cx();
            //dts.Columns.Add("选择", typeof(Boolean)).SetOrdinal(0);
            //排序
            
            
            //gridView1.FindFilterText = canZhaoTxt;
           
            
        }

        //查询项目编码，人员显示第一行的
        private void Cx()
        {
            try
            {
                string sql = string.Format(@"select cno  from zdy_lk_v_xmgongshi_xm");
                if (!string.IsNullOrEmpty(textEdit1.Text))
                {
                    sql += string.Format(" where cno like '%{0}%'", textEdit1.Text);

                }
                dts = DbHelper.ExecuteTable(sql);
                DataView dv = dts.DefaultView;
                dv.Sort = dts.Columns[0].ColumnName;
                gridControl1.DataSource = dts;

                if (dts.Rows.Count > 0)
                {
                   GetMx(0);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void GetMx(int iRow)
        {
            string cNo = DbHelper.GetDbString(dts.Rows[iRow]["cno"]);
           string sql = string.Format("select  cno, xmzy, zyxm from zdy_lk_v_xmperson where cno ='{0}'", cNo);
            dt2 = DbHelper.ExecuteTable(sql);
            dt2.Columns.Add("选择", typeof(Boolean)).SetOrdinal(0);
            for (int j = 0; j < dt2.Rows.Count; j++)
            {
                dt2.Rows[j]["选择"] = "True";
            }

            gridControl2.DataSource = dt2;
        }
        #endregion


   

     

        #region 双击返回数       
        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
           
           
        }
        #endregion

        private void btnOk_Click(object sender, EventArgs e)
        {
            drxz = dt2.Select("选择='true'");

            this.Close();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Cx();
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            GetMx(e.FocusedRowHandle);
        }



    }
}
