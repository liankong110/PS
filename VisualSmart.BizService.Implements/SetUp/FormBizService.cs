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
    public class FormBizService : EntityBizService<FormDomain, FormDao>, IFormBizService
    {
        public IList<FormDomain> GetRoleFromList(int roleId)
        {
            return EntityDao.GetRoleFromList(roleId);
        }


        public IList<FormDomain> GetFormByUserId(int UserId)
        {
            return EntityDao.GetFormByUserId(UserId);
        }


       /// <summary>
        /// 根据用户访问的地址 判断是否有权限访问
        /// </summary>
        /// <param name="controller">controller</param>
        /// <param name="action">action</param>
        /// <param name="UserId">用户</param>
        /// <returns></returns>
        public int GetFormByUserId(string controller, string action, int UserId)
        {
            return EntityDao.GetFormByUserId(controller, action, UserId);
        }
    }
}
