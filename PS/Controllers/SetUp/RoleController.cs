using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VisualSmart.BizService.Core;
using VisualSmart.Domain.SetUp;
using VisualSmart.Util;

namespace PS.Controllers.SetUp
{
    public class RoleController : BaseController
    {
        [ViewPageAttribute]
        public ActionResult Index(string RoleName, int page = 1)
        {
            var query = QueryCondition.Instance.AddOrderBy("Id", false).SetPager(page, 10);
            if (!string.IsNullOrEmpty(RoleName))
            {
                query.AddLike("RoleName", RoleName);
                ViewBag.RoleName = RoleName;
            }
            ViewBag.Page = query.GetPager();
            var userList = Smart.Instance.RoleBizService.GetAllDomain(query);
            return View(userList);
        }


        public ActionResult Add(int? Id, string Error)
        {
            ViewBag.Error = Error;
            var userModel = new RoleDomain();
            if (Id.HasValue)
            {
                var query = QueryCondition.Instance.AddEqual("Id", Id.Value.ToString());
                userModel = Smart.Instance.RoleBizService.GetAllDomain(query).FirstOrDefault();
            }
            AddOrUpdateBaseInfo(userModel);
            return View(userModel);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Add(RoleDomain user)
        {
            user.Updater = user.Creater = CurrentUser.Name;

            var query = QueryCondition.Instance;

            query.AddEqual("RoleName", user.RoleName);

            if (user.Id > 0)
            {
                query.AddNotEqual("Id", user.Id.ToString());
            }

            if (Smart.Instance.RoleBizService.GetAllDomain(query).Count > 0)
            {
                ViewBag.Error = string.Format("已经存在相同的角色名称:{0},请重新填写", user.RoleName);
                return View(user);
            }


            if (user.Id == 0)
            {
                Smart.Instance.RoleBizService.Add(user);
                ViewBag.Error = "1";
                return RedirectToAction("Add", "Role", new { Error = 1 });
            }
            ViewBag.Error = "1";
            Smart.Instance.RoleBizService.Update(user);

            return RedirectToAction("Index", "Role");
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult Delete(int id)
        {
            if (CurrentUser != null)
            {
                Smart.Instance.RoleBizService.Delete(id);
            }
            return Json(new { Mess = "success" });
        }
    }
}
