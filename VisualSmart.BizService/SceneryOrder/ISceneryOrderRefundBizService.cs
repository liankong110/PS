using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.BizService.Base;
using VisualSmart.Domain.Alipay;
using VisualSmart.Domain.SceneryOrder;
using VisualSmart.Domain.SetUp;
using VisualSmart.Util;

namespace VisualSmart.BizService.SceneryOrder
{
    public interface ISceneryOrderRefundBizService : IEntityBizService<SceneryOrderRefundDomain>
    {
       /// <summary>
        /// 将审批信息取消
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        bool CancelRefund(string ids, UserDomain CurrentUser);

         /// <summary>
        /// 退款申请 检查
        /// 检查数据是否存在
        /// 检查是否存在原始支付数据
        /// 0 可以插入
        /// 1 已经存在申请信息
        /// 2 没有找到原始支付数据
        /// -1 系统异常
        /// </summary>
        /// <param name="SceneryTCId"></param>
        /// <param name="SceneryName"></param>
        /// <param name="BatchNumber"></param>
        /// <param name="SerialId"></param>
        ///  <param name="SerialId"></param>
        /// <returns></returns>
        int RequestRefund(string SceneryTCId, string SceneryName, string BatchNumber, string SerialId,int PayType);

          /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<SceneryOrderRefundDomain> GetAllDomain(QueryCondition query, Hashtable hsWhere);
        
    }
}
