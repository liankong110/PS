using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.Domain.Base;

namespace VisualSmart.Domain.SceneryOrder
{
    [Serializable]
    public class SceneryOrderRefundDomain : Entity
    {
        /// <summary>
        /// 景区TCID
        /// </summary>
        public int SceneryTCId { get; set; }
        /// <summary>
        /// 景区名称
        /// </summary>
        public string SceneryName { get; set; }
        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNumber { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string SerialId { get; set; }
        /// <summary>
        /// 游玩时间
        /// </summary>
        public DateTime PlayDate { get; set; }
        /// <summary>
        /// 退款金额
        /// </summary>
        public decimal Total { get; set; }
        /// <summary>
        /// 审批状态 0 未审核 1已经审核 2作废
        /// </summary>
        public int ApprovalStatus { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public string  PlayDateFrom { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string PlayDateTo { get; set; }
        /// <summary>
        /// 费率
        /// </summary>
        public decimal SceneryRate { get; set; }
        public string ApprovalStatusString { get {
            if (ApprovalStatus == 0)
            {
                return "未审核";
            }
            if (ApprovalStatus == 1)
            {
                return "已审核";
            }            
            return "取消";        
        } }

        /// <summary>
        /// 支付类型 用于判断退款时 调用 支付宝 或者 微信
        /// 0 
        /// </summary>
        public int PayType { get; set; }
    }
}
