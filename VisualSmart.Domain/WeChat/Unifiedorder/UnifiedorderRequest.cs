using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSmart.Domain.WeChat.Unifiedorder
{
    public class UnifiedorderRequest
    {
        /// <summary>
        /// 商品简单描述
        /// </summary>
        public string body { get; set; }
        /// <summary>
        /// 附加数据，在查询API和支付通知中原样返回，可作为自定义参数使用
        /// </summary>
        public string attach { get; set; }
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>
        /// 订单总金额 （分）
        /// </summary>
        public decimal total_fee { get; set; }

        /// <summary>
        /// 商品ID，多个个组合时 用逗号分开 如：1,2,3
        /// </summary>
        public string productId { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }
    }
}
