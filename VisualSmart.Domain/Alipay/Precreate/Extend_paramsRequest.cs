using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSmart.Domain.Alipay.Precreate
{
    public class Extend_paramsRequest
    {
        /// <summary>
        /// 系统商编号 该参数作为系统商返佣数据提取的依据，请填写系统商签约协议的PID 
        /// </summary>
        public string sys_service_provid { get; set; }
    }
}
