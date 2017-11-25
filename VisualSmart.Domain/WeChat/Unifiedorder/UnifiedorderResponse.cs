using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSmart.Domain.WeChat.Unifiedorder
{
    public class UnifiedorderResponse
    {
        /// <summary>
        /// 二维码地址
        /// </summary>
        public string QRCode { get; set; }
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string Out_trade_no { get; set; }
    }
}
