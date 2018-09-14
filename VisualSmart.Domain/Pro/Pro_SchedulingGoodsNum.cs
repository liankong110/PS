using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.Domain.Base;
namespace VisualSmart.Domain.Pro
{
    /// <summary>
    /// Pro_SchedulingGoodsNum:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Pro_SchedulingGoodsNum : Entity
    {
        public Pro_SchedulingGoodsNum()
        { }
        #region Model
      
        private int _sgoodid = 0;
        private int _stype = 0;
        private DateTime _sdate;
        private int? _snum;
       
        /// <summary>
        /// 计划产品ID
        /// </summary>
        public int SGoodId
        {
            set { _sgoodid = value; }
            get { return _sgoodid; }
        }
        /// <summary>
        /// 1 需求，2 早班 ，3 中班， 4 晚班
        /// </summary>
        public int SType
        {
            set { _stype = value; }
            get { return _stype; }
        }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime SDate
        {
            set { _sdate = value; }
            get { return _sdate; }
        }
        /// <summary>
        /// 数据
        /// </summary>
        public int? SNum
        {
            set { _snum = value; }
            get { return _snum; }
        }
        #endregion Model

        public int Index { get; set; }
        /// <summary>
        /// 产品编码
        /// </summary>
        public string GoodNo { get; set; }
        /// <summary>
        /// ship-to
        /// </summary>
        public string ShipTo { get; set; }
    }
}
