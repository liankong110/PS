using System.Collections.Generic;
using VisualSmart.BizService.Base;
using VisualSmart.Domain.Pro;
using VisualSmart.Util;

namespace VisualSmart.BizService.Pro
{
    public interface IPro_ShipPlansBizService : IEntityBizService<Pro_ShipPlans>
    {
        IList<Pro_ShipPlans> GetAllDomainByLineNos(QueryCondition query);
    }
}
