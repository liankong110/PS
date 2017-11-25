using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VisualSmart.BizService.Core;

namespace PS.Controllers
{
    public class UploadFileController : BaseController
    {        
        public string PicUpload()
        {
            const string defaulturl = "/UploadFiles/Avatar/userImg.jpg";
            return System.Web.HttpContext.Current.Request.Files.Count == 0 ? defaulturl : FileUpload(CurrentUser.Id.ToString());
        }

        /// <summary>
        /// 生成文件名
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns></returns>
        private static string GetFileName(string filename)
        {
            var random = new Random();
            var extension = ".jpg";
            if (filename.Contains("."))
            {
                extension = filename.Substring(filename.IndexOf('.')).Length > 0
                                ? filename.Substring(filename.IndexOf('.'))
                                : ".jpg";
            }
            return DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond + random.Next(10, 99) + extension;
        }

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="folderName">上传文件夹名称</param>
        /// <param name="returntype">返回类型(1:返回文件URL 2:返回文件物理地址(dirpath)|文件URL(url)|文件名(filename)</param>
        /// <param name="zip">是否需要压缩文件(1:是 0:否)</param>
        /// <returns></returns>
        public string FileUpload(string folderName, int returntype = 1, bool zip = false)
        {
            var urlpath = string.Format(@"/UploadFiles/{0}/{1}/{2}/", folderName, DateTime.Now.Year.ToString("d4"), DateTime.Now.Month.ToString("d2"));
            var dirpath = Server.MapPath(urlpath);
            if (!Directory.Exists(dirpath))
            {
                Directory.CreateDirectory(dirpath);
            }
            var postedFile = System.Web.HttpContext.Current.Request.Files[0];
            if (postedFile == null)
            {
                return string.Empty;
            }

            try
            {
                System.Drawing.Image img = System.Drawing.Image.FromStream(postedFile.InputStream);
            }
            catch (Exception)
            {
                
                return string.Empty;
            }

            var filename = GetFileName(postedFile.FileName.ToLower());
            postedFile.SaveAs(dirpath + filename);
            if (zip && !filename.Contains(".zip") && !filename.Contains(".rar"))
            {
                var zipfilename = String.Format("{0}.zip", filename.Substring(0, filename.LastIndexOf(".")));
                //ZipHelper.CreateZipFile(dirpath + filename, dirpath + zipfilename);
                System.IO.File.Delete(dirpath + filename);
                filename = zipfilename;
            }
            string url= returntype.Equals(1)
                       ? string.Format("{0}{1}", urlpath, filename)
                       : string.Format("{0}|{1}|{2}", dirpath, Url, filename);
            CurrentUser.Avatar = url;
           Smart.Instance.UserBizService.Update(CurrentUser);
            return url;
        }

  

    }
}
