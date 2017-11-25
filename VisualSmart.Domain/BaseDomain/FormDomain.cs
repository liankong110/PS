using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.Domain.Base;

namespace VisualSmart.Domain.SetUp
{
    [Serializable]
    public partial class FormDomain : Entity
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        public virtual int MenuId { get; set; }
        /// <summary>
        /// 窗口名称
        /// </summary>
        public virtual string FormName { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public virtual string Path { get; set; }
        /// <summary>
        /// 窗口排序
        /// </summary>
        public virtual int FormIndex { get; set; }
        /// <summary>
        /// 窗口图片地址
        /// </summary>
        public virtual string FormImgURL { get; set; }
    }
}
