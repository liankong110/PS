using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VisualSmart.BizService.Core;
using VisualSmart.Domain.Alipay;
using VisualSmart.Util;

namespace PS.Controllers.Alipay
{
    public class SceneryController : BaseController
    {
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="SceneryName"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [ViewPageAttribute]
        public ActionResult Index(string SceneryName, int page = 1)
        {
            var query = QueryCondition.Instance.AddOrderBy("Scenery","Id", false).SetPager(page, 10);
            if (!string.IsNullOrEmpty(SceneryName))
            {
                query.AddLike("SceneryName", SceneryName);
                ViewBag.SceneryName = SceneryName;
            }
            query.AddEqual("Scenery", "RowState", "1");
            ViewBag.Page = query.GetPager();
            var userList = Smart.Instance.SceneryBizService.GetAllDomain(query);
            return View(userList);
        }

        /// <summary>
        /// 新增/修改
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Error"></param>
        /// <returns></returns>
        public ActionResult Add(int? Id, string Error)
        {
            ViewBag.Error = Error;
            var model = new SceneryDomain();
            if (Id.HasValue)
            {
                var query = QueryCondition.Instance.AddEqual("Scenery","Id", Id.Value.ToString());
                model = Smart.Instance.SceneryBizService.GetAllDomain(query).FirstOrDefault();
            }
            AddOrUpdateBaseInfo(model);
            LoadData();
            return View(model);
        }

        private void LoadData()
        {
            //支付宝帐号
            var query = QueryCondition.Instance.AddOrderBy("Id", false).AddEqual("RowState", "1");           
            var alipayList = Smart.Instance.AlipayBizService.GetAllDomain(query);
            ViewBag.AlipayList = new SelectList(alipayList, "Id", "AppName");

            //微信帐号
            var weChatList = Smart.Instance.WxPayConfigBizService.GetAllDomain(query);
            ViewBag.WeChatList = new SelectList(weChatList, "Id", "WeAppName");

        }
        /// <summary>
        /// 新增/修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Add(SceneryDomain model)
        {          
            #region 判断景区名称是否存在
            var query = QueryCondition.Instance;
            query.AddEqual("Scenery","SceneryName", model.SceneryName);
            query.AddEqual("Scenery","RowState", "1");
            if (model.Id > 0)
            {
                query.AddNotEqual("Scenery","Id", model.Id.ToString());
            }
            if (Smart.Instance.SceneryBizService.GetAllDomain(query).Count > 0)
            {
                LoadData(); 
                ViewBag.Error = string.Format("已经存在名称:{0},请重新填写", model.SceneryName);
                return View(model);
            } 
            #endregion

            #region 判断同程景区ID 是否存在
            query = QueryCondition.Instance;
            query.AddEqual("Scenery","SceneryTCId", model.SceneryTCId.ToString());
            query.AddEqual("Scenery","RowState", "1");
            if (model.Id > 0)
            {
                query.AddNotEqual("Scenery","Id", model.Id.ToString());
            }
            if (Smart.Instance.SceneryBizService.GetAllDomain(query).Count > 0)
            {
                LoadData(); 
                ViewBag.Error = string.Format("已经存在同程景区ID:{0},请重新填写", model.SceneryTCId);
                return View(model);
            } 
            #endregion

            model.Updater = CurrentUser.Name;
            model.UpdateTime = DateTime.Now;
            if (model.Id == 0)
            {
                model.CreateTime = DateTime.Now;
                model.Creater = CurrentUser.Name;
                Smart.Instance.SceneryBizService.Add(model);
                ViewBag.Error = "1";
                return RedirectToAction("Add", "Scenery", new { Error = 1 });
            }
            ViewBag.Error = "1";
            var oldModel = Smart.Instance.SceneryBizService.GetAllDomain(QueryCondition.Instance.AddEqual("Scenery","Id", model.Id.ToString()));
            if (Smart.Instance.AlipayDetailBizService.IsExistsSceneryAlipay(model.SceneryName)||
                Smart.Instance.WeChatDetailBizService.IsExistsSceneryWeChat(model.SceneryName))
            {
                model.SceneryName = oldModel[0].SceneryName;
                model.SceneryTCId = oldModel[0].SceneryTCId;
            }

            if (Smart.Instance.SceneryBizService.Update(model))
            {
                LogHelper.AlipayLog(string.Format("景区\r\n 修改人：{0} \r\n 修改前：{1} \r\n 修改后：{2}",
                      CurrentUser.Name, JsonConvert.SerializeObject(oldModel), JsonConvert.SerializeObject(model)));
            }
            return RedirectToAction("Index", "Scenery");
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult Delete(int id)
        {
            if (CurrentUser == null)
            {
                return Json(new { Mess = "网页超时，请刷新网页重新登录" });
            }
            var oldModel = Smart.Instance.SceneryBizService.GetAllDomain(QueryCondition.Instance.AddEqual("Scenery","Id", id.ToString()));
            if (Smart.Instance.AlipayDetailBizService.IsExistsSceneryAlipay(oldModel[0].SceneryName) ||
                Smart.Instance.WeChatDetailBizService.IsExistsSceneryWeChat(oldModel[0].SceneryName))
            {
                return Json(new { Mess = "已经存在交易记录无法删除！" });
            }
            if (Smart.Instance.SceneryBizService.Delete(id))
            {
                LogHelper.AlipayLog(string.Format("景区\r\n 删除人：{0} \r\n删除ID：{1}", CurrentUser.Name, id));                
            }
           
            return Json(new { Mess = "success" });
        }
    }
}