using PS.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VisualSmart.BizService.Core;
using VisualSmart.Domain.Pro;
using VisualSmart.Domain.ProBase;
using VisualSmart.Util;

namespace PS.Controllers.Pro
{
    public class SchedulingController : BaseController
    {
        /// <summary>
        /// 第一步 选择：产线信息
        /// </summary>
        /// <returns></returns>
        [ViewPageAttribute]
        public ActionResult ProLines(int ShipPlanMainId, string ShipMainProNo)
        {
            ViewBag.ShipMainProNo = ShipMainProNo;
            var stockList = Smart.Instance.Base_StockMainBizService.GetAllDomain(QueryCondition.Instance.AddOrderBy("Id", false).AddEqual("RowState", "1").SetPager(1, 50));
            foreach (var model in stockList)
            {
                model.ProNo = model.ProNo + "[" + model.Creater + "]";
            }
            ViewBag.StockList = new SelectList(stockList, "Id", "ProNo");
            ViewBag.ShipPlanMainId = ShipPlanMainId;
            return View(Smart.Instance.Base_ProductionLineBizService.GetAllProLineNos(ShipPlanMainId));
        }
        /// <summary>
        /// 排产
        /// </summary>
        /// <param name="proLineNosList">选择的产线信息</param>
        /// <param name="StockId">库存信息</param>
        /// <param name="ShipMainProNo">发运计划单号</param>
        /// <param name="Id"></param>
        /// <param name="MainId">排产单ID（修改时使用）</param>
        /// <param name="page"></param>
        /// <returns></returns>
        [ViewPageAttribute]
        public ActionResult Index(string proLineNosList, string StockId, string ShipMainProNo, int ShipPlanMainId = 0,
            int Id = 0, int MainId = 0, int page = 1)
        {
            List<Pro_ShipPlan> shipPlanList = new List<Pro_ShipPlan>();
            List<Pro_ShipPlans> _shipPlansList = new List<Pro_ShipPlans>();
            List<Pro_SchedulingLine> pro_SchedulingLineList = new List<Pro_SchedulingLine>();
            var lingHourList = new List<Base_LineHour>();
            if (MainId == 0)
            {
                ViewBag.Id = MainId;

                //1.发运计划时间段
                var mainDate = Smart.Instance.Pro_ShipPlanMainBizService.GetAllDomain(QueryCondition.Instance.AddOrderBy("Id", false).AddEqual("Id", ShipPlanMainId.ToString()))[0];
                ViewBag.MainDate = mainDate;

                var query = QueryCondition.Instance.AddEqual("LineNos", proLineNosList).AddEqual("StockMainId", StockId)
                    .AddEqual("MainId", ShipPlanMainId.ToString());
                //2.发运计划产品信息
                shipPlanList = Smart.Instance.Pro_ShipPlanBizService.GetAllDomainByLineNos(query).ToList();

                //3.发运时间明细
                _shipPlansList = Smart.Instance.Pro_ShipPlansBizService.GetAllDomainByLineNos(query).ToList();

                ViewBag.ShipPlansList = _shipPlansList;

                //4 获取每条产线最大的工时
                lingHourList = Smart.Instance.Base_LineHourBizService.GetLineHourList(proLineNosList).ToList();

                //生产线
                var ProLineNos = proLineNosList.Replace("'", "").Split(',').ToList();
                foreach (var line in ProLineNos)
                {
                    var lineModel = new Pro_SchedulingLine();
                    lineModel.ProLineNo = line;
                    var hourModel = lingHourList.Find(t => t.ProLineNo == line);
                    if (hourModel != null)
                    {
                        lineModel.MaxHour = hourModel.MaxHours;
                    }
                    pro_SchedulingLineList.Add(lineModel);
                }

                ViewBag.ShipMainProNo = ShipMainProNo;
                ViewBag.SchedulingList = pro_SchedulingLineList;
                return View(shipPlanList);
            }
            ViewBag.Id = MainId;
            //发运计划产品信息
            var schedulingModel = Smart.Instance.Pro_SchedulingBizService.GetAllDomain(QueryCondition.Instance.AddEqual("Id", MainId.ToString())).ToList()[0];
            Pro_ShipPlanMain model = new Pro_ShipPlanMain
            {
                PlanFromDate = schedulingModel.PlanFromDate,
                PlanFromTo = schedulingModel.PlanToDate
            };
            ViewBag.MainDate = model;
            //获取所有产线信息
            pro_SchedulingLineList = Smart.Instance.Pro_SchedulingLineBizService.GetAllDomain(QueryCondition.Instance.AddEqual("MainId", MainId.ToString())).ToList();
            var temp_proLineNosList = "";
            foreach (var line in pro_SchedulingLineList)
            {
                temp_proLineNosList += string.Format("'{0}',", line.ProLineNo);
            }
            //4 获取每条产线最大的工时
            lingHourList = Smart.Instance.Base_LineHourBizService.GetLineHourList(temp_proLineNosList.Trim(',')).ToList();
            foreach (var line in pro_SchedulingLineList)
            {
                var hourModel = lingHourList.Find(t => t.ProLineNo == line.ProLineNo);
                if (hourModel != null)
                {
                    line.MaxHour = hourModel.MaxHours;
                }
            }

            ViewBag.SchedulingList = pro_SchedulingLineList;

            //发运计划产品信息
            shipPlanList = Smart.Instance.Pro_ShipPlanBizService.GetPro_SchedulingByEdit(MainId).ToList();

            //发运时间明细
            _shipPlansList = Smart.Instance.Pro_ShipPlansBizService.GetPro_SchedulingGoodsNumByEdit(MainId).ToList();
            ViewBag.ShipPlansList = _shipPlansList;

            ViewBag.ShipMainProNo = schedulingModel.ShipMainProNo;
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
        public JsonResult GetProductions(string ProLineNo, string ShipMainProNo, string People)
        {
            if (string.IsNullOrEmpty(ProLineNo) || string.IsNullOrEmpty(ShipMainProNo) || string.IsNullOrEmpty(People))
            {
                return Json(new { Mess = "fail" });
            }
            var list = Smart.Instance.Base_ProductionLinesBizService.GetAllDomainByLineNoAndGoodNos(QueryCondition.Instance
                .AddEqual("LineNo", ProLineNo)
                .AddEqual("ShipMainProNo", ShipMainProNo)
                .AddEqual("People", People));
            return Json(new { Mess = "success", Data = list });
        }
        /// <summary>
        /// 1.1新增主表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult AddPro_Scheduling(Pro_Scheduling model)
        {
            if (model.Id == 0)
            {
                model.Creater = model.Updater = CurrentUser.Name;
                model.RowState = 1;
                return Json(new { Mess = "success", Id = Smart.Instance.Pro_SchedulingBizService.AddGetId(model) });
            }
            Smart.Instance.Pro_SchedulingBizService.Update(model);
            return Json(new { Mess = "success", Id = model.Id });
        }

        /// <summary>
        /// 1.2新增主表-
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult AddPro_SchedulingLine(Pro_SchedulingLine model)
        {
            model.RowState = 1;
            return Json(new { Mess = "success", Id = Smart.Instance.Pro_SchedulingLineBizService.AddGetId(model) });
        }

        /// <summary>
        /// 1.3新增主表-
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult AddPro_SchedulingGoods(string model, int SLineId, DateTime Date)
        {
            DateTime begin = DateTime.Now;
            var allGoodsList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<List<string>>>(model);
            var goodsNumBizService = Smart.Instance.Pro_SchedulingGoodsNumBizService;
            var schedulingGoodsBizService = Smart.Instance.Pro_SchedulingGoodsBizService;
            DataTable dt = GetTableSchema();
            for (int i = 0; i < allGoodsList.Count; i = i + 4)
            {
                var xuqiu = allGoodsList[i];
                var zao = allGoodsList[i + 1];
                var zhong = allGoodsList[i + 2];
                var wan = allGoodsList[i + 3];

                var morningNum = zao[5] == null ? 0 : Convert.ToInt32(zao[5]);
                var middleNum = zhong[5] == null ? 0 : Convert.ToInt32(zhong[5]);
                var eveningNum = wan[5] == null ? 0 : Convert.ToInt32(wan[5]);

                var goodModel = new Pro_SchedulingGoods();
                goodModel.SLineId = SLineId;
                goodModel.GoodNo = zhong[0];
                goodModel.GoodName = zhong[1];
                goodModel.ShipTo = zhong[2];
                goodModel.ShipToName = zhong[3];
                goodModel.StockNum = Convert.ToInt32(zhong[6]);
                goodModel.PackNum = zhong[30] == "" ? 0 : Convert.ToInt32(zhong[30]);
                goodModel.MorningNum = morningNum;
                goodModel.MiddleNum = middleNum;
                goodModel.EveningNum = eveningNum;
                goodModel.ParentGoodNo = zhong[28];
                goodModel.ParentGoodName = zhong[29];


                var id = schedulingGoodsBizService.AddGetId(goodModel);
                for (var colIndex = 7; colIndex < 28; colIndex++)
                {
                    var xu_val = 0;
                    int.TryParse(xuqiu[colIndex], out xu_val);

                    //4.保存 产线对应日期的数量
                    if (xu_val != 0)
                    {
                        DataRow r = dt.NewRow();
                        r[0] = id;
                        r[1] = 1;
                        r[2] = Date.AddDays(colIndex - 7);
                        r[3] = xu_val;
                        dt.Rows.Add(r);
                    }
                    var zao_val = 0;
                    int.TryParse(zao[colIndex], out zao_val);
                    if (zao_val != 0)
                    {
                        DataRow r = dt.NewRow();
                        r[0] = id;
                        r[1] = 2;
                        r[2] = Date.AddDays(colIndex - 7);
                        r[3] = zao_val;
                        dt.Rows.Add(r);
                    }
                    var zhong_val = 0;
                    int.TryParse(zhong[colIndex], out zhong_val);
                    if (zhong_val != 0)
                    {
                        DataRow r = dt.NewRow();
                        r[0] = id;
                        r[1] = 3;
                        r[2] = Date.AddDays(colIndex - 7);
                        r[3] = zhong_val;
                        dt.Rows.Add(r);
                    }
                    var wan_val = 0;
                    int.TryParse(wan[colIndex], out wan_val);
                    if (wan_val != 0)
                    {
                        DataRow r = dt.NewRow();
                        r[0] = id;
                        r[1] = 4;
                        r[2] = Date.AddDays(colIndex - 7);
                        r[3] = wan_val;
                        dt.Rows.Add(r);
                    }
                }

            }


            //DataTable dt = GetTableSchema();
            //foreach (var good in model)
            //{
            //    var id = 0;// schedulingGoodsBizService.AddGetId(good);

            //    if (good.Items != null)
            //    {

            //        foreach (var item in good.Items)
            //        {
            //            item.SGoodId = id;
            //            item.SDate = good.Date.AddDays(item.Index);                     

            //            DataRow r = dt.NewRow();
            //            r[0] = item.SGoodId;
            //            r[1] = item.SType;
            //            r[2] = item.SDate;
            //            r[3] = item.SNum;
            //            dt.Rows.Add(r);
            //        }                   
            //    }
            //}
            goodsNumBizService.BatchAdd(dt);
            var endTime = DateTime.Now;
            TimeSpan ts = endTime - begin;
            LogHelper.WriteLog(string.Format("用时：{0}s,数据：{1},{2}-{3}", ts.TotalSeconds, dt.Rows.Count, begin, endTime));
            return Json(new { Mess = "success" });
        }

        private DataTable GetTableSchema()
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[]{
            new DataColumn("SGoodId",typeof(int)),
            new DataColumn("SType",typeof(int)),
            new DataColumn("SDate",typeof(DateTime)),
            new DataColumn("SNum",typeof(int))});
            return dt;
        }
        /// <summary>
        /// 产线主表列表
        /// </summary>
        /// <returns></returns>
        [ViewPageAttribute]
        public ActionResult List(string ProNo, string ShipMainProNo,string GoodNo,string GoodName,
            string LineNo,string ShipTo, DateTime? Time, int page = 1)
        {
            Hashtable hs = new Hashtable();
            var query = QueryCondition.Instance.AddOrderBy("Id", false).SetPager(page, 10);
            query.AddEqual("RowState", "1");
            if (!string.IsNullOrEmpty(ProNo))
            {
                query.AddLike("ProNo", ProNo);
                ViewBag.ProNo = ProNo;
            }
            if (!string.IsNullOrEmpty(ShipMainProNo))
            {
                query.AddLike("ShipMainProNo", ShipMainProNo);
                ViewBag.ShipMainProNo = ShipMainProNo;
            }
            if (Time != null)
            {
                hs.Add("PlanFromDate", Time.Value.ToString("yyy-MM-dd"));
                ViewBag.Time = Time.Value.ToString("yyy-MM-dd");
            }
            if (!string.IsNullOrEmpty(GoodNo))
            {
                hs.Add("GoodNo", GoodNo);
                ViewBag.GoodNo = GoodNo;
            }
            if (!string.IsNullOrEmpty(GoodName))
            {
                hs.Add("GoodName", GoodName);
                ViewBag.GoodName = GoodName;
            }
            if (!string.IsNullOrEmpty(LineNo))
            {
                hs.Add("LineNo", LineNo);
                ViewBag.LineNo = LineNo;
            }
            if (!string.IsNullOrEmpty(ShipTo))
            {
                hs.Add("ShipTo", ShipTo);
                ViewBag.ShipTo = ShipTo;
            }

            ViewBag.Page = query.GetPager();
            var list = Smart.Instance.Pro_SchedulingBizService.GetList(query, hs);
            return View(list);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult Delete(int id)
        {
            if (CurrentUser != null)
            {
                Smart.Instance.Pro_SchedulingBizService.Delete(id);
            }
            return Json(new { Mess = "success" });
        }

        /// <summary>
        /// 根据主表ID 来查询具体生产计划信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [ViewPageAttribute]
        public ActionResult DetailList(int id, string GoodNo, string GoodName, int page = 1)
        {
            var query = QueryCondition.Instance.AddEqual("MainId", id.ToString());
            if (!string.IsNullOrEmpty(GoodNo))
            {
                query.AddLike("GoodNo", GoodNo);
                ViewBag.GoodNo = GoodNo;
            }
            if (!string.IsNullOrEmpty(GoodName))
            {
                query.AddLike("GoodName", GoodName);
                ViewBag.GoodName = GoodName;
            }
            var list = Smart.Instance.Pro_SchedulingGoodsBizService.GetDetailList(query);
            ViewBag.GoodNumlist = Smart.Instance.Pro_SchedulingGoodsNumBizService.GetDetailList(query).ToList();
            ViewBag.SchedulingModel = Smart.Instance.Pro_SchedulingBizService.GetAllDomain(QueryCondition.Instance.AddEqual("Id", id.ToString()))[0];
            ViewBag.id = id;
            return View(list);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult CreateNextBomList(int Id)
        {
            if (CurrentUser != null)
            {
                int resultId = Smart.Instance.Pro_SchedulingBizService.CreateNextBomList(QueryCondition.Instance.AddEqual("MainId", Id.ToString()), CurrentUser.Name);
                string ShipMainProNo = "";
                if (resultId != -1)
                {
                    ShipMainProNo = Smart.Instance.Pro_ShipPlanMainBizService.GetAllDomain(QueryCondition.Instance.AddEqual("Id", resultId.ToString()))[0].ProNo;
                }
                return Json(new { Mess = "success", ResultId = resultId, ShipMainProNo = ShipMainProNo });
            }
            return Json(new { Mess = "fail" });
        }

    }
}