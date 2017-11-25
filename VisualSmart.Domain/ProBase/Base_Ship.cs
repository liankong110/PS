using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.Domain.Base;

namespace VisualSmart.Domain.ProBase
{
    /// <summary>
    /// Base_Ship:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Base_Ship : Entity
    {
        public Base_Ship()
        { }
        #region Model
        private int _id;
        private string _shipto = "";
        private string _shiptoname = "";
        private string _cityno = "";
        private string _city = "";
        private int _shiptohour = 0;
        private int _shiptomins = 0;
        private decimal _leadtime = 0M;
     
        /// <summary>
        /// 
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
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
        /// 目的地编号
        /// </summary>
        public string CityNo
        {
            set { _cityno = value; }
            get { return _cityno; }
        }
        /// <summary>
        /// 目的地
        /// </summary>
        public string City
        {
            set { _city = value; }
            get { return _city; }
        }
        /// <summary>
        /// 发运窗口时间-小时
        /// </summary>
        public int ShipToHour
        {
            set { _shiptohour = value; }
            get { return _shiptohour; }
        }
        /// <summary>
        /// 发运窗口时间-分钟
        /// </summary>
        public int ShipToMins
        {
            set { _shiptomins = value; }
            get { return _shiptomins; }
        }
        /// <summary>
        /// 提前期/H
        /// </summary>
        public decimal LeadTime
        {
            set { _leadtime = value; }
            get { return _leadtime; }
        }
        
        #endregion Model

    }
}
