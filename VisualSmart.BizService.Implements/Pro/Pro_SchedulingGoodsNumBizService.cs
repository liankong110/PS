using System;
using System.Collections.Generic;
using System.Data;
using VisualSmart.BizService.Implements.Base;
using VisualSmart.BizService.Pro;
using VisualSmart.Dao.DataQuickStart.Pro;
using VisualSmart.Domain.Pro;
using VisualSmart.Util;


namespace VisualSmart.BizService.Implements.Pro
{
    public class Pro_SchedulingGoodsNumBizService : EntityBizService<Pro_SchedulingGoodsNum, Pro_SchedulingGoodsNumDao>, IPro_SchedulingGoodsNumBizService
    {
        public IList<Pro_SchedulingGoodsNum> GetDetailList(QueryCondition query)
        {
            return EntityDao.GetDetailList(query);
        }

        public bool BatchAdd(DataTable dt)
        {
            return EntityDao.BatchAdd(dt);
        }
    }
}
