using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VisualSmart.BizService.Core;
using VisualSmart.Domain.Pro;
using VisualSmart.Util;

namespace PS.Controllers.Pro
{
    /// <summary>
    /// 发运计划单
    /// </summary>
    public class ShipPlanController : BaseController
    {

        /// <summary>
        /// 发运计划主表列表
        /// </summary>
        /// <param name="ProNo"></param>
        /// <param name="PlanDate"></param>
        /// <param name="IsGuoQi">0 不过期  1 过去 -1 全部</param>
        /// <param name="page"></param>
        /// <returns></returns>
        [ViewPageAttribute]
        public ActionResult MainList(string ProNo, DateTime? PlanDate,int IsGuoQi=0, int page = 1)
        {
            Hashtable hs = new Hashtable();
            var query = QueryCondition.Instance.AddOrderBy("Id", false).SetPager(page, 10);
            query.AddEqual("RowState", "1");
            if (!string.IsNullOrEmpty(ProNo))
            {
                query.AddLike("ProNo", ProNo);
                ViewBag.ProNo = ProNo;
            }
            if (PlanDate != null)
            {
                hs.Add("PlanDate", PlanDate.Value.ToString("yyy-MM-dd"));
                ViewBag.PlanDate = PlanDate.Value.ToString("yyy-MM-dd");
            }
            if (IsGuoQi == 0)
            {
                query.AddEqualLarger("PlanFromTo", DateTime.Now.ToString("yyyy-MM-dd"));
            }
            if (IsGuoQi == 1)
            {
                query.AddSmaller("PlanFromTo", DateTime.Now.ToString("yyyy-MM-dd"));
            }
            ViewBag.IsGuoQi = IsGuoQi;
            ViewBag.Page = query.GetPager();
            var list = Smart.Instance.Pro_ShipPlanMainBizService.GetList(query,hs);
            return View(list);
        }

        /// <summary>
        /// 详细信息列表
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [ViewPageAttribute]
        public ActionResult Index(string GoodNo, string GoodName,int Id, int page = 1)
        {
            ViewBag.Id = Id;
            var query = QueryCondition.Instance.AddOrderBy("Id", true).SetPager(page, 100).AddEqual("MainId", Id.ToString());
           
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

            ViewBag.Page = query.GetPager();
            var list = Smart.Instance.Pro_ShipPlanBizService.GetAllDomain(query);

            var mainDate = Smart.Instance.Pro_ShipPlanMainBizService.GetAllDomain(QueryCondition.Instance.AddOrderBy("Id", true).AddEqual("Id", Id.ToString()))[0];
            ViewBag.MainDate = mainDate;

            var shipPlansList = Smart.Instance.Pro_ShipPlansBizService.GetAllDomainByMainId(Id, query);
            ViewBag.ShipPlansList = shipPlansList;

            return View(list);
        }


        /// <summary>
        /// Excel 导入数据
        /// </summary>
        /// <returns></returns>
        public ActionResult ImportData()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult Delete(int id)
        {
            if (CurrentUser != null)
            {
                Smart.Instance.Pro_ShipPlanMainBizService.Delete(id);
            }
            return Json(new { Mess = "success" });
        }

        public ActionResult UploadFile()
        {
            Response.Clear(); //清除所有之前生成的Response内容
            Response.Write(Upload("ShipPlan"));
            Response.End(); //停止Response后续写入动作，保证Response内只有我们写入内容
            return View();
        }
        /// <summary>
        /// 重载 解析Excel
        /// </summary>
        /// <param name="fileAddress"></param>
        /// <returns></returns>
        public override string LoadExcel(string fileAddress)
        {
            int rowIndex = 2;
            string error = "success";

            var _shipPlanBizService = Smart.Instance.Pro_ShipPlanBizService;
            var _shipPlansBizService = Smart.Instance.Pro_ShipPlansBizService;
            int mainId = 0;
           
            try
            {
                var goodsBizSer = Smart.Instance.Base_GoodsBizService;
                string strConn;
                strConn = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + fileAddress + "; Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1'";
                OleDbConnection conn = new OleDbConnection(strConn);
                conn.Open();
                DataTable sheetNames = conn.GetOleDbSchemaTable
                    (OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

                foreach (DataRow rowTable in sheetNames.Rows)
                {
                    string sheetName = rowTable["TABLE_NAME"].ToString();
                    using (conn = new OleDbConnection(strConn))
                    {
                        conn.Open();
                        OleDbCommand objCommand = new OleDbCommand(string.Format("select * from [" + sheetName + "]"), conn);
                        
                        Pro_ShipPlanMain mainModel = new Pro_ShipPlanMain();
                        using (OleDbDataReader dataReader = objCommand.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                if (dataReader.FieldCount < 30)
                                {
                                    error = "解析错误,Excel 列头必须是30列（9列+21天），请下载模板 进行对比！";
                                    break;
                                }
                                if (rowIndex == 2)
                                {                                   
                                    mainModel.ProNo = "20180401001";
                                    mainModel.Updater = mainModel.Creater = CurrentUser.Name;
                                    mainModel.RowState = 1;
                                    mainModel.PlanFromDate = Convert.ToDateTime(dataReader[9]);
                                    mainModel.PlanFromTo = Convert.ToDateTime(dataReader[9 + 20]);
                                    mainId = Smart.Instance.Pro_ShipPlanMainBizService.AddGetId(mainModel);
                                }
                                else if (rowIndex > 2)
                                {
                                    var model = new Pro_ShipPlan();
                                    model.ScheduleNo = dataReader[0].ToString();
                                    model.Term = Convert.ToInt32(dataReader[1]);
                                    model.EditionNo = dataReader[2].ToString();
                                    model.CityNo = dataReader[3].ToString();
                                    model.ShipDetailNo = dataReader[4].ToString();                                    
                                    model.ShipTo = dataReader[5].ToString();
                                    model.ShipToName = dataReader[6].ToString();
                                    model.GoodNo = dataReader[7].ToString();
                                    model.GoodName = dataReader[8].ToString();
                                    model.MainId = mainId;
                                    var shipId=_shipPlanBizService.AddGetId(model);
                                   
                                    for (int i = 9; i <= 9 + 20; i++)
                                    {
                                        var value = dataReader[i].ToString().Trim();
                                        if (!string.IsNullOrEmpty(value))
                                        {
                                            var sonModel = new Pro_ShipPlans();
                                            sonModel.PlanId = shipId;
                                            sonModel.PlanNum = Convert.ToInt32(value);
                                            sonModel.PlanDate = mainModel.PlanFromDate.AddDays(i-8);
                                            _shipPlansBizService.Add(sonModel);
                                        }
                                    }
                                    
                                }
                                rowIndex++;
                            }
                        }
                        conn.Close();
                    }
                    break;
                }
            }
            catch (Exception ex)
            {
                error = "Excel解析错误,成功" + (rowIndex - 1) + "条，错误行号：" + rowIndex + ",请检查 数据格式。";
                LogHelper.WriteLog("返货单导入错误", ex);

            }
            return error;
        }
    }
}