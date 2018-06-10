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
        public ActionResult MainList(string ProNo, int page = 1)
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
        public ActionResult Index(string GoodNo, string GoodName, string MainId, int page = 1)
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
        public ActionResult Add(int? Id,int MainId, string Error)
        {
            ViewBag.GoodList = Smart.Instance.Base_GoodsBizService.GetGoodName(QueryCondition.Instance.AddEqual("RowState", "1"));
            ViewBag.Error = Error;
            var model = new Base_Stock();
            if (Id.HasValue)
            {
                var query = QueryCondition.Instance.AddEqual("Id", Id.Value.ToString());
                model = _stockBizService.GetAllDomain(query).FirstOrDefault();
            }
            model.MainId = MainId;
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
            query.AddEqual("Batch", model.Batch);
            query.AddEqual("WH", model.WH);
            query.AddEqual("RowState", "1");
            if (model.Id > 0)
            {
                query.AddNotEqual("Id", model.Id.ToString());
            }
            query.AddEqual("MainId", model.MainId.ToString());
            
            if (_stockBizService.GetAllDomain(query).Count > 0)
            {
                ViewBag.GoodList = Smart.Instance.Base_GoodsBizService.GetGoodName(QueryCondition.Instance.AddEqual("RowState", "1"));
                ViewBag.Error = string.Format("已经存在相同的库位:{1},产品编号:{0},批次:{2},请重新填写", model.GoodNo,model.WH,model.Batch);
                return View(model);
            }

            if (model.Id == 0)
            {
                _stockBizService.Add(model);
                ViewBag.Error = "1";
                return RedirectToAction("Add", "Stock", new { Error = 1,MainId=model.MainId });
            }
            ViewBag.Error = "1";
            _stockBizService.Update(model);
            return RedirectToAction("Index", "Stock",new { MainId = model.MainId });
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
            int mainId = 0;
            int rowIndex = 2;
            string error = "success";
            try
            {
                string strConn;
                strConn = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + fileAddress + "; Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1'";
                OleDbConnection conn = new OleDbConnection(strConn);
                conn.Open();
                DataTable sheetNames = conn.GetOleDbSchemaTable
                    (OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                Hashtable ht = new Hashtable();
                foreach (DataRow rowTable in sheetNames.Rows)
                {
                    string sheetName = rowTable["TABLE_NAME"].ToString();
                    using (conn = new OleDbConnection(strConn))
                    {
                        conn.Open();
                        OleDbCommand objCommand = new OleDbCommand(string.Format("select * from [" + sheetName + "]"), conn);
                    
                        using (OleDbDataReader dataReader = objCommand.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                if (dataReader.FieldCount != 6)
                                {
                                    error = "解析错误,Excel 列头必须是6列，请下载模板 进行对比！";
                                    break;
                                }
                                if (rowIndex == 2)
                                {
                                    Base_StockMain mianModel = new Base_StockMain();
                                    mianModel.Creater = CurrentUser.Name;
                                    mianModel.Updater = CurrentUser.Name;
                                    mianModel.RowState = 1;
                                    mainId = Smart.Instance.Base_StockMainBizService.AddGetId(mianModel);
                                }
                                if (string.IsNullOrEmpty(dataReader[3].ToString()))
                                {
                                    continue;
                                }
                                
                                var model = new Base_Stock();
                                model.Location = dataReader[0].ToString();
                                model.WH = dataReader[1].ToString();
                                model.GoodNo = dataReader[2].ToString();
                                model.GoodName = dataReader[3].ToString();
                                var result = 0;
                                int.TryParse(dataReader[4].ToString(), out result);
                                model.Qty = result;
                                model.Batch = dataReader[5].ToString();
                                model.MainId = mainId;
                                model.Updater = model.Creater = CurrentUser.Name;
                                model.RowState = 1;
                              
                                if (!ht.ContainsKey(model.GoodNo+model.WH+model.Batch))
                                {
                                    _stockBizService.Add(model);
                                    ht.Add(model.GoodNo + model.WH+ model.Batch, null);
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
                LogHelper.WriteLog("库存导入错误", ex);
            }
            return error;
        }
    }
}