using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
namespace ITV.MvcApplication.Models
{
    public class FunClass
    {
        /// <summary>
        /// DataTable转Json格式数据
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string CreatJson(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("{\"Rows\":[");
                foreach (DataRow dr in dt.Rows)
                {
                    sb.Append("{");
                    foreach (DataColumn dc in dt.Columns)
                    {
                        sb.Append("\"" + dc + "\":\"" + dr[dc.ColumnName].ToString() + "\",");
                    }
                    sb.Replace(",", "}", sb.ToString().LastIndexOf(','), 1);
                    sb.Append(",");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("]}");
                return sb.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// DataSet 转换Xml字符串
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static string DsToXmlStr(DataSet ds)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<NewDataSet>");
            foreach (DataTable dt in ds.Tables)
            {
                if (dt.Rows.Count > 0)
                {

                    foreach (DataRow dr in dt.Rows)
                    {
                        sb.Append("<Table>");
                        foreach (DataColumn dc in dt.Columns)
                        {
                            sb.Append("<" + dc.ColumnName + ">");
                            sb.Append(dr[dc.ColumnName].ToString());
                            sb.Append("</" + dc.ColumnName + ">");
                        }
                        sb.Append("</Table>");
                    }

                }
            }
            sb.Append("</NewDataSet>");
            return sb.ToString();
        }
        public static void WriteLog(string ThisLog)
        {
            //日志处理

            try
            {
                string Path = ConfigParam.LOGPATH;
                string LogName = "MvcAppLog_" + System.DateTime.Now.Day.ToString();
                if (!Directory.Exists(Path))
                {
                    // Create the directory it does not exist.
                    Directory.CreateDirectory(Path);
                }
                if (File.Exists(Path + "\\" + LogName + ".txt"))
                {
                    if ((File.GetLastAccessTime(Path + "\\" + LogName + ".txt").ToString("yyyy-MM-dd") != DateTime.Now.ToString("yyyy-MM-dd")))
                    {
                        File.Delete(Path + "\\" + LogName + ".txt");
                    }
                }

                using (StreamWriter SW = new StreamWriter(Path + "\\" + LogName + ".txt", true))
                {
                    SW.WriteLine("\r\n======" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ## " + ThisLog);
                }
            }
            catch { }

        }


        static private string GetFormatDate(string vFormat)
        {
            string vDate;
            vDate = DateTime.Now.ToString(vFormat);
            return vDate;
        }
    }
}