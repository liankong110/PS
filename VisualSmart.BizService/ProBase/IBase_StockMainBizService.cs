using VisualSmart.BizService.Base;
using VisualSmart.Domain.ProBase;

namespace VisualSmart.BizService.ProBase
{
    public interface IBase_StockMainBizService : IEntityBizService<Base_StockMain>
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int AddGetId(Base_StockMain entity);
    }
}
