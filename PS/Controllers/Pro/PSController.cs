using NPOI.HPSF;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        public ActionResult Index(string ProNo, string LineNo, string DateFrom, string DateTo, string SchedulingProNo, int Type=0, int page = 1)
        {
            string title = "生产计划";
            Hashtable hs = new Hashtable();
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
                title += DateFrom;
                query.AddEqual("ProDate", DateFrom);
                ViewBag.DateFrom = DateFrom;
            }

            if (!string.IsNullOrEmpty(DateTo))
            {
                if (DateTo != DateFrom)
                {
                    if (!string.IsNullOrEmpty(DateFrom))
                    {
                        title += "到";
                    }
                    title += DateTo;
                }
                query.AddEqualSmaller("ProDate", DateTo);
                ViewBag.DateTo = DateTo;
                hs.Add("ProDateTo", DateTo);
            }

            if (!string.IsNullOrEmpty(SchedulingProNo))
            {
                query.AddLike("SchedulingProNo", SchedulingProNo);
                ViewBag.SchedulingProNo = SchedulingProNo;
            }
            if (Type == 1)//导出Excel
            {
                var exportList = Smart.Instance.Pro_PSDetailBizService.GetAllDomainToExcel(query, hs);
                string mapfilePath = ExportOrderDetailExcel(exportList, title);//路径

                string fileName = title + ".xlsx";

                return File(new FileStream(mapfilePath, FileMode.Open), MimeMapping.GetMimeMapping(fileName), fileName);
            }
            ViewBag.Page = query.GetPager();
            return View(Smart.Instance.Pro_PSBizService.GetAllDomain(query));
        }

        /// <summary>
        /// 网络订单列表-导出
        /// </summary>
        /// <param name="detailList"></param>
        /// <param name="sceneryName"></param>
        public string ExportOrderDetailExcel(IList<Pro_PSDetail> detailList, string title)
        {
            var columns = new List<string>
                {
                    "日期",
                    "零件号",
                    "零件名称",
                    "生产线",
                    "班次",
                    "客户（ship-to）",
                    "Qty",
                    "开始生产时间",
                    "结束生产时间"
                };

            IWorkbook workbook = new XSSFWorkbook();
            var dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "NPOI Team";
            var si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = "NPOI SDK Example";
            var sheetName = title;

            var sheet = workbook.CreateSheet(sheetName);

            //表头样式 加粗 浅灰色
            var headerStyle = workbook.CreateCellStyle();
            SetExcelBoderStyle(headerStyle);
            var headerFont = workbook.CreateFont();
            //headerFont.Boldweight = (short)FontBoldWeight.BOLD;
            headerStyle.SetFont(headerFont);
            //数据样式
            var allStyle = workbook.CreateCellStyle();
            //allStyle.BorderTop = BorderStyle.THIN;
            //allStyle.BorderBottom = BorderStyle.THIN;
            //allStyle.BorderLeft = BorderStyle.THIN;
            //allStyle.BorderRight = BorderStyle.THIN;
            //allStyle.TopBorderColor = IndexedColors.BLACK.Index;
            //allStyle.BottomBorderColor = IndexedColors.BLACK.Index;
            //allStyle.LeftBorderColor = IndexedColors.BLACK.Index;
            //allStyle.RightBorderColor = IndexedColors.BLACK.Index;

            sheet.CreateRow(0);

            for (var i = 0; i < columns.Count; i++)
            {
                sheet.GetRow(0).CreateCell(i).SetCellValue(columns[i]);
            }

            if (detailList != null && detailList.Count > 0)
            {
                for (var i = 0; i < detailList.Count; i++)
                {
                    sheet.CreateRow(1 + i);
                    var item = detailList[i];
                    sheet.GetRow(1 + i).CreateCell(0).SetCellValue(item.StartTime.ToString("yyyy/MM/dd"));
                 
                    sheet.GetRow(1 + i).CreateCell(1).SetCellValue(item.GoodNo);
                    sheet.GetRow(1 + i).CreateCell(2).SetCellValue(item.GoodName);
                    sheet.GetRow(1 + i).CreateCell(3).SetCellValue(item.ProLineNo);
                    sheet.GetRow(1 + i)
                         .CreateCell(4)
                         .SetCellValue(EnumOperate.GetEnumDesc((ClassType)item.SType));
                    sheet.GetRow(1 + i)
                         .CreateCell(5)
                         .SetCellValue(item.ShipTo); 
                    sheet.GetRow(1 + i)
                         .CreateCell(6)
                         .SetCellValue(item.Qty);
                    sheet.GetRow(1 + i).CreateCell(7).SetCellValue(item.StartTime.ToString("HH:mm:ss"));
                    sheet.GetRow(1 + i).CreateCell(8).SetCellValue(item.EndTime.ToString("HH:mm:ss")); 

                }
            }
            //设置列宽
            sheet.SetColumnWidth(0, 4600);
            sheet.SetColumnWidth(1, 4600);
            sheet.SetColumnWidth(2, 6200);
            sheet.SetColumnWidth(3, 2600);
            sheet.SetColumnWidth(4, 3600);
            sheet.SetColumnWidth(5, 3600);
            sheet.SetColumnWidth(6, 2600); //设置列宽  
            sheet.SetColumnWidth(7, 2600);
            sheet.SetColumnWidth(8, 4600);
            sheet.SetColumnWidth(9, 4600);

            sheet.ForceFormulaRecalculation = true;
            return CommonMethod.WriteToFileAndGetFileUrl(workbook, sheetName);
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
                PSDetail = Smart.Instance.Pro_PSDetailBizService.GetAllDomain(QueryCondition.Instance.AddOrderBy("Id", true).AddEqual("MainId", PSId.ToString()));
            }
            else
            {
                //新增
                if (SDate == null)
                {
                    SDate = DateTime.Now;
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

        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult Delete(int id)
        {
            if (CurrentUser != null)
            {
                Smart.Instance.Pro_PSBizService.Delete(id);
            }
            return Json(new { Mess = "success" });
        }

        public FileStreamResult ExportExcel(int id)
        {
            //编辑
            var model = Smart.Instance.Pro_PSBizService.GetAllDomain(QueryCondition.Instance.AddEqual("Id", id.ToString()))[0];
            //获取明细
            var PSDetail = Smart.Instance.Pro_PSDetailBizService.GetAllDomain(QueryCondition.Instance.AddOrderBy("Id", true).AddEqual("MainId", id.ToString()));
            string fileName = model.ProNo + "PS详细" + ".xlsx";
            string mapfilePath = ExportOrderDetailExcel(PSDetail, model, model.ProNo);//路径
            return File(new FileStream(mapfilePath, FileMode.Open), MimeMapping.GetMimeMapping(fileName), fileName);
        }

        /// <summary>
        /// 网络订单列表-导出
        /// </summary>
        /// <param name="detailList"></param>
        /// <param name="sceneryName"></param>
        public string ExportOrderDetailExcel(IList<Pro_PSDetail> detailList, Pro_PS model, string title)
        {
            var columns = new List<string>
                {
                    "序号",
                    "零件号",
                    "零件名称",
                    "班次",
                    "客户（ship-to）",
                    "整箱包装数",
                    "产能",
                    "数量",
                    "开始时间",
                    "结束时间",
                };

            IWorkbook workbook = new XSSFWorkbook();
            var dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "NPOI Team";
            var si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = "NPOI SDK Example";
            var sheetName = "PS详细";

            if (string.IsNullOrEmpty(title))
            {
                sheetName = title + "--" + sheetName;
            }
            else
            {
                sheetName = title + "--" + sheetName;
            }

            var sheet = workbook.CreateSheet(sheetName);

            //表头样式 加粗 浅灰色
            var headerStyle = workbook.CreateCellStyle();
            SetExcelBoderStyle(headerStyle);
            var headerFont = workbook.CreateFont();
            //headerFont.Boldweight = (short)FontBoldWeight.BOLD;
            headerStyle.SetFont(headerFont);
            //数据样式
            var allStyle = workbook.CreateCellStyle();
            //allStyle.BorderTop = BorderStyle.THIN;
            //allStyle.BorderBottom = BorderStyle.THIN;
            //allStyle.BorderLeft = BorderStyle.THIN;
            //allStyle.BorderRight = BorderStyle.THIN;
            //allStyle.TopBorderColor = IndexedColors.BLACK.Index;
            //allStyle.BottomBorderColor = IndexedColors.BLACK.Index;
            //allStyle.LeftBorderColor = IndexedColors.BLACK.Index;
            //allStyle.RightBorderColor = IndexedColors.BLACK.Index;

            sheet.CreateRow(0);
            sheet.CreateRow(1);
            sheet.CreateRow(2);
            sheet.CreateRow(3);
            sheet.CreateRow(4);


            //AddRow(0, 0, 0, 10, "生产日期:    " + model.ProDate.ToString("yyyy-MM-dd"), workbook, sheet, 13, HorizontalAlignment.LEFT);
            //AddRow(1, 1, 0, 10, "生  产  线:    " + model.ProLineNo, workbook, sheet, 13, NPOI.SS.UserModel.HorizontalAlignment.LEFT);
            //AddRow(2, 2, 0, 10, "早班人数:    " + (model.FinalMorningNum ?? 0).ToString(), workbook, sheet, 13, NPOI.SS.UserModel.HorizontalAlignment.LEFT);
            //AddRow(3, 3, 0, 10, "中班人数:    " + (model.FinalMiddleNum ?? 0).ToString(), workbook, sheet, 13, NPOI.SS.UserModel.HorizontalAlignment.LEFT);
            //AddRow(4, 4, 0, 10, "晚班人数:    " + (model.FinalEveningNum ?? 0).ToString(), workbook, sheet, 13, NPOI.SS.UserModel.HorizontalAlignment.LEFT);

            sheet.CreateRow(5);
            for (var i = 0; i < columns.Count; i++)
            {
                sheet.GetRow(5).CreateCell(i).SetCellValue(columns[i]);
                sheet.GetRow(5).GetCell(i).CellStyle = headerStyle;
            }
            sheet.GetRow(5).HeightInPoints = 18;
            if (detailList != null && detailList.Count > 0)
            {
                for (var i = 5; i < (detailList.Count + 5); i++)
                {
                    sheet.CreateRow(1 + i);
                    var item = detailList[i - 5];
                    sheet.GetRow(1 + i).CreateCell(0).SetCellValue(item.ProOrderIndex.ToString());
                    sheet.GetRow(1 + i).CreateCell(1).SetCellValue(item.GoodNo);
                    sheet.GetRow(1 + i).CreateCell(2).SetCellValue(item.GoodName);
                    sheet.GetRow(1 + i)
                         .CreateCell(3)
                         .SetCellValue(EnumOperate.GetEnumDesc((ClassType)item.SType));
                    sheet.GetRow(1 + i)
                         .CreateCell(4)
                         .SetCellValue(item.ShipTo);
                    sheet.GetRow(1 + i).CreateCell(5).SetCellValue(item.PackNum.ToString());
                    sheet.GetRow(1 + i).CreateCell(6).SetCellValue(item.ChanNeng.ToString());
                    sheet.GetRow(1 + i)
                         .CreateCell(7)
                         .SetCellValue(item.Qty.ToString());
                    sheet.GetRow(1 + i).CreateCell(8).SetCellValue(item.StartTime.ToString("yyyy-MM-dd HH:mm:ss"));
                    sheet.GetRow(1 + i).CreateCell(9).SetCellValue(item.EndTime.ToString("yyyy-MM-dd HH:mm:ss"));

                    for (var j = 0; j < columns.Count; j++)
                    {
                        sheet.GetRow(1 + i).GetCell(j).CellStyle = allStyle;
                    }
                }
            }
            //设置列宽
            sheet.SetColumnWidth(0, 1200);
            sheet.SetColumnWidth(1, 4600);
            sheet.SetColumnWidth(2, 5600);
            sheet.SetColumnWidth(3, 2600);
            sheet.SetColumnWidth(4, 3600);
            sheet.SetColumnWidth(5, 3600);
            sheet.SetColumnWidth(6, 2600); //设置列宽  
            sheet.SetColumnWidth(7, 2600);
            sheet.SetColumnWidth(8, 4600);
            sheet.SetColumnWidth(9, 4600);

            sheet.ForceFormulaRecalculation = true;
            return CommonMethod.WriteToFileAndGetFileUrl(workbook, sheetName);
        }
    }
}