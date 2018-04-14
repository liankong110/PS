using System;
using System.Collections.Generic;
using VisualSmart.BizService.Implements.Base;
using VisualSmart.BizService.Pro;
using VisualSmart.Dao.DataQuickStart.Pro;
using VisualSmart.Domain.Pro;
using VisualSmart.Util;


namespace VisualSmart.BizService.Implements.Pro
{
    public class Pro_SchedulingLineBizService : EntityBizService<Pro_SchedulingLine, Pro_SchedulingLineDao>, IPro_SchedulingLineBizService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int AddGetId(Pro_SchedulingLine entity)
        {
            return EntityDao.AddGetId(entity);
        }
    }
}
