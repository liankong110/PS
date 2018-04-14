using VisualSmart.BizService.Base;
using VisualSmart.Domain.Pro;

namespace VisualSmart.BizService.Pro
{
    public interface IPro_ShipPlanMainBizService : IEntityBizService<Pro_ShipPlanMain>
    {
        /// <summary>
        /// 新增 返回ID
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int AddGetId(Pro_ShipPlanMain entity);
    }
}
