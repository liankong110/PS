using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.Domain.Base;

namespace VisualSmart.Domain.SetUp
{
    public partial class Role_FormDomain : Entity
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public int Role_Id { get; set; }

        /// <summary>
        /// Form ID
        /// </summary>
        public int Form_Id { get; set; }
    }
}
