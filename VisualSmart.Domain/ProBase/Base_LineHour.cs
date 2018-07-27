using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.Domain.Base;

namespace VisualSmart.Domain.ProBase
{
    [Serializable]
    public class Base_LineHour : Entity
    {
        /// <summary>
        /// 产线
        /// </summary>
        public string ProLineNo { get; set; }
        /// <summary>
        /// 最大小时数
        /// </summary>
        public decimal MaxHours { get; set; }
    }
}
