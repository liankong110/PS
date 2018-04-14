using System.Collections.Generic;
using VisualSmart.BizService.Base;
using VisualSmart.Domain.Pro;
using VisualSmart.Util;

namespace VisualSmart.BizService.Pro
{
    public interface IPro_ShipPlanBizService : IEntityBizService<Pro_ShipPlan>
    {
        IList<Pro_ShipPlan> GetAllDomainByLineNos(QueryCondition query);
        /// <summary>
        /// 新增获取ID
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int AddGetId(Pro_ShipPlan entity);
    }
}
