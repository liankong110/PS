﻿using VisualSmart.BizService.Base;
using VisualSmart.Domain.ProBase;
using VisualSmart.Util;

namespace VisualSmart.BizService.ProBase
{
    public interface IBase_MatchingBizService : IEntityBizService<Base_Matching>
    {
        /// <summary>
        /// 删除-所有信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        bool DeleteAll(int Id);

        /// <summary>
        /// 获取id
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        int GetId(QueryCondition query);
    }
}
