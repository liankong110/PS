using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.BizService.Implements.Base;
using VisualSmart.BizService.SetUp;
using VisualSmart.Dao.DataQuickStart.SetUp;
using VisualSmart.Domain.SetUp;

namespace VisualSmart.BizService.Implements.SetUp
{
    public class FunctionBizService : EntityBizService<FunctionDomain, FunctionDao>, IFunctionBizService
    {
        public IList<FunctionDomain> GetRoleFunctionList(int roleId)
        {
            return EntityDao.GetRoleFunctionList(roleId);
        }
    }
}
