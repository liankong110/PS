using Spring.Transaction.Interceptor;
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
    public class Role_FormBizService : EntityBizService<Role_FormDomain, Role_FormDao>, IRole_FormBizService
    {
        [Transaction(ReadOnly = false)]
        public bool SaveRole_FormAndFunctioin(int roleId, string allMenuids, string selectedFormIds, string selectedFunctionIds, string userName)
        {
            if (string.IsNullOrEmpty(allMenuids))
            {
                return true;
            }
            return EntityDao.SaveRole_FormAndFunctioin(roleId, allMenuids, selectedFormIds, selectedFunctionIds, userName);
        }

          /// <summary>
        /// 对于'不能编辑' 来说 如果返回为false 说明 不能编辑  反正 可以
        /// 对于'查看所有' 来说 如果返回为false 说明 不能查看所有  反正 能查看所有
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <param name="displayName"></param>
        /// <param name="textName"></param>
        /// <returns></returns>
        public bool GetUserRight(int currentUserId, string displayName, string textName)
        {
            return EntityDao.GetUserRight(currentUserId,displayName,textName);
        }
    }
}
