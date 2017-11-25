using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.Domain.Base;

namespace VisualSmart.Domain.ProBase
{
    /// <summary>
    /// Base_Stock:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Base_Stock : Entity
    {
        public Base_Stock()
        { }
        #region Model
        private int _id;
        private string _location;
        private string _wh = "";
        private string _goodno = "";
        private decimal? _qty = 0M;
        private string _batch = "";
      
        /// <summary>
        /// 
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 域
        /// </summary>
        public string Location
        {
            set { _location = value; }
            get { return _location; }
        }
        /// <summary>
        /// 库位
        /// </summary>
        public string WH
        {
            set { _wh = value; }
            get { return _wh; }
        }
        /// <summary>
        /// 零件号
        /// </summary>
        public string GoodNo
        {
            set { _goodno = value; }
            get { return _goodno; }
        }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal? Qty
        {
            set { _qty = value; }
            get { return _qty; }
        }
        /// <summary>
        /// 批次
        /// </summary>
        public string Batch
        {
            set { _batch = value; }
            get { return _batch; }
        }
        
        #endregion Model

    }
}
