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
    public class RoleBizService : EntityBizService<RoleDomain, RoleDao>, IRoleBizService
    {
        public IList<RoleDomain> GetUser_RoleList(int UserId)
        {
            return EntityDao.GetUser_RoleList(UserId);
        }
    }
}
