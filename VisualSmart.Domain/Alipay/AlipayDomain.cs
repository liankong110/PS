using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.Domain.Base;

namespace VisualSmart.Domain.Alipay
{
    [Serializable]
    public class AlipayDomain : Entity
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string AppName { get; set; }
        /// <summary>
        /// 开放平台应用的APPID
        /// </summary>
        public string APP_ID { get; set; }
        /// <summary>
        /// 支付宝公钥
        /// </summary>		
        public string ALIPAY_PUBLIC_KEY { get; set; }

        /// <summary>
        /// 应用私钥
        /// </summary>		
        public string APP_PRIVATE_KEY { get; set; }

        /// <summary>
        /// 应用公钥
        /// </summary>		
        public string APP_PUBLIC_KEY { get; set; }
        /// <summary>
        /// PID
        /// </summary>
        public string PID { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
 
    }
}
