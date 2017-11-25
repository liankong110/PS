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
    public class WeChatQRCodeBizService : EntityBizService<WeChatQRCodeDomain, WeChatQRCodeDao>, IWeChatQRCodeBizService
    {
        /// <summary>
        ///  检查二维码表中是否存在商户号，如果存在 检查 支付信息中是否存在该商户信息，如果没有则返回景区名称
        ///  -1 没有生成二维码创建记录 异常订单
        ///  -2 已经存在支付信息
        ///  其他 景区名称
        /// </summary>
        /// <param name="out_trade_no"></param>
        /// <returns></returns>

        public WeChatQRCodeDomain IsExistOut_trade_no(string out_trade_no, int Total_amount, string APP_ID)
        {
            return EntityDao.IsExistOut_trade_no(out_trade_no, Total_amount, APP_ID);
        }

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
        public int CheckApplayRefund(int id)
        {
            return EntityDao.CheckApplayRefund(id);
        }

        
    }
}
