using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace VisualSmart.Util
{
    /// <summary>
    /// 分页控件
    /// </summary>
    public static class PageHelper
    {
        /// <summary>
        /// 获取输入框的值
        /// </summary>
        /// <param name="html"></param>
        /// <param name="name"></param>
        /// <param name="defaultvalue"></param>
        /// <returns></returns>
        public static string GetInputStringValue(this HtmlHelper html, string name, string defaultvalue = "")
        {
            return html.ViewContext.HttpContext.Request[name] ?? defaultvalue;
        }

        /// <summary>
        /// 排序标签
        /// </summary>
        /// <param name="html"></param>
        /// <param name="sortParamsData">排序字段及其值</param>
        /// <param name="className">样式(Class Name)</param>
        /// <returns></returns>
        public static MvcHtmlString SortLink(this HtmlHelper html, object sortParamsData, string className)
        {
            var queryString = html.ViewContext.HttpContext.Request.QueryString;
            var routeValueDictionary = new RouteValueDictionary(html.ViewContext.RouteData.Values);
            foreach (var key in from string key in queryString.Keys
                                where !string.IsNullOrEmpty(key)
                                where !string.IsNullOrEmpty(queryString[key])
                                select key)
            {
                routeValueDictionary[key] = queryString[key];
            }
            if (sortParamsData != null)
            {
                var pis = sortParamsData.GetType().GetProperties();
                foreach (var pi in pis)
                {
                    var key = pi.Name;
                    if (routeValueDictionary.Count(t => t.Key == key) > 0)
                    {
                        routeValueDictionary.Remove(key);
                    }
                    routeValueDictionary.Add(key, pi.GetValue(sortParamsData, null));
                }
            }
            return html.RouteLink(" ", routeValueDictionary, new Dictionary<string, object> { { "class", className } });
        }

        /// <summary>
        /// 分页(HttpGet)
        /// </summary>
        /// <param name="html">Html Helper</param>
        /// <param name="currentPageString">当前页</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="totalRecords">总记录数</param>
        /// <returns>分页生产后的HTML</returns>
        public static MvcHtmlString BootStrapPager(this HtmlHelper html, string currentPageString, long pageSize, long totalRecords, long CurrentPage, long TotalPageCount)
        {
            var queryString = html.ViewContext.HttpContext.Request.QueryString;
            long currentPage;
            if (!long.TryParse(queryString[currentPageString], out currentPage))
            {
                currentPage = 1;
            }
            var pageCount = Math.Max((totalRecords + pageSize - 1) / pageSize, 1);           
            var output = new StringBuilder();
           
            output.Append("<div class=\"container-fluid\"><div class='row'> <div class=\"form-group form-group-sm col-md-4\">");
            output.AppendFormat(" <span> {0}-{1} of {2}</span>",
                ((CurrentPage - 1) * pageSize + 1), (CurrentPage * pageSize), totalRecords);
            output.Append(" </div>");

            output.Append("");
            output.Append("  <ul class=\"pagination pull-right\" style=\"margin:0px; padding:0px;\">");
          

            if (pageCount > 1)
            {
                if (currentPage != 1)
                {
                    //首页                   
                    output.Append("<li>");
                    output.Append("<a href=\"javascript:PostForm(1);\">  <i class=\"ace-icon fa fa-angle-double-left\"></i></a>");
                   
                    output.Append("</li>");
                }
                else
                {
                    output.Append("<li class='disabled'>");
                    output.Append("<a href=\"#\">  <i class=\"ace-icon fa fa-angle-double-left\"></i></a>");
                    output.Append("</li>");
                }              

                const long currlong = 8;
                const long showPageMaxSize = 16;
                long lastPage = 0;
                for (var i = 0; i < showPageMaxSize; i++)
                {
                    //一共最多显示10个页码，前面5个，后面5个
                    if ((currentPage + i - currlong) >= 1 && (currentPage + i - currlong) <= pageCount)
                    {
                        if (currlong.Equals(i))
                        {
                            //处理当前页面
                            output.Append("<li class='active'>");
                            output.AppendFormat("<a href=\"#\">{0}</a>", currentPage);
                            output.Append("</li>");
                            lastPage = currentPage;
                        }
                        else
                        {
                            //普通页面处理                          
                            output.Append("<li>");
                            output.AppendFormat("<a href=\"javascript:PostForm({0});\">{0}</a>", currentPage + i - currlong);                            
                            output.Append("</li>");
                            lastPage = currentPage + i - currlong;
                        }
                    }                    
                }

                if (currentPage < pageCount)
                {
                    if (lastPage < pageCount)
                    {                     
                        output.Append("<li>");                        
                        output.AppendFormat("<a href=\"#\">...</a>", lastPage + 1);
                        output.Append("</li>");

                        output.Append("<li>");
                        output.AppendFormat("<a href=\"javascript:PostForm({0});\">{0}</a>", pageCount);
                        output.Append("</li>");
                    }      
                }
                else
                {
                    output.Append("<li class='disabled'>");
                    output.Append("<a href=\"#\"><i class=\"ace-icon fa fa-angle-double-right\"></i></a>");
                    output.Append("</li>");
                }        

                if (currentPage != pageCount)
                { 
                    output.Append("<li>");
                    output.AppendFormat("<a href=\"javascript:PostForm({0});\"><i class=\"ace-icon fa fa-angle-double-right\"></i></a>", pageCount);
                    output.Append("</li>");                   
                }
                output.Append(" ");
            }
           
            output.Append(" </ul> ");        
            output.Append("</div></div>");
            return new MvcHtmlString(output.ToString());
        }


        ///// <summary>
        ///// 分页(HttpGet)
        ///// </summary>
        ///// <param name="html">Html Helper</param>
        ///// <param name="currentPageString">当前页</param>
        ///// <param name="pageSize">页面大小</param>
        ///// <param name="totalRecords">总记录数</param>
        ///// <returns>分页生产后的HTML</returns>
        //public static MvcHtmlString BootStrapPager(this HtmlHelper html, string currentPageString, long pageSize, long totalRecords, long CurrentPage, long TotalPageCount)
        //{
        //    var queryString = html.ViewContext.HttpContext.Request.QueryString;
        //    long currentPage;
        //    if (!long.TryParse(queryString[currentPageString], out currentPage))
        //    {
        //        currentPage = 1;
        //    }
        //    var pageCount = Math.Max((totalRecords + pageSize - 1) / pageSize, 1);
        //    var routeValueDictionary = new RouteValueDictionary(html.ViewContext.RouteData.Values);
        //    var output = new StringBuilder();
        //    output.Append("<div class=\"container-fluid\"><div class='row'> <div class=\"form-group form-group-sm col-md-4\">");
        //    output.AppendFormat(" <span> {0}-{1} of {2}</span>",
        //        ((CurrentPage - 1) * pageSize + 1), (CurrentPage * pageSize), totalRecords);
        //    output.Append(" </div>");

        //    output.Append("<div class=\"form-group form-group-sm col-md-8\">");
        //    output.Append(" <nav> <ul class=\"pagination pull-right\" style=\"margin:0px; padding:0px;\">");
        //    foreach (var key in from string key in queryString.Keys
        //                        where !string.IsNullOrEmpty(key)
        //                        where !string.IsNullOrEmpty(queryString[key])
        //                        select key)
        //    {
        //        routeValueDictionary[key] = queryString[key];
        //    }

        //    if (pageCount > 1)
        //    {
        //        if (currentPage != 1)
        //        {
        //            //首页
        //            routeValueDictionary[currentPageString] = 1;
        //            output.Append("<li>");
        //            output.AppendFormat(html.RouteLink("{0}", routeValueDictionary).ToString(), "首页");
        //            output.Append("</li>");
        //        }

        //        if (currentPage > 1)
        //        {
        //            //上一页链接
        //            routeValueDictionary[currentPageString] = currentPage - 1;
        //            output.Append("<li>");
        //            output.Append(html.RouteLink("", routeValueDictionary));
        //            output.Append("</li>");
        //        }
        //        else
        //        {
        //            output.Append("<li class='disabled'>");
        //            output.Append("<a href=\"#\">  <i class=\"icon-double-angle-left\"></i></a>");
        //            output.Append("</li>");
        //        }
        //        output.Append(" ");

        //        const long currlong = 8;
        //        const long showPageMaxSize = 16;
        //        long lastPage = 0;
        //        for (var i = 0; i < showPageMaxSize; i++)
        //        {
        //            //一共最多显示10个页码，前面5个，后面5个
        //            if ((currentPage + i - currlong) >= 1 && (currentPage + i - currlong) <= pageCount)
        //            {
        //                if (currlong.Equals(i))
        //                {
        //                    //处理当前页面
        //                    output.Append("<li class='active'>");
        //                    output.AppendFormat("<a href=\"#\">{0}</a>", currentPage);
        //                    output.Append("</li>");
        //                    lastPage = currentPage;
        //                }
        //                else
        //                {
        //                    //普通页面处理
        //                    routeValueDictionary[currentPageString] = currentPage + i - currlong;
        //                    output.Append("<li>");
        //                    output.AppendFormat("{0}", html.RouteLink((currentPage + i - currlong).ToString(), routeValueDictionary));
        //                    output.Append("</li>");
        //                    lastPage = currentPage + i - currlong;
        //                }
        //            }
        //            output.Append(" ");
        //        }

        //        if (currentPage < pageCount)
        //        {
        //            if (lastPage < pageCount)
        //            {
        //                routeValueDictionary[currentPageString] = lastPage + 1;
        //                output.Append("<li>");
        //                output.AppendFormat("{0}", html.RouteLink("...", routeValueDictionary));
        //                output.Append("</li>");
        //                //output.Append("<li><a href=\"#\">...</a></li>");
        //            }
        //            //处理下一页链接
        //            routeValueDictionary[currentPageString] = currentPage + 1;
        //            output.Append("<li>");
        //            output.Append(html.RouteLink("下一页", routeValueDictionary));
        //            output.Append("</li>");
        //        }
        //        else
        //        {
        //            output.Append("<li class='disabled'>");
        //            output.Append("<a href=\"#\"><i class=\"icon-double-angle-right\"></i></a>");
        //            output.Append("</li>");
        //        }

        //        output.Append(" ");

        //        if (currentPage != pageCount)
        //        {
        //            routeValueDictionary[currentPageString] = pageCount;
        //            output.Append("<li>");
        //            output.AppendFormat(html.RouteLink("{0}", routeValueDictionary).ToString(), "尾页");
        //            output.Append("</li>");
        //        }

        //        output.Append(" ");
        //    }

        //    output.Append(" </ul> </nav>");
        //    output.Append(" ");
        //    output.Append("</div></div></div>");
        //    return new MvcHtmlString(output.ToString());
        //}


        /// <summary>
        /// 分页(HttpGet)
        /// </summary>
        /// <param name="html">Html Helper</param>
        /// <param name="currentPageString">当前页</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="totalRecords">总记录数</param>
        /// <returns>分页生产后的HTML</returns>
        public static MvcHtmlString Pager(this HtmlHelper html, string currentPageString, long pageSize, long totalRecords, long CurrentPage, long TotalPageCount)
        {
            var queryString = html.ViewContext.HttpContext.Request.QueryString;
            long currentPage;
            if (!long.TryParse(queryString[currentPageString], out currentPage))
            {
                currentPage = 1;
            }
            var pageCount = Math.Max((totalRecords + pageSize - 1) / pageSize, 1);
            var routeValueDictionary = new RouteValueDictionary(html.ViewContext.RouteData.Values);
            var output = new StringBuilder();
            output.Append(" <div class=\"am-cf\">");
            output.AppendFormat("  共<b>"+totalRecords+"</b> 条数据 当前第<b>"+CurrentPage+"</b>页 共<b>"+TotalPageCount+"</b>页");
            output.AppendFormat(" <div class=\"am-fr\"><ul class=\"am-pagination\">");


            

            foreach (var key in from string key in queryString.Keys
                                where !string.IsNullOrEmpty(key)
                                where !string.IsNullOrEmpty(queryString[key])
                                select key)
            {
                routeValueDictionary[key] = queryString[key];
            }

            if (pageCount > 1)
            {
                if (currentPage != 1)
                {
                    //首页
                    routeValueDictionary[currentPageString] = 1;
                    output.Append("<li class='am-active'>");
                    output.AppendFormat(html.RouteLink("{0}", routeValueDictionary, new Dictionary<string, object> { { "class", "pagePrev" } }).ToString(), "首页");
                    output.Append("</li>");
                }

                if (currentPage > 1)
                {
                    //上一页链接
                    routeValueDictionary[currentPageString] = currentPage - 1;
                    output.Append("<li class='am-active'>");
                    output.Append(html.RouteLink("上一页", routeValueDictionary));
                    output.Append("</li>");
                }
                else
                {
                    output.Append("<li class='am-disabled'>");
                    output.Append("<a href=\"#\">上一页</a>");
                    output.Append("</li>");
                }
                output.Append(" ");

                const long currlong = 8;
                const long showPageMaxSize = 16;
                long lastPage = 0;
                for (var i = 0; i < showPageMaxSize; i++)
                {
                    //一共最多显示10个页码，前面5个，后面5个
                    if ((currentPage + i - currlong) >= 1 && (currentPage + i - currlong) <= pageCount)
                    {
                        if (currlong.Equals(i))
                        {
                            //处理当前页面
                            output.Append("<li class='am-active'>");
                            output.AppendFormat("<a href=\"#\">{0}</a>", currentPage);
                            output.Append("</li>");
                            lastPage = currentPage;
                        }
                        else
                        {
                            //普通页面处理
                            routeValueDictionary[currentPageString] = currentPage + i - currlong;
                            output.Append("<li>");
                            output.AppendFormat("{0}", html.RouteLink((currentPage + i - currlong).ToString(), routeValueDictionary));
                            output.Append("</li>");
                            lastPage = currentPage + i - currlong;
                        }
                    }
                    output.Append(" ");
                }

                if (currentPage < pageCount)
                {
                    if (lastPage < pageCount)
                    {
                        routeValueDictionary[currentPageString] = lastPage+1;
                        output.Append("<li>");
                        output.AppendFormat("{0}", html.RouteLink("...", routeValueDictionary));
                        output.Append("</li>");
                        //output.Append("<li><a href=\"#\">...</a></li>");
                    }
                    //处理下一页链接
                    routeValueDictionary[currentPageString] = currentPage + 1;
                    output.Append("<li class='am-active'>");
                    output.Append(html.RouteLink("下一页", routeValueDictionary));
                    output.Append("</li>");
                }
                else
                {
                    output.Append("<li class='am-disabled'>");
                    output.Append("<a href=\"#\">下一页</a>");
                    output.Append("</li>");
                }

                output.Append(" ");

                if (currentPage != pageCount)
                {
                    routeValueDictionary[currentPageString] = pageCount;
                    output.Append("<li class='am-active'>");
                    output.AppendFormat(html.RouteLink("{0}", routeValueDictionary, new Dictionary<string, object> { { "class", "pageNext" } }).ToString(), "尾页");
                    output.Append("</li>");
                }

                output.Append(" ");
            }
            //output.AppendFormat("{0} / {1}", currentPage, pageCount);
            output.Append(" </ul> </div>");
            output.Append("</div>");
            return new MvcHtmlString(output.ToString());
        }

        /// <summary>
        /// 分页(HttpGet)
        /// </summary>
        /// <param name="html">Html Helper</param>
        /// <param name="currentPageString">当前页</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="totalRecords">总记录数</param>
        /// <returns>分页生产后的HTML</returns>
        public static MvcHtmlString Pager(this HtmlHelper html, string currentPageString, long pageSize, long totalRecords)
        {
            var queryString = html.ViewContext.HttpContext.Request.QueryString;
            long currentPage;
            if (!long.TryParse(queryString[currentPageString], out currentPage))
            {
                currentPage = 1;
            }
            var pageCount = Math.Max((totalRecords + pageSize - 1) / pageSize, 1);
            var routeValueDictionary = new RouteValueDictionary(html.ViewContext.RouteData.Values);
            var output = new StringBuilder();

            foreach (var key in from string key in queryString.Keys
                                where !string.IsNullOrEmpty(key)
                                where !string.IsNullOrEmpty(queryString[key])
                                select key)
            {
                routeValueDictionary[key] = queryString[key];
            }

            if (pageCount > 1)
            {
                if (currentPage != 1)
                {
                    //首页
                    routeValueDictionary[currentPageString] = 1;
                    output.Append("<li class='am-active'>");
                    output.AppendFormat(html.RouteLink("{0}", routeValueDictionary, new Dictionary<string, object> { { "class", "pagePrev" } }).ToString(), "首页");
                    output.Append("</li>");
                }

                if (currentPage > 1)
                {
                    //上一页链接
                    routeValueDictionary[currentPageString] = currentPage - 1;
                    output.Append("<li class='am-active'>");
                    output.Append(html.RouteLink("上一页", routeValueDictionary));
                    output.Append("</li>");
                }
                else
                {
                    output.Append("<li class='am-disabled'>");
                    output.Append("<a href=\"#\">上一页</a>");
                    output.Append("</li>");
                }
                output.Append(" ");

                const long currlong = 5;
                const long showPageMaxSize = 10;
                for (var i = 0; i < showPageMaxSize; i++)
                {
                    //一共最多显示10个页码，前面5个，后面5个
                    if ((currentPage + i - currlong) >= 1 && (currentPage + i - currlong) <= pageCount)
                    {
                        if (currlong.Equals(i))
                        {
                            //处理当前页面
                            output.Append("<li class='am-active'>");
                            output.AppendFormat("<a href=\"#\">{0}</a>", currentPage);
                            output.Append("</li>");
                        }
                        else
                        {
                            //普通页面处理
                            routeValueDictionary[currentPageString] = currentPage + i - currlong;
                            output.Append("<li>");
                            output.AppendFormat("{0}", html.RouteLink((currentPage + i - currlong).ToString(), routeValueDictionary));
                            output.Append("</li>");
                        }
                    }
                    output.Append(" ");
                }

                if (currentPage < pageCount)
                {
                    //处理下一页链接
                    routeValueDictionary[currentPageString] = currentPage + 1;
                    output.Append("<li class='am-active'>");
                    output.Append(html.RouteLink("下一页", routeValueDictionary));
                    output.Append("</li>");
                }
                else
                {
                    output.Append("<li class='am-disabled'>");
                    output.Append("<a href=\"#\">下一页</a>");
                    output.Append("</li>");
                }

                output.Append(" ");

                if (currentPage != pageCount)
                {
                    routeValueDictionary[currentPageString] = pageCount;
                    output.Append("<li class='am-active'>");
                    output.AppendFormat(html.RouteLink("{0}", routeValueDictionary, new Dictionary<string, object> { { "class", "pageNext" } }).ToString(), "尾页");
                    output.Append("</li>");
                }

                output.Append(" ");
            }
            //output.AppendFormat("{0} / {1}", currentPage, pageCount);
            return new MvcHtmlString(output.ToString());
        }

        /// <summary>
        /// 分页(HttpPost)
        /// Controller中通过Request["page"]获取当前页
        /// </summary>
        /// <param name="html">Html Helper</param>
        /// <param name="formName">表单名称</param>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="totalRecords">总记录数</param>
        /// <returns></returns>
        public static MvcHtmlString Pager(this HtmlHelper html, string formName, long currentPage, long pageSize, long totalRecords)
        {
            var pageCount = Math.Max((totalRecords + pageSize - 1) / pageSize, 1);
            var output = new StringBuilder();
            //增加JS Script
            output.Append(
                "<script type=\"text/javascript\"> function pagea(currentpage) { document.getElementById(\"page\").value=currentpage; document.forms[\"" +
                formName + "\"].submit(); } </script>");
            output.AppendFormat("<input type=\"hidden\" id=\"page\" name=\"page\" />");
            if (pageCount > 1)
            {
                if (currentPage != 1)
                {
                    //首页
                    output.AppendFormat("<a href=\"javascript:void(0);\" onclick=\"javascript:pagea({0})\" class=\"pagePrev\">{0}</a>", "&nbsp;");
                }
                output.Append(" ");
                const long currlong = 5;
                const long showPageMaxSize = 10;
                for (var i = 0; i < showPageMaxSize; i++)
                {
                    //一共最多显示10个页码，前面5个，后面5个
                    if ((currentPage + i - currlong) >= 1 && (currentPage + i - currlong) <= pageCount)
                    {
                        if (currlong.Equals(i))
                        {
                            //处理当前页面
                            output.AppendFormat("<a href=\"javascript:void(0);\" class=\"at\">{0}</a>", currentPage);
                        }
                        else
                        {
                            //普通页面处理
                            output.AppendFormat("<a href=\"javascript:void(0);\" onclick=\"javascript:pagea({0})\">{0}</a>", currentPage + i - currlong);
                        }
                    }
                    output.Append(" ");
                }
                output.Append(" ");
                if (currentPage != pageCount)
                {
                    output.AppendFormat("<a href=\"javascript:void(0);\" onclick=\"javascript:pagea({0})\" class=\"pageNext\">{0}</a>", "&nbsp;");
                }
                output.Append(" ");
            }
            return new MvcHtmlString(output.ToString());
        }
    }
}
