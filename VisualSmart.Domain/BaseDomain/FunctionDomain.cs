using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.Domain.Base;

namespace VisualSmart.Domain.SetUp
{
    public partial class FunctionDomain : Entity
    { 
        /// <summary>
        /// 窗体ID
        /// </summary>
        public int Form_Id { get; set; }

        /// <summary>
        /// 方法Text
        /// </summary>
        public string FunctionText { get; set; }

        /// <summary>
        /// 方法Name
        /// </summary>
        public string FunctionName { get; set; }
    }
}
