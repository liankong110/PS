using System.Collections.Generic;
using VisualSmart.BizService.Base;
using VisualSmart.Domain.Pro;
using VisualSmart.Util;

namespace VisualSmart.BizService.Pro
{
    public interface IPro_ShipPlanBizService : IEntityBizService<Pro_ShipPlan>
    {
        IList<Pro_ShipPlan> GetAllDomainByLineNos(QueryCondition query);
    }
}
