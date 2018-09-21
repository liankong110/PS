using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.BizService.Base;
using VisualSmart.Domain.ProBase;
using VisualSmart.Util;

namespace VisualSmart.BizService.ProBase
{
    public interface IBase_GoodsBizService : IEntityBizService<Base_Goods>
    {
        /// <summary>
        /// 获取总数量
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        int GetId(QueryCondition query);

        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<string> GetGoodName(QueryCondition query);

        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        List<Base_Goods> GetBomName(string parentNo, string sonNo);
    }
}
