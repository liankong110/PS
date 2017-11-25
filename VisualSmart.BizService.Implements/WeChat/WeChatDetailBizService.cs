using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using VisualSmart.BizService.Implements.Base;
using VisualSmart.BizService.WeChat; 
using VisualSmart.Dao.DataQuickStart.WeChat; 
using VisualSmart.Domain.WeChat;

namespace VisualSmart.BizService.Implements.WeChat
{
    public class WeChatDetailBizService : EntityBizService<WeChatDetailDomain, WeChatDetailDao>, IWeChatDetailBizService
    {
     
        /// <summary>
        /// 微信
        /// 检查是否是否存在
        /// </summary>
        /// <param name="Out_trade_no">商户号</param>
        /// <param name="Total_amount">订单金额</param>
        /// <param name="APP_ID">APPID</param>
        /// <returns></returns>
        public bool IsExistsOut_trade_no(string Out_trade_no, int Total_amount, string APP_ID)
        {
            return EntityDao.IsExistsOut_trade_no(Out_trade_no, Total_amount, APP_ID);
        }

         /// <summary>
        /// 微信
        /// 帐号信息是否存在支付记录
        /// </summary>
        /// <param name="alipayId">微信帐号ID</param>
        /// <returns></returns>
        public bool IsExistsWeChatDetail(int alipayId)
        {
            return EntityDao.IsExistsWeChatDetail(alipayId);
        }

         /// <summary>
        /// 微信
        /// 景区信息是否存在支付记录
        /// </summary>
        /// <param name="SceneryName">景区名称</param>
        /// <returns></returns>
        public bool IsExistsSceneryWeChat(string SceneryName)
        {
            return EntityDao.IsExistsSceneryWeChat(SceneryName);
        }
    }
}
