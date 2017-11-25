using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.BizService.Base;
using VisualSmart.Domain.Alipay;
using VisualSmart.Domain.SetUp;

namespace VisualSmart.BizService.Alipay
{
    public interface IAlipayQRCodeBizService : IEntityBizService<AlipayQRCodeDomain>
    {
        /// <summary>
        ///  检查二维码表中是否存在商户号，如果存在 检查 支付信息中是否存在该商户信息，如果没有则返回景区名称
        ///  -1 没有生成二维码创建记录 异常订单
        ///  -2 已经存在支付信息
        ///  其他 景区名称
        /// </summary>
        /// <param name="out_trade_no"></param>
        /// <returns></returns>

        AlipayQRCodeDomain IsExistOut_trade_no(string out_trade_no, decimal Total_amount, string APP_ID);

        /// <summary>
        /// 检查是否可以提出申请功能
        /// -1--已经存在申请记录
        /// -2 --没有原始支付记录
        /// -3--已经存在退款记录	 
        /// 0--验证通过
        /// -10;//系统异常
        /// </summary>
        /// <param name="id">QRCode id</param>
        /// <returns></returns>
        int CheckApplayRefund(int id);
    }
}
