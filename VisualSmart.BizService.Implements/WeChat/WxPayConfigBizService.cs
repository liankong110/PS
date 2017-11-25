using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.BizService.Implements.Base;
using VisualSmart.BizService.WeChat;
using VisualSmart.Dao.DataQuickStart.WeChat;
using VisualSmart.Domain.WeChat;
using VisualSmart.Util;

namespace VisualSmart.BizService.Implements.WeChat
{
    public class WxPayConfigBizService : EntityBizService<WxPayConfigDomain, WxPayConfigDao>, IWxPayConfigBizService
    {
        /// <summary>
        /// 静态缓存
        /// </summary>
        public static IList<WxPayConfigDomain> wxPayConfigList; 

        /// <summary>
        /// 清空静态信息 重新填充
        /// </summary>
        public void ReLoadWxPayList(string APPID)
        {
            wxPayConfigList = EntityDao.GetAllDomain(QueryCondition.Instance.AddEqual("RowState", "1").AddEqual("APPID", APPID));
        }
    }
}
