using VisualSmart.BizService.Base;
using VisualSmart.Domain.Pro;

namespace VisualSmart.BizService.Pro
{
    public interface IPro_PSBizService : IEntityBizService<Pro_PS>
    {
        Pro_PS GetPSBySchedulingLineId(int SchedulingLineId);
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int AddGetId(Pro_PS entity);
    }
}
