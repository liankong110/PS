using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.Domain.Alipay;
using VisualSmart.Domain.Alipay.Precreate;
using VisualSmart.Domain.Alipay.Query;
using VisualSmart.Domain.SetUp;

namespace VisualSmart.BizService.Alipay
{
    /// <summary>
    /// 支付宝核心功能公共类
    /// </summary>
    public interface IAlipayTradeBizService
    {
        /// <summary>
        /// 预创建交易
        /// </summary>
        /// <param name="request"></param>
        /// <param name="sceneryDomain"></param>
        /// <returns></returns>
        PrecreateResponse Precreate(PrecreateRequest request, SceneryDomain sceneryDomain);
         /// <summary>
         /// 查询交易情况
         /// </summary>
         /// <param name="request"></param>
         /// <param name="sceneryDomain"></param>
         /// <returns></returns>
        QueryResponse Query(QueryRequest request, SceneryDomain sceneryDomain);
        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="alipayId"></param>
        /// <param name="sceneryDomain"></param>
        /// <returns></returns>
        int Refund(int alipayId, UserDomain CurrentUser);
        /// <summary>
        /// 分销系统订单申请退款
        /// </summary>
        /// <param name="alipayId"></param>
        /// <param name="sceneryDomain"></param>
        /// <returns>0 成功 1系统已经存在信息 2退款异常</returns>
        string SceneryOrderRefund(int alipayId, UserDomain CurrentUser);

    }
}
