using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.BizService.Base;
using VisualSmart.Domain.SetUp;

namespace VisualSmart.BizService.SetUp
{
    public interface IRole_FormBizService : IEntityBizService<Role_FormDomain>
    {  
        /// <summary>
        /// 保存 权限
        /// </summary>
        /// <param name="roleId">角色</param>
        /// <param name="allFormids">所有展开的窗口</param>
        /// <param name="selectedFormIds">所有选中的窗口</param>
        /// <param name="selectedFunctionIds">所有选中的窗口方法</param>
        /// <param name="userName">用户名称</param>
        /// <returns></returns>
        bool SaveRole_FormAndFunctioin(int roleId, string allFormids, string selectedFormIds, string selectedFunctionIds, string userName);

          /// <summary>
        /// 对于'不能编辑' 来说 如果返回为false 说明 不能编辑  反正 可以
        /// 对于'查看所有' 来说 如果返回为false 说明 不能查看所有  反正 能查看所有
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <param name="displayName"></param>
        /// <param name="textName"></param>
        /// <returns></returns>
        bool GetUserRight(int currentUserId, string displayName, string textName);
    }
}
