using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VisualSmart.Bootcss.Models;
using VisualSmart.Util;

namespace VisualSmart.Bootcss.Controllers.SetUp
{
    public class RoleFormController : Controller
    {

        public ActionResult Index(string RoleName="请选择角色")
        {            
            HomeModel homeModel = new HomeModel();
            homeModel.MenuList = BizService.Core.Smart.Instance.MenuBizService.GetAllDomain(QueryCondition.Instance.AddOrderBy("MenuIndex", true));
            homeModel.FormList = BizService.Core.Smart.Instance.FormBizService.GetAllDomain(QueryCondition.Instance.AddOrderBy("FormIndex", true));
            homeModel.RoleList = BizService.Core.Smart.Instance.RoleBizService.GetAllDomain(QueryCondition.Instance.AddOrderBy("Id", true));
            homeModel.FuntionList = BizService.Core.Smart.Instance.FunctionBizService.GetAllDomain(QueryCondition.Instance.AddOrderBy("Id", true));
            ViewBag.RoleName = RoleName;
            return View(homeModel);
        }

    }
}
