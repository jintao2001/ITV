using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using System.Configuration;
using System.Net;


namespace ITV.MvcApplication.Models
{

    public class ConfigParam
    {
       
        /// <summary>
        /// 数据库连接
        /// </summary>
        public static string DBConn
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["DBConn"].ToString(); }
        }
       
        /// <summary>
        /// 页面title
        /// </summary>
        public static string WXPageTitle
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["PageTitle"].ToString(); }
        }
        /// <summary>
        /// 页面footer版权
        /// </summary>
        public static string WXPageFooter
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["PageFooter"].ToString(); }
        }
       
    }
}