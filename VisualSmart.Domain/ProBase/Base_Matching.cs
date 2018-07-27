using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.Domain.Base;

namespace VisualSmart.Domain.ProBase
{
    [Serializable]
    public class Base_Matching : Entity
    {
        public Base_Matching()
        { }
       
       
        private string _leftGoodNo = "";
        private string _leftGoodName = "";
        private string _rightGoodNo = "";
        private string _rightGoodName = "";       
        
        /// <summary>
        /// 父商品编码
        /// </summary>
        public string LeftGoodNo
        {
            set { _leftGoodNo = value; }
            get { return _leftGoodNo; }
        }
        /// <summary>
        /// 父商品名称
        /// </summary>
        public string LeftGoodName
        {
            set { _leftGoodName = value; }
            get { return _leftGoodName; }
        }
        /// <summary>
        /// 子商品编码
        /// </summary>
        public string RightGoodNo
        {
            set { _rightGoodNo = value; }
            get { return _rightGoodNo; }
        }
        /// <summary>
        /// 子商品名称
        /// </summary>
        public string RightGoodName
        {
            set { _rightGoodName = value; }
            get { return _rightGoodName; }
        }
      
    }
}
