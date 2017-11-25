using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSmart.Domain.Alipay.Precreate
{
    public class GoodsDetailRequest
    {
        /// <summary>
        /// id
        /// </summary>
        public string goods_id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string goods_name { get; set; }
        /// <summary>
        /// 种类
        /// </summary>
        public string goods_category { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public string price { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public string quantity { get; set; }
    }
}
