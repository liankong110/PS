using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.BizService.Implements.Base;
using VisualSmart.BizService.ProBase;
using VisualSmart.Dao.DataQuickStart.ProBase;
using VisualSmart.Domain.ProBase;

namespace VisualSmart.BizService.Implements.ProBase
{
    public class Base_BomBizService : EntityBizService<Base_Bom, Base_BomDao>, IBase_BomBizService
    {
        /// <summary>
        /// 删除-所有信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool DeleteAll(int Id)
        {
            return EntityDao.DeleteAll(Id);
        }
    }
}
