using System.Collections.Generic;
using VisualSmart.BizService.Base;
using VisualSmart.Domain.Pro;
using VisualSmart.Util;

namespace VisualSmart.BizService.Pro
{
    public interface IPro_ShipPlanBizService : IEntityBizService<Pro_ShipPlan>
    {
        IList<Pro_ShipPlan> GetAllDomainByLineNos(QueryCondition query);
        /// <summary>
        /// 新增获取ID
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int AddGetId(Pro_ShipPlan entity);

        /// <summary>
        /// 修改时 使用 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        IList<Pro_ShipPlan> GetPro_SchedulingByEdit(int Id);
    }
}
