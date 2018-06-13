using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using VisualSmart.BizService.Core;
using VisualSmart.Util;

namespace PS.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            Session.Clear();
            return View();
        }

        [HttpPost]
        public ActionResult Login(string txtName, string txtPwd, string rd_remember)
        {
            ViewBag.Name = txtName;
            ViewBag.Pwd = txtPwd;
            if (!string.IsNullOrEmpty(rd_remember))
            {
                ViewBag.Checked = "checked = checked";
            }
            if (string.IsNullOrEmpty(txtName) || string.IsNullOrEmpty(txtPwd))
            {
                return View();
            }
            var userList = Smart.Instance.UserBizService.GetAllDomain(QueryCondition.Instance.AddEqual("LoginId", txtName));
            if (userList.Count != 1 || userList[0].loginPwd != MD5Util.Encrypt(txtPwd))
            {
                //用户不存在
                ViewBag.Error = "用户名或密码输入错误";
                return View();
            }

            FormsAuthentication.SetAuthCookie(userList[0].Id.ToString(), !string.IsNullOrEmpty(rd_remember));
            Session["User"] = userList[0];
            return RedirectToAction("Index", "Home");
        }

    }
}
