using System;
using System.Collections.Generic;
using VisualSmart.BizService.Implements.Base;
using VisualSmart.BizService.Pro;
using VisualSmart.Dao.DataQuickStart.Pro;
using VisualSmart.Domain.Pro;
using VisualSmart.Util;

namespace VisualSmart.BizService.Implements.Pro
{
    public class Pro_ShipPlansBizService : EntityBizService<Pro_ShipPlans, Pro_ShipPlansDao>, IPro_ShipPlansBizService
    {
        public IList<Pro_ShipPlans> GetAllDomainByLineNos(QueryCondition query)
        {
            return EntityDao.GetAllDomainByLineNos(query);
        }
    }
}
