using System.Collections;
using System.Collections.Generic;
using VisualSmart.BizService.Implements.Base;
using VisualSmart.BizService.Pro;
using VisualSmart.Dao.DataQuickStart.Pro;
using VisualSmart.Domain.Pro;
using VisualSmart.Util;

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

        public IList<Pro_ShipPlanMain> GetList(QueryCondition query, Hashtable hs)
        {
            return EntityDao.GetList(query,hs);
        }
    }
}
