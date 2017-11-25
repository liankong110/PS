using Spring.Context;
using Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.BizService.Alipay;
using VisualSmart.BizService.SceneryOrder;
using VisualSmart.BizService.SetUp;
using VisualSmart.BizService.WeChat;


namespace VisualSmart.BizService.Core
{
    public class Smart
    {
        public static IApplicationContext ctx;

        public static Smart Instance = new Smart();        

        #region 基础设置模块
        public IUserBizService UserBizService { get; set; }
        public IFormBizService FormBizService { get; set; }
        public IMenuBizService MenuBizService { get; set; }
        public IFunctionBizService FunctionBizService { get; set; }
        public IRole_UserBizService Role_UserBizService { get; set; }
        public IRoleBizService RoleBizService { get; set; }
        public IRole_FunctionBizService Role_FunctionBizService { get; set; }
        public IRole_FormBizService Role_FormBizService { get; set; }


        #endregion

        #region 微信模块
        /// <summary>
        /// 微信帐号信息
        /// </summary>
        public IWxPayConfigBizService WxPayConfigBizService { get; set; }
        /// <summary>
        /// 微信二维码请求信息
        /// </summary>
        public IWeChatQRCodeBizService WeChatQRCodeBizService { get; set; }
        /// <summary>
        /// 微信支付信息
        /// </summary>
        public IWeChatDetailBizService WeChatDetailBizService { get; set; }
        /// <summary>
        /// 微信支付
        /// </summary>
        public INativePay NativePay { get; set; }
        #endregion

        #region 支付宝模块
        /// <summary>
        /// 支付宝基本信息
        /// </summary>
        public IAlipayBizService AlipayBizService { get; set; }
        /// <summary>
        /// 景区支付信息
        /// </summary>
        public ISceneryBizService SceneryBizService { get; set; }
        /// <summary>
        /// 支付宝交易详情
        /// </summary>
        public IAlipayDetailBizService AlipayDetailBizService { get; set; }
        /// <summary>
        /// 支付宝交易日志
        /// </summary>
        public IAlipayQRCodeBizService AlipayQRCodeBizService { get; set; }

        /// <summary>
        /// 支付宝核心类
        /// </summary>
        public IAlipayTradeBizService AlipayTradeBizService { get; set; }
        #endregion

        #region 订单模块
        /// <summary>
        /// 订单退款
        /// </summary>
        public ISceneryOrderRefundBizService SceneryOrderRefundBizService { get; set; }
        #endregion
        public static void Init()
        {
            ctx = ContextRegistry.GetContext();
            ctx.ConfigureObject(Instance, "Smart");
            
        }
    }
}
