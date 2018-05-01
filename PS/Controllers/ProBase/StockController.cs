using System;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web.Mvc;
using VisualSmart.BizService.Core;
using VisualSmart.BizService.ProBase;
using VisualSmart.Domain.ProBase;
using VisualSmart.Util;
using System.IO;
using System.Collections.Generic;
using System.Collections;


namespace PS.Controllers.ProBase
{
    public class StockController : BaseController
    {
        /// <summary>
        /// 主表
        /// </summary>
        IBase_StockBizService _stockBizService = Smart.Instance.Base_StockBizService;

        /// <summary>
        /// 库存主表列表
        /// </summary>
        /// <returns></returns>
        [ViewPageAttribute]
        public ActionResult MainList(string ProNo,int page = 1)
        {
            var query = QueryCondition.Instance.AddOrderBy("Id", false).SetPager(page, 10);
            query.AddEqual("RowState", "1");
            if (!string.IsNullOrEmpty(ProNo))
            {
                query.AddLike("ProNo", ProNo);
                ViewBag.ProNo = ProNo;
            }
            ViewBag.Page = query.GetPager();
            var list = Smart.Instance.Base_StockMainBizService.GetAllDomain(query);
            return View(list);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult DeleteAll(int id)
        {
            if (CurrentUser != null)
            {
                Smart.Instance.Base_StockMainBizService.Delete(id);
            }
            return Json(new { Mess = "success" });
        }

        [ViewPageAttribute]
        public ActionResult Index(string GoodNo, string GoodName,string MainId, int page = 1)
        {
            var query = QueryCondition.Instance.AddOrderBy("Id", false).SetPager(page, 10);
            query.AddEqual("RowState", "1");
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
            query.AddEqual("MainId", MainId);
            ViewBag.MainId = MainId;
            
            ViewBag.Page = query.GetPager();
            var list = Smart.Instance.Base_StockBizService.GetAllDomain(query);
            return View(list);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Error"></param>
        /// <returns></returns>
        public ActionResult Add(int? Id, string Error)
        {
            ViewBag.Error = Error;
            var model = new Base_Stock();
            if (Id.HasValue)
            {
                var query = QueryCondition.Instance.AddEqual("Id", Id.Value.ToString());
                model = _stockBizService.GetAllDomain(query).FirstOrDefault();
            }
            AddOrUpdateBaseInfo(model);
            return View(model);
        }
        /// <summary>
        /// 新增/修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Add(Base_Stock model)
        {
            model.RowState = 1;
            model.Updater = model.Creater = CurrentUser.Name;

            var query = QueryCondition.Instance;
            query.AddEqual("GoodNo", model.GoodNo);          
            query.AddEqual("RowState", "1");
            if (model.Id > 0)
            {
                query.AddNotEqual("Id", model.Id.ToString());
            }
            if (_stockBizService.GetAllDomain(query).Count > 0)
            {
                ViewBag.Error = string.Format("已经存在相同的产品编号:{0},请重新填写", model.GoodNo);
                return View(model);
            } 

            if (model.Id == 0)
            {
               _stockBizService.Add(model);
                ViewBag.Error = "1";
                return RedirectToAction("Add", "Stock", new { Error = 1 });
            }
            ViewBag.Error = "1";
            _stockBizService.Update(model);           
            return RedirectToAction("Index", "Stock");
        }
       
        /// <summary>
        /// Excel 导入数据
        /// </summary>
        /// <returns></returns>
        public ActionResult ImportData()
        {
            return View();
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult Delete(int id)
        {
            if (CurrentUser != null)
            {
                _stockBizService.Delete(id);
            }
            return Json(new { Mess = "success" });
        }

        public ActionResult UploadFile()
        {
            //清除所有之前生成的Response内容
            Response.Clear();
            Response.Write(Upload("Stock"));
            //停止Response后续写入动作，保证Response内只有我们写入内容
            Response.End();
            return View();
        }
        /// <summary>
        /// 重载 解析Excel
        /// </summary>
        /// <param name="fileAddress"></param>
        /// <returns></returns>
        public override string LoadExcel(string fileAddress)
        {
            int mainId=0;
            string error = "";
            try
            {
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
                        int rowIndex = 1;
                        using (OleDbDataReader dataReader = objCommand.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                if (dataReader.FieldCount != 6)
                                {
                                    error = "Excel解析错误";
                                    break;
                                }
                                if (rowIndex == 1)
                                {
                                    Base_StockMain mianModel = new Base_StockMain();
                                    mianModel.Creater = CurrentUser.Name;
                                    mianModel.Updater = CurrentUser.Name;
                                    mianModel.RowState = 1;
                                    mainId =Smart.Instance.Base_StockMainBizService.AddGetId(mianModel);
                                }
                                //if (rowIndex > 1)
                                //{
                                    var model = new Base_Stock();
                                    model.Location = dataReader[0].ToString();
                                    model.WH = dataReader[1].ToString();
                                    model.GoodNo = dataReader[2].ToString();
                                    model.GoodName = dataReader[3].ToString();
                                    model.Qty =Convert.ToInt32(dataReader[4]);
                                    model.Batch = dataReader[5].ToString();
                                    model.MainId = mainId;
                                    model.Updater = model.Creater = CurrentUser.Name;
                                    model.RowState = 1;
                                    _stockBizService.Add(model);                                   
                                //}
                                rowIndex++;
                            }
                        }
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                error = "Excel解析错误";

            }
            return error;
        }
    }
}