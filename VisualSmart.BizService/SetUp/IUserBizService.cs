using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.BizService.Base;
using VisualSmart.Domain.SetUp;

namespace VisualSmart.BizService.SetUp
{
    public interface IUserBizService : IEntityBizService<UserDomain>
    {
        bool Add_Update_User_Role(UserDomain entity, string roleIds, string userName);
          /// <summary>
        /// 密码修改
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="loginPwd"></param>
        /// <returns></returns>
        bool ChangePassword(int Id, string loginPwd);
    }
}
