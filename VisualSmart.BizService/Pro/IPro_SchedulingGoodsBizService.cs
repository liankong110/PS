using System.Collections.Generic;
using VisualSmart.BizService.Base;
using VisualSmart.Domain.Pro;
using VisualSmart.Util;

namespace VisualSmart.BizService.Pro
{
    public interface IPro_SchedulingGoodsBizService : IEntityBizService<Pro_SchedulingGoods>
    {
        IList<Pro_SchedulingGoods> GetDetailList(QueryCondition query);
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int AddGetId(Pro_SchedulingGoods entity);
    }
}
