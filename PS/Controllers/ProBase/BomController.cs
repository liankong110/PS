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
        public ActionResult Index(string ParentGoodNo, string SonGoodNo, int page = 1)
        {
            var query = QueryCondition.Instance.AddOrderBy("Id", false).SetPager(page, 10);
            query.AddEqual("RowState", "1");
            if (!string.IsNullOrEmpty(ParentGoodNo))
            {
                query.AddLike("ParentGoodNo", ParentGoodNo);
                ViewBag.ParentGoodNo = ParentGoodNo;
            }
            if (!string.IsNullOrEmpty(SonGoodNo))
            {
                query.AddLike("SonGoodNo", SonGoodNo);
                ViewBag.SonGoodNo = SonGoodNo;
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
        public ActionResult Add(int? Id, string Error, string ParentGoodNo, string ParentGoodName)
        {
            ViewBag.GoodList = Smart.Instance.Base_GoodsBizService.GetGoodName(QueryCondition.Instance.AddEqual("RowState", "1"));
            ViewBag.Error = Error;
            var model = new Base_Bom();
            model.BiLi = 1;
            if (Id.HasValue)
            {
                var query = QueryCondition.Instance.AddEqual("Id", Id.Value.ToString());
                model = _bomBizService.GetAllDomain(query).FirstOrDefault();
            }
            else
            {
                if (ParentGoodName != null && ParentGoodNo != null)
                {
                    model.ParentGoodName = ParentGoodName;
                    model.ParentGoodNo = ParentGoodNo;
                }
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
                ViewBag.GoodList = Smart.Instance.Base_GoodsBizService.GetGoodName(QueryCondition.Instance.AddEqual("RowState", "1"));
                ViewBag.Error = string.Format("已经存在相同的父商品:{0},子商品:{1},请重新填写", model.ParentGoodNo, model.SonGoodNo);
                return View(model);
            }
            var bomGoods=Smart.Instance.Base_GoodsBizService.GetBomName(model.ParentGoodNo, model.SonGoodNo);
            if (bomGoods.Count <2)
            {
                ViewBag.GoodList = Smart.Instance.Base_GoodsBizService.GetGoodName(QueryCondition.Instance.AddEqual("RowState", "1"));
                ViewBag.Error = string.Format("没有找到产品信息，请检查维护的产品在产品中是否存在！");
                return View(model);
            }
            model.ParentGoodName = bomGoods.Find(t => t.GoodNo == model.ParentGoodNo).GoodName;
            model.SonGoodName = bomGoods.Find(t => t.GoodNo == model.SonGoodNo).GoodName;
            if (model.Id == 0)
            {
                _bomBizService.Add(model);
                ViewBag.Error = "1";
                return RedirectToAction("Add", "Bom", new { Error = 1, ParentGoodNo = model.ParentGoodNo, ParentGoodName = model.ParentGoodName });
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

        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult ReSetView()
        {
            _bomBizService.ReSetView();            
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
            int rowIndex = 2;
            string error = "success";
            var GoodList = Smart.Instance.Base_GoodsBizService.GetGoodName(QueryCondition.Instance.AddEqual("RowState", "1")).ToList();
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

                        using (OleDbDataReader dataReader = objCommand.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                if (dataReader.FieldCount != 3)
                                {
                                    error = "解析错误,Excel 列头必须是3列，请下载模板 进行对比！";
                                    break;
                                }
                                //if (rowIndex == 2)
                                //{
                                //    _bomBizService.DeleteAll(0);
                                //}

                                var model = new Base_Bom();
                                model.ParentGoodNo = dataReader[0].ToString();                               
                                model.SonGoodNo = dataReader[1].ToString();                         
                                model.BiLi = Convert.ToInt32(dataReader[2]);
                                model.Updater = model.Creater = CurrentUser.Name;
                                model.RowState = 1;
                                var p=GoodList.Find(t => t.Contains(model.ParentGoodNo));
                                var s = GoodList.Find(t => t.Contains(model.SonGoodNo));
                                if (p==null || s==null)
                                {
                                    error = "Excel解析错误,成功" + (rowIndex - 1) + "条，错误行号：" + rowIndex + ",没有找到对应的商品信息，请检查。";
                                    break;
                                }

                                model.ParentGoodName = p.Split(',')[1];                             
                                model.SonGoodName = s.Split(',')[1];
                                var query = QueryCondition.Instance;
                                query.AddEqual("ParentGoodNo", model.ParentGoodNo);
                                query.AddEqual("SonGoodNo", model.SonGoodNo);
                                query.AddEqual("RowState", "1");

                                var id = _bomBizService.GetId(query);
                                if (id == -1)
                                {
                                    _bomBizService.Add(model);
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

                LogHelper.WriteLog("BOM导入错误", ex);
            }
            return error;
        }

    }
}