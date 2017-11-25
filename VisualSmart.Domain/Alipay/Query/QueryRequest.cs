using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSmart.Domain.Alipay.Query
{
    public class QueryRequest
    {         
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string Out_trade_no { get; set; }

        /// <summary>
        /// 是否是最有一次请求 0 否 1 是
        /// </summary>
        public int IsLastRequest { get; set; }
    }
}
