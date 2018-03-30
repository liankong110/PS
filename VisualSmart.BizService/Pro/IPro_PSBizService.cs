using VisualSmart.BizService.Base;
using VisualSmart.Domain.Pro;

namespace VisualSmart.BizService.Pro
{
    public interface IPro_PSBizService : IEntityBizService<Pro_PS>
    {
        Pro_PS GetPSBySchedulingLineId(int SchedulingLineId);
    }
}
