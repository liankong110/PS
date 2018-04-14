using System.Collections.Generic;
using VisualSmart.BizService.Base;
using VisualSmart.Domain.Pro;
using VisualSmart.Util;

namespace VisualSmart.BizService.Pro
{
    public interface IPro_ShipPlansBizService : IEntityBizService<Pro_ShipPlans>
    {
        IList<Pro_ShipPlans> GetAllDomainByLineNos(QueryCondition query);
        /// <summary>
        /// 获取主表中所有的子表明细
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        IList<Pro_ShipPlans> GetAllDomainByMainId(int Id);
    }
}
