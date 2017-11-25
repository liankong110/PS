using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSmart.Domain.Alipay.Refund
{

    public class Alipay_trade_refund_response
    {
       public RefundResponse alipay_trade_refund_response = new RefundResponse();
    }
    public class RefundResponse
    {
        /// <summary>
        /// 支付宝交易号
        /// </summary>
        public string trade_no { get; set; }
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>
        /// 退款总金额
        /// </summary>
        public decimal refund_fee { get; set; }
        /// <summary>
        /// 退款支付时间
        /// </summary>
        public DateTime gmt_refund_pay { get; set; }
        /// <summary>
        /// 返回编码
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string msg { get; set; }
    }
}
