using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSmart.Domain.WeChat
{
    /// <summary>
    /// 微信配置账号信息
    /// </summary>
    public class WxPayConfigDomain:Base.Entity
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string WeAppName { get; set; }
        /// <summary>
        /// 公众账号ID.绑定支付的APPID
        /// </summary>
        public string APPID { get; set; }
        /// <summary>
        /// 商户号
        /// </summary>
        public string MCHID { get; set; }
        /// <summary>
        /// 商户支付密钥，参考开户邮件设置（必须配置）
        /// </summary>
        public string PAYKEY { get; set; }
        /// <summary>
        ///  APPSECRET：公众帐号secert（仅JSAPI支付的时候需要配置）
        /// </summary>
        public string APPSECRET { get; set; }
        /// <summary>
        /// 证书路径,注意应该填写绝对路径（仅退款、撤销订单时需要）
        /// </summary>
        public string SSLCERT_PATH { get; set; }
        /// <summary>
        /// 证书密码
        /// </summary>
        public string SSLCERT_PASSWORD { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        //=======【上报信息配置】===================================
        /* 测速上报等级，0.关闭上报; 1.仅错误时上报; 2.全量上报
        */
        public const int REPORT_LEVENL = 1;

        //=======【日志级别】===================================
        /* 日志等级，0.不输出日志；1.只输出错误信息; 2.输出错误和正常信息; 3.输出错误信息、正常信息和调试信息
        */
        public const int LOG_LEVENL = 0;
    }
}
