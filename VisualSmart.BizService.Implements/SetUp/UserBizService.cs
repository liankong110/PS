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
    public class UserBizService : EntityBizService<UserDomain, UserDao>, IUserBizService
    {

        public bool Add_Update_User_Role(UserDomain entity, string roleIds, string userName)
        {
            return EntityDao.Add_Update_User_Role(entity, roleIds,userName);
        }

          /// <summary>
        /// 密码修改
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="loginPwd"></param>
        /// <returns></returns>
        public bool ChangePassword(int Id, string loginPwd)
        {
            return EntityDao.ChangePassword(Id, loginPwd);
        }
    }
}
