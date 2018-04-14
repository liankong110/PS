using System.Collections.Generic;
using System.Data;
using VisualSmart.BizService.Base;
using VisualSmart.Domain.Pro;
using VisualSmart.Util;

namespace VisualSmart.BizService.Pro
{
    public interface IPro_SchedulingGoodsNumBizService : IEntityBizService<Pro_SchedulingGoodsNum>
    {
        IList<Pro_SchedulingGoodsNum> GetDetailList(QueryCondition query);
        bool BatchAdd(DataTable dt);
    }
}
