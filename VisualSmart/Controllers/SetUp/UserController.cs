using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VisualSmart.BizService.Core;
using VisualSmart.Util;

namespace VisualSmart.Controllers.SetUp
{
    public class UserController : Controller
    {
        public ActionResult Index(int page = 1)
        {
            var query = QueryCondition.Instance.AddOrderBy("Id", false).SetPager(page,10);
            ViewBag.Page = query.GetPager();            
            var userList = BizService.Core.Smart.Instance.UserBizService.GetAllDomain(query);
            return View(userList);
        }
        
    }
}
