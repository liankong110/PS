using VisualSmart.BizService.Implements.Base;
using VisualSmart.BizService.ProBase;
using VisualSmart.Dao.DataQuickStart.ProBase;
using VisualSmart.Domain.ProBase;

namespace VisualSmart.BizService.Implements.ProBase
{
    public class Base_StockMainBizService : EntityBizService<Base_StockMain, Base_StockMainDao>, IBase_StockMainBizService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int AddGetId(Base_StockMain entity)
        {
            return EntityDao.AddGetId(entity);
        }
    }
}
