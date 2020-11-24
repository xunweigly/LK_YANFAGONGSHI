using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using LKU8.shoukuan;
using System.Text.RegularExpressions;

namespace fuzhu
{
    static class ClsDealField
    {
        public static string DLookUp(string stfield, string sttable, string stwhere)
        {
            String strsql;
            String objvalue;
            strsql = "";
            strsql = strsql + "SELECT " + stfield + " from " + sttable;
            if (stwhere != "")
            {
                strsql += " WHERE " + stwhere;
            }
            DataSet mydataset = new DataSet();
            SqlDataAdapter thisAdapter;
            try
            {
                thisAdapter = new SqlDataAdapter(strsql,canshu.conStr);
                thisAdapter.Fill(mydataset, "table");
                objvalue = "";
                if (mydataset.Tables["table"].Rows.Count > 0)
                {
                    for (int i = 0; i <= mydataset.Tables["table"].Rows.Count - 1; i++)
                    {
                        objvalue = DbHelper.GetDbString(mydataset.Tables["table"].Rows[i][0]);
                        return objvalue;
                    }
                }
            }
            catch (Exception)
            {
                return "";
            }
            return objvalue;
        }


        public static string DMax(string stfield, string sttable, string stwhere) 
        {
            String strsql;
            String objvalue;
            strsql = "";
            strsql = strsql + "SELECT max(" + stfield + ") from " + sttable;
            if (stwhere != "")
            {
                strsql += " WHERE " + stwhere;
            }
            DataSet mydataset = new DataSet();
            SqlConnection thisConnection;
            SqlDataAdapter thisAdapter;
            try
            {
                thisConnection = new SqlConnection(canshu.conStr);
                thisAdapter = new SqlDataAdapter(strsql, thisConnection);
                thisAdapter.Fill(mydataset, "table");
                objvalue = "0";
                if (mydataset.Tables["table"].Rows.Count > 0)
                {
                    for (int i = 0; i <= mydataset.Tables["table"].Rows.Count - 1; i++)
                    {
                        objvalue = DbHelper.GetDbString(mydataset.Tables["table"].Rows[i][0]);
                    }
                }
            }
            catch (Exception)
            {
                return "0";
            }
            return objvalue;
        }

        public static string DMin(string stfield, string sttable, string stwhere)
        {
            String strsql;
            String objvalue;
            strsql = "";
            strsql = strsql + "SELECT min(" + stfield + ") from " + sttable;
            if (stwhere != "")
            {
                strsql += " WHERE " + stwhere;
            }
            DataSet mydataset = new DataSet();
            SqlConnection thisConnection;
            SqlDataAdapter thisAdapter;
            try
            {
                thisConnection = new SqlConnection(canshu.conStr);
                thisAdapter = new SqlDataAdapter(strsql, thisConnection);
                thisAdapter.Fill(mydataset, "table");
                objvalue = "";
                if (mydataset.Tables["table"].Rows.Count > 0)
                {
                    for (int i = 0; i <= mydataset.Tables["table"].Rows.Count - 1; i++)
                    {
                        objvalue = DbHelper.GetDbString(mydataset.Tables["table"].Rows[i][0]);
                    }
                }
            }
            catch (Exception)
            {
                return "";
            }
            return objvalue;
        }
        public static string DSum(string stfield, string sttable, string stwhere)
        {
            String strsql;
            String objvalue;
            strsql = "";
            strsql = strsql + "SELECT Sum(" + stfield + ") from " + sttable;
            if (stwhere != "")
            {
                strsql += " WHERE " + stwhere;
            }
            DataSet mydataset = new DataSet();
            SqlConnection thisConnection;
            SqlDataAdapter thisAdapter;
            try
            {
                thisConnection = new SqlConnection(canshu.conStr);
                thisAdapter = new SqlDataAdapter(strsql, thisConnection);
                thisAdapter.Fill(mydataset, "table");
                objvalue = "0";
                if (mydataset.Tables["table"].Rows.Count > 0)
                {
                    for (int i = 0; i <= mydataset.Tables["table"].Rows.Count - 1; i++)
                    {
                        objvalue = DbHelper.GetDbString(mydataset.Tables["table"].Rows[i][0]);
                    }
                }
            }
            catch (Exception)
            {
                return "0";
            }
            return objvalue;
        }

        public static bool IsNumeric(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        }
        public static bool IsInt(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*$");
        }
        public static bool IsUnsign(string value)
        {
            return Regex.IsMatch(value, @"^\d*[.]?\d*$");
        }

        public static bool isTel(string strInput)
        {
            return Regex.IsMatch(strInput, @"\d{3}-\d{8}|\d{4}-\d{7}");
        }
    }
}
