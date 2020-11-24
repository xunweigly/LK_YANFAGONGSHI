using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using fuzhu;
using ADODB;
using MSXML2;
using UFIDA.U8.U8APIFramework;
using UFIDA.U8.U8MOMAPIFramework;
using UFIDA.U8.U8APIFramework.Parameter;
using System.Threading;
using System.Data.SqlClient;
using Process;
using System.IO;

namespace LKU8.shoukuan
{
    public partial class UserControl1 : UserControl
    {

        private DataTable dtMo;
        string chk2;

        public UserControl1()
        {
            InitializeComponent();

            
        }

          
        #region 加载
      private void UserControl1_Load(object sender, EventArgs e)
        {


            DevExpress.Accessibility.AccLocalizer.Active = new DevExpress.LocalizationCHS.DevExpressUtilsLocalizationCHS();
            DevExpress.XtraEditors.Controls.Localizer.Active = new DevExpress.LocalizationCHS.DevExpressXtraEditorsLocalizationCHS();
            DevExpress.XtraGrid.Localization.GridLocalizer.Active = new DevExpress.LocalizationCHS.DevExpressXtraGridLocalizationCHS();
            DevExpress.XtraLayout.Localization.LayoutLocalizer.Active = new DevExpress.LocalizationCHS.DevExpressXtraLayoutLocalizationCHS();
            //DevExpress.XtraNavBar.NavBarLocalizer.Active = new DevExpress.LocalizationCHS.DevExpressXtraNavBarLocalizationCHS();
            DevExpress.XtraPrinting.Localization.PreviewLocalizer.Active = new DevExpress.LocalizationCHS.DevExpressXtraPrintingLocalizationCHS();

          //查询最近未关闭的立项书编码
          string sql = @"select a.cno as 项目编码 from LK_XM_LX A
LEFT JOIN zdy_lk_projectsum B ON A.CNO = B.lxscno
WHERE (isnull(b.dsumdate,'')='' or b.dsumdate>dateadd(YYYY,-1,GETDATE()))
AND A.xmzt='成功' and a.dMakeDate>=dateadd(YYYY,-2,GETDATE())";
          repositoryItemSearchLookUpEdit1.DataSource = DbHelper.ExecuteTable(sql);
          repositoryItemSearchLookUpEdit1.DisplayMember = "项目编码";
          repositoryItemSearchLookUpEdit1.ValueMember = "项目编码";



            Cx();
            //更新布局
            string fileName = CommonHelper.currenctDir +"\\config"+ canshu.cMenuname;
            if (File.Exists(fileName))
            {
                gridView1.RestoreLayoutFromXml(fileName);
            }



        }
        #endregion

      #region 项目限定条件，只可以做自己负责的项目
      private string StrWhere()
      {
          string strWhere = string.Empty;
         Boolean bOn = ClsDataAuth.GetAuth(canshu.u8Login.cUserId);
          if (!bOn )
          {
            strWhere = strWhere + string.Format(" and (a.xzr like '%{0}/%' or a.xmgly='{0}'  or a.fzr='{0}' )", canshu.u8Login.cUserId);

          }
        
          
      
          return strWhere;
      }
      #endregion


      #region 查询
      public void Cx()
      {
          try
          {
              SearchCondition searchObj = new SearchCondition();
              searchObj.AddCondition("b.cno", txtXmbm.Text, SqlOperator.Equal);
              searchObj.AddCondition("b.czyxm", txtPerson.Text, SqlOperator.Equal);
              searchObj.AddCondition("b.dbdate", dateTimePicker1.Value.ToString("yyyy-MM-dd"), SqlOperator.MoreThanOrEqual, dateTimePicker1.Checked == false);
              searchObj.AddCondition("b.dbdate", dateTimePicker2.Value.ToString("yyyy-MM-dd"), SqlOperator.LessThanOrEqual, dateTimePicker2.Checked == false);
              searchObj.AddCondition("cStatus", comboBox1.Text, SqlOperator.Equal);

              string conditionSql = searchObj.BuildConditionSql(2);
              StringBuilder strb = new StringBuilder(@"
SELECT b.id, b.cno,b.dbdate,b.dedate,b.czybm,b.czyxm,dgongshi,b.cmemo,b.cmaker,b.dmaketime,b.cverifier,b.dverifytime,cstatus FROM   LK_XM_LX a,zdy_lk_xm_gongshi b WHERE a.cNo = b.cno ");
              strb.Append(conditionSql);

              dtMo = DbHelper.Execute(strb.ToString()).Tables[0];
              dtMo.Columns.Add("chk", typeof(Boolean));
              //dtMo.Columns.Add("bz", typeof(string));
              gridControl1.DataSource = dtMo;
          }
          catch (Exception ex)
          {
              MessageBox.Show(ex.Message);
              return;
          }
      }


      #endregion


      #region 增加行
      public void Add()
      {

          try
          {

              DataRow dr = dtMo.NewRow();
              dr["dbdate"] = DateTime.Now.ToString("yyyy-MM-dd");
              dr["dedate"] = DateTime.Now.ToString("yyyy-MM-dd");
              dr["dmaketime"] = canshu.u8Login.CurDate;
              dr["cstatus"] = "未审核";
              dr["chk"] = true;
              dr["cmaker"] = canshu.userName;


              dtMo.Rows.Add(dr);

          }
          catch (Exception ex)
          {
              MessageBox.Show(ex.Message);
              return;
          }
      }
      #endregion

      #region 删除
      public void Del()
      {
          gridView1.PostEditor();
          gridView1.UpdateCurrentRow();
          //已录入时间的，怎么处理
          int j = 0;
          DialogResult result = CommonHelper.MsgQuestion("确认要删除已勾选行吗？");
          if (result == DialogResult.Yes)
          {
              try
              {
                  for (int i = dtMo.Rows.Count - 1; i >= 0; i--)
                  {
                      if (Convert.ToBoolean(dtMo.Rows[i]["chk"] == DBNull.Value ? false : dtMo.Rows[i]["chk"]))
                      {


                          //没id，自动保存。有id，判断是否modifyed，如果更改了，update
                          string id = DbHelper.GetDbString(dtMo.Rows[i]["id"]);

                          //没保存的直接删除，保存的，删除数据库

                          if (id == "" || string.IsNullOrEmpty(id))
                          {
                              dtMo.Rows.RemoveAt(i);
                              j++;
                          }
                          else
                          {
                              string cZt = DbHelper.GetDbString(dtMo.Rows[i]["cStatus"]);
                              if (cZt == "未审核")
                              {
                                 string sql = " delete from zdy_lk_xm_gongshi where id=@Id ";
                                  DbHelper.ExecuteNonQuery(sql, new SqlParameter[] { new SqlParameter("@Id", id) });
                                  j++;

                                  dtMo.Rows.RemoveAt(i);
                              }
                              else
                              {
                                  MessageBox.Show("第" + (i + 1).ToString() + "已审核，请先弃审再删除");
                                  continue;

                              }

                              //dtXunjia.Rows.RemoveAt(i);
                          }


                      }
                  }
              }
              catch (Exception ex)
              {
                  //DbHelper.RollbackAndCloseConnection(tran);
                  CommonHelper.MsgError("删除失败！原因：" + ex.Message);
              }
              if (j > 0)
              {
                  CommonHelper.MsgInformation("删除完成！");
                  //加保存

              }
          }
          //Cx();
      }
      #endregion

      #region 保存
      public void Save()
      {
          

              comboBox1.Focus();
              gridView1.PostEditor();
              gridView1.UpdateCurrentRow();
              string str = "";
              for (int i = 0; i < dtMo.Rows.Count; i++)
              {

                  string cNo = DbHelper.GetDbString(dtMo.Rows[i]["cno"]);
                  if (string.IsNullOrEmpty(cNo))
                  {

                      str = str + "第" + (i + 1) + "行项目编码未输入\r\n";

                  }
                  string cName = DbHelper.GetDbString(dtMo.Rows[i]["czyxm"]);
                  if (string.IsNullOrEmpty(cName))
                  {

                      str = str + "第" + (i + 1) + "行人员姓名未输入\r\n";

                  }
                
                  DateTime dtBegin =DbHelper.GetDbDate(dtMo.Rows[i]["dbdate"]);
                   DateTime dtEnd=DbHelper.GetDbDate(dtMo.Rows[i]["dedate"]);
                   if (dtEnd < dtBegin)
                  {
                      str = str + "第" + (i + 1) + "行截止日期小于起始日期\r\n";
                  }
                  //不是同一个月份的不可保存
                   if (dtEnd.ToString("yyyyMM") != dtBegin.ToString("yyyyMM"))
                  {
                      str = str + "第" + (i + 1) + "行起始日期和截止日期不在同一个月份！\r\n";

                  }
                  string cError = "";
                  //检查总账是否结帐，已结帐的不可更改
                  cError = ClsDataAuth.XZ_CheckJieZhang(dtBegin.ToString("yyyy-MM-dd"));

                  if (cError != "")
                  {
                      str = str + "第" + (i + 1) + "行" + cError + "\r\n";
                  }


                
                  


              }
              if (str != "")
              {
                  CommonHelper.MsgError(str);
                  return;
              }

              SqlTransaction tran = DbHelper.BeginTrans();
              try
              {
                  DataTable dt2 = dtMo.GetChanges();
                  if (dt2 != null)
                  {
                      for (int i = 0; i < dt2.Rows.Count; i++)
                      {

                          string cNo = DbHelper.GetDbString(dt2.Rows[i]["cno"]);
                          string cName = DbHelper.GetDbString(dt2.Rows[i]["czyxm"]);
                          DateTime dtBegin = DbHelper.GetDbDate(dt2.Rows[i]["dbdate"]);
                          DateTime dtEnd = DbHelper.GetDbDate(dt2.Rows[i]["dedate"]);
                          string cId = DbHelper.GetDbString(dt2.Rows[i]["id"]);
                          if (string.IsNullOrEmpty(cId))
                          {
                              cId = "";
                          }

                          //检查日期是否冲突，和保存的数据
                          SqlParameter[] param = new SqlParameter[]{ 
                new SqlParameter("@cno",cNo), new SqlParameter("@cxm",cName),
                 new SqlParameter("@dbdate",dtBegin),
                 new SqlParameter("@dedate",dtEnd),
                 new SqlParameter("@id",cId),
                 new SqlParameter("@ire",SqlDbType.Int)                                         
          };
                          param[5].Direction = ParameterDirection.Output;

                          DbHelper.ExecuteSqlWithTrans("zdy_lk_sp_gongshichongfu", param, CommandType.StoredProcedure, tran);
                          //检查条码是否存在
                          int iRe = DbHelper.GetDbInt(param[5].Value);
                          if (iRe == 1)
                          {
                              str = string.Format("项目:{0}, 人员:{1},开始日期:{2}截止日期:{3}：\r\n和之前记录冲突！", cNo, cName, dtBegin.ToShortDateString(), dtEnd.ToShortDateString());
                              throw new Exception(str);
                          }
                          else
                          {
                              if (cId == "")
                              {
                                  string sql = @"insert into zdy_lk_xm_gongshi(cno
      ,[dbdate]
      ,[dedate]
      ,[czybm]
      ,[czyxm]
      ,[dgongshi]
      ,[cmemo]
      ,[cmaker]
      ,[dmaketime],cstatus) values(@cno
      ,@dbdate
      ,@dedate
      ,@czybm
      ,@czyxm
      ,@dgongshi
      ,@cmemo
      ,@cmaker
      ,getdate(),'未审核')";
                                  DbHelper.ExecuteSqlWithTrans(sql, new SqlParameter[]{ 
                new SqlParameter("@cno",cNo),
                 new SqlParameter("@dbdate",dtBegin),
                 new SqlParameter("@dedate",dtEnd),
                  new SqlParameter("@czybm",DbHelper.ToDbValue(dt2.Rows[i]["czybm"])),
                  new SqlParameter("@czyxm",cName),
                   new SqlParameter("@dgongshi",DbHelper.ToDbValue(dt2.Rows[i]["dgongshi"])),
                   new SqlParameter("@cmemo",DbHelper.ToDbValue(dt2.Rows[i]["cmemo"])),
                         new SqlParameter("@cmaker",DbHelper.ToDbValue(dt2.Rows[i]["cmaker"]))

                              }, CommandType.Text, tran);
                              }
                              else
                              {
                                  string sql = @"update zdy_lk_xm_gongshi set cno=@cno,
      
      ,dbdate=@dbdate
      ,dedate=@dedate
      ,czybm=@czybm
      ,czyxm=@czyxm
      ,dgongshi=@dgongshi
      ,cmemo=@cmemo where id =@id)";
                                  DbHelper.ExecuteSqlWithTrans(sql, new SqlParameter[]{ 
                new SqlParameter("@cno",cNo),
                 new SqlParameter("@dbdate",dtBegin),
                 new SqlParameter("@dedate",dtEnd),
                  new SqlParameter("@czybm",DbHelper.ToDbValue(dt2.Rows[i]["czybm"])),
                  new SqlParameter("@czyxm",cName),
                   new SqlParameter("@dgongshi",DbHelper.ToDbValue(dt2.Rows[i]["dgongshi"])),
                   new SqlParameter("@cmemo",DbHelper.ToDbValue(dt2.Rows[i]["cmemo"])),
                         new SqlParameter("@id",cId)

                              }, CommandType.Text, tran);

                              }
                          }



                      }


                      DbHelper.CommitTransAndCloseConnection(tran);
                      MessageBox.Show("保存成功！");
                      Cx();
                  }
              }
              catch (Exception ex)
              {
                  DbHelper.RollbackAndCloseConnection(tran);
                  MessageBox.Show(ex.Message);
                  return;
              }
         
          


      }
      #endregion



      #region 弃审
     public void QiShen()
      {
          //校验是否有审核权限
          Boolean bOn = ClsDataAuth.GetAuth(canshu.u8Login.cUserId);
          if (!bOn)
          {
              MessageBox.Show("没有弃审权限！");
              return;

          }

          try
          {
              gridView1.CloseEditor();
              string str = "";
              int j = 0;
              for (int i = 0; i < dtMo.Rows.Count; i++)
              {
                  if (Convert.ToBoolean(dtMo.Rows[i]["chk"] == DBNull.Value ? false : dtMo.Rows[i]["chk"]))
                  {
                      //没id，自动保存。有id，判断是否modifyed，如果更改了，update
                      string id = DbHelper.GetDbString(dtMo.Rows[i]["id"]);


                      string cZt = DbHelper.GetDbString(dtMo.Rows[i]["cStatus"]);
                      if (cZt == "审核")
                      {
                          //检查是否已录入工时和入库


                          //已录入的无法进行弃审

                          string sql = @" update zdy_lk_xm_gongshi
                                    set cStatus ='未审核',cverifier=null,dverifytime=null
                                     where id = @id  ";
                          DbHelper.ExecuteNonQuery(sql, new SqlParameter[]{ 
                                                                       new SqlParameter("@id",DbHelper.GetDbString(dtMo.Rows[i]["id"]))
                                              });
                          j++;
                      }

                      else
                      {
                          str = str + "第" + (i + 1).ToString() + "行未审核\r\n";
                          continue;

                      }

                  }
              }

              if (!string.IsNullOrEmpty(str))
              {
                  MessageBox.Show(str);

              }
              if (j > 0)
              {
                  MessageBox.Show("弃审完成");
                  Cx();
              }
          }
          catch (Exception ex)
          {
              MessageBox.Show(ex.Message);
              return;
          }

      }
      #endregion

      #region 审核
   public void ShenHe()
      {


          //校验是否有审核权限
          Boolean bOn = ClsDataAuth.GetAuth(canshu.u8Login.cUserId);
          if (!bOn )
          {
              MessageBox.Show("没有审核权限！");
              return;

          }


          try
          {
              string str = "";
              gridView1.CloseEditor();
              int j = 0;
              for (int i = 0; i < dtMo.Rows.Count; i++)
              {
                  if (Convert.ToBoolean(dtMo.Rows[i]["chk"] == DBNull.Value ? false : dtMo.Rows[i]["chk"]))
                  {
                     
                      //没id，自动保存。有id，判断是否modifyed，如果更改了，update
                      string id = DbHelper.GetDbString(dtMo.Rows[i]["id"]);

                      //没id，无法进行操作
                      if (string.IsNullOrEmpty(id))
                      {
                          MessageBox.Show("第" + i.ToString() + "没有保存，请先保存");
                          return;
                      }
                      else
                      {


                          string cZt = DbHelper.GetDbString(dtMo.Rows[i]["cstatus"]);
                          if (cZt == "未审核")
                          {

                              string sql = @" update zdy_lk_xm_gongshi 
                                    set cStatus ='审核',cverifier=@cverifier,dverifytime=@dverifydate
                                     where id = @id  ";
                                  DbHelper.ExecuteNonQuery(sql, new SqlParameter[]{ 
                                                                       new SqlParameter("@id",DbHelper.GetDbString(dtMo.Rows[i]["id"])),
                                                                       new SqlParameter("@cverifier",canshu.userName),
                                                                         new SqlParameter("@dverifydate",canshu.u8Login.CurDate)
                                              });
                                  //处理审核事件
                                  j++;
                            

                          }

                          else
                          {
                              str = str + "第" + (i + 1).ToString() + "行已审核\r\n";
                              continue;

                          }
                      }
                  }
              }

              if (!string.IsNullOrEmpty(str))
              {
                  MessageBox.Show(str);
              }
              if (j > 0)
              {
                  MessageBox.Show("审核完成");
                  Cx();
              }

          }
          catch (Exception ex)
          {
              MessageBox.Show(ex.Message);
              return;
          }

      }
      #endregion


      #region 设置审核行不可更改
      private void gridView1_ShowingEditor(object sender, CancelEventArgs e)
      {
          DataRow row = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
          if (row != null)
          {
              if (row["cStatus"].ToString() == "审核" && this.gridView1.FocusedColumn.Name != "chk")
              {
                  e.Cancel = true;
                  //该行不可编辑      
              }
          }
      }
      #endregion


      #region 全选全消
       public void Checkall()
      {

          if (string.IsNullOrEmpty(chk2))
          {
              for (int i = 0; i < dtMo.Rows.Count; i++)
              {
                  dtMo.Rows[i]["chk"] = true;
              }
              chk2 = "chkAll";
          }
          else
          {
              for (int i = 0; i < dtMo.Rows.Count; i++)
              {
                  dtMo.Rows[i]["chk"] = false;
              }
              chk2 = string.Empty;
          }
      }
      #endregion



      #region 布局
    public void SaveBuju()
      {
          if (!Directory.Exists(Application.StartupPath + @"\\config"))
              Directory.CreateDirectory(Application.StartupPath + @"\\config");

          string fileName = CommonHelper.currenctDir + "\\config" + canshu.cMenuname;
          gridView1.SaveLayoutToXml(fileName);
          CommonHelper.MsgInformation("保存布局成功！");

      }
    public void DelBuju()
      {


          string fileName = CommonHelper.currenctDir + "\\config" + canshu.cMenuname;
          if (File.Exists(fileName))
          {
              File.Delete(fileName);
              CommonHelper.MsgInformation("删除布局成功，请重新打开！");
          }
          else
          {
              CommonHelper.MsgInformation("布局未保存，无需删除！");
          }


      }

      #endregion


      #region 导出excel
      public void Excel()
      {

          SaveFileDialog saveFileDialog = new SaveFileDialog();
          saveFileDialog.Title = "导出Excel";
          //saveFileDialog.Filter = "Excel文件(*.pdf)|*.pdf";
          saveFileDialog.Filter = "Excel文件(*.xls)|*.xls";
          DialogResult dialogResult = saveFileDialog.ShowDialog(this);

          if (dialogResult == DialogResult.OK)
          {
              //DevExpress.XtraPrinting.XlsExportOptions options = new DevExpress.XtraPrinting.XlsExportOptions();

              gridView1.OptionsPrint.AutoWidth = false;
              gridView1.ExportToXls(saveFileDialog.FileName);
              DevExpress.XtraEditors.XtraMessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
          }

      }
      #endregion


      private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
      {

          if (e.Info.IsRowIndicator && e.RowHandle >= 0)
          {
              e.Info.DisplayText = (e.RowHandle + 1).ToString();
          }

      }

      private void gridView1_RowCountChanged(object sender, EventArgs e)
      {

          //动态设置第一列的宽度
          string MeasureString = String.Format("{0}WA", gridView1.RowCount);
          gridView1.IndicatorWidth = this.gridControl1.CreateGraphics().MeasureString(MeasureString, new Font("宋体", 9)).ToSize().Width;

      }

      #region 项目编码
      private void txtDep_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
          try
          {
              string sql = string.Format(@"select cno as 项目编码 from zdy_lk_v_xmgongshi_xm");
              DataTable  dtDep = DbHelper.ExecuteTable(sql);

              frm_canzhao frm = new frm_canzhao(dtDep, txtXmbm.Text, "项目档案");
              frm.ShowDialog();
              if (frm.drxz != null)
              {
                  txtXmbm.Text = DbHelper.GetDbString(frm.drxz["项目编码"]);

              }
          }
          catch (Exception EX)
          {
              MessageBox.Show(EX.Message);
              return;
          }
      }
    #endregion

   
      
      #region 项目按钮
      private void xmButtonEdit2_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
           int iRowIndex = gridView1.GetFocusedDataSourceRowIndex();
          string cNo =DbHelper.GetDbString( dtMo.Rows[iRowIndex]["cno"]) ;
          frm_canzhaoMulti frm = new frm_canzhaoMulti(cNo);
          frm.ShowDialog();
         
          if (frm.drxz != null)
          {
              int iCnt = frm.drxz.Length;
              for (int i = 0; i < iCnt; i++)
              {
                  if (i == 0)
                  {
                      dtMo.Rows[iRowIndex]["cno"] = frm.drxz[i]["cno"];
                      dtMo.Rows[iRowIndex]["czyxm"] = frm.drxz[i]["zyxm"];
                      dtMo.Rows[iRowIndex]["czybm"] = frm.drxz[i]["xmzy"];

                  }
                  else
                  {
                      DataRow dr = dtMo.NewRow();
                      dr["dbdate"] = DateTime.Now.ToString("yyyy-MM-dd");
                      dr["dedate"] = DateTime.Now.ToString("yyyy-MM-dd");
                      dr["dmaketime"] = canshu.u8Login.CurDate;
                      dr["cstatus"] = "未审核";
                      dr["chk"] = true;
                      dr["cmaker"] = canshu.userName;
                      dr["cno"] = frm.drxz[i]["cno"];
                      dr["czyxm"] = frm.drxz[i]["zyxm"];
                      dr["czybm"] = frm.drxz[i]["xmzy"];
                      dr["dbdate"] = dtMo.Rows[iRowIndex]["dbdate"];
                      dr["dedate"] = dtMo.Rows[iRowIndex]["dedate"];

                      dtMo.Rows.Add(dr);

                  }

              
              }
              

          }
      }
      #endregion







    }
}
