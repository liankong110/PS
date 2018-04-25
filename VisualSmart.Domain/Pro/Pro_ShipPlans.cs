using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.Domain.Base;

namespace VisualSmart.Domain.Pro
{
    /// <summary>
    /// Pro_ShipPlans:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Pro_ShipPlans : Entity
    {
        public Pro_ShipPlans()
        {
            ///默认值=1 需求
            SType = 1;
        }
        #region Model
      
        private int _planid = 0;
        private DateTime _plandate;
        private int _plannum = 0;
       
        /// <summary>
        /// 
        /// </summary>
        public int PlanId
        {
            set { _planid = value; }
            get { return _planid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime PlanDate
        {
            set { _plandate = value; }
            get { return _plandate; }
        }
        /// <summary>
        /// 需求
        /// </summary>
        public int PlanNum
        {
            set { _plannum = value; }
            get { return _plannum; }
        }

        #endregion Model

        
        private int _stype = 1;
        /// <summary>
        /// 1 需求，2 早班 ，3 中班， 4 晚班
        /// </summary>
        public int SType { get; set; }

    }
}
