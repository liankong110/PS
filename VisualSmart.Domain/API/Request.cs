using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VisualSmart.Domain.API
{
    /// <summary>
    /// 请求
    /// </summary>
    public class Request : RequestExtend
    {
        /// <summary>
        /// 请求头部
        /// </summary>
        public RequestHeader Header { get; set; }

        /// <summary>
        /// 请求内部
        /// </summary>
        public string Body { get; set; }
    }
    /// <summary>
    /// 请求扩展
    /// </summary>
    public class RequestExtend
    {
        public string RequestUrl { get; set; }
    }
    /// <summary>
    /// 请求头部
    /// </summary>
    public class RequestHeader
    {
        /// <summary>
        /// 接口帐号
        /// </summary>
        public string AccountId { get; set; }

        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 数字签名
        /// </summary>
        public string DigitalSign { get; set; }

        /// <summary>
        /// 请求时间
        /// </summary>
        public DateTime ReqTime { get; set; }

        /// <summary>
        /// 机器号
        /// </summary>
        public string MachineCode { get; set; }

        /// <summary>
        /// 景区ID
        /// </summary>
        public int SceneryId { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 同程景区ID
        /// </summary>
        public string TcSceneryId { get; set; }

    }
}