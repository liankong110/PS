using System;
using System.Collections;
using System.Collections.Generic;
using VisualSmart.BizService.Base;
using VisualSmart.Domain.Pro;
using VisualSmart.Util;

namespace VisualSmart.BizService.Pro
{
    public interface IPro_PSDetailBizService : IEntityBizService<Pro_PSDetail>
    {
        IList<Pro_PSDetail> GetPSDetailBySchedulingLineId(Pro_PS model, Pro_SchedulingLine fristLine, DateTime SDate);
        /// <summary>
        /// 获取信息列表 导出到Excel
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<Pro_PSDetail> GetAllDomainToExcel(QueryCondition query, Hashtable hs);
    }
}
