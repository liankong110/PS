using System;
using VisualSmart.Domain.Base;

namespace VisualSmart.Domain.Pro
{
    /// <summary>
    /// Pro_PS:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Pro_PS : Entity
    {
        public Pro_PS()
        { }
       
       
        private string _prono = "";
        private DateTime _prodate;
        private int? _finalmorningnum = 0;
        private int? _finalmiddlenum;
        private int? _finaleveningnum;
     
     
        /// <summary>
        /// 单号
        /// </summary>
        public string ProNo
        {
            set { _prono = value; }
            get { return _prono; }
        }
        /// <summary>
        /// 生产日期
        /// </summary>
        public DateTime ProDate
        {
            set { _prodate = value; }
            get { return _prodate; }
        }
        /// <summary>
        /// 早班产能
        /// </summary>
        public int? FinalMorningNum
        {
            set { _finalmorningnum = value; }
            get { return _finalmorningnum; }
        }
        /// <summary>
        /// 中班产能
        /// </summary>
        public int? FinalMiddleNum
        {
            set { _finalmiddlenum = value; }
            get { return _finalmiddlenum; }
        }
        /// <summary>
        /// 晚班产能
        /// </summary>
        public int? FinalEveningNum
        {
            set { _finaleveningnum = value; }
            get { return _finaleveningnum; }
        }

        /// <summary>
        /// 产线信息
        /// </summary>
        public string ProLineNo { get; set; }
        /// <summary>
        /// 生产排产 产线ID
        /// </summary>
        public int LineId { get; set; }

        public string SchedulingProNo { get; set; }
    }
}
