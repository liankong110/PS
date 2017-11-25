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
    public interface ISceneryBizService : IEntityBizService<SceneryDomain>
    {
       /// <summary>
        /// 获取单个景区信息
        /// </summary>
        /// <param name="AccountId">账户ID</param>
        /// <param name="SceneryTCId">同程景区ID</param>
        /// <returns></returns>
        SceneryDomain GetDomain(string AccountId, string SceneryTCId);
          /// <summary>
        /// 获取所有景区信息
        /// </summary>      
        /// <returns></returns>
        List<SceneryDomain> GetStaticDomain();
    }
}
