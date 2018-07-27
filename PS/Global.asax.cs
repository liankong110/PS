using System;
using System.Web.Mvc;
using System.Web.Routing;
using VisualSmart.BizService.Core;
using VisualSmart.Util;

namespace PS
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static MailHelper mailHelp = new MailHelper();

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            LogHelper.SetConfig();
            Smart.Init();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            if (exception == null)
                return;

            LogHelper.WriteLog("全局异常捕获", exception);

            //MailMessage message = new MailMessage();
            //message.Subject = "自助支付平台";

            //message.Body = "URL:" + Request.Url + "\r\n 时间：" + DateTime.Now + "\r\n " + exception.ToString();
            //message.To = new string[] { "fj3174@ly.com" };
            //mailHelp.Send(message);
            //Server.ClearError();
        }
    }
}
