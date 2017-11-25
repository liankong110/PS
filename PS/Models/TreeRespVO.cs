using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PS.Models
{
    /**
 * 功能: 数据返回对象
 */
   [Serializable]
    public class TreeRespVO
    {
        private List<Item> data = new List<Item>();

        public List<Item> getData()
        {
            return data;
        }

        public void setData(List<Item> data)
        {
            this.data = data;
        }


    }

}