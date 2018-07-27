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
    public class LineHourController : BaseController
    {
        /// <summary>
        /// 主表
        /// </summary>
        IBase_LineHourBizService _lineHourBizService = Smart.Instance.Base_LineHourBizService;
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [ViewPageAttribute]
        public ActionResult Index(string LineNo, int page = 1)
        {
            var query = QueryCondition.Instance.AddOrderBy("Id", false).SetPager(page, 10);
            query.AddEqual("RowState", "1");
            if (!string.IsNullOrEmpty(LineNo))
            {
                query.AddLike("ProLineNo", LineNo);
                ViewBag.LineNo = LineNo;
            }           
            ViewBag.Page = query.GetPager();
            var list = _lineHourBizService.GetAllDomain(query);
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
            var model = new Base_LineHour();
            model.MaxHours = 24;
            if (Id.HasValue)
            {
                var query = QueryCondition.Instance.AddEqual("Id", Id.Value.ToString());
                model = _lineHourBizService.GetAllDomain(query).FirstOrDefault();
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
        public ActionResult Add(Base_LineHour model)
        {
            model.RowState = 1;
            model.Updater = model.Creater = CurrentUser.Name;
            var query = QueryCondition.Instance;
            query.AddEqual("ProLineNo", model.ProLineNo);            
            query.AddEqual("RowState", "1");
            if (model.Id > 0)
            {
                query.AddNotEqual("Id", model.Id.ToString());
            }
            if (_lineHourBizService.GetAllDomain(query).Count > 0)
            {              
                ViewBag.Error = string.Format("已经存在相同的产线:{0},请重新填写", model.ProLineNo);
                return View(model);
            }
            if (model.Id == 0)
            {
                _lineHourBizService.Add(model);
                ViewBag.Error = "1";
                return RedirectToAction("Add", "LineHour", new { Error = 1});
            }
            ViewBag.Error = "1";
            _lineHourBizService.Update(model);
            return RedirectToAction("Index", "LineHour");
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
                _lineHourBizService.Delete(id);
            }
            return Json(new { Mess = "success" });
        }

        public ActionResult UploadFile()
        {
            Response.Clear(); //清除所有之前生成的Response内容
            Response.Write(Upload("LineHour"));
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
                                if (dataReader.FieldCount != 2)
                                {
                                    error = "解析错误,Excel 列头必须是2列，请下载模板 进行对比！";
                                    break;
                                }
                                //if (rowIndex == 2)
                                //{
                                //    _bomBizService.DeleteAll(0);
                                //}

                                var model = new Base_LineHour();
                                model.ProLineNo = dataReader[0].ToString();
                                model.MaxHours =Convert.ToDecimal(dataReader[1]);
                                
                                model.Updater = model.Creater = CurrentUser.Name;
                                model.RowState = 1;
                                var query = QueryCondition.Instance;
                                query.AddEqual("ProLineNo", model.ProLineNo);                                
                                query.AddEqual("RowState", "1");
                                var id = _lineHourBizService.GetId(query);
                                if (id == -1)
                                {
                                    _lineHourBizService.Add(model);
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