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
    public class RoleController : Controller
    {

        public ActionResult Index(int page=1)
        {
            var query = QueryCondition.Instance.AddOrderBy("Id", false).SetPager(page, 10);
            ViewBag.Page = query.GetPager();
            var userList = BizService.Core.Smart.Instance.RoleBizService.GetAllDomain(query);
            return View(userList);           
        }


        public ActionResult Add(string Error)
        {
            ViewBag.Error = Error;
            return View(new RoleDomain());
        }

        [HttpPost]
        public ActionResult Add(RoleDomain user)
        {
            user.Creater = "admin";
            user.Updater = "admin";
            if (BizService.Core.Smart.Instance.RoleBizService.Add(user))
            {
                ViewBag.Error = "1";//成功
                return RedirectToAction("add", new { Error=1 });              
            }
            else
            {
                ViewBag.Error = "0";//失败
            }
            return View(user);
        }

        public ActionResult Delete(int id)
        {
            BizService.Core.Smart.Instance.RoleBizService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
