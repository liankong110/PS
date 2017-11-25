using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.Domain.Base;

namespace VisualSmart.Domain.SetUp
{
    public partial class Role_FunctionDomain : Entity
    {
        /// <summary>
        /// 窗体ID
        /// </summary>
        public virtual int FormId { get; set; }
        /// <summary>
        /// 方法ID
        /// </summary>
        public virtual int FunctionId { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        public virtual int RoleId { get; set; }
    }
}
