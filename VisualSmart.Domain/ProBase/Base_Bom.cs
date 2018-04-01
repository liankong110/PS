using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.Domain.Base;

namespace VisualSmart.Domain.ProBase
{
    [Serializable]
    public class Base_Bom : Entity
    {
        public Base_Bom()
        { }
       
       
        private string _parentgoodno = "";
        private string _parentgoodname = "";
        private string _songoodno = "";
        private string _songoodname = "";       
        
        /// <summary>
        /// 父商品编码
        /// </summary>
        public string ParentGoodNo
        {
            set { _parentgoodno = value; }
            get { return _parentgoodno; }
        }
        /// <summary>
        /// 父商品名称
        /// </summary>
        public string ParentGoodName
        {
            set { _parentgoodname = value; }
            get { return _parentgoodname; }
        }
        /// <summary>
        /// 子商品编码
        /// </summary>
        public string SonGoodNo
        {
            set { _songoodno = value; }
            get { return _songoodno; }
        }
        /// <summary>
        /// 子商品名称
        /// </summary>
        public string SonGoodName
        {
            set { _songoodname = value; }
            get { return _songoodname; }
        }
    }
}
