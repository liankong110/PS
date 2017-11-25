using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.Domain.Base;
using VisualSmart.Domain.WeChat;

namespace VisualSmart.Domain.Alipay
{
    [Serializable]
    public class SceneryDomain : Entity
    {
        /// <summary>
        /// 景区名称
        /// </summary>
        public string SceneryName { get; set; }
        /// <summary>
        /// 同程景区ID
        /// </summary>
        public int SceneryTCId { get; set; }
        /// <summary>
        /// 支付宝帐号ID
        /// </summary>		
        public int AlipayId { get; set; }
        /// <summary>
        /// 支付宝名称
        /// </summary>
        public string AlipayName { get; set; }
        /// <summary>
        /// 微信ID
        /// </summary>		
        public int WeChatId { get; set; }

         /// <summary>
        /// 微信名称
        /// </summary>		
        public string WeChatName { get; set; }
        

        /// <summary>
        /// 分销系统AccountId
        /// </summary>
        public string AccountId { get; set; }

        /// <summary>
        /// 分销系统AccountPWD
        /// </summary>
        public string AccountPwd { get; set; }

        /// <summary>
        /// 支付宝费率
        /// </summary>
        public decimal Rate { get; set; }
        /// <summary>
        /// 微信费率
        /// </summary>
        public decimal WeChatRate { get; set; }
        /// <summary>
        /// 支付宝信息
        /// </summary>
        public AlipayDomain Apipay = new AlipayDomain();

        /// <summary>
        /// 微信详细信息
        /// </summary>
        public WxPayConfigDomain WeChat = new WxPayConfigDomain();
    }
}
