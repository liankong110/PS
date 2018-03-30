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
        /// <summary>
        /// 排产
        /// </summary>
        /// <param name="proLineNosList"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [ViewPageAttribute]
        public ActionResult Index(string proLineNosList, int page = 1)
        {
            var query = QueryCondition.Instance.AddEqual("LineNos", proLineNosList);
            //发运计划产品信息
            var shipPlanList = Smart.Instance.Pro_ShipPlanBizService.GetAllDomainByLineNos(query).ToList();

            for (int i = 0; i < 8; i++)
            {
                shipPlanList.AddRange(shipPlanList);
                if (shipPlanList.Count > 2000)
                {
                    break;
                }
            }

            //发运计划时间段
            var mainDate = Smart.Instance.Pro_ShipPlanMainBizService.GetAllDomain(QueryCondition.Instance.AddOrderBy("Id", false))[0];
            ViewBag.MainDate = mainDate;

            //发运时间明细
            var shipPlansList = Smart.Instance.Pro_ShipPlansBizService.GetAllDomainByLineNos(query);
            ViewBag.ShipPlansList = shipPlansList;

            //生产线
            ViewBag.ProLineNos = proLineNosList.Replace("'", "").Split(',').ToList();

            return View(shipPlanList); 
        }

        /// <summary>
        /// 获取产能，根据产线 商品 人数
        /// </summary>
        /// <param name="ProLineNo"></param>
        /// <param name="GoodNos"></param>
        /// <param name="People"></param>
        /// <returns></returns>

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


        /// <summary>
        /// 产线主表列表
        /// </summary>
        /// <returns></returns>
        [ViewPageAttribute]
        public ActionResult List(int page = 1)
        {
            var query = QueryCondition.Instance.AddOrderBy("Id", false).SetPager(page, 10);
            query.AddEqual("RowState", "1");
            ViewBag.Page = query.GetPager();
            var list = Smart.Instance.Pro_SchedulingBizService.GetAllDomain(query);
            return View(list);
        }

        /// <summary>
        /// 根据主表ID 来查询具体生产计划信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [ViewPageAttribute]
        public ActionResult DetailList(int id,int page = 1)
        {
            var query = QueryCondition.Instance.AddEqual("MainId", id.ToString()); 
            var list = Smart.Instance.Pro_SchedulingGoodsBizService.GetDetailList(query);
            ViewBag.GoodNumlist = Smart.Instance.Pro_SchedulingGoodsNumBizService.GetDetailList(query).ToList();

            ViewBag.SchedulingModel = Smart.Instance.Pro_SchedulingBizService.GetAllDomain(QueryCondition.Instance.AddEqual("Id", id.ToString()))[0];

            return View(list);
        }
    }
}