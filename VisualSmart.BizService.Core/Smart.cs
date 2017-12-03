using Spring.Context;
using Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.BizService.Alipay;
using VisualSmart.BizService.Pro;
using VisualSmart.BizService.ProBase;
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

        /// <summary>
        /// 发运计划主表
        /// </summary>
        public IPro_ShipPlanBizService Pro_ShipPlanBizService { get; set; }
     
        /// <summary>
        /// 发运计划子表
        /// </summary>
        public IPro_ShipPlansBizService Pro_ShipPlansBizService { get; set; }

        /// <summary>
        /// 发运计划主表
        /// </summary>
        public IPro_ShipPlanMainBizService Pro_ShipPlanMainBizService { get; set; }

        /// <summary>
        /// 零件信息
        /// </summary>
        public IBase_GoodsBizService Base_GoodsBizService { get; set; }
        /// <summary>
        /// 生产产能主表
        /// </summary>
        public IBase_ProductionLineBizService Base_ProductionLineBizService { get; set; }
        /// <summary>
        /// 生产产能子表
        /// </summary>
        public IBase_ProductionLinesBizService Base_ProductionLinesBizService { get; set; }
        /// <summary>
        /// 发运信息
        /// </summary>
        public IBase_ShipBizService Base_ShipBizService { get; set; }
        /// <summary>
        /// 库存信息
        /// </summary>
        public IBase_StockBizService Base_StockBizService { get; set; }
        /// <summary>
        /// 生产计划-1 主表纪录单号信息
        /// </summary>
        public IPro_SchedulingBizService Pro_SchedulingBizService { get; set; }
        /// <summary>
        /// 生产计划-2 次表 纪录所有产线的信息
        /// </summary>
        public IPro_SchedulingLineBizService Pro_SchedulingLineBizService { get; set; }
        /// <summary>
        /// 生产计划-3 次表 纪录所有产品的信息
        /// </summary>
        public IPro_SchedulingGoodsBizService Pro_SchedulingGoodsBizService { get; set; }
        /// <summary>
        /// 生产计划-4 次表 纪录所有产品日期数量信息（需求早中晚数量）
        /// </summary>
        public IPro_SchedulingGoodsNumBizService Pro_SchedulingGoodsNumBizService { get; set; }

        public static void Init()
        {
            ctx = ContextRegistry.GetContext();
            ctx.ConfigureObject(Instance, "Smart");
            
        }
    }
}
