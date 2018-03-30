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

namespace PS.Controllers.ProBase
{
    public class ProductionLineController : BaseController
    {
        /// <summary>
        /// 主表
        /// </summary>
        IBase_ProductionLineBizService _productionLineBizService = Smart.Instance.Base_ProductionLineBizService;
        /// <summary>
        /// 子表
        /// </summary>
        IBase_ProductionLinesBizService _productionLinesBizService = Smart.Instance.Base_ProductionLinesBizService;

        [ViewPageAttribute]
        public ActionResult Index(string GoodNo, string GoodName, int page = 1)
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
            ViewBag.Page = query.GetPager();
            var list = _productionLineBizService.GetAllDomain(query);
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
            var model = new Base_ProductionLine();
            if (Id.HasValue)
            {
                var query = QueryCondition.Instance.AddEqual("Id", Id.Value.ToString());
                model = _productionLineBizService.GetAllDomain(query).FirstOrDefault();
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
        public ActionResult Add(Base_ProductionLine model)
        {
            model.RowState = 1;
            model.Updater = model.Creater = CurrentUser.Name;

            var query = QueryCondition.Instance;
            query.AddEqual("GoodNo", model.GoodNo);
            query.AddEqual("ProLineNo", model.ProLineNo);
            query.AddEqual("RowState", "1");
            if (model.Id > 0)
            {
                query.AddNotEqual("Id", model.Id.ToString());
            }
            if (_productionLineBizService.GetAllDomain(query).Count > 0)
            {
                ViewBag.Error = string.Format("已经存在相同的生产线:{1},产品编号:{0},请重新填写", model.ProLineNo, model.GoodNo);
                return View(model);
            }
            if (model.Id == 0)
            {
                _productionLineBizService.Add(model);
                //解析人员配置及每小时产出 列：7人75件/H，6人64件/H，5人53件/H
                 //model.ProCapacityDesc;
                ViewBag.Error = "1";
                return RedirectToAction("Add", "Goods", new { Error = 1 });
            }
            ViewBag.Error = "1";
            _productionLineBizService.Update(model);
            return RedirectToAction("Index", "Goods");
        }

        private Dictionary<int, int> GetCapacityList()
        {
            Dictionary<int, int> list = new Dictionary<int, int>();
            return list;
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
                _productionLineBizService.Delete(id);
            }
            return Json(new { Mess = "success" });
        }

        public ActionResult UploadFile()
        {
            //清除所有之前生成的Response内容
            Response.Clear(); 
            Response.Write(Upload("Goods"));
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
                                if (rowIndex > 1)
                                {
                                    var model = new Base_ProductionLine();
                                    model.ProLineNo = dataReader[0].ToString();
                                    model.GoodNo = dataReader[1].ToString();
                                    model.GoodName = dataReader[2].ToString();
                                    model.ProCapacityDesc = dataReader[3].ToString();
                                    model.BoxNum = Convert.ToInt32(dataReader[4]);
                                    model.LineMins = Convert.ToInt32(dataReader[5]);

                                    model.ProShift = 0; 
                                    model.PCS =0;
                                    model.StandPers = 0;
                                    model.MinProNum = 0;                                

                                    model.Updater = model.Creater = CurrentUser.Name;
                                    model.RowState = 1;
                                    var query = QueryCondition.Instance;
                                    query.AddEqual("GoodNo", model.GoodNo);
                                    query.AddEqual("ProLineNo", model.ProLineNo);
                                    query.AddEqual("RowState", "1");
                                    var id = _productionLineBizService.GetId(query);
                                    if (id > 0)
                                    {
                                        model.Id = id;
                                        _productionLineBizService.Update(model);
                                    }
                                    else
                                    {
                                        _productionLineBizService.Add(model);
                                    }
                                }
                                rowIndex++;
                            }
                        }
                        conn.Close();
                    }
                }
            }
            catch (Exception)
            {
                error = "Excel解析错误";

            }
            return error;
        }
    }
}