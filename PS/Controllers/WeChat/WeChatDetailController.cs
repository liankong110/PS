using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VisualSmart.BizService.Core;
using VisualSmart.Domain.WeChat;
using VisualSmart.Util;

namespace PS.Controllers.WeChat
{
    public class WeChatDetailController : BaseController
    {
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="AppName"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [ViewPageAttribute]
        public ActionResult Index(string SceneryName, string Out_trade_no, string Open_id, string PayDateFrom, string PayDateTo,
            string PlayDateFrom, string PlayDateTo,
            string cboStatus, string SerialId, string BatchNumber, int page = 1, int isExcel = 0)
        {
            var query = QueryCondition.Instance.AddOrderBy("Id", false);
            if (!string.IsNullOrEmpty(SceneryName))
            {
                query.AddLike("SceneryName", SceneryName);
                ViewBag.SceneryName = SceneryName;
            }
            if (!string.IsNullOrEmpty(Out_trade_no))
            {
                query.AddLike("Out_trade_no", Out_trade_no);
                ViewBag.Out_trade_no = Out_trade_no;
            }
            
            if (!string.IsNullOrEmpty(SerialId))
            {
                query.AddLike("SerialId", SerialId);
                ViewBag.SerialId = SerialId;
            }
            if (!string.IsNullOrEmpty(BatchNumber))
            {
                query.AddLike("BatchNumber", BatchNumber);
                ViewBag.BatchNumber = BatchNumber;
            }
            if (!string.IsNullOrEmpty(BatchNumber))
            {
                query.AddLike("BatchNumber", BatchNumber);
                ViewBag.BatchNumber = BatchNumber;
            }
            if (cboStatus == "1")//支付交易
            {
                query.AddEqualLarger("Total_fee", "0");
                ViewBag.Status = cboStatus;
            }
            if (cboStatus == "2")//退款交易
            {
                query.AddSmaller("Total_fee", "0");
                ViewBag.Status = cboStatus;
            }

            if (!string.IsNullOrEmpty(PayDateFrom))
            {
                query.AddEqualLarger("Time_end", PayDateFrom);
                ViewBag.PayDateFrom = PayDateFrom;
            }
            //else
            //{
            //    query.AddEqualLarger("Send_pay_date", DateTime.Now.ToString("yyyy-MM-dd"));
            //    ViewBag.PayDateFrom = DateTime.Now.ToString("yyyy-MM-dd");
            //}

            if (!string.IsNullOrEmpty(PayDateTo))
            {
                query.AddEqualSmaller("Time_end", Convert.ToDateTime(PayDateTo).AddDays(1).ToString("yyyy-MM-dd"));
                ViewBag.PayDateTo = PayDateTo;
            }
            //else
            //{
            //    query.AddEqualSmaller("Send_pay_date", DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));
            //    ViewBag.PayDateTo = DateTime.Now.ToString("yyyy-MM-dd");
            //}


            if (!string.IsNullOrEmpty(PlayDateFrom))
            {
                query.AddEqualLarger("WeChatPlayDate", PlayDateFrom);
                ViewBag.PlayDateFrom = PlayDateFrom;
            }
            else
            {
                query.AddEqualLarger("WeChatPlayDate", DateTime.Now.ToString("yyyy-MM-dd"));
                ViewBag.PlayDateFrom = DateTime.Now.ToString("yyyy-MM-dd");
            }

            if (!string.IsNullOrEmpty(PlayDateTo))
            {
                query.AddEqualSmaller("WeChatPlayDate", Convert.ToDateTime(PlayDateTo).ToString("yyyy-MM-dd"));
                ViewBag.PlayDateTo = PlayDateTo;
            }
            else
            {
                query.AddEqualSmaller("WeChatPlayDate", DateTime.Now.ToString("yyyy-MM-dd"));
                ViewBag.PlayDateTo = DateTime.Now.ToString("yyyy-MM-dd");
            }
            query.AddEqual("RowState", "1");

            if (isExcel == 1)
            {
                //OutExcelToList(Smart.Instance.WeChatDetailBizService.GetAllDomain(query));
            }

            ViewBag.Page = query.SetPager(page, 10).GetPager();
            var userList = Smart.Instance.WeChatDetailBizService.GetAllDomain(query);

            return View(userList);
        }

        ///// <summary>
        ///// 导出企业证件信息表
        ///// </summary>
        ///// <param name="weChatDetailList">数据源</param>
        ///// <param name="sheetName">EXCEL标题名称</param>
        //private void OutExcelToList(IList<WeChatDetailDomain> weChatDetailList)
        //{
        //    var hssfworkbook = new HSSFWorkbook();
        //    var dsi = PropertySetFactory.CreateDocumentSummaryInformation();
        //    dsi.Company = "NPOI Team";
        //    hssfworkbook.DocumentSummaryInformation = dsi;
        //    SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
        //    si.Subject = "NPOI SDK Example";
        //    hssfworkbook.SummaryInformation = si;
        //    var sheet = hssfworkbook.CreateSheet("微信交易列表");
        //    //设置单元格式-居中
        //    var stylecenter = hssfworkbook.CreateCellStyle();
        //    stylecenter.VerticalAlignment = VerticalAlignment.CENTER;
        //    stylecenter.BorderTop = CellBorderType.THIN;
        //    stylecenter.BorderBottom = CellBorderType.THIN;
        //    stylecenter.BorderLeft = CellBorderType.THIN;
        //    stylecenter.BorderRight = CellBorderType.THIN;
        //    stylecenter.TopBorderColor = IndexedColors.BLACK.Index;
        //    stylecenter.BottomBorderColor = IndexedColors.BLACK.Index;
        //    stylecenter.LeftBorderColor = IndexedColors.BLACK.Index;
        //    stylecenter.RightBorderColor = IndexedColors.BLACK.Index;
        //    var columns = new List<string>
        //                      {
        //                          "景区名称",
        //                          "批次号",
        //                          "订单号",
        //                          "游玩日期",
        //                          "商户订单号",
        //                          "交易时间",
        //                          "交易金额", 
        //                          "用户标识",
        //                          "微信APPID",  
        //                          "微信支付订单号",       
        //                          "交易状态",     
                                 
        //                      };

        //    sheet.CreateRow(0).HeightInPoints = 40;
        //    AddRow(0, 0, 0, columns.Count, "微信交易列表", hssfworkbook, sheet, 13, NPOI.SS.UserModel.HorizontalAlignment.CENTER);


        //    //添加列名
        //    sheet.CreateRow(1);
        //    for (var i = 0; i < columns.Count; i++)
        //    {
        //        sheet.GetRow(1).CreateCell(i).SetCellValue(columns[i]);
        //        var cell = sheet.GetRow(1).GetCell(i);
        //        cell.CellStyle = stylecenter;
        //    }
        //    for (var i = 0; i < weChatDetailList.Count; i++)
        //    {
        //        sheet.CreateRow(2 + i);
        //        var detail = weChatDetailList[i];
        //        sheet.GetRow(2 + i).CreateCell(columns.IndexOf("景区名称")).SetCellValue(detail.SceneryName);
        //        sheet.GetRow(2 + i).CreateCell(columns.IndexOf("批次号")).SetCellValue(detail.BatchNumber);
        //        sheet.GetRow(2 + i).CreateCell(columns.IndexOf("订单号")).SetCellValue(detail.SerialId);
        //        sheet.GetRow(2 + i).CreateCell(columns.IndexOf("游玩日期")).SetCellValue(detail.WeChatPlayDate.ToString("yyyy-MM-dd"));
        //        sheet.GetRow(2 + i).CreateCell(columns.IndexOf("商户订单号")).SetCellValue(detail.Out_trade_no);
        //        sheet.GetRow(2 + i).CreateCell(columns.IndexOf("交易时间")).SetCellValue(detail.Time_end.ToString("yyyy-MM-dd hh:mm:ss"));
        //        sheet.GetRow(2 + i).CreateCell(columns.IndexOf("交易金额")).SetCellValue(detail.Total_fee.ToString());
        //        sheet.GetRow(2 + i).CreateCell(columns.IndexOf("用户标识")).SetCellValue(detail.Openid);
        //        sheet.GetRow(2 + i).CreateCell(columns.IndexOf("微信APPID")).SetCellValue(detail.AppId);

        //        sheet.GetRow(2 + i).CreateCell(columns.IndexOf("微信支付订单号")).SetCellValue(detail.Transaction_id);
        //        sheet.GetRow(2 + i).CreateCell(columns.IndexOf("交易状态")).SetCellValue(detail.Err_code_des);

        //        for (var j = 0; j < columns.Count; j++)
        //        {
        //            var cell = sheet.GetRow(2 + i).GetCell(j);
        //            cell.CellStyle = stylecenter;
        //        }
        //    }
        //    for (var i = 0; i < columns.Count; i++)
        //    {
        //        sheet.AutoSizeColumn(i);
        //    }
        //    sheet.SetColumnWidth(0, 20 * 256);
        //    sheet.SetColumnWidth(1, 22 * 256);
        //    sheet.SetColumnWidth(2, 20 * 256);
        //    sheet.SetColumnWidth(3, 20 * 256);
        //    sheet.SetColumnWidth(4, 30 * 256);

        //    sheet.SetColumnWidth(5, 20 * 256);
        //    sheet.SetColumnWidth(6, 20 * 256);
        //    sheet.SetColumnWidth(7, 20 * 256);
        //    sheet.SetColumnWidth(8, 15 * 256);
        //    sheet.SetColumnWidth(9, 15 * 256);
        //    sheet.ForceFormulaRecalculation = true;
        //    CommonMethod.WriteToFile(hssfworkbook, "微信交易列表");
        //}
        
    }
}