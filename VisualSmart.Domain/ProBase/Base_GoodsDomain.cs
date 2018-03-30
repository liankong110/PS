using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.Domain.Base;

namespace VisualSmart.Domain.ProBase
{
    /// <summary>
	/// Base_Goods:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
    public partial class Base_Goods : Entity
    {
        public Base_Goods()
        { }
        #region Model
      
        private string _goodno = "";
        private string _goodname = "";
        private string _shipto = "";
        private string _shiptoname = "";
        private string _pml = "";
        private string _shippkgqty = "";
        private string _um = "";
        private decimal _standarddays = 0M;      
        
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
        /// P/M/L
        /// </summary>
        public string PML
        {
            set { _pml = value; }
            get { return _pml; }
        }
        /// <summary>
        /// Ship Pkg Qty
        /// </summary>
        public string ShipPkgQty
        {
            set { _shippkgqty = value; }
            get { return _shippkgqty; }
        }
        /// <summary>
        /// UM
        /// </summary>
        public string UM
        {
            set { _um = value; }
            get { return _um; }
        }
        /// <summary>
        /// 库存标准天数
        /// </summary>
        public decimal StandardDays
        {
            set { _standarddays = value; }
            get { return _standarddays; }
        }
       
        #endregion Model

    }
}
