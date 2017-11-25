using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PS.Models
{
    /**
  * 功能: 节点的信息
  */
     [Serializable]
    public class Item
    {
         public string id { get; set; }
         public string name { get;set;}

         public string type{get;set;}
         public AdditionalParameters additionalParameters=new AdditionalParameters ();  
         //public List<Item> children = new List<Item>();
        ///**
        //* 节点的名字
        //*/
        //private String text;

        ///**
        //* 节点的类型："item":文件  "folder":目录
        //*/
        //private String type;

        ///**
        //* 子节点的信息
        //*/
        //private AdditionalParameters additionalParameters;

        //public String getText()
        //{
        //    return text;
        //}

        //public void setText(String text)
        //{
        //    this.text = text;
        //}

        //public String getType()
        //{
        //    return type;
        //}

        //public void setType(String type)
        //{
        //    this.type = type;
        //}

        //public AdditionalParameters getAdditionalParameters()
        //{
        //    return additionalParameters;
        //}

        //public void setAdditionalParameters(AdditionalParameters additionalParameters)
        //{
        //    this.additionalParameters = additionalParameters;
        //}
    }

}