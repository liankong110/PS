using VisualSmart.BizService.Implements.Base;
using VisualSmart.BizService.Pro;
using VisualSmart.Dao.DataQuickStart.Pro;
using VisualSmart.Domain.Pro;

namespace VisualSmart.BizService.Implements.Pro
{
    public class Pro_ShipPlanMainBizService : EntityBizService<Pro_ShipPlanMain, Pro_ShipPlanMainDao>, IPro_ShipPlanMainBizService
    {
        /// <summary>
        /// 新增 返回ID
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int AddGetId(Pro_ShipPlanMain entity)
        {
            return EntityDao.AddGetId(entity);
        }
    }
}
