using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VisualSmart.BizService.Core;
using VisualSmart.Domain.Alipay;
using VisualSmart.Domain.WeChat;
using VisualSmart.Util;

namespace PS.Controllers.WeChat
{
    public class WxPayConfigController : BaseController
    {
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="WeAppName"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [ViewPageAttribute]
        public ActionResult Index(string WeAppName, int page = 1)
        {
            var query = QueryCondition.Instance.AddOrderBy("Id", false).SetPager(page, 10);
            if (!string.IsNullOrEmpty(WeAppName))
            {
                query.AddLike("WeAppName", WeAppName);
                ViewBag.AppName = WeAppName;
            }
            query.AddEqual("RowState", "1");
            ViewBag.Page = query.GetPager();
            var queryList = Smart.Instance.WxPayConfigBizService.GetAllDomain(query);
            return View(queryList);
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
            var model = new WxPayConfigDomain();
            if (Id.HasValue)
            {
                var query = QueryCondition.Instance.AddEqual("Id", Id.Value.ToString());
                model = Smart.Instance.WxPayConfigBizService.GetAllDomain(query).FirstOrDefault();
            }
            AddOrUpdateBaseInfo(model);
            return View(model);
        }
        /// <summary>
        /// 新增/修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Add(WxPayConfigDomain model)
        {
            var query = QueryCondition.Instance;
            query.AddEqual("AppName", model.WeAppName);
            query.AddEqual("RowState", "1");
            if (model.Id > 0)
            {
                query.AddNotEqual("Id", model.Id.ToString());
            }
            if (Smart.Instance.AlipayBizService.GetAllDomain(query).Count > 0)
            {
                ViewBag.Error = string.Format("已经存在名称:{0},请重新填写", model.WeAppName);
                return View(model);
            }            
            model.Updater = CurrentUser.Name;
            model.UpdateTime = DateTime.Now;
            if (model.Id == 0)
            {
                model.CreateTime = DateTime.Now;
                model.Creater = CurrentUser.Name;
                Smart.Instance.WxPayConfigBizService.Add(model);

               
                ViewBag.Error = "1";
                return RedirectToAction("Add", "WxPayConfig", new { Error = 1 });
            }

            ViewBag.Error = "1";
            var oldModel = Smart.Instance.WxPayConfigBizService.GetAllDomain(QueryCondition.Instance.AddEqual("Id", model.Id.ToString()));
            if (Smart.Instance.WeChatDetailBizService.IsExistsWeChatDetail(model.Id))
            {
                model.APPID = oldModel[0].APPID;
                model.WeAppName = oldModel[0].WeAppName;
            }

            if (Smart.Instance.WxPayConfigBizService.Update(model))
            {
                LogHelper.AlipayLog(string.Format("微信\r\n 修改人：{0} \r\n 修改前：{1} \r\n 修改后：{2}",
                    CurrentUser.Name, JsonConvert.SerializeObject(oldModel), JsonConvert.SerializeObject(model)));
            }

            return RedirectToAction("Index", "WxPayConfig");
        }

        /// <summary>
        /// 新增/修改
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult Delete(int id)
        {
            //判断支付宝信息是否已经存在支付记录
            if (CurrentUser == null)
            {
                return Json(new { Mess = "网页超时，请刷新网页重新登录" });
            }
            if (Smart.Instance.WeChatDetailBizService.IsExistsWeChatDetail(id))
            {
                return Json(new { Mess = "已经存在交易记录无法删除！" });
            }
            if (Smart.Instance.WxPayConfigBizService.Delete(id))
            {
                LogHelper.AlipayLog(string.Format("支付宝\r\n  删除人：{0} \r\n删除ID：{1}", CurrentUser.Name, id));
            }
            return Json(new { Mess = "success" });
        }
    }
}