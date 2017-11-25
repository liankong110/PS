using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PS.Models;
using VisualSmart.Util;
using VisualSmart.BizService.Core;
using VisualSmart.Domain.SetUp;

namespace PS.Controllers
{
    public class HomeController : BaseController
    {
        /// <summary>
        /// HOME
        /// </summary>
        /// <returns></returns>
        //[ViewPageAttribute]        
        public ActionResult Index()
        { 
            HomeModel homeModel = new HomeModel();
            homeModel.FormList = Smart.Instance.FormBizService.GetFormByUserId(CurrentUser.Id);       
            homeModel.MenuList = Smart.Instance.MenuBizService.GetAllDomain(QueryCondition.Instance.AddOrderBy("MenuIndex", true));               
            return View(homeModel);            
        }

        public ActionResult Welcome()
        {
            return View();
        }
        public ActionResult Test()
        {
            return View();
        }

        public ActionResult Footer()
        {
            return View();
        }
        public ActionResult Sidebar()
        {
            return View();
        }


        public ActionResult Error()
        {
            return View();
        }
        public ActionResult Empty()
        {
            return View();
        }

        public ActionResult InadequatePermissions()
        {
            return View();
        }
    }
}
