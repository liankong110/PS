using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PS.Models
{
    public class SchedulingDateNumModel
    {
        public DateTime date { get; set; }

        /// <summary>
        /// 需求
        /// </summary>
        public int? RequestNum { get; set; }
        /// <summary>
        /// 早班数量
        /// </summary>

        public int? MorningNum { get; set; }
        /// <summary>
        /// 中班数量
        /// </summary>
        public int? MiddleNum { get; set; }
        /// <summary>
        /// 数量 晚班数量
        /// </summary>
        public int? EveningNum { get; set; }
        /// <summary>
        /// 需求差异
        /// </summary>
        public int? RequestDiffNum
        {
            get
            {
                if (RequestNum == null)
                {
                    return TopGongRequestDiffNum;
                }

                return (TopGongRequestDiffNum ?? 0) - (RequestNum ?? 0);
            }
 
        }
        /// <summary>
        /// 供需求差异
        /// </summary>
        public int? GongRequestDiffNum
        {
            get
            {
                if (MorningNum == null && MiddleNum == null && EveningNum == null)
                {
                    return RequestDiffNum;
                }
                return (MorningNum ?? 0) + (MiddleNum ?? 0) + (EveningNum ?? 0) + RequestDiffNum ?? 0;
            }

        }

        public int? TopGongRequestDiffNum { get; set; }

    }
}