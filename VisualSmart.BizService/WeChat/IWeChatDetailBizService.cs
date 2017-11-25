using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.BizService.Base;
using VisualSmart.Domain.Alipay;
using VisualSmart.Domain.SetUp;
using VisualSmart.Domain.WeChat;

namespace VisualSmart.BizService.WeChat
{
    public interface IWeChatDetailBizService : IEntityBizService<WeChatDetailDomain>
    {
        /// <summary>
        /// 微信
        /// 检查是否是否存在
        /// </summary>
        /// <param name="Out_trade_no">商户号</param>
        /// <param name="Total_amount">订单金额</param>
        /// <returns></returns>
        bool IsExistsOut_trade_no(string Out_trade_no, int Total_amount, string APP_ID);

        /// <summary>
        /// 微信
        /// 帐号信息是否存在支付记录
        /// </summary>
        /// <param name="alipayId">微信帐号ID</param>
        /// <returns></returns>
        bool IsExistsWeChatDetail(int WeChatId);

        /// <summary>
        /// 微信
        /// 景区信息是否存在支付记录
        /// </summary>
        /// <param name="SceneryName">景区名称</param>
        /// <returns></returns>
        bool IsExistsSceneryWeChat(string SceneryName);
    }
}
