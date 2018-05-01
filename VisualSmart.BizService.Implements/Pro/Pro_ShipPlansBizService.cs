using System;
using System.Collections.Generic;
using VisualSmart.BizService.Implements.Base;
using VisualSmart.BizService.Pro;
using VisualSmart.Dao.DataQuickStart.Pro;
using VisualSmart.Domain.Pro;
using VisualSmart.Util;

namespace VisualSmart.BizService.Implements.Pro
{
    public class Pro_ShipPlansBizService : EntityBizService<Pro_ShipPlans, Pro_ShipPlansDao>, IPro_ShipPlansBizService
    {
        public IList<Pro_ShipPlans> GetAllDomainByLineNos(QueryCondition query)
        {
            return EntityDao.GetAllDomainByLineNos(query);
        }
        /// <summary>
        /// 获取主表中所有的子表明细
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public IList<Pro_ShipPlans> GetAllDomainByMainId(int Id, QueryCondition query)
        {
            return EntityDao.GetAllDomainByMainId(Id, query);
        }

        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<Pro_ShipPlans> GetPro_SchedulingGoodsNumByEdit(int Id)
        {
            return EntityDao.GetPro_SchedulingGoodsNumByEdit(Id);
        }
    }
}
