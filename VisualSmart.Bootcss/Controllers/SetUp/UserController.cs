using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VisualSmart.Domain.SetUp;
using VisualSmart.Util;

namespace VisualSmart.Bootcss.Controllers.SetUp
{
    public class UserController : Controller
    {

        public ActionResult Index(int page=1)
        {
            var query = QueryCondition.Instance.AddOrderBy("Id", false).SetPager(page, 10);
            ViewBag.Page = query.GetPager();
            var userList = BizService.Core.Smart.Instance.UserBizService.GetAllDomain(query);
            return View(userList);           
        }


        public ActionResult Add()
        {
            return View(new UserDomain());
        }

        [HttpPost]
        public ActionResult Add(UserDomain user)
        {
            user.Creater = "admin";
            user.Updater = "admin";
            
            BizService.Core.Smart.Instance.UserBizService.Add(user);
            return View();
        }

        public JsonResult Delete(int id)
        {
           
           // BizService.Core.Smart.Instance.UserBizService.Delete(id);
            return Json(new { Mess="成功！" });
        }
    }
}
