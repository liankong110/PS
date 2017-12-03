using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSmart.Util.Menus
{
    /// <summary>
    /// 1 需求，2 早班 ，3 中班， 4 晚班
    /// </summary>
    public enum ClassType
    {
        /// <summary>
        /// 需求
        /// </summary>
        [Description("需求")]
        Requet = 1,
        /// <summary>
        /// 早班
        /// </summary>
        [Description("早班")]
        Morning = 2,
        /// <summary>
        /// 中班
        /// </summary>
        [Description("中班")]
        Middle = 3,
        /// <summary>
        /// 晚班
        /// </summary>
        [Description("晚班")]
        Evening = 4,

    }
}
