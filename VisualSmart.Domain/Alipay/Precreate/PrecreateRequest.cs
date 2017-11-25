using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSmart.Domain.Alipay.Precreate
{
    /// <summary>
    /// 支付宝预创建订单请求实体
    /// </summary>
    public class PrecreateRequest
    {
        /// <summary>
        /// 商家订单号
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>
        /// 订单总金额，单位为元，精确到小数点后两位
        /// </summary>
        public string total_amount { get; set; }
        /// <summary>
        /// 可打折金额. 参与优惠计算的金额，单位为元
        /// </summary>
        public string discountable_amount { get; set; }
        /// <summary>
        /// 订单标题
        /// </summary>
        public string subject { get; set; }
        /// <summary>
        /// 对交易或商品的描述
        /// </summary>
        public string body { get; set; }
        /// <summary>
        /// 商户操作员编号
        /// </summary>
        public string operator_id { get; set; }
        /// <summary>
        /// 商户门店编号
        /// </summary>
        public string store_id { get; set; }
        /// <summary>
        /// 商户机具终端编号
        /// </summary>
        public string terminal_id { get; set; }
        /// <summary>
        /// 该笔订单允许的最晚付款时间，逾期将关闭交易。
        /// 取值范围：1m～15d。m-分钟，h-小时，d-天，1c-当天（1c-当天的情况下，无论交易何时创建，都在0点关闭）。
        /// 该参数数值不接受小数点， 如 1.5h，可转换为 90m。
        /// </summary>
        public string timeout_express { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 商品明细
        /// </summary>
        public List<GoodsDetailRequest> goods_detail = new List<GoodsDetailRequest>();
        /// <summary>
        /// 系统商编号 该参数作为系统商返佣数据提取的依据，请填写系统商签约协议的PID 
        /// </summary>
        public Extend_paramsRequest extend_params = new Extend_paramsRequest();
    }
}
