using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data;

namespace fuzhu
{
    class Dgvfuzhu
    {

        #region 窗体布局保存读取
        // 加载XMLDataGridView 样式
        public static void BindReadDataGridViewStyle(string strFromName, DataGridView dgvMain)
        {
            try
            {
                string XMLPath;
                
                XMLPath = Application.StartupPath + @"\\config" + "\\";


                if (!Directory.Exists(Application.StartupPath + @"\\config"))
                    Directory.CreateDirectory(Application.StartupPath + @"\\config");

                string FileName = XMLPath + strFromName + "_" + dgvMain.Name + ".xml";
                if (!File.Exists(FileName))
                {
                    return;
                }

                DataTable DTname = new DataTable();
                DTname.TableName = dgvMain.Name;
                DTname.Columns.Add("ColName");  //列名
                DTname.Columns.Add("ColHeaderText");  //标题
                DTname.Columns.Add("ColWidth");  //宽度 
                DTname.Columns.Add("ColVisble");  //是否显示               
                DTname.Columns.Add("ColId");  //显示顺序
                //DTname.Columns.Add("DefaultCellStyle");  //单元格样式
                //DTname.Columns.Add("ColumnType");  //单元格类型
                DTname.ReadXml(FileName);

                foreach (DataRow row in DTname.Rows)
                {
                    dgvMain.Columns[row["ColName"].ToString().Trim()].HeaderText = row["ColHeaderText"].ToString().Trim();
                    dgvMain.Columns[row["ColName"].ToString().Trim()].Width = int.Parse(row["ColWidth"].ToString().Trim());
                    dgvMain.Columns[row["ColName"].ToString().Trim()].Visible = Boolean.Parse(row["ColVisble"].ToString().Trim());
                    dgvMain.Columns[row["ColName"].ToString().Trim()].DisplayIndex = int.Parse(row["ColId"].ToString().Trim());
                    //dgvMain.Columns[row["name"].ToString()].DefaultCellStyle.Alignment = (DataGridViewContentAlignment)row["DefaultCellStyle"];
                    //dgvMain.Columns[row["ColumnType"].ToString()].DataPropertyName = row["ColumnType"].ToString();              
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 保存用户自定义列表顺序
        /// </summary>
        /// <param name="strFormName">窗体名称</param>
        /// <param name="DgvMain">DataGridView名称</param>
        public static void SaveDataGridViewStyle(string strFormName, DataGridView DgvMain)
        {
            try
            {
                string XMLPath;


                XMLPath = Application.StartupPath + @"\\config" + "\\";
                if (!Directory.Exists(Application.StartupPath + @"\\config"))
                    Directory.CreateDirectory(Application.StartupPath + @"\\config");

                string FileName = XMLPath + strFormName + "_" + DgvMain.Name + ".xml";  //生成文件到指定目录
                DataTable DTname = new DataTable();
                DTname.TableName = DgvMain.Name;
                DTname.Columns.Add("ColName");  //列名
                DTname.Columns.Add("ColHeaderText");  //标题
                DTname.Columns.Add("ColWidth");  //宽度 
                DTname.Columns.Add("ColVisble");  //是否显示             
                DTname.Columns.Add("ColId");  //显示顺序              
                //DTname.Columns.Add("DefaultCellStyle");  //单元格样式
                //DTname.Columns.Add("ColumnType");  //单元格类型         

                string[] array = new string[DgvMain.Columns.Count];
                //获取Visble =true 的列  
                foreach (DataGridViewColumn column in DgvMain.Columns)
                {
                    if (column.Visible == true)
                    {

                        //  拖动列顺序 
                        array[column.DisplayIndex] = column.Name + '|' + column.HeaderText + '|' + column.Width + '|' + column.Visible + '|' + column.DisplayIndex;
                    }
                }
                int ColumnsCount = array.Length;
                //取列属性
                for (int i = 0; i < ColumnsCount; i++)
                {
                    string[] str = new string[5];
                    try
                    {
                        DataRow row = DTname.NewRow();
                        str = array.GetValue(i).ToString().Split('|'); //分隔
                        row["ColName"] = str[0];
                        row["ColHeaderText"] = str[1];
                        row["ColWidth"] = str[2];
                        row["ColVisble"] = str[3];
                        row["ColId"] = str[4];
                        DTname.Rows.Add(row);
                        DTname.AcceptChanges();
                    }
                    catch
                    {
                        continue;
                    }
                }
                DTname.WriteXml(FileName);
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // 删除指定的XML文件
        public static bool deleteDataGridViewStyle(string strFromName, DataGridView dgvMain)
        {
            string XMLPath;
            XMLPath = Application.StartupPath + @"\\config" + "\\";
            string FileName = XMLPath + strFromName + "_" + dgvMain.Name + ".xml";
            if (File.Exists(FileName))
            {
                File.Delete(FileName); return true;
            }
            else
            {
                return false;
            }
        }
        #endregion


       

    }
}
