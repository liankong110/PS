using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.BizService.Alipay;
using VisualSmart.BizService.Implements.Base;
using VisualSmart.Dao.DataQuickStart.Alipay;
using VisualSmart.Domain.Alipay;

namespace VisualSmart.BizService.Implements.Alipay
{
    public class SceneryBizService : EntityBizService<SceneryDomain, SceneryDao>, ISceneryBizService
    {
        /// <summary>
        /// 获取单个景区信息
        /// </summary>
        /// <param name="AccountId">账户ID</param>
        /// <param name="SceneryTCId">同程景区ID</param>
        /// <returns></returns>
        public SceneryDomain GetDomain(string AccountId, string SceneryTCId)
        {
            return EntityDao.GetDomain(AccountId, SceneryTCId);
        }

          /// <summary>
        /// 获取所有景区信息
        /// </summary>      
        /// <returns></returns>
        public List<SceneryDomain> GetStaticDomain()
        {
            return EntityDao.GetStaticDomain();
        }
    }
}
