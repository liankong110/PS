using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VisualSmart.Domain.SetUp;

namespace VisualSmart.Bootcss.Models
{
    public class HomeModel
    {  
        /// <summary>
        /// 菜单集合
        /// </summary>
        public IList<MenuDomain> MenuList { get; set; }
        /// <summary>
        /// 窗体集合
        /// </summary>
        public IList<FormDomain> FormList { get; set; }

        /// <summary>
        /// 角色集合
        /// </summary>
        public IList<RoleDomain> RoleList { get; set; }

        /// <summary>
        /// 方法集合
        /// </summary>
        public IList<FunctionDomain> FuntionList { get; set; }
    }
}