﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.BizService.Implements.Base;
using VisualSmart.BizService.ProBase;
using VisualSmart.Dao.DataQuickStart.ProBase;
using VisualSmart.Domain.ProBase;
using VisualSmart.Util;

namespace VisualSmart.BizService.Implements.ProBase
{
    public class Base_LineHourBizService : EntityBizService<Base_LineHour, Base_LineHourDao>, IBase_LineHourBizService
    {
        /// <summary>
        /// 删除-所有信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool DeleteAll(int Id)
        {
            return EntityDao.DeleteAll(Id);
        }

        /// <summary>
        /// 获取id
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public int GetId(QueryCondition query)
        {
            return EntityDao.GetId(query);
        }
        /// 获取信息列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<Base_LineHour> GetLineHourList(string proLineNosList)
        {
            return EntityDao.GetLineHourList(proLineNosList);
        }
    }
}
