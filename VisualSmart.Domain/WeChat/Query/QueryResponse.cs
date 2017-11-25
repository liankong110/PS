using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSmart.Domain.WeChat.Query
{
    public class QueryResponse
    {  
        /// <summary>
        /// 微信交易号
        /// </summary>
        public string transaction_id { get; set; }

        public DateTime time_end { get; set; }
    }
}
