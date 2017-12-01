using System;
using System.Collections.Generic;
using VisualSmart.BizService.Implements.Base;
using VisualSmart.BizService.Pro;
using VisualSmart.Dao.DataQuickStart.Pro;
using VisualSmart.Domain.Pro;
using VisualSmart.Util;

namespace VisualSmart.BizService.Implements.Pro
{
    public class Pro_ShipPlanBizService : EntityBizService<Pro_ShipPlan, Pro_ShipPlanDao>, IPro_ShipPlanBizService
    {
        public IList<Pro_ShipPlan> GetAllDomainByLineNos(QueryCondition query)
        {
            return EntityDao.GetAllDomainByLineNos(query);
        }
    }
}
