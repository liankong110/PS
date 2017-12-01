using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VisualSmart.BizService.Core;
using VisualSmart.Util;

namespace PS.Controllers.Pro
{
    public class SchedulingController : Controller
    {
        /// <summary>
        /// 第一步 选择：产线信息
        /// </summary>
        /// <returns></returns>
        [ViewPageAttribute]
        public ActionResult ProLines()
        {
            return View(Smart.Instance.Base_ProductionLineBizService.GetAllProLineNos());
        }

        [ViewPageAttribute]
        public ActionResult Index(string proLineNosList, int page = 1)
        {        
            var query = QueryCondition.Instance.AddEqual("LineNos", proLineNosList);
            //发运计划产品信息
            var shipPlanList = Smart.Instance.Pro_ShipPlanBizService.GetAllDomainByLineNos(query);

            //发运计划时间段
            var mainDate = Smart.Instance.Pro_ShipPlanMainBizService.GetAllDomain(QueryCondition.Instance.AddOrderBy("Id", false))[0];
            ViewBag.MainDate = mainDate;

            //发运时间明细
            var shipPlansList = Smart.Instance.Pro_ShipPlansBizService.GetAllDomainByLineNos(query);
            ViewBag.ShipPlansList = shipPlansList;

            //生产线
            ViewBag.ProLineNos = proLineNosList.Replace("'","").Split(',').ToList();

            return View(shipPlanList);
        }



        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult GetProductions(string ProLineNo, string GoodNos,string People)
        {
            if (string.IsNullOrEmpty(ProLineNo) || string.IsNullOrEmpty(GoodNos) || string.IsNullOrEmpty(People))
            {
                return Json(new { Mess = "fail" });
            }            
            var list=Smart.Instance.Base_ProductionLinesBizService.GetAllDomainByLineNoAndGoodNos(QueryCondition.Instance
                .AddEqual("LineNo", ProLineNo)
                .AddEqual("GoodNos", GoodNos)
                .AddEqual("People", People));
            return Json(new { Mess = "success",Data= list });
        }
    }
}