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
    public partial class frm_canzhao : Form
    {

         DataTable dts;//数据源
         string CZname;


         public DataRow drxz { get; set; }

        public frm_canzhao()
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
        public  frm_canzhao(DataTable dt,string canZhaoTxt,string canZhaoMing)
        {
            

            InitializeComponent();
            this.Text = canZhaoMing + " 参照";
            dts = dt;
            //排序
            DataView dv = dts.DefaultView; 
            dv.Sort = dts.Columns[0].ColumnName;
            gridControl1.DataSource = dts;
            //gridView1.FindFilterText = canZhaoTxt;
            gridView1.ApplyFindFilter(canZhaoTxt); 
            

        }
      

        private void frm_canzhao_Load(object sender, EventArgs e)
        {
            //DevExpress.Accessibility.AccLocalizer.Active = new DevExpress.LocalizationCHS.DevExpressUtilsLocalizationCHS();
            //DevExpress.XtraEditors.Controls.Localizer.Active = new DevExpress.LocalizationCHS.DevExpressXtraEditorsLocalizationCHS();
            //DevExpress.XtraGrid.Localization.GridLocalizer.Active = new DevExpress.LocalizationCHS.DevExpressXtraGridLocalizationCHS();
            //DevExpress.XtraLayout.Localization.LayoutLocalizer.Active = new DevExpress.LocalizationCHS.DevExpressXtraLayoutLocalizationCHS();
            ////DevExpress.XtraNavBar.NavBarLocalizer.Active = new DevExpress.LocalizationCHS.DevExpressXtraNavBarLocalizationCHS();
            //DevExpress.XtraPrinting.Localization.PreviewLocalizer.Active = new DevExpress.LocalizationCHS.DevExpressXtraPrintingLocalizationCHS();
            
        }
        #endregion


   

     

        #region 双击返回数       
        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle >=0)
            {
                drxz = gridView1.GetFocusedDataRow();
            }
                this.Close();
           
        }
        #endregion



    }
}
