using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using VisualSmart.BizService.Core;
using VisualSmart.Util;

namespace VisualSmart
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {/// <summary>
        /// 日志句柄
        /// </summary>
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        /// <summary>
        /// 日志记录
        /// </summary>
        /// <param name="strLogMsg"></param>
        /// <param name="type"></param>
        private static void WriteLog(string strLogMsg, string type)
        {
            switch (type)
            {
                case "Error":
                    log.Error(strLogMsg);
                    break;
                default:
                    log.Info(strLogMsg);
                    break;
            }

        }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // 在应用程序启动时运行的代码
            ServiceAppSetting.LoggerHander = WriteLog;
            Smart.Init();
        }
    }
}