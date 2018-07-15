using System;
using VisualSmart.Domain.Base;

namespace VisualSmart.Domain.Pro
{
    /// <summary>
    /// Pro_PSDetail:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Pro_PSDetail : Entity
    {
        public Pro_PSDetail()
        { }
        #region Model
        public int MainId { get; set; }
      
        private string _goodno = "";
        private string _goodname = "";
        private string _shipto = "";
        private string _shiptoname = "";
        private int _packnum = 0;
        private int _qty = 0;
        private float _proorderindex = 0;
        private int _stype = 0;
        private DateTime _starttime = DateTime.Now;
        private DateTime _endtime = DateTime.Now;

        /// <summary>
        /// 产品编码
        /// </summary>
        public string GoodNo
        {
            set { _goodno = value; }
            get { return _goodno; }
        }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string GoodName
        {
            set { _goodname = value; }
            get { return _goodname; }
        }
        /// <summary>
        /// Ship-To
        /// </summary>
        public string ShipTo
        {
            set { _shipto = value; }
            get { return _shipto; }
        }
        /// <summary>
        /// Ship-To Name
        /// </summary>
        public string ShipToName
        {
            set { _shiptoname = value; }
            get { return _shiptoname; }
        }
        /// <summary>
        /// 整箱包装数
        /// </summary>
        public int PackNum
        {
            set { _packnum = value; }
            get { return _packnum; }
        }
        /// <summary>
        /// 排产数量
        /// </summary>
        public int Qty
        {
            set { _qty = value; }
            get { return _qty; }
        }
        /// <summary>
        /// 顺序
        /// </summary>
        public float ProOrderIndex
        {
            set { _proorderindex = value; }
            get { return _proorderindex; }
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
        /// 开始生产时间
        /// </summary>
        public DateTime StartTime
        {
            set { _starttime = value; }
            get { return _starttime; }
        }
        /// <summary>
        /// 结束生产时间
        /// </summary>
        public DateTime EndTime
        {
            set { _endtime = value; }
            get { return _endtime; }
        }
        #endregion Model
        /// <summary>
        /// 产能
        /// </summary>
        public int ChanNeng { get; set; }

        public string STypeString { get; set; }

        public string StartTimeString { get { return StartTime.ToString("yyyy-MM-dd HH:mm:ss"); } }
        public string EndTimeString { get { return EndTime.ToString("yyyy-MM-dd HH:mm:ss"); } }


        public string ProLineNo { get; set; }
        public DateTime ProDate { get; set; }

    }
}
