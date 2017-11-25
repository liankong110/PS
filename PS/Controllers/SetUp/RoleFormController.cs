using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VisualSmart.Domain.SetUp;
using PS.Models;
using VisualSmart.Util;
using VisualSmart.BizService.Core;
namespace PS.Controllers.SetUp
{
    public class RoleFormController : BaseController
    {
        [HttpGet]
        public ActionResult Index(string RoleName = "超级管理员", int RoleId = 1)
        {
            return View(LoadData(RoleName, RoleId));
        }

        /// <summary>
        /// 加载基本信息
        /// </summary>
        /// <param name="RoleName"></param>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        private HomeModel LoadData(string RoleName, int RoleId)
        {
            HomeModel homeModel = new HomeModel();
            homeModel.MenuList = Smart.Instance.MenuBizService.GetAllDomain(QueryCondition.Instance.AddOrderBy("MenuIndex", true));
            homeModel.FormList = Smart.Instance.FormBizService.GetRoleFromList(RoleId);
            homeModel.RoleList = Smart.Instance.RoleBizService.GetAllDomain(QueryCondition.Instance.AddOrderBy("Id", true));
            homeModel.FuntionList = Smart.Instance.FunctionBizService.GetRoleFunctionList(RoleId);
            ViewBag.RoleName = RoleName;
            ViewBag.RoleId = RoleId;
            return homeModel;
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Index(string RoleName, int RoleId, string Temp)
        {
            bool success = Smart.Instance.Role_FormBizService.SaveRole_FormAndFunctioin(RoleId, Request["menu"], Request["form"], Request["function"], CurrentUser.Name);
            ViewBag.Error = success ? 1 : 0;
            return View(LoadData(RoleName, RoleId));
        }

        private string GetThml(string value, string name, string id, bool isSelected)
        {
            return string.Format("<input class=\"ace\" " + (isSelected ? "checked='checked'" : "") + " value=\"" + id + "\" type=\"checkbox\" name=\"" + name + "\"/><span class=\"lbl\"> " + value + "</span>&nbsp;&nbsp;");
        }
        [HttpPost]
        public JsonResult GetTree(int roleId)
        {
            var MenuList = Smart.Instance.MenuBizService.GetAllDomain(QueryCondition.Instance.AddOrderBy("MenuIndex", true));
            var FormList = Smart.Instance.FormBizService.GetRoleFromList(roleId);
            var FunctionList = Smart.Instance.FunctionBizService.GetRoleFunctionList(roleId);

            List<Item> voItemList = new List<Item>();

            //Tree和数据库对应的实体bean对象       
            foreach (MenuDomain tree in MenuList)
            {
                Item item = new Item { name = tree.DisplayName, type = "folder" };

                var child_Forms = FormList.ToList().FindAll(t => t.MenuId == tree.Id);
                string name = "  <input type=\"checkbox\" checked='checked' value=\"" + tree.Id + "\"  name=\"menu\" style=\"display:none;\"/>";
                foreach (var f in child_Forms)
                {
                    name += GetThml(f.FormName, "form", f.Id.ToString(), f.IsSelected);
                    foreach (var funciton in FunctionList.ToList().FindAll(t => t.Form_Id == f.Id))
                    {
                        name += GetThml(funciton.FunctionText, "function", (f.Id + "_" + funciton.Id), funciton.IsSelected);
                    }
                    var form = new Item { name = name, type = "item" };
                    //form.additionalParameters.children.Add(new Item { name = GetThml("删除") + GetThml("添加") + GetThml("修改") + GetThml("删除") + GetThml("添加") + GetThml("修改"), type = "item" });
                    item.additionalParameters.children.Add(form);
                    name = "";
                }
                voItemList.Add(item);
            }
            var m = JsonConvert.SerializeObject(voItemList);


            return this.Json(voItemList);
        }

    }
}
