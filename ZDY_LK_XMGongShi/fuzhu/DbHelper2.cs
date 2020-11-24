using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using LKU8.shoukuan;
//using fuzhu;
//using WinServicesControl;

namespace fuzhu
{
    class DbHelper2
    {
       

          /// <summary>
        /// 数据库连接对象
        /// </summary>
        //private static SqlConnection conn = new SqlConnection(conStr);

        /// <summary>
        /// 打开数据库连接
        /// </summary>
        public static void Open(SqlConnection  conna)
        {
            //判断数据库连接是否关闭
            if (conna.State == ConnectionState.Closed)
            {
                conna.Open();
            }
        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public static void Close(SqlConnection conna)
        {
            //判断数据库连接是否打开
            if (conna.State == ConnectionState.Open)
            {
                conna.Close();
            }
        }

        /// <summary>
        /// 执行Command对象的ExecuteScalar方法
        /// </summary>
        /// <param name="sSql">要执行的SQL语句</param>
        /// <returns></returns>
        public static object ExecuteScalar(string sSql, string conStr)
        {
            object obj = null;
            using (SqlConnection conna = new SqlConnection(conStr))
            {
                SqlCommand comm = new SqlCommand(sSql, conna);

                try
                {
                    //调用当前类的数据库打开方法
                    Open(conna);

                    //执行Command对象的命令
                    obj = comm.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    //DbHelper2.WriteError(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, sSql);
                    MessageBox.Show(ex.Message);
                    //把异常抛到调用该方法的地方
                    //throw;
                }
                finally
                {
                    //调用当前类的数据库关闭方法
                    Close(conna);
                }

                return obj;
            }
        }

        /// <summary>
        /// 执行Command对象的ExecuteNonQuery方法
        /// </summary>
        /// <param name="sSql">要执行的SQL语句</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string sSql, string conStr)
        {
            int iResult=0;
            using (SqlConnection conna = new SqlConnection(conStr))
            {
                SqlCommand comm = new SqlCommand(sSql, conna);

                try
                {
                    //调用当前类的数据库打开方法
                    Open(conna);

                    //执行Command对象的命令
                    iResult = comm.ExecuteNonQuery();
                }
                catch (Exception ex)
                //catch
                {
                    DbHelper2.WriteError(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, sSql);
                    //把异常抛到调用该方法的地方
                    MessageBox.Show(ex.Message);
                    //return ;
                    //throw;
                }
                finally
                {
                    //调用当前类的数据库关闭方法
                    Close(conna);
                }

                return iResult;
            }
        }

        /// <summary>
        /// 执行Command对象的ExecuteNonQuery方法
        /// </summary>
        /// <param name="sSql">要执行的SQL语句</param>
        /// <returns></returns>
        public static bool ExecuteNonQuery(List<string> sSql, string conStr)
        {
            bool bResult = false;
            using (SqlConnection conna = new SqlConnection(conStr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = conna;

                try
                {
                    //调用当前类的数据库打开方法
                    Open(conna);
                    comm.Transaction = conna.BeginTransaction();

                    foreach (string s in sSql)
                    {
                        comm.CommandText = s;
                        //执行Command对象的命令
                        comm.ExecuteNonQuery();
                    }

                    comm.Transaction.Commit();
                    bResult = true;
                }
                catch (Exception ex)
                {
                    //DbHelper.WriteError(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, sSql);
                    comm.Transaction.Rollback();
                    MessageBox.Show(ex.Message);
                    //把异常抛到调用该方法的地方
                    //throw;
                }
                finally
                {
                    //调用当前类的数据库关闭方法
                    Close(conna);
                }

                return bResult;
            }
        }


        public static int ExecuteNonQuery(string SQLString, SqlParameter[] Params, CommandType CommandType, string conStr)
        {
            using (SqlConnection connection = new SqlConnection(conStr))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();

                        if (Params != null)
                        {
                            foreach (SqlParameter parameter in Params)
                                cmd.Parameters.Add(parameter);
                        }

                        //cmd.Parameters.Add(Params);
                        cmd.CommandType = CommandType;
                        cmd.CommandTimeout =6000;
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }
        /// <summary>
        /// 执行Command对象的ExecuteReader方法
        /// </summary>
        /// <param name="sSql">要执行的SQL语句</param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(string sSql, string conStr)
        {
            SqlDataReader dr = null;
            SqlConnection conna = new SqlConnection(conStr);
            
                Open(conna);
                SqlCommand comm = new SqlCommand(sSql, conna);

                try
                {
                    //调用当前类的数据库打开方法
                   

                    //执行Command对象的命令
                    dr = comm.ExecuteReader(CommandBehavior.CloseConnection);
                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    //把异常抛到调用该方法的地方
                    //throw;
                }
                //该处不能关闭数据库连接，否则返回的DataReader对象，在外面不能读取到数据

                return dr;
            
        }

        /// <summary>
        /// 通过适配器填充数据集
        /// </summary>
        /// <param name="sSql">要执行的SQL语句</param>
        /// <returns></returns>
        public static DataSet Execute(string sSql, string conStr)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conna = new SqlConnection(conStr))
            {
               SqlDataAdapter da = new SqlDataAdapter(sSql, conna);

                try
                {
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    //throw;
                }
            }
                return ds;
            
        }

        public static DataTable ExecuteTable(string SQLString, SqlParameter[] Params, CommandType CommandType, string conStr)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conna = new SqlConnection(conStr))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, conna))
                {
                    try
                    {
                        conna.Open();
                        cmd.Parameters.AddRange(Params);
                        cmd.CommandType = CommandType;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(ds);
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        conna.Close();
                        throw e;
                    }
                }
            }
            return ds.Tables[0];
        }

        /// <summary>
        /// 通过适配器填充数据集格式
        /// </summary>
        /// <param name="sSql">要执行的SQL语句</param>
        /// <returns></returns>
        public static DataSet ExecuteSchema(string sSql, string conStr)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conna = new SqlConnection(conStr))
            {
                SqlDataAdapter da = new SqlDataAdapter(sSql, conna);

                try
                {
                    da.FillSchema(ds, SchemaType.Source);
                 }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    //throw;
                }
            }
            return ds;

        }

        public static SqlTransaction BeginTrans(string conStr)
        {
            SqlConnection connection = new SqlConnection(conStr);
            connection.Open();
            return connection.BeginTransaction();
        }

        public static void CommitTransAndCloseConnection(SqlTransaction tr)
        {
            if (tr != null)
            {
                tr.Commit();
                if (tr.Connection != null)
                {
                    if (tr.Connection.State == ConnectionState.Open)
                    {
                        tr.Connection.Close();
                    }
                }
            }
        }

        public static void RollbackAndCloseConnection(SqlTransaction tr)
        {
            if (tr != null)
            {
                tr.Rollback();
                if (tr.Connection != null)
                {
                    if (tr.Connection.State == ConnectionState.Open)
                    {
                        tr.Connection.Close();
                    }
                }
            }
        }

        public static int ExecuteSqlWithTrans(string SQLString, SqlTransaction tr)
        {

            using (SqlCommand cmd = new SqlCommand(SQLString, tr.Connection))
            {
                try
                {
                    cmd.Transaction = tr;
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (System.Data.SqlClient.SqlException e)
                {
                    throw e;
                }
            }
        }


        public static int ExecuteSqlWithTrans(string SQLString, SqlParameter[] Params, CommandType CommandType, SqlTransaction tr)
        {
            using (SqlCommand cmd = new SqlCommand(SQLString, tr.Connection))
            {
                try
                {
                    cmd.Transaction = tr;
                    //if (Params != null)
                    //{
                    //    foreach (SqlParameter parameter in Params)
                            cmd.Parameters.AddRange(Params);
                    //}

                    cmd.CommandType = CommandType;
                    int rows = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear(); //For parameters reuse
                    return rows;
                }
                catch (System.Data.SqlClient.SqlException e)
                {
                    throw e;
                }
            }
        }



        public static object GetSingle(string SQLString,string conStr)
        {
            using (SqlConnection connection = new SqlConnection(conStr))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }


        public static object GetSingle(string SQLString, SqlParameter[] Paras, string conStr)
        {
            using (SqlConnection connection = new SqlConnection(conStr))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        cmd.Parameters.AddRange(Paras);
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }




        public static object GetSingleWithTrans(string SQLString, SqlParameter[] Params, SqlTransaction tr)
        {
            using (SqlCommand cmd = new SqlCommand(SQLString, tr.Connection))
            {
                try
                {
                    cmd.Parameters.AddRange(Params);
                    cmd.Transaction = tr;
                    object obj = cmd.ExecuteScalar();
                    if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                    {
                        return null;
                    }
                    else
                    {
                        return obj;
                    }
                }
                catch (System.Data.SqlClient.SqlException e)
                {
                    throw e;
                }
            }
        }

        public static object GetSingleWithTrans(string SQLString, SqlTransaction tr)
        {
            using (SqlCommand cmd = new SqlCommand(SQLString, tr.Connection))
            {
                try
                {
                   
                    cmd.Transaction = tr;
                    object obj = cmd.ExecuteScalar();
                    if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                    {
                        return null;
                    }
                    else
                    {
                        return obj;
                    }
                }
                catch (System.Data.SqlClient.SqlException e)
                {
                    throw e;
                }
            }
        }

        public static DataSet QueryWithTrans(string SQLString, SqlTransaction tr)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand(SQLString, tr.Connection, tr);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(ds, "ds");
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            return ds;
        }

        public static void WriteError(string cMenu, string cError, string cSql)
        {
            string sql = @"insert into zdy_log([cmenu]
           ,[cerror]
           ,[ddate]
           ,[ip]
           ,[cusername],sql) values(@cmenu,@cerror,getdate(),@ip,@cusername,@sql)";
            DbHelper2.ExecuteNonQuery(sql, new SqlParameter[]{
                       new SqlParameter("@cmenu",cMenu),
                       new SqlParameter("@cerror",cError),
                       new SqlParameter("@ip",""),
                       new SqlParameter("@cusername",""),
                       new SqlParameter("@sql",cSql)
            }, CommandType.Text,canshu.conStr );



        }
    }

    }

