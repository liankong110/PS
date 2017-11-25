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
    public class AlipayController : BaseController
    {
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="AppName"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [ViewPageAttribute]
        public ActionResult Index(string AppName, int page = 1)
        {
            var query = QueryCondition.Instance.AddOrderBy("Id", false).SetPager(page, 10);
            if (!string.IsNullOrEmpty(AppName))
            {
                query.AddLike("AppName", AppName);
                ViewBag.AppName = AppName;
            }
            query.AddEqual("RowState", "1");
            ViewBag.Page = query.GetPager();
            var userList = Smart.Instance.AlipayBizService.GetAllDomain(query);          
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
            var model = new AlipayDomain();
            if (Id.HasValue)
            {
                var query = QueryCondition.Instance.AddEqual("Id", Id.Value.ToString());
                model = Smart.Instance.AlipayBizService.GetAllDomain(query).FirstOrDefault();
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
        public ActionResult Add(AlipayDomain model)
        {
            var query = QueryCondition.Instance;
            query.AddEqual("AppName", model.AppName);
            query.AddEqual("RowState", "1");
            if (model.Id > 0)
            {
                query.AddNotEqual("Id", model.Id.ToString());
            }
            if (Smart.Instance.AlipayBizService.GetAllDomain(query).Count > 0)
            {
                ViewBag.Error = string.Format("已经存在名称:{0},请重新填写", model.AppName);
                return View(model);
            }
            
            model.Updater = CurrentUser.Name;
            model.UpdateTime = DateTime.Now;
            if (model.Id == 0)
            {
                model.CreateTime = DateTime.Now;
                model.Creater = CurrentUser.Name;
                Smart.Instance.AlipayBizService.Add(model);
                ViewBag.Error = "1";
                return RedirectToAction("Add", "Alipay", new { Error = 1 });
            }
         
            ViewBag.Error = "1";
            var oldModel=Smart.Instance.AlipayBizService.GetAllDomain(QueryCondition.Instance.AddEqual("Id", model.Id.ToString()));
            if (Smart.Instance.AlipayDetailBizService.IsExistsAlipayDetail(model.Id))
            {
                model.APP_ID = oldModel[0].APP_ID;
                model.AppName = oldModel[0].AppName;                
            }

            if (Smart.Instance.AlipayBizService.Update(model))
            {
                LogHelper.AlipayLog(string.Format("支付宝\r\n 修改人：{0} \r\n 修改前：{1} \r\n 修改后：{2}",
                    CurrentUser.Name,JsonConvert.SerializeObject(oldModel), JsonConvert.SerializeObject(model)));
            }
            
            return RedirectToAction("Index", "Alipay");
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
            if (Smart.Instance.AlipayDetailBizService.IsExistsAlipayDetail(id))
            {
                return Json(new { Mess = "已经存在交易记录无法删除！" });
            }
            if (Smart.Instance.AlipayBizService.Delete(id))
            {
                LogHelper.AlipayLog(string.Format("支付宝\r\n  删除人：{0} \r\n删除ID：{1}", CurrentUser.Name, id));
            }
            return Json(new { Mess = "success" });
        }
    }
}