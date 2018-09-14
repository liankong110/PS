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


        /// <summary>
        /// 获取当前Bom 是否有下级商品信息 需要排产
        /// -1 没有数据 其他就是新增的发运计划单ID 可以跳转
        /// </summary>
        /// <param name="query"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        int CreateNextBomList(QueryCondition query, string name);
    }
}
