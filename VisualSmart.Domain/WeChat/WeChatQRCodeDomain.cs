using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.Domain.Base;

namespace VisualSmart.Domain.WeChat
{
    public class WeChatQRCodeDomain:Entity
    {
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string Out_trade_no { get; set; }
        /// <summary>
        /// 景区名称
        /// </summary>
        public string SceneryName { get; set; }
        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNumber { get; set; }
        /// <summary>
        /// 请求JSON
        /// </summary>
        public string RequestJson { get; set; }
        /// <summary>
        /// URL
        /// </summary>
        public string QrCode { get; set; }
        /// <summary>
        /// 返回信息 正确还是错误的信息
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 支付宝APPID
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 费率
        /// </summary>
        public decimal SceneryRate { get; set; }

    }
}
