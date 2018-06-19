using System.Collections;
using System.Collections.Generic;
using VisualSmart.BizService.Base;
using VisualSmart.Domain.Pro;
using VisualSmart.Util;

namespace VisualSmart.BizService.Pro
{
    public interface IPro_ShipPlanMainBizService : IEntityBizService<Pro_ShipPlanMain>
    {
        /// <summary>
        /// 新增 返回ID
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int AddGetId(Pro_ShipPlanMain entity);

        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<Pro_ShipPlanMain> GetList(QueryCondition query, Hashtable hs);
    }
}
