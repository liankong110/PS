using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.Domain.Base;

namespace VisualSmart.Domain.SetUp
{
    public partial class UserDomain : Entity
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public virtual string loginId { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        public virtual string loginPwd { get; set; }
        /// <summary>
        /// 登录状态 1 在线 0 离线
        /// </summary>
        public virtual byte loginState { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 用户状态 1.在职 0 离职
        /// </summary>
        public virtual byte UserState { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public virtual string Phone { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public virtual string Email { get; set; }
        /// <summary>
        /// 性别 1 男 0 女
        /// </summary>
        public virtual bool Sex { get; set; }
        /// <summary>
        /// 职位
        /// </summary>
        public virtual string Position { get; set; }
        /// <summary>
        /// 所属上级
        /// </summary>
        public virtual int Leader { get; set; }
        /// <summary>
        /// 公司
        /// </summary>
        public virtual string Company { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public virtual string Avatar { get; set; }
    }
}
