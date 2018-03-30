using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VisualSmart.BizService.Core;
using VisualSmart.Util;

namespace PS.Controllers.Pro
{
    public class ShipPlanController : Controller
    {

        /// <summary>
        /// 发运计划主表列表
        /// </summary>
        /// <returns></returns>
        [ViewPageAttribute]
        public ActionResult MainList(int page = 1)
        {
            var query = QueryCondition.Instance.AddOrderBy("Id", false).SetPager(page, 10);
            query.AddEqual("RowState", "1");
            ViewBag.Page = query.GetPager();
            var list = Smart.Instance.Pro_ShipPlanMainBizService.GetAllDomain(query);
            return View(list);
        }
        /// <summary>
        /// 详细信息列表
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [ViewPageAttribute]
        public ActionResult Index(int page = 1)
        {
            var query = QueryCondition.Instance.AddOrderBy("Id", false).SetPager(page, 10);
            query.AddEqual("RowState", "1");
            ViewBag.Page = query.GetPager();
            var list = Smart.Instance.Pro_ShipPlanBizService.GetAllDomain(query);

            var mainDate = Smart.Instance.Pro_ShipPlanMainBizService.GetAllDomain(QueryCondition.Instance.AddOrderBy("Id", false))[0];
            ViewBag.MainDate = mainDate;

            var shipPlansList = Smart.Instance.Pro_ShipPlansBizService.GetAllDomain(QueryCondition.Instance.AddOrderBy("Id", false));
            ViewBag.ShipPlansList = shipPlansList;

            return View(list);
        }
    }
}