using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.Domain.Base;
namespace VisualSmart.Domain.Pro
{
    /// <summary>
    /// Pro_Scheduling:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Pro_Scheduling : Entity
    {
        public Pro_Scheduling()
        { }
        #region Model
        
        private string _prono = "";
        private DateTime _planfromdate;
        private DateTime _plantodate;
     
        
        /// <summary>
        /// 表单单号
        /// </summary>
        public string ProNo
        {
            set { _prono = value; }
            get { return _prono; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime PlanFromDate
        {
            set { _planfromdate = value; }
            get { return _planfromdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime PlanToDate
        {
            set { _plantodate = value; }
            get { return _plantodate; }
        }
        /// <summary>
        /// 发运计划单号
        /// </summary>
        public string ShipMainProNo { get; set; }
        #endregion Model

    }
}
