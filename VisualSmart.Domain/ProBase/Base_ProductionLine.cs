using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.Domain.Base;

namespace VisualSmart.Domain.ProBase
{
    /// <summary>
    /// Base_ProductionLine:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Base_ProductionLine : Entity
    {
        public Base_ProductionLine()
        { }
        #region Model
        private int _id;
        private string _prolineno = "";
        private string _goodno = "";
        private string _goodname = "";
        private int _pcs = 0;
        private int _standpers = 0;
        private int _minpronum = 0;
        private int _boxnum = 0;
        private int _linemins = 0;
        private int _proshift = 0;
        private string _procapacitydesc = "";
      
        /// <summary>
        /// 
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 生产线
        /// </summary>
        public string ProLineNo
        {
            set { _prolineno = value; }
            get { return _prolineno; }
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
        /// 标配产能（PCS/H)
        /// </summary>
        public int PCS
        {
            set { _pcs = value; }
            get { return _pcs; }
        }
        /// <summary>
        /// 标配人员
        /// </summary>
        public int StandPers
        {
            set { _standpers = value; }
            get { return _standpers; }
        }
        /// <summary>
        /// 最小经济批量
        /// </summary>
        public int MinProNum
        {
            set { _minpronum = value; }
            get { return _minpronum; }
        }
        /// <summary>
        /// 整托包装数
        /// </summary>
        public int BoxNum
        {
            set { _boxnum = value; }
            get { return _boxnum; }
        }
        /// <summary>
        /// 换线时间（Min）
        /// </summary>
        public int LineMins
        {
            set { _linemins = value; }
            get { return _linemins; }
        }
        /// <summary>
        /// 生产班次
        /// </summary>
        public int ProShift
        {
            set { _proshift = value; }
            get { return _proshift; }
        }
        /// <summary>
        /// 人员配置及每小时产出
        /// </summary>
        public string ProCapacityDesc
        {
            set { _procapacitydesc = value; }
            get { return _procapacitydesc; }
        }
        
        
        #endregion Model

    }
}
