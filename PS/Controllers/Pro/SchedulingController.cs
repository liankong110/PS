﻿using PS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VisualSmart.BizService.Core;
using VisualSmart.Domain.Pro;
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
        public ActionResult ProLines(string ShipMainProNo)
        {
            ViewBag.ShipMainProNo = ShipMainProNo;
            ViewBag.StockList = new SelectList(Smart.Instance.Base_StockMainBizService.GetAllDomain(QueryCondition.Instance.AddOrderBy("Id", false).AddEqual("RowState", "1")), "Id", "ProNo");
            return View(Smart.Instance.Base_ProductionLineBizService.GetAllProLineNos());
        }
        /// <summary>
        /// 排产
        /// </summary>
        /// <param name="proLineNosList"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [ViewPageAttribute]
        public ActionResult Index(string proLineNosList, string StockId, string ShipMainProNo, int Id = 0, int page = 1)
        {
            ViewBag.Id = Id;
            var query = QueryCondition.Instance.AddEqual("LineNos", proLineNosList).AddEqual("StockMainId", StockId);
            //发运计划产品信息
            var shipPlanList = Smart.Instance.Pro_ShipPlanBizService.GetAllDomainByLineNos(query).ToList();

            //发运计划时间段
            var mainDate = Smart.Instance.Pro_ShipPlanMainBizService.GetAllDomain(QueryCondition.Instance.AddOrderBy("Id", false))[0];
            ViewBag.MainDate = mainDate;

            //发运时间明细
            var shipPlansList = Smart.Instance.Pro_ShipPlansBizService.GetAllDomainByLineNos(query);
            ViewBag.ShipPlansList = shipPlansList;

            //生产线
            ViewBag.ProLineNos = proLineNosList.Replace("'", "").Split(',').ToList();
            ViewBag.ShipMainProNo = ShipMainProNo;
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
        /// 新增主表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult AddPro_Scheduling(Pro_Scheduling model)
        {
            if (model.Id == 0)
            {
                model.ProNo = "20180410001";
                model.Creater = model.Updater = CurrentUser.Name;
                return Json(new { Mess = "success", Id = Smart.Instance.Pro_SchedulingBizService.AddGetId(model) });
            }
            Smart.Instance.Pro_SchedulingBizService.Update(model);
            return Json(new { Mess = "success", Id = model.Id });
        }

        /// <summary>
        /// 新增主表-
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult AddPro_SchedulingLine(Pro_SchedulingLine model)
        {
            return Json(new { Mess = "success", Id = Smart.Instance.Pro_SchedulingLineBizService.AddGetId(model) });
        }

        /// <summary>
        /// 新增主表-
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
                goodModel.PackNum = 0;
                goodModel.MorningNum = morningNum;
                goodModel.MiddleNum = middleNum;
                goodModel.EveningNum = eveningNum;
                var id = schedulingGoodsBizService.AddGetId(goodModel);
                for (var colIndex = 7; colIndex <28; colIndex++)
                {
                    var xu_val = xuqiu[colIndex];
                    //4.保存 产线对应日期的数量
                    if (xu_val != null)
                    {
                        DataRow r = dt.NewRow();
                        r[0] = id;
                        r[1] = 1;
                        r[2] = Date.AddDays(colIndex - 7);
                        r[3] = xu_val;
                        dt.Rows.Add(r);
                    }
                    var zao_val = zao[colIndex];
                    if (zao_val != null)
                    {
                        DataRow r = dt.NewRow();
                        r[0] = id;
                        r[1] = 2;
                        r[2] = Date.AddDays(colIndex - 7);
                        r[3] = zao_val;
                        dt.Rows.Add(r);
                    }
                    var zhong_val = zhong[colIndex];
                    if (zhong_val != null)
                    {
                        DataRow r = dt.NewRow();
                        r[0] = id;
                        r[1] = 3;
                        r[2] = Date.AddDays(colIndex - 7);
                        r[3] = zhong_val;
                        dt.Rows.Add(r);
                    }
                    var wan_val = wan[colIndex];
                    if (wan_val != null)
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
        public ActionResult DetailList(int id, int page = 1)
        {
            var query = QueryCondition.Instance.AddEqual("MainId", id.ToString());
            var list = Smart.Instance.Pro_SchedulingGoodsBizService.GetDetailList(query);
            ViewBag.GoodNumlist = Smart.Instance.Pro_SchedulingGoodsNumBizService.GetDetailList(query).ToList();

            ViewBag.SchedulingModel = Smart.Instance.Pro_SchedulingBizService.GetAllDomain(QueryCondition.Instance.AddEqual("Id", id.ToString()))[0];

            return View(list);
        }
    }
}