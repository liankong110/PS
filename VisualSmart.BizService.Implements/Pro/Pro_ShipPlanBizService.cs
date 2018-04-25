using System;
using System.Collections.Generic;
using VisualSmart.BizService.Implements.Base;
using VisualSmart.BizService.Pro;
using VisualSmart.Dao.DataQuickStart.Pro;
using VisualSmart.Domain.Pro;
using VisualSmart.Util;

namespace VisualSmart.BizService.Implements.Pro
{
    public class Pro_ShipPlanBizService : EntityBizService<Pro_ShipPlan, Pro_ShipPlanDao>, IPro_ShipPlanBizService
    {
        public IList<Pro_ShipPlan> GetAllDomainByLineNos(QueryCondition query)
        {
            return EntityDao.GetAllDomainByLineNos(query);
        }
        /// <summary>
        /// 新增获取ID
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int AddGetId(Pro_ShipPlan entity)
        {
            return EntityDao.AddGetId(entity);
        }

        /// <summary>
        /// 修改时 使用 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public IList<Pro_ShipPlan> GetPro_SchedulingByEdit(int Id)
        {
            return EntityDao.GetPro_SchedulingByEdit(Id);
        }
    }
}
