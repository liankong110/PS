using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using VisualSmart.Domain.Alipay;
using VisualSmart.Domain.SetUp;
using VisualSmart.Domain.WeChat.Query;
using VisualSmart.Domain.WeChat.Unifiedorder;

namespace VisualSmart.BizService.WeChat
{
    public interface INativePay
    {
        /// <summary>
        /// 获取微信支付二维码链接
        /// </summary>
        /// <param name="orderRequest"></param>
        /// <param name="sceneryDomain"></param>
        /// <returns></returns>
        UnifiedorderResponse GetPayUrl(UnifiedorderRequest orderRequest, SceneryDomain sceneryDomain);

        /// <summary>
        /// 查询订单支付状态
        /// </summary>
        /// <param name="request"></param>
        /// <param name="sceneryDomain"></param>
        /// <returns></returns>
        QueryResponse OrderQuery(QueryRequest request, SceneryDomain sceneryDomain);

        
        /// <summary>
        /// 回调处理基类
        /// 主要负责接收微信支付后台发送过来的数据，对数据进行签名验证
        /// </summary>
        /// <param name="responseMessage"></param>
        /// <param name="request"></param>      
        void Notify(HttpResponseMessage responseMessage, HttpRequest request);
        /// <summary>
        /// 微信 
        /// 分销系统订单申请退款
        /// </summary>
        /// <param name="WeChatId"></param>
        /// <param name="sceneryDomain"></param>
        /// <returns>0 成功 1系统已经存在信息 2退款异常</returns>
        string SceneryOrderRefund(int WeChatId, UserDomain CurrentUser);
    }
}
