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
    public class UserController : BaseController
    {
        /// <summary>
        /// 密码修改
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangePassword()
        {
            return View();
        }

        /// <summary>
        /// 密码修改
        /// </summary>
        /// <param name="newPwdConfim">密码</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ChangePassword(string oldPwd,string newPwdConfim)
        {
            if (MD5Util.Encrypt(oldPwd) != CurrentUser.loginPwd)
            {
                ViewBag.Error = "旧密码不正确";
                return View();
            }          
            if (Smart.Instance.UserBizService.ChangePassword(CurrentUser.Id, MD5Util.Encrypt(newPwdConfim)))
            {
                ViewBag.Error = "1";
            }
            else
            {
                ViewBag.Error = "修改失败";
            }
            return View();
        }
        /// <summary>
        /// 用户详情
        /// </summary>
        /// <returns></returns>
        public ActionResult Detail()
        {
            if (string.IsNullOrEmpty(CurrentUser.Avatar))
            {
                CurrentUser.Avatar = "~/CONTENT/avatars/profile-pic.jpg";
            }
            return View(CurrentUser);
        }

        /// <summary>
        /// 用户详情
        /// </summary>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Detail(UserDomain user)
        {
            CurrentUser.Phone = user.Phone;
            CurrentUser.Email = user.Email;
            Smart.Instance.UserBizService.Update(CurrentUser);
            ViewBag.Error = "1";             
            return View(CurrentUser);
        }
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Index(string userName, int page = 1)
        {
            var query = QueryCondition.Instance.AddOrderBy("Id", false).SetPager(page, 10);
            if (!string.IsNullOrEmpty(userName))
            {
                query.AddLike("loginId", userName);
                ViewBag.LoginId = userName;
            }
            ViewBag.Page = query.GetPager();
            var userList =Smart.Instance.UserBizService.GetAllDomain(query);
            return View(userList);
        }

        /// <summary>
        /// 添加 /修改用户信息
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Error"></param>
        /// <returns></returns>
        public ActionResult Add(int? Id, string Error)
        {
            ViewBag.Error = Error;
            var userModel = new UserDomain { CreateTime = DateTime.Now, Avatar = "~/CONTENT/avatars/profile-pic.jpg" };
            if (Id.HasValue)
            {
                var query = QueryCondition.Instance.AddEqual("Id", Id.Value.ToString());
                userModel =Smart.Instance.UserBizService.GetAllDomain(query).FirstOrDefault();
            }
            LoadData(userModel);
            return View(userModel);
        }

        private void LoadData(UserDomain userModel)
        {
            AddOrUpdateBaseInfo(userModel);

            ViewBag.RoleList = Smart.Instance.RoleBizService.GetUser_RoleList(userModel.Id).ToList();
        }

        /// <summary>
        /// 添加 /修改用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Add(UserDomain user)
        {
            user.loginPwd = MD5Util.Encrypt("123456");           
            user.RowState = 1;
            var query = QueryCondition.Instance;

            query.AddEqual("loginId", user.loginId);
            query.AddEqual("RowState", "1");
            
            if (user.Id > 0)
            {
                query.AddNotEqual("Id", user.Id.ToString());
            }

            if (Smart.Instance.UserBizService.GetAllDomain(query).Count > 0)
            {
                ViewBag.Error = string.Format("已经存在相同的用户名:{0},请重新填写", user.loginId);
                LoadData(user);
                return View(user);
            }

            query = QueryCondition.Instance;
            query.AddEqual("Name", user.Name);

            if (user.Id > 0)
            {
                query.AddNotEqual("Id", user.Id.ToString());
            }

            if (Smart.Instance.UserBizService.GetAllDomain(query).Count > 0)
            {
                ViewBag.Error = string.Format("已经存在相同的真实姓名:{0},请重新填写", user.Name);
                LoadData(user);
                return View(user);
            }
          
            ViewBag.Error = "1";
           
            if (user.Id == 0)
            {
                
                user.Creater = CurrentUser.Name;
            
                user.Updater = CurrentUser.Name;

                Smart.Instance.UserBizService.Add_Update_User_Role(user, Request["RoleId"],CurrentUser.Name);
                ViewBag.Error = "1";
                return RedirectToAction("Add", "User", new { Error = 1 });
            }
            user.Updater = CurrentUser.Name;
            Smart.Instance.UserBizService.Add_Update_User_Role(user, Request["RoleId"], CurrentUser.Name);
            return RedirectToAction("Index", "User");
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult Delete(int id)
        {
            if (CurrentUser != null)
            {
                Smart.Instance.UserBizService.Delete(id);
            }           
            return Json(new { Mess = "success" });
        }
    }
}
