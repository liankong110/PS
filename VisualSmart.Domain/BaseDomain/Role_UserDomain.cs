using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.Domain.Base;

namespace VisualSmart.Domain.SetUp
{
    public partial class Role_UserDomain : Entity
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public int Role_Id { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int User_Id { get; set; }
    }
}
