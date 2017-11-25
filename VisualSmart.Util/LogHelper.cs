
//*****************************************************************************
//<copyright  company='同程软件有限公司' file='LogHelper.cs'>
//  Copyright (c) 版本 V1
//  所属部门：景区B项目部-系统项目组
//  所属项目：智慧分销
//  作    者：季健国
//◇创建日期：2013年10月11日
//  版本历史：
//      如有新增或修改请再次添加描述(格式：时间+作者+描述)
//</copyright>
//*****************************************************************************
using System;
using System.IO;
using log4net;

namespace VisualSmart.Util
{
    [Serializable]
    public class LogHelper
    {
        //记录基本日志信息
        private static readonly ILog Loginfo = LogManager.GetLogger("loginfo");
        //记录所有报错信息
        private static readonly ILog Error = LogManager.GetLogger("Error");
        //记录支付宝支付记录
        private static readonly ILog Alipay = LogManager.GetLogger("Alipay");
        //记录支付宝异步调用记录
        private static readonly ILog AlipayError = LogManager.GetLogger("AlipayError");

        //记录微信支付记录
        private static readonly ILog WeChat = LogManager.GetLogger("WeChat");
        //记录微信异步调用记录
        private static readonly ILog WeChatError = LogManager.GetLogger("WeChatError");     
        

        public static void SetConfig()
        {
            var file = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "\\Config\\log4net.cfg.xml");
            log4net.Config.XmlConfigurator.Configure(file);
        }
 
        /// <summary>
        /// 写入基本信息
        /// </summary>
        /// <param name="info"></param>
        public static void WriteLog(string info)
        {
            if (Loginfo.IsInfoEnabled)
            {
                Loginfo.Info(info);
            }
        }
        /// <summary>
        /// 写入报错信息
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ex"></param>
        public static void WriteLog(string info, Exception ex)
        {
            if (Error.IsErrorEnabled)
            {
                Error.Error(info, ex);
            }
        }
        /// <summary>
        /// 支付宝信息写入
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ex"></param>
        public static void AlipayLog(string info)
        {
            if (Alipay.IsErrorEnabled)
            {
                Alipay.Info(info);
            }
        }
        /// <summary>
        /// 支付宝异常订单信息写入
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ex"></param>
        public static void AlipayErrorLog(string info, Exception ex)
        {
            if (AlipayError.IsErrorEnabled)
            {
                AlipayError.Info(info, ex);
            }
        }

        /// <summary>
        /// 微信信息写入
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ex"></param>
        public static void WeChatLog(string info)
        {
            if (WeChat.IsErrorEnabled)
            {
                WeChat.Info(info);
            }
        }
        /// <summary>
        /// 微信异常订单信息写入
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ex"></param>
        public static void WeChatErrorLog(string info, Exception ex)
        {
            if (WeChatError.IsErrorEnabled)
            {
                WeChatError.Info(info, ex);
            }
        }
    }
}
