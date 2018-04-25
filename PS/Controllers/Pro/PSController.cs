using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VisualSmart.BizService.Core;
using VisualSmart.Domain.Pro;
using VisualSmart.Util;
using VisualSmart.Util.Menus;

namespace PS.Controllers.Pro
{
    public class PSController : BaseController
    {
        // GET: PS
        public ActionResult Index(string ProNo, string LineNo, string DateFrom, string DateTo, int page = 1)
        {
            var query = QueryCondition.Instance.AddOrderBy("Id", false).SetPager(page, 10);
            query.AddEqual("RowState", "1");
            if (!string.IsNullOrEmpty(ProNo))
            {
                query.AddLike("ProNo", ProNo);
                ViewBag.ProNo = ProNo;
            }
            if (!string.IsNullOrEmpty(LineNo))
            {
                query.AddLike("ProLineNo", LineNo);
                ViewBag.LineNo = LineNo;
            }
            if (!string.IsNullOrEmpty(DateFrom))
            {
                query.AddEqualLarger("ProDate", DateFrom);
                ViewBag.DateFrom = DateFrom;
            }
            if (!string.IsNullOrEmpty(DateTo))
            {
                query.AddEqualSmaller("ProDate", DateTo);
                ViewBag.DateTo = DateTo;
            }
            ViewBag.Page = query.GetPager();
            return View(Smart.Instance.Pro_PSBizService.GetAllDomain(query));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SchedulingId">产线信息</param>
        /// <returns></returns>
        public ActionResult Edit(DateTime? SDate, string SchedulingProNo, int SchedulingId = 0, int PSId = 0)
        {
            IList<Pro_PSDetail> PSDetail = new List<Pro_PSDetail>();
            Pro_SchedulingLine fristLine = new Pro_SchedulingLine();
            Pro_PS model = new Pro_PS();
            if (PSId > 0)
            {
                //编辑
                model = Smart.Instance.Pro_PSBizService.GetAllDomain(QueryCondition.Instance.AddEqual("Id", PSId.ToString()))[0];
                //只加载一个产线，即 不能修改其他产线，下拉框只能选择一个
                List<Pro_SchedulingLine> scheduList = new List<Pro_SchedulingLine>();
                scheduList.Add(new Pro_SchedulingLine { Id = model.LineId, ProLineNo = model.ProLineNo });
                ViewBag.AllLine = new SelectList(scheduList, "Id", "ProLineNo", fristLine.Id);
                //获取明细
                PSDetail = Smart.Instance.Pro_PSDetailBizService.GetAllDomain(QueryCondition.Instance.AddOrderBy("Id", false).AddEqual("MainId", PSId.ToString()));
            }
            else
            {
                //新增
                if (SDate == null)
                {
                    SDate = Convert.ToDateTime("2017-11-07");// DateTime.Now; 测试
                }
                //1.获取所有产线信息
                var allLineList = Smart.Instance.Pro_SchedulingLineBizService.GetAllDomain(QueryCondition.Instance.AddOrderBy("Id", true).AddEqual("MainId", SchedulingId.ToString()));

                if (allLineList.Count > 0)
                {
                    fristLine = allLineList[0];
                    model = new Pro_PS
                    {
                        FinalEveningNum = fristLine.EveningShift,
                        FinalMiddleNum = fristLine.MiddleShift,
                        FinalMorningNum = fristLine.MorningShift,
                        ProLineNo = fristLine.ProLineNo,
                        ProDate = DateTime.Now,
                    };
                }
                model.SchedulingProNo = SchedulingProNo;
                model.ProDate = SDate.Value;
                ViewBag.AllLine = new SelectList(allLineList, "Id", "ProLineNo", fristLine.Id);
                PSDetail = Smart.Instance.Pro_PSDetailBizService.GetPSDetailBySchedulingLineId(model, fristLine, SDate.Value);
            }

            ViewBag.PSModel = model;

            return View(PSDetail);
        }

        /// <summary>
        /// 产线+时间 获取明细
        /// </summary>
        /// <param name="LineId"></param>
        /// <param name="SDate"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult GetGoodList(int LineId, DateTime SDate)
        {
            //1.获取所有产线信息
            Pro_SchedulingLine fristLine = Smart.Instance.Pro_SchedulingLineBizService.GetAllDomain(QueryCondition.Instance.AddOrderBy("Id", true).AddEqual("Id", LineId.ToString()))[0];
            Pro_PS model = new Pro_PS();
            model = new Pro_PS
            {
                FinalEveningNum = fristLine.EveningShift,
                FinalMiddleNum = fristLine.MiddleShift,
                FinalMorningNum = fristLine.MorningShift,
                ProLineNo = fristLine.ProLineNo,
                ProDate = DateTime.Now,
            };

            ViewBag.PSModel = model;
            var pSDetail = Smart.Instance.Pro_PSDetailBizService.GetPSDetailBySchedulingLineId(model, fristLine, SDate);

            return Json(new { PSModel = model, PSDetail = pSDetail });
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
        public JsonResult GetProductions(int LineId, DateTime ProDate, string People)
        {
            if (LineId <= 0 || ProDate == null || string.IsNullOrEmpty(People))
            {
                return Json(new { Mess = "fail" });
            }
            var list = Smart.Instance.Base_ProductionLinesBizService.GetAllDomainByScheduing(QueryCondition.Instance
                .AddEqual("LineId", LineId.ToString())
                .AddEqual("ProDate", ProDate.ToString("yyyy-MM-dd"))
                .AddEqual("People", People));
            return Json(new { Mess = "success", Data = list });
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult Save(string Pro_PS, string Pro_PSDetail)
        {
            var pSModel = Newtonsoft.Json.JsonConvert.DeserializeObject<Pro_PS>(Pro_PS);           
            pSModel.RowState = 1;
            pSModel.Creater = pSModel.Updater = CurrentUser.Name;
            var pSDetailList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Pro_PSDetail>>(Pro_PSDetail);
            var pro_PSDetailBizService = Smart.Instance.Pro_PSDetailBizService;
            if (pSModel.Id > 0)
            {
                Smart.Instance.Pro_PSBizService.Update(pSModel);
                foreach (var model in pSDetailList)
                {
                    if (model.STypeString == "早班")
                    {
                        model.SType = (int)ClassType.Morning;
                    }
                    else if (model.STypeString == "中班")
                    {
                        model.SType = (int)ClassType.Middle;
                    }
                    else if (model.STypeString == "晚班")
                    {
                        model.SType = (int)ClassType.Evening;
                    }
                    model.MainId = pSModel.Id;
                    pro_PSDetailBizService.Add(model);
                }
            }
            else
            {                 
                int mainId = Smart.Instance.Pro_PSBizService.AddGetId(pSModel);
                foreach (var model in pSDetailList)
                {
                    if (model.STypeString == "早班")
                    {
                        model.SType = (int)ClassType.Morning;
                    }
                    else if (model.STypeString == "中班")
                    {
                        model.SType = (int)ClassType.Middle;
                    }
                    else if (model.STypeString == "晚班")
                    {
                        model.SType = (int)ClassType.Evening;
                    }
                    model.MainId = mainId;
                    pro_PSDetailBizService.Add(model);
                }
            }
            return Json(new { Mess = "success" });
        }
    }
}