using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSmart.Domain.Base
{
    /// <summary>
    /// 实体抽象类
    /// </summary>
    [Serializable]
    public abstract class Entity : IEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public virtual string Creater { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public virtual DateTime UpdateTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public virtual string Updater { get; set; }
        /// <summary>
        /// 行状态 1 正常 0 删除
        /// </summary>
        public virtual byte RowState { get; set; }

        /// <summary>
        /// 审核状态2 待审核  1 通过
        /// </summary>
        public int AuditStatus { get; set; }

        /// <summary>
        /// 审核状态（显示）2 待审核  1 通过
        /// </summary>
        public string AuditStatusString
        {
            get
            {
                return AuditStatus == 2 ? "待审核" : "已审核";
            }
        }
    }
}
