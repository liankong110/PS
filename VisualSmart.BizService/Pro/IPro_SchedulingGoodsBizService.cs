using System.Collections.Generic;
using VisualSmart.BizService.Base;
using VisualSmart.Domain.Pro;
using VisualSmart.Util;

namespace VisualSmart.BizService.Pro
{
    public interface IPro_SchedulingGoodsBizService : IEntityBizService<Pro_SchedulingGoods>
    {
        IList<Pro_SchedulingGoods> GetDetailList(QueryCondition query);
    }
}
