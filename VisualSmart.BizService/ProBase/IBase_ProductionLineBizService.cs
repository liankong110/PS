using System.Collections.Generic;
using VisualSmart.BizService.Base;
using VisualSmart.Domain.ProBase;
using VisualSmart.Util;

namespace VisualSmart.BizService.ProBase
{
    public interface IBase_ProductionLineBizService : IEntityBizService<Base_ProductionLine>
    {
        /// <summary>
        /// 获取所有产线信息
        /// </summary>
        /// <returns></returns>
        IList<string> GetAllProLineNos();

        /// <summary>
        /// 获取总数量
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        int GetId(QueryCondition query);
    }
}
