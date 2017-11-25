using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSmart.Domain.Alipay.Query
{
    public class QueryResponse
    {  /// <summary>
        /// 支付宝交易号
        /// </summary>
        public string trade_no { get; set; }
        /// <summary>
        /// 买家支付宝账号
        /// </summary>
        public string buyer_logon_id { get; set; }
        /// <summary>
        /// 交易支付时间 例如：2014-11-27 15:45:57
        /// </summary>
        public DateTime gmt_payment { get; set; }
       
    }
}
