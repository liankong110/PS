using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.BizService.Base;
using VisualSmart.Domain.Alipay;
using VisualSmart.Domain.SetUp;

namespace VisualSmart.BizService.Alipay
{
    public interface IAlipayDetailBizService : IEntityBizService<AlipayDetailDomain>
    {
      /// <summary>
        /// 检查是否是否存在
        /// </summary>
        /// <param name="Out_trade_no">商户号</param>
        /// <param name="Total_amount">订单金额</param>
        /// <returns></returns>
        bool IsExistsOut_trade_no(string Out_trade_no, decimal Total_amount,string APP_ID);

         /// <summary>
        /// 支付宝帐号信息是否存在支付记录
        /// </summary>
        /// <param name="alipayId">支付宝帐号ID</param>
        /// <returns></returns>
        bool IsExistsAlipayDetail(int alipayId);

        /// <summary>
        /// 景区信息是否存在支付记录
        /// </summary>
        /// <param name="SceneryName">景区名称</param>
        /// <returns></returns>
        bool IsExistsSceneryAlipay(string SceneryName);
    }
}
