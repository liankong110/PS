using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VisualSmart.BizService.Core;
using VisualSmart.Util;

namespace PS.Controllers
{
    public class TableController : Controller
    {
        // GET: Table
        public ActionResult Index(string userName, int page = 1)
        {
            var query = QueryCondition.Instance.AddOrderBy("Id", false).SetPager(page, 10);
            if (!string.IsNullOrEmpty(userName))
            {
                query.AddLike("loginId", userName);
                ViewBag.LoginId = userName;
            }
            ViewBag.Page = query.GetPager();
            var userList = Smart.Instance.UserBizService.GetAllDomain(query);
            return View(userList);
        }
    }
}