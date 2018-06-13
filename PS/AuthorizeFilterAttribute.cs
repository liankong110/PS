using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VisualSmart.BizService.Core;
using VisualSmart.Domain.SetUp;
using VisualSmart.Util;

namespace PS
{
 
    /// <summary>
    /// 权限拦截
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class AuthorizeFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            var path = filterContext.HttpContext.Request.Path.ToLower();
            if (path == "/" || path == "/Account/Login".ToLower())
                return;//忽略对Login登录页的权限判定

            //var user = filterContext.HttpContext.Session["User"] as UserDomain;//获取当前用户信息


            object[] attrs = filterContext.ActionDescriptor.GetCustomAttributes(typeof(ViewPageAttribute), true);
            var isViewPage = attrs.Length == 1;//当前Action请求是否为具体的功能页

            if (this.AuthorizeCore(filterContext, isViewPage) == false)//根据验证判断进行处理
            {
                //注：如果未登录直接在URL输入功能权限地址提示不是很友好；
                //如果登录后输入未维护的功能权限地址，那么也可以访问，这个可能会有安全问题
                if (isViewPage == true)
                {
                    filterContext.HttpContext.Response.Redirect("/Home/InadequatePermissions");
                }
                else
                {
                    filterContext.Result = new ContentResult { Content = @"抱歉,你不具有当前操作的权限" };//功能权限弹出提示框
                }
            }
        }


        //权限判断业务逻辑
        protected virtual bool AuthorizeCore(ActionExecutingContext filterContext, bool isViewPage)
        {
            if (filterContext.HttpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

            var user = filterContext.HttpContext.Session["User"] as UserDomain;//获取当前用户信息
            if (user == null)
            {
                //防止重定向
                //filterContext.HttpContext.Response.Redirect("/Account/Login");
                //return false;

                string domainName = Environment.UserDomainName;
                string username = Environment.UserName;
                var LoginId = domainName + "\\" + username;
                var userList = Smart.Instance.UserBizService.GetAllDomain(QueryCondition.Instance.AddEqual("LoginId", LoginId));
                if (userList.Count == 0)
                {
                    //新增
                    user = new UserDomain();
                    user.loginPwd = MD5Util.Encrypt("123456");
                    user.RowState = 1;
                    user.Creater = "system";
                    user.Updater = "system";
                    user.loginId = LoginId;
                    user.Name = username;
                    Smart.Instance.UserBizService.Add_Update_User_Role(user, System.Configuration.ConfigurationManager.AppSettings["DefaultRoleId"], "system");
                    user.Id = Smart.Instance.UserBizService.GetAllDomain(QueryCondition.Instance.AddEqual("LoginId", LoginId))[0].Id;
                    filterContext.HttpContext.Session["User"] = user;
                }
                else
                {
                    user = userList[0];
                }
            }
            var controllerName = filterContext.RouteData.Values["controller"].ToString();
            var actionName = filterContext.RouteData.Values["action"].ToString();
            if (isViewPage && (controllerName.ToLower() != "main" && actionName.ToLower() != "masterpage"))//如果当前Action请求为具体的功能页并且不是MasterPage页
            {
                if (Smart.Instance.FormBizService.GetFormByUserId(controllerName, actionName, user.Id) == 0)
                    return false;
            }
            else
            {
                //var actions = ContainerFactory.GetContainer().Resolve<IAuthorityFacade>().GetAllActionPermission();//所有被维护的Action权限
                //if (actions.Count(a => a.ControllerName == controllerName && a.ActionName == actionName) != 0)//如果当前Action属于被维护的Action权限
                //{
                //    if (user.ActionPermission.Count(a => a.ControllerName == controllerName && a.ActionName == actionName) == 0)
                //        return false;
                //}
            }
            return true;
        }
    }
 
}