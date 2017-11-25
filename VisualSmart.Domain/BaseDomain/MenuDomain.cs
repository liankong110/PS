using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.Domain.Base;

namespace VisualSmart.Domain.SetUp
{
    [Serializable]
    public partial class MenuDomain:Entity
    {
        /// <summary>
        /// 显示名称
        /// </summary>
        public virtual string DisplayName { get; set; }
        /// <summary>
        /// 排序值
        /// </summary>
        public virtual int MenuIndex { get; set; }
    }
}
