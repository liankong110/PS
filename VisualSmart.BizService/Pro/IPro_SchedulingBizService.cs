using VisualSmart.BizService.Base;
using VisualSmart.Domain.Pro;

namespace VisualSmart.BizService.Pro
{
    public interface IPro_SchedulingBizService : IEntityBizService<Pro_Scheduling>
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int AddGetId(Pro_Scheduling entity);
    }
}
