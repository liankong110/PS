using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using VisualSmart.Bootcss.Models;
using VisualSmart.Domain.SetUp;
using VisualSmart.Util;

namespace VisualSmart.Bootcss.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Logo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Logo(string txtName, string txtPwd, string rd_remember)
        {
            //System.Threading.Thread.Sleep(10000);
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
            var userList = BizService.Core.Smart.Instance.UserBizService.GetAllDomain(QueryCondition.Instance.AddEqual("LoginId", txtName));
            if (userList.Count != 1 || userList[0].loginPwd != MD5Util.Encrypt(txtPwd))
            {
                //用户不存在
                ViewBag.Error = "用户名或密码输入错误";
                return View();
            }

            FormsAuthentication.SetAuthCookie(userList[0].Id.ToString(), !string.IsNullOrEmpty(rd_remember));

            return RedirectToAction("Index", "User");
        }

        public ActionResult Logo_BackImg()
        {
            return View();
        }


        public ActionResult Home()
        {          
            HomeModel homeModel = new HomeModel();
            homeModel.MenuList = BizService.Core.Smart.Instance.MenuBizService.GetAllDomain(QueryCondition.Instance.AddOrderBy("MenuIndex", true));
            homeModel.FormList = BizService.Core.Smart.Instance.FormBizService.GetAllDomain(QueryCondition.Instance.AddOrderBy("FormIndex", true));

            return View(homeModel);
             
        }
    }
}
