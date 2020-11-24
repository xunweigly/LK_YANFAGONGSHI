using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Data;

namespace fuzhu
{
    public class CommonHelper
    {
        public static string currenctDir = AppDomain.CurrentDomain.BaseDirectory;
        private static string msgTilte = "提示";
      
        public static void MsgInformation(string msg)
        {
            MessageBox.Show(msg, msgTilte, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public static DialogResult MsgQuestion(string msg)
        {
            return MessageBox.Show(msg, msgTilte, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        public static void MsgError(string msg)
        {
            MessageBox.Show(msg, msgTilte, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void MsgAsterisk(string msg)
        {
            MessageBox.Show(msg, msgTilte, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// 将数据集中的数据导出到EXCEL文件
        /// </summary>
        /// <param name="dataSet">输入数据集</param>
        /// <param name="isShowExcle">是否显示该EXCEL文件</param>
        /// <returns></returns>
        public static bool DataSetToExcel(DataTable dt2, bool isShowExcle)
        {
            DataTable dataTable = dt2;
            int rowNumber = dataTable.Rows.Count;//不包括字段名
            int columnNumber = dataTable.Columns.Count;
            int colIndex = 0;

            if (rowNumber == 0)
            {
                return false;
            }

            //建立Excel对象 
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            //excel.Application.Workbooks.Add(true);
            Microsoft.Office.Interop.Excel.Workbook workbook = excel.Workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
            Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];
            excel.Visible = isShowExcle;
            //Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)excel.Worksheets[1];
            Microsoft.Office.Interop.Excel.Range range;

            //生成字段名称 
            foreach (DataColumn col in dataTable.Columns)
            {
                colIndex++;
                excel.Cells[1, colIndex] = col.ColumnName;
            }

            object[,] objData = new object[rowNumber, columnNumber];

            for (int r = 0; r < rowNumber; r++)
            {
                for (int c = 0; c < columnNumber; c++)
                {
                    objData[r, c] = dataTable.Rows[r][c];
                }
                //Application.DoEvents();
            }

            // 写入Excel 
            range = worksheet.get_Range(excel.Cells[2, 1], excel.Cells[rowNumber + 1, columnNumber]);
            range.NumberFormat = "@";//设置单元格为文本格式
            range.Value2 = objData;
            //worksheet.get_Range(excel.Cells[2, 1], excel.Cells[rowNumber + 1, 1]).NumberFormat = "yyyy-m-d h:mm";

            return true;
        }
       
    }

}
