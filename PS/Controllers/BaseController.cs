using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using VisualSmart.BizService.Core;
using VisualSmart.Domain;
using VisualSmart.Domain.SetUp;
using VisualSmart.Util;

namespace PS.Controllers
{
    [AuthorizeFilter]
    public class BaseController : Controller
    {
        /// <summary>
        /// 当前用户
        /// </summary>
        public UserDomain CurrentUser
        {
            get
            {
                if (Session["User"] == null)
                {
                    string domainName = Environment.UserDomainName;
                    string username = Environment.UserName;
                    var LoginId = domainName + "\\" + username;
                    var userList = Smart.Instance.UserBizService.GetAllDomain(QueryCondition.Instance.AddEqual("LoginId", LoginId));
                    if (userList.Count == 0)
                    {
                        //新增
                        var user = new UserDomain();
                        user.loginPwd = MD5Util.Encrypt("123456");
                        user.RowState = 1;
                        user.Creater = "system";
                        user.Updater = "system";
                        user.loginId = LoginId;
                        user.Name = username;
                        Smart.Instance.UserBizService.Add_Update_User_Role(user, System.Configuration.ConfigurationManager.AppSettings["DefaultRoleId"], "system");
                        user.Id = Smart.Instance.UserBizService.GetAllDomain(QueryCondition.Instance.AddEqual("LoginId", LoginId))[0].Id;
                        Session["User"] = user;
                    }
                    else
                    {
                        Session["User"] = userList[0];
                    }
                }
                return Session["User"] as UserDomain;
            }

        }

        /// <summary>
        /// 添加修改时 基本信息设置
        /// </summary>
        /// <param name="T"></param>
        public void AddOrUpdateBaseInfo(VisualSmart.Domain.Base.Entity T)
        {
            if (T != null && T.Id == 0)
            {
                ViewBag.ViewTitle = "新增";
            }
            else
            {
                ViewBag.ViewTitle = "修改";
            }
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            //string loginUrl = "/Account/Login?returnurl=" + Request.RawUrl;
            //if (CurrentUser != null)
            //{
            //if (requestContext.HttpContext.Request.HttpMethod == "GET")
            //{
            //    var controllername = requestContext.RouteData.Values["controller"].ToString().ToLower();
            //    var actionname = requestContext.RouteData.Values["action"].ToString().ToLower();
            //    var currentController = controllername;


            //    List<ComDefinedColumnsEntity> userDefinePages = new ComDefinedColumnsBll().GetUserDefinePages();
            //    if (userDefinePages.Any(t => t.Controller.ToLower() == controllername && t.Action.ToLower() == actionname))
            //    {
            //        if (Request.Cookies["ActionName"] != null)
            //        {
            //            Request.Cookies.Remove("ActionName");
            //        }
            //        HttpCookie actionName = new HttpCookie("ActionName");
            //        actionName.Value = actionname;
            //        Response.AppendCookie(actionName);
            //        if (Request.Cookies["ControllerName"] != null)
            //        {
            //            Request.Cookies.Remove("ControllerName");
            //        }
            //        HttpCookie controllerName = new HttpCookie("ControllerName");
            //        controllerName.Value = controllername;
            //        Response.AppendCookie(controllerName);

            //    }
            //    if (Session["CurrentController"] == null || Session["CurrentController"].ToString() != currentController)
            //    {
            //        controllername = string.Format("'{0}'", controllername);
            //        var currentUrl = string.Format("/{0}", controllername);
            //        if (!string.IsNullOrEmpty(OtherControllers))
            //        {
            //            controllername += "," + OtherControllers;
            //        }
            //        var myAllActions = new MembershipManager().GetUserMenus(CurrentUser.CrmUser.Id, controllername, SystemType.Main);
            //        Session["CurrentUrl"] = currentUrl;
            //        Session["MyAllActions"] = myAllActions.ToLower();
            //    }
            //}
            //}
            //else
            //{
            //    FormsAuthentication.SignOut();
            //    Response.Redirect(loginUrl);
            //}
        }



        /// <summary>
        /// 增加一个合并功能
        /// </summary>
        /// <param name="fristRow">开始行号</param>
        /// <param name="lastRow">结束行号</param>
        /// <param name="fristColumn">开始列号</param>
        /// <param name="lastColumn">结束列号</param>
        /// <param name="title">内容</param>
        /// <param name="hssfworkbook">EXCEL工作簿</param>
        /// <param name="sheet">工作薄</param>
        /// <param name="fontSize">字体大小</param>
        /// <param name="alignment">字体对齐方式</param>
        protected void AddRow(int fristRow, int lastRow, int fristColumn, int lastColumn, string title, IWorkbook hssfworkbook, ISheet sheet, short fontSize, NPOI.SS.UserModel.HorizontalAlignment alignment)
        {
            sheet.AddMergedRegion(new CellRangeAddress(fristRow, lastRow, fristColumn, lastColumn - 1));
            sheet.GetRow(fristRow).CreateCell(fristColumn).SetCellValue(title);
            for (var i = 1; i < lastColumn; i++)
            {
                sheet.GetRow(fristRow).CreateCell(i).SetCellValue("");
            }
            var subtotalStyle = hssfworkbook.CreateCellStyle();
            subtotalStyle.Alignment = alignment;
            subtotalStyle.VerticalAlignment = VerticalAlignment.CENTER;
            subtotalStyle.BorderTop = BorderStyle.THIN;
            subtotalStyle.BorderBottom = BorderStyle.THIN;
            subtotalStyle.BorderLeft = BorderStyle.THIN;
            subtotalStyle.BorderRight = BorderStyle.THIN;
            subtotalStyle.TopBorderColor = IndexedColors.BLACK.Index;
            subtotalStyle.BottomBorderColor = IndexedColors.BLACK.Index;
            subtotalStyle.LeftBorderColor = IndexedColors.BLACK.Index;
            subtotalStyle.RightBorderColor = IndexedColors.BLACK.Index;
            var font = hssfworkbook.CreateFont();
            font.FontHeightInPoints = fontSize;
            font.Boldweight = 100 * 100;
            var currentRow = sheet.GetRow(fristRow);
            for (var i = 0; i < lastColumn; i++)
            {
                currentRow.GetCell(i).CellStyle = subtotalStyle;
                currentRow.GetCell(i).CellStyle.SetFont(font);
            }
        }


        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="folderName">上传文件夹名称</param>
        /// <param name="returntype">返回类型(1:返回文件URL 2:返回文件物理地址(dirpath)|文件URL(url)|文件名(filename)</param>
        /// <param name="zip">是否需要压缩文件(1:是 0:否)</param>
        /// <returns></returns>
        public string FileUpload(int returntype = 1)
        {
            var urlpath = string.Format(@"/UploadFiles/WeChat/{0}/", Guid.NewGuid());
            var dirpath = Server.MapPath(urlpath);
            if (!Directory.Exists(dirpath))
            {
                Directory.CreateDirectory(dirpath);
            }
            if (System.Web.HttpContext.Current.Request.Files.Count == 0)
            {
                return string.Empty;
            }
            var postedFile = System.Web.HttpContext.Current.Request.Files[0];
            if (postedFile == null)
            {
                return string.Empty;
            }

            var filename = postedFile.FileName.ToLower();
            postedFile.SaveAs(dirpath + filename);

            string url = returntype.Equals(1)
                       ? string.Format("{0}{1}", urlpath, filename)
                       : string.Format("{0}|{1}|{2}", dirpath, Url, filename);

            return url;
        }


        public string GetFileName(string filename)
        {
            var random = new Random();
            var extension = filename.Substring(filename.LastIndexOf('.')).Length > 0
                             ? filename.Substring(filename.LastIndexOf('.'))
                             : ".jpg";
            return DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond + random.Next(10, 99) + extension;
        }

        /// <summary>
        /// 上传附件
        /// </summary>
        /// <param name="folderName">对应的文件夹</param>
        /// <returns></returns>
        public string Upload(string folderName)
        {
            var urlpath = string.Format(@"/Attachment/{0}/{1}/", folderName, DateTime.Now.ToString("yyyyMM"));
            var dirpath = Server.MapPath(urlpath);
            if (!Directory.Exists(dirpath))
            {
                Directory.CreateDirectory(dirpath);
            }
            var postedFile = System.Web.HttpContext.Current.Request.Files[0];
            if (postedFile == null)
            {
                return "";
            }
            var filename = GetFileName(postedFile.FileName.ToLower());
            postedFile.SaveAs(dirpath + filename);

            //开始解析Excel
            var error = LoadExcel(dirpath + filename);
            if (error != "")
            {
                return error;
            }

            return urlpath + filename;
        }

        public virtual string LoadExcel(string file)
        {
            throw new Exception("请实现方法");
        }

        public void LoadSystem()
        {
            string domainName = Environment.UserDomainName;
            string username = Environment.UserName;

        }

        /// <summary>
        /// 设置excel公共样式，边框加黑线      
        /// </summary>
        /// <param name="cs"></param>
        protected  void SetExcelBoderStyle(ICellStyle cs)
        {
            cs.Alignment = HorizontalAlignment.CENTER;
            cs.VerticalAlignment = VerticalAlignment.CENTER;
            cs.BorderTop = BorderStyle.THIN;
            cs.BorderBottom = BorderStyle.THIN;
            cs.BorderLeft = BorderStyle.THIN;
            cs.BorderRight = BorderStyle.THIN;
            cs.TopBorderColor = IndexedColors.BLACK.Index;
            cs.BottomBorderColor = IndexedColors.BLACK.Index;
            cs.LeftBorderColor = IndexedColors.BLACK.Index;
            cs.RightBorderColor = IndexedColors.BLACK.Index;
            cs.WrapText = true; //自动换行
        }
    }
}
