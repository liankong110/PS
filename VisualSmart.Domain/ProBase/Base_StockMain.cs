using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.Domain.Base;

namespace VisualSmart.Domain.ProBase
{
    /// <summary>
    /// Pro_ShipPlanMain:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Base_StockMain : Entity
    {
        public Base_StockMain()
        { }
        #region Model        
        public string ProNo { get; set; }

        #endregion Model

    }
}
