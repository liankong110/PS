using System.Collections.Generic;
using VisualSmart.BizService.Base;
using VisualSmart.Domain.ProBase;
using VisualSmart.Util;

namespace VisualSmart.BizService.ProBase
{
    public interface IBase_ProductionLinesBizService : IEntityBizService<Base_ProductionLines>
    {
        /// <summary>
        /// 根据产线 和 商品获取对应的产能信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<Base_ProductionLines> GetAllDomainByLineNoAndGoodNos(QueryCondition query);
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        bool DeleteByMainId(int MainId);
        /// <summary>
        /// 根据生产计划 产线 和 商品获取对应的产能信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<Base_ProductionLines> GetAllDomainByScheduing(QueryCondition query);
    }
}
