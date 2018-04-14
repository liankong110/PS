using System.Collections.Generic;
using VisualSmart.BizService.Implements.Base;
using VisualSmart.BizService.Pro;
using VisualSmart.Dao.DataQuickStart.Pro;
using VisualSmart.Domain.Pro;
using VisualSmart.Util;

namespace VisualSmart.BizService.Implements.Pro
{
    public class Pro_SchedulingGoodsBizService : EntityBizService<Pro_SchedulingGoods, Pro_SchedulingGoodsDao>, IPro_SchedulingGoodsBizService
    {
        public IList<Pro_SchedulingGoods> GetDetailList(QueryCondition query)
        {
            return EntityDao.GetDetailList(query);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int AddGetId(Pro_SchedulingGoods entity)
        {
            return EntityDao.AddGetId(entity);
        }
    }
}
