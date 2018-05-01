using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.BizService.Implements.Base;
using VisualSmart.BizService.ProBase;
using VisualSmart.Dao.DataQuickStart.ProBase;
using VisualSmart.Domain.ProBase;
using VisualSmart.Util;

namespace VisualSmart.BizService.Implements.ProBase
{
    public class Base_ProductionLineBizService : EntityBizService<Base_ProductionLine, Base_ProductionLineDao>, IBase_ProductionLineBizService
    {
        public IList<string> GetAllProLineNos(int ShipPlanMainId)
        {
            return EntityDao.GetAllProLineNos(ShipPlanMainId);
        }

        /// <summary>
        /// 获取ID
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public int GetId(QueryCondition query)
        {
            return EntityDao.GetId(query);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int AddGetId(Base_ProductionLine entity)
        {
            return EntityDao.AddGetId(entity);
        }
        
    }
}
