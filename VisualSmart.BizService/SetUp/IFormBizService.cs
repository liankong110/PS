using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.BizService.Base;
using VisualSmart.Domain.SetUp;

namespace VisualSmart.BizService.SetUp
{
    public interface IFormBizService : IEntityBizService<FormDomain>
    {
        IList<FormDomain> GetRoleFromList(int roleId);

        IList<FormDomain> GetFormByUserId(int UserId);

        /// <summary>
        /// 根据用户访问的地址 判断是否有权限访问
        /// </summary>
        /// <param name="controller">controller</param>
        /// <param name="action">action</param>
        /// <param name="UserId">用户</param>
        /// <returns></returns>
        int GetFormByUserId(string controller, string action, int UserId);
         
    }
}
