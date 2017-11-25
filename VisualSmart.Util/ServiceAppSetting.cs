using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSmart.Util
{ /// <summary>
    /// 系统配置信息，全局静态变量和常量定义
    /// </summary>
    public class ServiceAppSetting
    { 
        /// <summary>
        /// 日志句柄
        /// </summary>
        /// <param name="strLogMessage"></param>
        /// <param name="type"></param>
        public delegate void LoggerHanderDelegate(string strLogMessage, string type);

        /// <summary>
        /// 日志句柄
        /// </summary>
        /// <param name="strLogMessage"></param>
        /// <param name="type"></param>
        public delegate void LoggerExceptionDelegate(Exception strLogMessage, string type);

        /// <summary>
        /// 具体执行方法
        /// </summary>
        //public static LoggerHanderDelegate LoggerHander;

        /// <summary>
        /// 具体执行方法
        /// </summary>
        //public static LoggerExceptionDelegate LoggerException;


    }
}
