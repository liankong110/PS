using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PS.Models
{
   /**
 * 功能: 子节点的信息
 */
    [Serializable]
public class AdditionalParameters
{
     /**
     * 子节点列表
     */
        public List<Item> children = new List<Item>();
    
    // /**
    // * 节点的Id
    // */
    // private String id;
    
    // /**
    // * 是否有选中属性
    // */
    // [JsonProperty( "item-selected" )]  
    // private bool itemSelected ;

    // public bool isItemSelected()
    //{
    //      return itemSelected ;
    //}

    // public void setItemSelected( bool itemSelected )
    //{
    //      this .itemSelected = itemSelected;
    //}

    // public List<Item> getChildren()
    //{
    //      return children ;
    //}

    // public void setChildren(List<Item> children )
    //{
    //      this .children = children;
    //}

    // public String getId()
    //{
    //      return id ;
    //}

    // public void setId(String id )
    //{
    //      this .id = id;
    //}
    
    
}

}