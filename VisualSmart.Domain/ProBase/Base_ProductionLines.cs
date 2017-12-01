using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.Domain.Base;

namespace VisualSmart.Domain.ProBase
{
    /// <summary>
    /// Base_ProductionLines:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Base_ProductionLines : Entity
    {
        public Base_ProductionLines()
        { }
        #region Model
        private int _id;
        private int _prolineid = 0;
        private int _people = 0;
        private int _number = 0;
        /// <summary>
        /// 
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 主表ID
        /// </summary>
        public int ProLineId
        {
            set { _prolineid = value; }
            get { return _prolineid; }
        }
        /// <summary>
        /// 人数
        /// </summary>
        public int People
        {
            set { _people = value; }
            get { return _people; }
        }
        /// <summary>
        /// 件/H
        /// </summary>
        public int Number
        {
            set { _number = value; }
            get { return _number; }
        }
        #endregion Model

        /// <summary>
        /// 商品名称
        /// </summary>
        public string GoodNo { get; set; }

    }
 
}
