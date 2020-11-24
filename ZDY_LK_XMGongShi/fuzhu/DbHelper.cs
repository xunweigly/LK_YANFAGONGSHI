using System;
using System.Collections.Generic;

using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using LKU8.shoukuan;

namespace fuzhu
{
    class DbHelper
    {
        public static readonly string conStr = canshu.conStr;
          
          /// <summary>
        /// 数据库连接对象
        /// </summary>
        //private static SqlConnection conn = new SqlConnection(conStr);

        /// <summary>
        /// 打开数据库连接
        /// </summary>
        public static void Open(SqlConnection conna)
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
        public static object ExecuteScalar(string sSql)
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

        public static object GetSingle(string SQLString, SqlParameter[] Paras)
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
        /// <summary>
        /// 执行Command对象的ExecuteNonQuery方法
        /// </summary>
        /// <param name="sSql">要执行的SQL语句</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string sSql)
        {
            int iResult = 0;
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
        public static int ExecuteNonQuery(string SQLString, SqlParameter[] Paras)
        {
            using (SqlConnection connection = new SqlConnection(conStr))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        cmd.Parameters.AddRange(Paras);
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
        public static int ExecuteNonQuery(string SQLString, SqlParameter[] Params, CommandType CommandType)
        {
            using (SqlConnection connection = new SqlConnection(conStr))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        cmd.Parameters.AddRange(Params);
                        cmd.CommandType = CommandType;
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
        /// 执行Command对象的ExecuteNonQuery方法
        /// </summary>
        /// <param name="sSql">要执行的SQL语句</param>
        /// <returns></returns>
        public static bool ExecuteNonQuery(List<string> sSql)
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

        /// <summary>
        /// 执行Command对象的ExecuteReader方法
        /// </summary>
        /// <param name="sSql">要执行的SQL语句</param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(string sSql)
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
        public static DataSet Execute(string sSql)
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

        /// <summary>
        /// 通过适配器填充数据集
        /// </summary>
        /// <param name="sSql">要执行的SQL语句</param>
        /// <returns></returns>
        public static DataTable ExecuteTable(string sSql)
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
            return ds.Tables[0];

        }

        /// <summary>
        /// 通过适配器填充数据集格式
        /// </summary>
        /// <param name="sSql">要执行的SQL语句</param>
        /// <returns></returns>
        public static DataSet ExecuteSchema(string sSql)
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


        public static SqlTransaction BeginTrans()
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


        public static Object ToDbValue(object value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            else
            {
                return value;
            }
        }

        public static int ExecuteSqlWithTrans(string SQLString, SqlParameter[] Params, SqlTransaction tr)
        {
            using (SqlCommand cmd = new SqlCommand(SQLString, tr.Connection))
            {
                try
                {
                    cmd.Parameters.AddRange(Params);
                    cmd.Transaction = tr;
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

        public static int ExecuteSqlWithTrans(string SQLString, SqlParameter[] Params, CommandType CommandType, SqlTransaction tr)
        {
            using (SqlCommand cmd = new SqlCommand(SQLString, tr.Connection))
            {
                try
                {
                    cmd.Transaction = tr;
                    cmd.Parameters.AddRange(Params);
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
        /// <summary>
        /// Especially for UFIDA U8 Series
        /// </summary>
        /// <param name="RemoteId"></param>
        /// <param name="cAccount"></param>
        /// <param name="cVouchType"></param>
        /// <param name="iAmount"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static int[] GetIDOnTransaction(string RemoteId, string cAccount, string cVouchType, int iAmount, SqlTransaction tran)
        {
            //string cAcc_Id = cAccount.Substring(7, 3);
            SqlCommand myCommand = new SqlCommand("sp_GetId", tran.Connection, tran);
            myCommand.CommandType = CommandType.StoredProcedure;

            //添加存储过程输入输出参数类型及输入参数值
            myCommand.Parameters.Add("@RemoteId", SqlDbType.VarChar, 2).Value = RemoteId;
            myCommand.Parameters.Add("@cAcc_Id", SqlDbType.VarChar, 3).Value = cAccount; //U8帐套号
            myCommand.Parameters.Add("@cVouchType", SqlDbType.VarChar, 50).Value = cVouchType;
            myCommand.Parameters.Add("@iAmount", SqlDbType.Int).Value = iAmount;        //需要处理明细 记录数
            myCommand.Parameters.Add("@iFatherId", SqlDbType.Int);
            myCommand.Parameters.Add("@iChildId", SqlDbType.Int);
            //输入参数
            myCommand.Parameters["@RemoteId"].DbType = DbType.String;
            myCommand.Parameters["@RemoteId"].Direction = ParameterDirection.Input;
            myCommand.Parameters["@cAcc_Id"].DbType = DbType.String;
            myCommand.Parameters["@cAcc_Id"].Direction = ParameterDirection.Input;
            myCommand.Parameters["@cVouchType"].DbType = DbType.String;
            myCommand.Parameters["@cVouchType"].Direction = ParameterDirection.Input;
            myCommand.Parameters["@iAmount"].DbType = DbType.Int32;
            myCommand.Parameters["@iAmount"].Direction = ParameterDirection.Input;
            //输出参数
            myCommand.Parameters["@iFatherId"].DbType = DbType.Int32;
            myCommand.Parameters["@iFatherId"].Direction = ParameterDirection.Output;
            myCommand.Parameters["@iChildId"].DbType = DbType.Int32;
            myCommand.Parameters["@iChildId"].Direction = ParameterDirection.Output;

            //执行存储过程 此处类似于查询语句
            myCommand.ExecuteScalar();

            //接受执行存储过程后的返回值
            int FatherId = (int)myCommand.Parameters["@iFatherId"].Value;   //主表ID
            int ChildId = (int)myCommand.Parameters["@iChildId"].Value;     //子表ID

            int[] getID = new int[2];
            getID[0] = FatherId;
            getID[1] = ChildId;
            return getID;
        }


        public static string GetDbString(object obj)
        {
            if (obj == DBNull.Value || obj == null)
                return null;
            //return DBNull.Value;
            else if (string.IsNullOrEmpty(obj.ToString()))
                return null;
            else
                return obj.ToString();
        }



        public static int GetDbInt(object obj)
        {
            if (obj == null)
                return 0;
            //return DBNull.Value;
            else if (obj == DBNull.Value)
                return 0;
            else
                return Convert.ToInt32(obj);
        }

        /// <summary>
        /// 把数据库读出的空，转换成数值0
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static decimal GetDbdecimal(object obj)
        {
            if (obj == DBNull.Value)
                return 0.00m;
            //return DBNull.Value;
            else
                return Convert.ToDecimal(obj);
        }
        public static DateTime GetDbDate(object obj)
        {
            if (obj == null)
                return Convert.ToDateTime("1900-01-01");
            //return DBNull.Value;
            else if (obj == DBNull.Value)
                return Convert.ToDateTime("1900-01-01");
            else
                return Convert.ToDateTime(obj);
        }

        /// <summary>
        /// 执行DataTable中的查询返回新的DataTable
        /// </summary>
        /// <param name="dt">源数据DataTable</param>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        private DataTable GetNewDataTable(DataTable dt, string condition)
        {
            DataTable newdt = new DataTable();
            newdt = dt.Clone();
            DataRow[] dr = dt.Select(condition);
            for (int i = 0; i < dr.Length; i++)
            {
                newdt.ImportRow((DataRow)dr[i]);
            }
            return newdt;//返回的查询结果
        }


        public static DataSet Query(string SQLString, SqlParameter[] Params, CommandType cmdType)
        {
            using (SqlConnection connection = new SqlConnection(conStr))
            {
                DataSet ds = new DataSet();
                connection.Open();
                SqlCommand command = new SqlCommand(SQLString, connection);
                command.CommandTimeout = 300000;
                command.CommandType = cmdType;
                command.Parameters.AddRange(Params);
                SqlDataAdapter adp = new SqlDataAdapter(command);
                adp.Fill(ds);
                return ds;
            }
        }
    }
    }

