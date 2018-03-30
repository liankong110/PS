using System;
using System.Collections.Generic;
using VisualSmart.BizService.Base;
using VisualSmart.Domain.Pro;

namespace VisualSmart.BizService.Pro
{
    public interface IPro_PSDetailBizService : IEntityBizService<Pro_PSDetail>
    {
        IList<Pro_PSDetail> GetPSDetailBySchedulingLineId(Pro_PS model,int SchedulingLineId, DateTime SDate);
    }
}
