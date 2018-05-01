using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VisualSmart.BizService.Core;
using VisualSmart.Domain.SceneryOrder;
using VisualSmart.Util;

namespace PS.Controllers.Alipay
{
    public class SceneryOrderRefundController : BaseController
    {
        [ViewPageAttribute]
        public ActionResult Index(SceneryOrderRefundDomain model, int isExcel = 0, int page = 1)
        {
            //审批状态
            var ApprovalStatusItems = new List<SelectListItem>
                {                   
                    new SelectListItem {Text = "全部", Value = "-1"},
                    new SelectListItem {Text = "未审核", Value = "0"},
                    new SelectListItem {Text = "已审核", Value = "1"},
                    new SelectListItem {Text = "已取消", Value = "2"},
                     new SelectListItem {Text = "未审核+已审核", Value = "3"}
                };
            ViewBag.ApprovalStatusItems = ApprovalStatusItems;
            Hashtable hsWhere = new Hashtable();
            var query = QueryCondition.Instance;
            if (!string.IsNullOrEmpty(model.SceneryName))
            {
                query.AddEqual("SceneryName", model.SceneryName);
            }
            if (!string.IsNullOrEmpty(model.BatchNumber))
            {
                query.AddLike("BatchNumber", model.BatchNumber);
            }
            if (!string.IsNullOrEmpty(model.SerialId))
            {
                query.AddLike("SerialId", model.SerialId);
            }
            if (model.ApprovalStatus != -1&&model.ApprovalStatus!=3)
            {
                query.AddEqual("ApprovalStatus", model.ApprovalStatus.ToString());
            }
            if (model.ApprovalStatus == 3)
            {              
                hsWhere.Add("ApprovalStatus","3");
            }
            if (!string.IsNullOrEmpty(model.PlayDateTo))
            {
                query.AddSmaller("PlayDate", Convert.ToDateTime(model.PlayDateTo).AddDays(1).ToString("yyyy-MM-dd"));
            }
            else
            {
                query.AddEqualSmaller("PlayDate", DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));   
                model.PlayDateTo = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (!string.IsNullOrEmpty(model.PlayDateFrom))
            {
                query.AddEqualLarger("PlayDate", model.PlayDateFrom);
            }
            else
            {
                query.AddEqualLarger("PlayDate", DateTime.Now.ToString("yyyy-MM-dd"));
                model.PlayDateFrom = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (isExcel == 1)
            {
                OutExcelToList(Smart.Instance.SceneryOrderRefundBizService.GetAllDomain(query, hsWhere));
            }
            query.SetPager(page, 10);
            var refundList = Smart.Instance.SceneryOrderRefundBizService.GetAllDomain(query, hsWhere);
            ViewBag.Page = query.GetPager();
            ViewBag.RefundList = refundList;
            return View(model);
        }


        /// <summary>
        /// 订单退款审批列表
        /// </summary>
        /// <param name="refundList">数据源</param>      
        private void OutExcelToList(IList<SceneryOrderRefundDomain> refundList)
        {
            var hssfworkbook = new HSSFWorkbook();
            var dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "NPOI Team";
            hssfworkbook.DocumentSummaryInformation = dsi;
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = "NPOI SDK Example";
            hssfworkbook.SummaryInformation = si;
            var sheet = hssfworkbook.CreateSheet("订单退款审批列表");
            //设置单元格式-居中
            var stylecenter = hssfworkbook.CreateCellStyle();
            stylecenter.VerticalAlignment = VerticalAlignment.CENTER;
            //stylecenter.BorderTop = CellBorderType.THIN;
            //stylecenter.BorderBottom = CellBorderType.THIN;
            //stylecenter.BorderLeft = CellBorderType.THIN;
            //stylecenter.BorderRight = CellBorderType.THIN;
            stylecenter.TopBorderColor = IndexedColors.BLACK.Index;
            stylecenter.BottomBorderColor = IndexedColors.BLACK.Index;
            stylecenter.LeftBorderColor = IndexedColors.BLACK.Index;
            stylecenter.RightBorderColor = IndexedColors.BLACK.Index;
            var columns = new List<string>
                              {
                                  "景区名称",
                                  "批次号",
                                  "订单号",
                                  "游玩时间",
                                  "退款金额",
                                  "审批状态",                                                                   
                              };


            sheet.CreateRow(0).HeightInPoints = 40;
            //AddRow(0, 0, 0, columns.Count, "订单退款审批列表", hssfworkbook, sheet, 12, NPOI.SS.UserModel.HorizontalAlignment.CENTER);


            //添加列名
            sheet.CreateRow(1);
            for (var i = 0; i < columns.Count; i++)
            {
                sheet.GetRow(1).CreateCell(i).SetCellValue(columns[i]);
                var cell = sheet.GetRow(1).GetCell(i);
                cell.CellStyle = stylecenter;
            }
            for (var i = 0; i < refundList.Count; i++)
            {
                sheet.CreateRow(2 + i);
                var refund = refundList[i];


                sheet.GetRow(2 + i).CreateCell(columns.IndexOf("景区名称")).SetCellValue(refund.SceneryName);
                sheet.GetRow(2 + i).CreateCell(columns.IndexOf("批次号")).SetCellValue(refund.BatchNumber);
                sheet.GetRow(2 + i).CreateCell(columns.IndexOf("订单号")).SetCellValue(refund.SerialId);
                sheet.GetRow(2 + i).CreateCell(columns.IndexOf("游玩时间")).SetCellValue(refund.PlayDate.ToString("yyyy-MM-dd"));
                sheet.GetRow(2 + i).CreateCell(columns.IndexOf("退款金额")).SetCellValue(refund.Total.ToString());
                sheet.GetRow(2 + i).CreateCell(columns.IndexOf("审批状态")).SetCellValue(refund.ApprovalStatusString);

                for (var j = 0; j < columns.Count; j++)
                {
                    var cell = sheet.GetRow(2 + i).GetCell(j);
                    cell.CellStyle = stylecenter;
                }
            }
            for (var i = 0; i < columns.Count; i++)
            {
                sheet.AutoSizeColumn(i);
            }

            sheet.SetColumnWidth(0, 25 * 256);
            sheet.SetColumnWidth(1, 25 * 256);
            sheet.SetColumnWidth(2, 25 * 256);
            sheet.SetColumnWidth(3, 25 * 256);
            sheet.SetColumnWidth(4, 25 * 256);

            sheet.ForceFormulaRecalculation = true;
            CommonMethod.WriteToFile(hssfworkbook, "订单退款审批列表");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ConfirmRefund(int id, int type=0)
        {
            if (CurrentUser != null)
            {
                string mess = "";
                if (type == (int)VisualSmart.Util.Menus.PayTypeMenu.Alipay)
                {
                    mess = Smart.Instance.AlipayTradeBizService.SceneryOrderRefund(id, CurrentUser);
                }
                if (type == (int)VisualSmart.Util.Menus.PayTypeMenu.WeChat)
                {
                    LogHelper.WeChatLog(string.Format("微信退款开始-》操作人：{0}", CurrentUser.Name));
                    mess = Smart.Instance.NativePay.SceneryOrderRefund(id, CurrentUser);
                }
                if (!string.IsNullOrEmpty(mess))
                {
                    return Json(new { Mess =mess});
                }
            }
            return Json(new { Mess = "success" });
        }

        /// <summary>
        ///  设置信息为 客户已经支付
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CancelRefund(string ids)
        {
            string mess = "";
            if (CurrentUser != null)
            {
                mess = Smart.Instance.SceneryOrderRefundBizService.CancelRefund(ids,CurrentUser) ? "success" : "fail";
            }
            return Json(new { Mess = mess });
        }
    }
}