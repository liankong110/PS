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
    public class Base_ProductionLinesBizService : EntityBizService<Base_ProductionLines, Base_ProductionLinesDao>, IBase_ProductionLinesBizService
    {
        /// <summary>
        /// 根据产线 和 商品获取对应的产能信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<Base_ProductionLines> GetAllDomainByLineNoAndGoodNos(QueryCondition query)
        {
            return EntityDao.GetAllDomainByLineNoAndGoodNos(query);
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool DeleteByMainId(int MainId)
        {
            return EntityDao.DeleteByMainId(MainId);
        }
    }
}
