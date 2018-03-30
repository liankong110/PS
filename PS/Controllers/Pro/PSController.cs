using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VisualSmart.BizService.Core;
using VisualSmart.Util;

namespace PS.Controllers.Pro
{
    public class PSController : Controller
    {
        // GET: PS
        public ActionResult Index(int page = 1)
        {
            var query = QueryCondition.Instance.AddOrderBy("Id", false).SetPager(page, 10);
            query.AddEqual("RowState", "1");
            ViewBag.Page = query.GetPager();
            return View(Smart.Instance.Pro_PSBizService.GetAllDomain(query));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SchedulingLineId">产线信息</param>
        /// <returns></returns>
        public ActionResult Edit(int SchedulingLineId,DateTime? SDate)
        {
            if (SDate == null)
            {
                SDate = DateTime.Now;
            }
            var model=VisualSmart.BizService.Core.Smart.Instance.Pro_PSBizService.GetPSBySchedulingLineId(SchedulingLineId);
            model.ProDate = SDate.Value;
            ViewBag.PSModel = model;
            var PSDetail = VisualSmart.BizService.Core.Smart.Instance.Pro_PSDetailBizService.GetPSDetailBySchedulingLineId(model,SchedulingLineId, SDate.Value);
            return View(PSDetail);
        }
    }
}