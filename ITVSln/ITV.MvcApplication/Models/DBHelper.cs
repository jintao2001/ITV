using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using System.Collections;
using System.IO;

namespace ITV.MvcApplication.Models
{
    public class DBHelper
    {
        #region 构造
        public DBHelper()
        {

        }
        #endregion
        #region sql底层访问方法

        #region  对外方法   Public

        /// <summary>
        /// 执行sql语句, 返回DataSet
        /// </summary>
        /// <param name="sSQL"></param>
        /// <returns></returns>
        public static DataSet QrySQL_DS(string sSQL)
        {

            using (MySqlConnection MyConn = new MySqlConnection(ConfigParam.DBConn))
            {
                using (MySqlDataAdapter adp = new MySqlDataAdapter(sSQL, MyConn))
                {
                    DataSet ResultDs = new DataSet();
                    try
                    {
                        if (MyConn.State != ConnectionState.Open)
                        {
                            MyConn.Open();
                            adp.Fill(ResultDs);
                        }


                    }
                    catch (Exception e)
                    {

                        FunClass.WriteLog("执行SQL语句" + sSQL + "时，出现异常：" + e.Message);
                    }
                    return ResultDs;
                }
            }

        }

        /// <summary>
        ///  执行sql语句, 返回DataSet
        /// </summary>
        /// <param name="sSQL"></param>
        /// <returns></returns>

        public static DataTable QrySQL_DT(string sSQL)
        {
            using (MySqlConnection MyConn = new MySqlConnection(ConfigParam.DBConn))
            {
                using (MySqlDataAdapter adp = new MySqlDataAdapter(sSQL, MyConn))
                {
                    DataTable ResultDt = new DataTable();
                    try
                    {
                        if (MyConn.State != ConnectionState.Open)
                        {
                            MyConn.Open();
                            adp.Fill(ResultDt);
                        }
                    }
                    catch (Exception e)
                    {

                        FunClass.WriteLog("执行SQL语句" + sSQL + "时，出现异常：" + e.Message);
                    }
                    return ResultDt.Copy();
                }
            }

        }
        /// <summary>
        /// 执行 update insert sql
        /// </summary>
        /// <param name="sSQL"></param>
        /// <returns></returns>
        public static bool ExecSQL(string sSQL)
        {

            using (MySqlConnection MyConn = new MySqlConnection(ConfigParam.DBConn))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    bool bResult = false;
                    try
                    {
                        cmd.Connection = MyConn;
                        if (MyConn.State != ConnectionState.Open)
                        {
                            MyConn.Open();
                            cmd.CommandText = sSQL;
                            cmd.ExecuteNonQuery();
                            bResult = true;
                        }
                    }
                    catch (Exception e)
                    {
                        FunClass.WriteLog("执行SQL语句" + sSQL + "时，出现异常：" + e.Message);
                    }

                    return bResult;
                }
            }

        }
        /// <summary>
        /// 批量执行 update insert sql
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool ExecSQL_Bat(ArrayList list)
        {

            using (MySqlConnection MyConn = new MySqlConnection(ConfigParam.DBConn))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    bool bResult = false;
                    MySqlTransaction trans;
                    if (MyConn.State != ConnectionState.Open)
                        MyConn.Open();
                    trans = MyConn.BeginTransaction();
                    try
                    {
                        cmd.Connection = MyConn;
                        cmd.Transaction = trans;
                        for (int i = 0; i < list.Count; i++)
                        {
                            cmd.CommandText = list[i].ToString();
                            cmd.ExecuteNonQuery();
                        }
                        trans.Commit();
                        bResult = true;
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        FunClass.WriteLog("执行SQL语句" + cmd.CommandText.ToString() + "时，出现异常：" + e.Message);

                    }

                    return bResult;
                }
            }

        }
        #endregion

        /// <summary>
        /// 根据sql返回单表结果集(带OracleParameter[]参数 )
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="ParamArray"></param>
        /// <returns></returns>
        public static DataSet QrySQL_DS(string sqlStr, MySqlParameter[] ParamArray)
        {

            using (MySqlConnection MyConn = new MySqlConnection(ConfigParam.DBConn))
            {
                using (MySqlCommand cmd = new MySqlCommand(sqlStr, MyConn))
                {
                    DataSet ds = new DataSet();
                    foreach (MySqlParameter mp in ParamArray)
                    {
                        cmd.Parameters.Add(mp);
                    }
                    MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                    try
                    {
                        if (MyConn.State != ConnectionState.Open)
                        {
                            MyConn.Open();
                        }

                        adp.Fill(ds);
                    }
                    catch (Exception e)
                    {
                        FunClass.WriteLog("执行SQL语句" + sqlStr + "时，出现异常：" + e.Message);
                    }

                    return ds;
                }
            }

        }
        /// <summary>
        /// 执行存储过程返回DataSet
        /// </summary>
        /// <param name="ProName">过程名</param>
        /// <param name="ParamArray">参数列表</param>
        /// <returns></returns>
        public static DataSet QryPro_DS(string ProName, MySqlParameter[] ParamArray)
        {

            using (MySqlConnection MyConn = new MySqlConnection(ConfigParam.DBConn))
            {
                using (MySqlCommand cmd = new MySqlCommand(ProName, MyConn))
                {
                    DataSet ds = new DataSet();
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (MyConn.State != ConnectionState.Open)
                    {
                        MyConn.Open();
                    }
                    foreach (MySqlParameter mp in ParamArray)
                    {
                        cmd.Parameters.Add(mp);
                    }

                    MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                    try
                    {
                        adp.Fill(ds);
                    }
                    catch (Exception e)
                    {

                        FunClass.WriteLog("执行存储过程" + ProName + "时，出现异常：" + e.Message);
                    }

                    return ds;
                }
            }


        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="ProName">过程名</param>
        /// <param name="ParamArray">参数列表</param>
        /// <returns></returns>
        public static int ExecPro(string ProName, MySqlParameter[] ParamArray)
        {
            using (MySqlConnection MyConn = new MySqlConnection(ConfigParam.DBConn))
            {
                using (MySqlCommand cmd = new MySqlCommand(ProName, MyConn))
                {
                    if (MyConn.State != ConnectionState.Open)
                    {
                        MyConn.Open();

                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (MySqlParameter mp in ParamArray)
                    {
                        cmd.Parameters.Add(mp);
                    }
                    int ret = 0;
                    try
                    {
                        ret = cmd.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        FunClass.WriteLog("执行存储过程" + ProName + "时，出现异常：" + e.Message);
                    }

                    return ret;
                }

            }

        }


        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="ProName">过程名</param>
        /// <param name="ParamArray">参数列表</param>
        /// <returns></returns>
        public static int ExecPro_Tran(string ProName, MySqlParameter[] ParamArray)
        {
            using (MySqlConnection MyConn = new MySqlConnection(ConfigParam.DBConn))
            {
                using (MySqlCommand cmd = new MySqlCommand(ProName, MyConn))
                {
                    if (MyConn.State != ConnectionState.Open)
                    {
                        MyConn.Open();
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Transaction = MyConn.BeginTransaction();
                    foreach (MySqlParameter mp in ParamArray)
                    {
                        cmd.Parameters.Add(mp);
                    }
                    int ret = 0;
                    try
                    {
                        ret = cmd.ExecuteNonQuery();
                        cmd.Transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        cmd.Transaction.Rollback();
                        FunClass.WriteLog("执行存储过程" + ProName + "时，出现异常：" + e.Message);
                    }
                    return ret;
                }
            }

        }

        /// <summary>
        /// 特定用于订单提交过程
        /// </summary>
        /// <param name="ProName"></param>
        /// <param name="ParamArray"></param>
        /// <returns></returns>
        public static string ExecPro_Tran_List(string ProName, MySqlParameter[,] ParamArray)
        {
            //用于存储订单号逗号分隔集合
            string RetOrderString = string.Empty;
            //创建链接
            using (MySqlConnection MyConn = new MySqlConnection(ConfigParam.DBConn))
            {
                //打开链接并创建事务
                if (MyConn.State != ConnectionState.Open) { MyConn.Open(); }
                MySqlTransaction MST = MyConn.BeginTransaction();

                //循环执行过程
                for (int i = 0; i < ParamArray.GetLength(0); i++)
                {
                    using (MySqlCommand cmd = new MySqlCommand(ProName, MyConn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Transaction = MST;
                        for (int j = 0; j < ParamArray.GetLength(1); j++) { cmd.Parameters.Add(ParamArray[i, j]); }
                        try
                        {
                            cmd.ExecuteNonQuery();
                            //执行结果处理
                            if (ParamArray[i, 14].Value.ToString() == "1")
                            {
                                //记录订单号
                                RetOrderString = RetOrderString + ParamArray[i, 15].Value.ToString() + ",";
                            }
                            else if (ParamArray[i, 14].Value.ToString() == "-1")
                            {
                                //库存不足回滚退出
                                MST.Rollback();
                                return "Err01|" + RetOrderString + ParamArray[i, 15].Value.ToString();
                            }
                            else
                            {
                                //回滚退出
                                MST.Rollback();
                                return string.Empty;
                            }
                        }
                        catch
                        {
                            //回滚退出
                            MST.Rollback();
                            return string.Empty;
                        }
                    }
                }
                //提交事务
                MST.Commit();
                return RetOrderString.Remove(RetOrderString.Length - 1, 1);
            }
        }
        #endregion
    }
}