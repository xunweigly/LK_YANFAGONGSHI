using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using fuzhu;
using System.Data.SqlClient;

namespace LKU8.shoukuan
{
   class ClsDataAuth
    {

        /// <summary>  
        /// 返回部门权限，考勤用 
        /// </summary>  
        /// <param name="key"></param>  
        /// <param name="value"></param>  
        public static string GetDept( string cColName)
        {
            string cWhere = "";
            if (canshu.userName == "demo")
            {
                cWhere = " and 1=1 ";

            }
            else
            {
                string sql = string.Format("SELECT distinct b.cdepcode FROM    XZ_DepPerson a,department b where  b.cdepcode like a.cdepcode+'%' and cpersoncode = '{0}'", canshu.u8Login.cUserId);
                DataTable dt = DbHelper.Execute(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    cWhere = " and  "+cColName+" in ('";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        cWhere += DbHelper.GetDbString(dt.Rows[i]["cdepcode"]);
                        if (i != dt.Rows.Count - 1)
                        { 
                        cWhere +="','";
                        }
                        else
                        {
                            cWhere += "')";
                        }
                    }

                }
                else
                {
                    //没有权限
                    cWhere = " and 1=2 ";
                }
            
            }
            return cWhere;
        }



        /// <summary>  
        /// 精确查询考勤部门
        /// </summary>  
        /// <param name="key"></param>  
        /// <param name="value"></param>  
        public static string GetDeptKaoqin(string cColName)
        {
            string cWhere = "";
            if (canshu.u8Login.cUserId == "demo")
            {
                cWhere = " and 1=1 ";

            }
            else
            {
                string sql = string.Format("SELECT distinct b.cdepcode FROM    XZ_DepPerson a,department b where  b.cdepcode like a.cdepcode+'%' and a.bkaoqin=1 and cpersoncode = '{0}'",canshu.u8Login.cUserId);
                DataTable dt = DbHelper.Execute(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    cWhere = " and  " + cColName + " in ('";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        cWhere += DbHelper.GetDbString(dt.Rows[i]["cdepcode"]);
                        if (i != dt.Rows.Count - 1)
                        {
                            cWhere += "','";
                        }
                        else
                        {
                            cWhere += "')";
                        }
                    }

                }
                else
                {
                    //没有权限
                    cWhere = " and 1=2 ";
                }

            }
            return cWhere;
        }


       

              /// <summary>  
        /// 检查是否有审核权限
        /// </summary>  
        /// <param name="key"></param>  
        /// <param name="value"></param>  
        public static bool GetAuth(string cUserid)
        {
            bool bOn= false;

             //加一个
            string sql = "select cSysUserName from UA_User where cSysUserName is not null and isnull(nState,0)=0 and   cUser_Id='" + cUserid + "'";
          DataTable dt = DbHelper.ExecuteTable(sql);
          string cQx;
          if (dt.Rows.Count > 0)
          {
              cQx = DbHelper.GetDbString(dt.Rows[0]["cSysUserName"]);
          }
          else
          {
              cQx = "0";

          }

          //是否有权限
          if (canshu.userName != "demo" && cQx != "1" && cQx != "2")
          {
              bOn = false;
          }
          else
          {
              bOn = true;
          }

         return bOn;
        }

        /// <summary>
        /// 判断是否结帐 
        /// </summary>
        /// <param name="ddt"></param>
        /// <returns></returns>
        public static string XZ_CheckJieZhang(string ddt)
        {
            string cError = "";
           SqlParameter[] param = new SqlParameter[]{ 
                new SqlParameter("@ddate",ddt),
                 new SqlParameter("@cSubsys","GL"),
                 new SqlParameter("@bjz",SqlDbType.Int)
                                         
          };
            param[2].Direction = ParameterDirection.Output;

            DbHelper.ExecuteNonQuery("zdy_lk_sp_getJieZhang", param, CommandType.StoredProcedure);
            //检查条码是否存在
            int iRe = DbHelper.GetDbInt(param[2].Value);
          
            if (iRe == 1)
                cError = "日期所在月份已记账，无法进行增加或更改。";
            return cError;
        }

    }
}
