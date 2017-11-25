using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using VisualSmart.BizService.Core;
using VisualSmart.Domain.Alipay;
using VisualSmart.Domain.SceneryOrder;
using VisualSmart.Util;

namespace PS.Controllers.WeChat
{
    public class WeChatQRCodeController : BaseController
    {
       /// <summary>
       /// 
       /// </summary>
       /// <returns></returns>
        [ViewPageAttribute]
        public ActionResult Index(string SceneryName, string BatchNumber, string Phone, string CreateDateFrom, string CreateDateTo, string MachineCode, int page = 1)
        {
            var query = QueryCondition.Instance.AddOrderBy("Id", false).SetPager(page, 10);
            if (!string.IsNullOrEmpty(SceneryName))
            {
                query.AddLike("SceneryName", SceneryName);
                ViewBag.AppName = SceneryName;
            }
            if (!string.IsNullOrEmpty(BatchNumber))
            {
                query.AddLike("BatchNumber", BatchNumber);
                ViewBag.BatchNumber = BatchNumber;
            }
            if (!string.IsNullOrEmpty(Phone))
            {
                query.AddLike("Phone", Phone);
                ViewBag.Phone = Phone;
            }
            if (!string.IsNullOrEmpty(CreateDateFrom))
            {
                query.AddEqualLarger("CreateTime", CreateDateFrom);
                ViewBag.CreateDateFrom = CreateDateFrom;
            }
            else
            {
                query.AddEqualLarger("CreateTime", DateTime.Now.ToString("yyyy-MM-dd"));
                ViewBag.CreateDateFrom = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (!string.IsNullOrEmpty(CreateDateTo))
            {
                query.AddEqualSmaller("CreateTime", Convert.ToDateTime(CreateDateTo).AddDays(1).ToString("yyyy-MM-dd"));
                ViewBag.CreateDateTo = CreateDateTo;
            }
            else
            {
                query.AddEqualSmaller("CreateTime", DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));
                ViewBag.CreateDateTo = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (!string.IsNullOrEmpty(MachineCode))
            {
                query.AddLike("Creater", MachineCode);
                ViewBag.MachineCode = MachineCode;
            }
            query.AddEqual("RowState", "1");
            ViewBag.Page = query.GetPager();
            var qrCodeList = Smart.Instance.WeChatQRCodeBizService.GetAllDomain(query);
            return View(qrCodeList);
        }


        /// <summary>
        /// 申请退款
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult ApplayRefund(int id)
        {
          
            if (CurrentUser == null)
            {
                return Json(new { Mess ="系统超时，请刷新界面重试" });
            }
            var models = Smart.Instance.WeChatQRCodeBizService.GetAllDomain(QueryCondition.Instance.AddEqual("Id", id.ToString()).AddEqual("RowState", "1"));
            if (models.Count != 1)
            {
                return Json(new { Mess = "数据不存在，请刷新界面重试" });
            }

            var modelQRCode = models[0];
            var result = Smart.Instance.WeChatQRCodeBizService.CheckApplayRefund(id);
         
            // -1--已经存在申请记录
            // -2 --没有原始支付记录
            // -3--已经存在退款记录	 
            // 0--验证通过
            // -10系统异常
            if (result == 0)
            {
                var sceneryNameList = Smart.Instance.SceneryBizService.GetAllDomain(QueryCondition.Instance.AddEqual("Scenery", "SceneryName", modelQRCode.SceneryName).AddEqual("Scenery", "RowState", "1"));
                if (sceneryNameList.Count != 1)
                {
                    return Json(new { Mess = "景区名称不存在" });
                }
                //解析XML
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(modelQRCode.RequestJson);
                var xml = xmlDoc.SelectSingleNode("xml");
                decimal total_fee = Convert.ToDecimal(xml.SelectSingleNode("total_fee").InnerText); 

                SceneryOrderRefundDomain model = new SceneryOrderRefundDomain();
                model.Creater = CurrentUser.Name;
                model.CreateTime = DateTime.Now;
                model.ApprovalStatus = 0;
                model.BatchNumber = modelQRCode.BatchNumber;
                model.PlayDate = modelQRCode.CreateTime;
                model.RowState = 1;
                model.SceneryName = modelQRCode.SceneryName;
                model.SceneryRate = modelQRCode.SceneryRate;
                model.SceneryTCId = Convert.ToInt32(sceneryNameList[0].SceneryTCId);
                model.SerialId ="";
                model.Total = total_fee/100;
                model.PayType = 1;
                Smart.Instance.SceneryOrderRefundBizService.Add(model);
                return Json(new { Mess = "success" });   
            }

            if (result == -1)
            {
                return Json(new { Mess = "已经存在申请记录,不能申请！" });
            }
            if (result == -2)
            {
                return Json(new { Mess = "没有原始支付记录,不能申请！" });
            }
            if (result == -3)
            {
                return Json(new { Mess = "已经存在退款记录,不能申请！" });
            }
            if (result == -10)
            {
                return Json(new { Mess = "系统异常！" });
            }
            return Json(new { Mess = "" });
        }
    }
}