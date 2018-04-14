using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.Domain.Base;
namespace VisualSmart.Domain.Pro
{
    /// <summary>
    /// Pro_SchedulingGoods:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Pro_SchedulingGoods : Entity
    {
        public Pro_SchedulingGoods()
        { }
        #region Model
      
        private int _slineid = 0;
        private string _goodno = "";
        private string _goodname = "";
        private string _shipto = "";
        private string _shiptoname = "";
        private decimal _stocknum = 0M;
        private int _packnum = 0;
        private int? _morningnum = 0;
        private int? _middlenum;
        private int? _eveningnum;
       
        /// <summary>
        /// 计划产线id
        /// </summary>
        public int SLineId
        {
            set { _slineid = value; }
            get { return _slineid; }
        }
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
        /// 初期库存
        /// </summary>
        public decimal StockNum
        {
            set { _stocknum = value; }
            get { return _stocknum; }
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
        /// 早班产能
        /// </summary>
        public int? MorningNum
        {
            set { _morningnum = value; }
            get { return _morningnum; }
        }
        /// <summary>
        /// 中班产能
        /// </summary>
        public int? MiddleNum
        {
            set { _middlenum = value; }
            get { return _middlenum; }
        }
        /// <summary>
        /// 晚班产能
        /// </summary>
        public int? EveningNum
        {
            set { _eveningnum = value; }
            get { return _eveningnum; }
        }
        #endregion Model
        /// <summary>
        /// 产线
        /// </summary>
        public string ProLineNo { get; set; }
        /// <summary>
        /// 产线班次
        /// </summary>
        public int SType { get; set; }

        /// <summary>
        /// 生产数量
        /// </summary>
        public int SNum { get; set; }

        public DateTime Date { get; set; }


        public List<Pro_SchedulingGoodsNum> Items { get; set; }
    }
}
