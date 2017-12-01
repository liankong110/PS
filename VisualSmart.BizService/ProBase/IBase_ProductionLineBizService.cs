using System.Collections.Generic;
using VisualSmart.BizService.Base;
using VisualSmart.Domain.ProBase;

namespace VisualSmart.BizService.ProBase
{
    public interface IBase_ProductionLineBizService : IEntityBizService<Base_ProductionLine>
    {
        /// <summary>
        /// 获取所有产线信息
        /// </summary>
        /// <returns></returns>
        IList<string> GetAllProLineNos();
    }
}
