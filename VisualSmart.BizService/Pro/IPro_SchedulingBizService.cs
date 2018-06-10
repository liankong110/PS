using System.Collections;
using System.Collections.Generic;
using VisualSmart.BizService.Base;
using VisualSmart.Domain.Pro;
using VisualSmart.Util;

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

        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<Pro_Scheduling> GetList(QueryCondition query, Hashtable hs);
    }
}
