using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VisualSmart.Domain.API
{
    /// <summary>
    /// 响应
    /// </summary>
    public class Response
    {
        /// <summary>
        /// 响应头部
        /// </summary>
        public ResponseHeader Header { get; set; }

        /// <summary>
        /// 响应内部
        /// </summary>
        public string Body { get; set; }
    }

    /// <summary>
    /// 响应头部
    /// </summary>
    public class ResponseHeader
    {
        /// <summary>
        /// 响应代码
        /// </summary>
        public int RspCode { get; set; }

        /// <summary>
        /// 响应代码描述
        /// </summary>
        public string RspDesc { get; set; }

        /// <summary>
        /// 响应错误信息
        /// </summary>
        public string RspErrorMsg { get; set; }

        /// <summary>
        /// 响应时间
        /// </summary>
        public DateTime RspTime = DateTime.Now;
    }
}