using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.Domain.Base;

namespace VisualSmart.Domain.Pro
{
    /// <summary>
    /// Pro_ShipPlan:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Pro_ShipPlan : Entity
    {
        public Pro_ShipPlan()
        { }
        #region Model
        
        private int _mainid = 0;
        private string _scheduleno = "";
        private int _term = 0;
        private string _editionno = "";
        private string _cityno;
        private string _shipto = "";
        private string _shiptoname = "";
        private string _goodno = "";
        private string _goodname = "";

       
        /// <summary>
        /// 
        /// </summary>
        public int MainId
        {
            set { _mainid = value; }
            get { return _mainid; }
        }
        /// <summary>
        /// 日程
        /// </summary>
        public string ScheduleNo
        {
            set { _scheduleno = value; }
            get { return _scheduleno; }
        }
        /// <summary>
        /// 项
        /// </summary>
        public int Term
        {
            set { _term = value; }
            get { return _term; }
        }
        /// <summary>
        /// 版本ID
        /// </summary>
        public string EditionNo
        {
            set { _editionno = value; }
            get { return _editionno; }
        }
        /// <summary>
        /// 城市编码
        /// </summary>
        public string CityNo
        {
            set { _cityno = value; }
            get { return _cityno; }
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

        #endregion Model

        /// <summary>
        /// 生产线
        /// </summary>
        public string ProLineNo{ get; set; }

        /// <summary>
        /// 库存信息
        /// </summary>
        public int Qty { get; set; }

        public string ShipDetailNo { get; set; }


        /// <summary>
        /// 产线ID
        /// </summary>
        public int SLineId { get; set; }
        /// <summary>
        /// 早班 产能
        /// </summary>
        public int MorningNum { get; set; }
        /// <summary>
        /// 中班 产能
        /// </summary>
        public int MiddleNum { get; set; }
        /// <summary>
        /// 晚班 产能
        /// </summary>
        public int EveningNum { get; set; }


        /// <summary>
        /// 父节点- 商品编码
        /// </summary>
        public string ParentGoodNo { get; set; }

        /// <summary>
        /// 父节点-商品名称
        /// </summary>
        public string ParentGoodName { get; set; }

        /// <summary>
        /// 整托包装数
        /// </summary>
        public int? ShipPkgQty { get; set; }
        public int BiLi { get; set; }
        /// <summary>
        /// 配套产品-右侧产品
        /// </summary>
        public string RightGoodNo { get; set; }
    }
}
