using System;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web.Mvc;
using VisualSmart.BizService.Core;
using VisualSmart.BizService.ProBase;
using VisualSmart.Domain.ProBase;
using VisualSmart.Util;

namespace PS.Controllers.ProBase
{
    /// <summary>
    /// BOM信息设置
    /// </summary>
    public class BomController : BaseController
    {
        /// <summary>
        /// 主表
        /// </summary>
        IBase_BomBizService _bomBizService = Smart.Instance.Base_BomBizService;
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
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
            var list = _bomBizService.GetAllDomain(query);
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
            var model = new Base_Bom();
            if (Id.HasValue)
            {
                var query = QueryCondition.Instance.AddEqual("Id", Id.Value.ToString());
                model = _bomBizService.GetAllDomain(query).FirstOrDefault();
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
        public ActionResult Add(Base_Bom model)
        {
            model.RowState = 1;
            model.Updater = model.Creater = CurrentUser.Name;
            var query = QueryCondition.Instance;
            query.AddEqual("ParentGoodNo", model.ParentGoodNo);
            query.AddEqual("SonGoodNo", model.SonGoodNo);
            query.AddEqual("RowState", "1");
            if (model.Id > 0)
            {
                query.AddNotEqual("Id", model.Id.ToString());
            }
            if (_bomBizService.GetAllDomain(query).Count > 0)
            {
                ViewBag.Error = string.Format("已经存在相同的父商品:{1},子商品:{0},请重新填写", model.ParentGoodNo, model.SonGoodNo);
                return View(model);
            }
            if (model.Id == 0)
            {
                _bomBizService.Add(model);
                ViewBag.Error = "1";
                return RedirectToAction("Add", "Bom", new { Error = 1 });
            }
            ViewBag.Error = "1";
            _bomBizService.Update(model);
            return RedirectToAction("Index", "Bom");
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
                _bomBizService.Delete(id);
            }
            return Json(new { Mess = "success" });
        }

        public ActionResult UploadFile()
        {
            Response.Clear(); //清除所有之前生成的Response内容
            Response.Write(Upload("Bom"));
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
            string error = "";
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
                        int rowIndex = 1;
                        using (OleDbDataReader dataReader = objCommand.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                if (dataReader.FieldCount != 4)
                                {
                                    error = "Excel解析错误";
                                    break;
                                }
                                if (rowIndex == 2)
                                {
                                    _bomBizService.DeleteAll(0);
                                }
                                if (rowIndex > 1)
                                {
                                    var model = new Base_Bom();
                                    model.ParentGoodNo = dataReader[0].ToString();
                                    model.ParentGoodName = dataReader[1].ToString();
                                    model.SonGoodNo = dataReader[2].ToString();
                                    model.SonGoodName = dataReader[3].ToString();
                                    model.Updater = model.Creater = CurrentUser.Name;
                                    model.RowState = 1;
                                    _bomBizService.Add(model);

                                }
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
                LogHelper.WriteLog(ex.Message);

            }
            return error;
        }

    }
}