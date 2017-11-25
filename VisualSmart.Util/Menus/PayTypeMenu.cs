using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSmart.Util.Menus
{
    public enum PayTypeMenu
    {
        /// <summary>
        /// 支付宝
        /// </summary>
        [Description("支付宝")]
        Alipay=0,
        /// <summary>
        /// 微信
        /// </summary>
        [Description("微信")]
        WeChat=1
    }
}
