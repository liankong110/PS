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
    public class Base_GoodsBizService : EntityBizService<Base_Goods, Base_GoodsDao>, IBase_GoodsBizService
    {
    }
}
