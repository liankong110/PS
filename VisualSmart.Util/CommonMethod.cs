//-------------------------------------------------------------------------
// <copyright  company="同程软件有限公司" file="CommonMethod.cs">
//  Copyright (c) 版本 V1
//  所属部门：景区B项目部
//  所属项目：智慧旅游
//  作    者：刘文信
//  创建日期：2012-09-14 16:52:42
//  功能描述：公用方法
//  版本历史:
//      如有新增或修改请再次添加描述(格式：时间+作者+描述)
//</copyright>
//-------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using NPOI.HSSF.UserModel;
using System.Threading;
using System.ComponentModel;

 

namespace VisualSmart.Util
{
    public class EnumOperate
    {
        /// <summary>
        /// 获取枚举的描述信息
        /// </summary>
        /// <param name="e">传入枚举对象</param>
        /// <returns>得到对应描述信息</returns>
        public static String GetEnumDesc(Enum e)
        {
            var enumInfo = e.GetType().GetField(e.ToString());
            var enumAttributes = (DescriptionAttribute[])enumInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return enumAttributes.Length > 0 ? enumAttributes[0].Description : e.ToString();
        }
    }
    /// <summary>
    /// 通用类
    /// </summary>
    public class CommonMethod
    {
       
        /// <summary>
        /// 向页面输出excel文件
        /// 添加人：刘文信
        /// 添加时间：2013-05-09
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="excelName">Excel的名称</param>
        public static void WriteToFile(HSSFWorkbook workbook, string excelName)
        {
            var rootPath = HttpContext.Current.Server.MapPath("/UploadFiles/Excel/");
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }
            var title = "/" + excelName.Replace(" ","") + ".xls";
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.Browser.Browser))
            {
                if (HttpContext.Current.Request.Browser.Browser.ToLower().IndexOf("ie") >= 0)
                {
                    title = HttpContext.Current.Server.UrlEncode(title);
                }
            }
            var fs = new FileStream(rootPath + "/" + excelName + ".xls", FileMode.Create, FileAccess.ReadWrite);
            workbook.Write(fs);
            fs.Close();
            //把文件以流方式指定xls格式提供下载
            fs = File.OpenRead(rootPath + "/" + excelName + ".xls");
            var fileArray = new byte[fs.Length];
            fs.Read(fileArray, 0, fileArray.Length);
            fs.Close();
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.Charset = "GB2312";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + title);
            HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            HttpContext.Current.Response.AddHeader("Content-Length", fileArray.Length.ToString());
            HttpContext.Current.Response.BinaryWrite(fileArray);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
            HttpContext.Current.Response.Clear();
        }
        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="str">要截取的字符串</param> 
        /// <param name="num">截取位数</param>
        /// <param name="ellipsis">省略号</param>
        /// <returns></returns>
        public static string GetSubString(string str, int num, bool ellipsis = true)
        {
            var substr = str;
            if (!string.IsNullOrEmpty(str))
            {
                if (str.Length > num)
                {
                    substr = ellipsis ? str.Substring(0, num) + "..." : str.Substring(0, num);
                }
            }
            return substr;
        }

        /// <summary>
        /// 获得Url或表单参数的值, 先判断Url参数是否为空字符串, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">参数</param>
        /// <returns>Url或表单参数的值</returns>
        public static string GetString(string strName)
        {
            return "".Equals(GetQueryString(strName)) ? GetFormString(strName) : GetQueryString(strName);
        }

        /// <summary>
        /// 获得指定表单参数的值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <returns>表单参数的值</returns>
        private static string GetFormString(string strName)
        {
            return HttpContext.Current.Request.Form[strName] == null ? "" : HttpContext.Current.Request.Form[strName].Trim();
        }

        /// <summary>
        /// 获得指定Url参数的值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <returns>Url参数的值</returns>
        private static string GetQueryString(string strName)
        {
            return HttpContext.Current.Request.QueryString[strName] == null ? "" : HttpContext.Current.Request.QueryString[strName].Trim();
        }

        /// <summary>
        /// 获取最终字符串（排除DBNull，null，string.Empty 或空值后的真实值）
        /// </summary>
        /// <param name="objSource">object字符串</param>
        /// <returns></returns>
        public static string FinalString(object objSource)
        {
            if ((objSource == DBNull.Value) || (objSource == null))
                return string.Empty;
            return objSource.ToString().Trim();
        }

        /// <summary>
        /// 过滤输入内容的恶意html标记
        /// </summary>
        /// <param name="text">输入内容</param>
        /// <returns>过滤后的内容</returns>
        public static string InputText(string text)
        {
            return InputText(text, text.Length);
        }

        /// <summary>
        /// 过滤html，文本显示
        /// </summary>
        /// <param name="text">输入内容</param>
        /// <returns>过滤后的内容</returns>
        public static string FinalHtml(string text)
        {
            text = text.Trim();
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            text = Regex.Replace(text, "[\\s]{2,}", " ");//两个或两个以上空格
            text = Regex.Replace(text, "(<[b|B][r|R]/*>)+|(<[p|P](.|\\n)*?>)", "\n");//<br>
            text = Regex.Replace(text, "(\\s*&[n|N][b|B][s|S][p|P];\\s*)+", " ");//&nbsp;
            text = Regex.Replace(text, "<(.|\\n)*?>", string.Empty);//其他标记
            text = text.Replace("'", "''");
            return text;
        }

        /// <summary>
        /// 过滤输入内容的恶意html标记
        /// </summary>
        /// <param name="text">输入内容</param>
        /// <param name="maxLength">指定需过滤字符串长度</param>
        /// <returns>过滤后的内容</returns>
        public static string InputText(string text, int maxLength)
        {
            text = text.Trim();
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            if (text.Length > maxLength)
                text = text.Substring(0, maxLength);
            text = Regex.Replace(text, "[\\s]{2,}", " ");//两个或两个以上空格
            text = Regex.Replace(text, "(<[b|B][r|R]/*>)+|(<[p|P](.|\\n)*?>)", "\n");//<br>
            text = Regex.Replace(text, "(\\s*&[n|N][b|B][s|S][p|P];\\s*)+", " ");//&nbsp;
            text = Regex.Replace(text, "<(.|\\n)*?>", string.Empty);//其他标记
            text = text.Replace("'", "''");
            return text;
        }


        /// <summary>
        /// 含有textarea的字符串转化到显示格式p
        /// </summary>
        /// <param name="encodeText"></param>
        /// <returns></returns>
        public static string EncodeToString(string encodeText)
        {
            var sb = new StringBuilder();
            if (encodeText != null)
            {
                var temp1 = encodeText.Split('\r');
                foreach (var t in temp1)
                {
                    var temp2 = t.Trim();
                    if (!temp2.Equals("\n") && !string.IsNullOrEmpty(temp2))
                    {
                        sb.AppendFormat("<p>{0}</p>", temp2);
                    }
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 含有p的字符串转化到textarea显示格式
        /// </summary>
        /// <param name="theString"></param>
        /// <returns></returns>
        public static string StringToEncode(string theString)
        {
            var str = string.Empty;
            if (theString != null)
            {
                theString = theString.Replace("</p><p>", "\r\n");
                theString = theString.Replace("<p>", "");
                theString = theString.Replace("</p>", "\r\n");
                str = theString;
            }
            return str;
        }



        /// <summary>
        /// 判断一个List是否为空或者未初始化
        /// </summary>
        /// <typeparam name="T">任意Model</typeparam>
        /// <param name="list">任意实现Ilist接口的类型</param>
        /// <returns>返回true表示空，否则不为空</returns>
        public static bool ListIsNullOrEmpty<T>(IList<T> list)
        {
            return list == null || list.Count.Equals(0);
        }

        #region 日期处理
        /// <summary>
        /// 格式化日期为2006-12-22
        /// </summary>
        /// <param name="dTime"></param>
        /// <returns></returns>
        public static string FormatDate(DateTime dTime)
        {
            return dTime.Year + "-" + dTime.Month + "-" + dTime.Day;
        }

        /// <summary>
        /// 获取日期
        /// </summary>
        /// <param name="sDate"></param>
        /// <returns></returns>
        public static string GetWeek(DateTime sDate)
        {
            Calendar myCal = CultureInfo.InvariantCulture.Calendar;
            var rStr = "";
            switch (myCal.GetDayOfWeek(sDate).ToString())
            {
                case "Sunday":
                    rStr = "星期日";
                    break;
                case "Monday":
                    rStr = "星期一";
                    break;
                case "Tuesday":
                    rStr = "星期二";
                    break;
                case "Wednesday":
                    rStr = "星期三";
                    break;
                case "Thursday":
                    rStr = "星期四";
                    break;
                case "Friday":
                    rStr = "星期五";
                    break;
                case "Saturday":
                    rStr = "星期六";
                    break;
            }
            return rStr;
        }
        #endregion

        #region 生成0-9随机数
        /// <summary>
        /// 生成0-9随机数
        /// </summary>
        /// <param name="vcodeNum">生成长度</param>
        /// <returns></returns>
        public static string RndNum(int vcodeNum)
        {
            var sb = new StringBuilder(vcodeNum);
            var rand = new Random();
            for (int i = 1; i < vcodeNum + 1; i++)
            {
                int t = rand.Next(9);
                sb.AppendFormat("{0}", t);
            }
            return sb.ToString();

        }
        #endregion

        /// <summary>
        /// 对字符串进行MD5加密
        /// </summary>
        /// <param name="strText">需加密的字符串</param>
        /// <returns>加密后字符串</returns>
        public static string Md5(string strText)
        {
            if (string.IsNullOrEmpty(strText))
                return string.Empty;
            var targetString = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(strText, "MD5");
            if (targetString != null)
                return targetString.ToUpper();
            return null;
        }

      

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <param name="dt">时间</param>
        /// <returns></returns>
        public static long GetTimeStamp(DateTime dt)
        {
            var startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            return (long)Math.Round((dt - startTime).TotalSeconds, MidpointRounding.AwayFromZero);
        }

        

         
        /// <summary>
        /// 生成充值流水号
        /// </summary>
        /// <returns></returns>
        public static string GetAddBalanceSerialId()
        {
            return string.Format("A{0}{1}", DateTime.Now.ToString("yyMMddHHmmss"), new Random().Next(100, 999));
        }

        /// <summary>
        /// 加载处根节点之外的列表集合
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="parentId">父节点Id</param>
        /// <param name="resourceList">需要递归的列表集合</param>
        /// <param name="lists">新的集合</param>
        /// <param name="blank">空格</param>
        /// <returns></returns>
        protected static List<T> GetChildren<T>(long parentId, List<T> resourceList, List<T> lists, string blank) where T : new()
        {
            blank += "　";
            foreach (dynamic t in resourceList)
            {
                if (t.ParentId != parentId || t.ParentId <= 0) continue;
                dynamic it = new T();
                it.Id = t.Id;
                it.Name = blank + "|—" + t.Name;
                it.ParentId = t.ParentId;
                lists.Add(it);
                GetChildren(t.Id, resourceList, lists, blank);
            }
            return lists;
        }

        /// <summary>
        /// 加载处根节点之外的列表集合
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="parentId">父节点Id</param>
        /// <param name="resourceList">需要递归的列表集合</param>
        /// <param name="lists">新的集合</param>
        /// <returns></returns>
        public static List<T> GetChildren<T>(long parentId, List<T> resourceList, List<T> lists) where T : new()
        {
            foreach (dynamic t in resourceList)
            {
                if (t.ParentId == parentId && t.ParentId > 0)
                {
                    lists.Add(t);
                    GetChildren(t.Id, resourceList, lists);
                }
            }
            return lists;
        }

        /// <summary>
        /// string型转换为decimal型
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static decimal StrToDecimal(string strValue, decimal defValue)
        {
            if (strValue.Length == 0 || strValue.Length > 16)
            {
                return defValue;
            }
            return Convert.ToDecimal(strValue);
        }

        #region 计算两个时间的间隔
        /// <summary>
        /// 计算两个时间的间隔
        /// 根据printType的值有不同的输出,已满足不通显示要求
        /// </summary>
        /// <param name="dateBegin">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <param name="printType">输出类型(目前只支持0)</param>
        /// <returns></returns>
        public static string GetTwoDateSpace(string dateBegin, string dateEnd, int printType)
        {
            string dateDiff = string.Empty;
            try
            {
                TimeSpan ts1 = new TimeSpan(Convert.ToDateTime(dateEnd).Ticks);
                TimeSpan ts2 = new TimeSpan(Convert.ToDateTime(dateBegin).Ticks);
                TimeSpan ts = ts1.Subtract(ts2).Duration();
                if (printType == 0) //显示天/小时/分钟
                {
                    if (ts.Days > 0)
                    {
                        dateDiff = string.Format("{0}天{1}时{2}分", ts.Days, ts.Hours, ts.Minutes);
                    }
                    else
                    {
                        dateDiff = string.Format("{0}时{1}分", ts.Hours, ts.Minutes);
                    }
                }
            }
            catch
            {
                dateDiff = "日期格式错误";
            }
            return dateDiff;
        }
        #endregion
        #region 获取时间（周，月）
        public static DateTime GetWeekDateStart()
        {
            //本周
            int weeknow = Convert.ToInt32(DateTime.Now.DayOfWeek);
            //星期日 获取weeknow为0
            weeknow = weeknow == 0 ? 7 : weeknow;
            int daydiff = (-1) * weeknow + 1;
            //本周第一天
            return DateTime.Now.AddDays(daydiff);
        }
        public static DateTime GetWeekDateEnd()
        {
            //本周
            int weeknow = Convert.ToInt32(DateTime.Now.DayOfWeek);
            //星期日 获取weeknow为0
            weeknow = weeknow == 0 ? 7 : weeknow;
            int dayadd = 7 - weeknow;
            //本周最后一天
            return DateTime.Now.AddDays(dayadd);
        }
        public static DateTime GetMonthDateStart()
        {
            DateTime dt = DateTime.Now;
            //本月第一天时间 
            DateTime firstDate = dt.AddDays(-(dt.Day) + 1);
            return firstDate;
        }
        public static DateTime GetMonthDateEnd()
        {
            DateTime dt = DateTime.Now;
            //将本月月数+1 
            DateTime dt2 = dt.AddMonths(1);
            //本月最后一天时间 
            DateTime lastDate = dt2.AddDays(-(dt.Day));
            return lastDate;
        }
        #endregion
        /// <summary>
        /// 获取同比
        /// </summary>
        /// <param name="currentYear">本期数据</param>
        /// <param name="lastYear">上期数据</param>
        /// <returns></returns>
        public static string GetYearToYear(decimal currentYear, decimal lastYear)
        {
            if (lastYear == 0)
            {
                return "-";
            }
            return string.Format("{0:n2}%", ((currentYear - lastYear) / lastYear) * 100);
        }

        /// <summary>
        /// 向页面输出excel文件
        /// 添加人：刘文信
        /// 添加时间：2013-05-09
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="excelName">Excel的名称</param>
        public static void ResponseExcel(HSSFWorkbook workbook, string excelName)
        {
            var rootPath = HttpContext.Current.Server.MapPath("/UploadFiles/Excel/");
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }
            var title = "/" + excelName + ".xlsx";
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.Browser.Browser))
            {
                if (HttpContext.Current.Request.Browser.Browser.ToLower().IndexOf("ie") >= 0)
                {
                    title = HttpContext.Current.Server.UrlEncode(title);
                }
            }
            var fs = new FileStream(rootPath + "/" + excelName + ".xlsx", FileMode.Create, FileAccess.ReadWrite);
            workbook.Write(fs);
            fs.Close();
            //把文件以流方式指定xls格式提供下载
            fs = File.OpenRead(rootPath + "/" + excelName + ".xlsx");
            var fileArray = new byte[fs.Length];
            fs.Read(fileArray, 0, fileArray.Length);
            fs.Close();
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.Charset = "GB2312";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + title);
            HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            HttpContext.Current.Response.AddHeader("Content-Length", fileArray.Length.ToString());
            HttpContext.Current.Response.BinaryWrite(fileArray);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
            HttpContext.Current.Response.Clear();
        }

        /// <summary>
        /// 根据索引返回颜色
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public static string GetAllColors(int index)
        {
            var allColor = new[] { "#FF6600", "#008000", "#4B0082", "#800000", "#008080", "#6A5ACD", "#C2B9FF", "#00BFFF", "#FFF0F5","#32CD32" 
           ,"#8B7500"  ,"#228B22" ,"#6B8E23" ,"#8B6914" ,"#8B658B" ,"#CD5C5C" ,"#FF6A6A" ,"#8B3A3A" ,"#FF0000" ,"#FF3030" ,"#8B7D6B" ,"#836FFF" ,"#473C8B" ,"#0000CD" ,"#7A378B" ,"#607B8B" ,"#1C1C1C" ,"#696969" ,"#008B8B" ,"#B2DFEE" ,"#7A8B8B" };
            return allColor.Length < index ? allColor[0] : allColor[index];
        }

        /// <summary>
        /// 获取节点是否存在和值是否为空
        /// </summary>
        /// <param name="node">任意实现Ilist接口的类型</param>
        /// <returns></returns>
        public static string IsNodeNullOrEmpty(XmlNode node)
        {
            if (node == null || string.IsNullOrEmpty(node.InnerText))
                return string.Empty;
            return node.InnerText.Trim();
        }

        /// <summary>
        /// 获取分页总页数
        /// </summary>
        /// <param name="recordCnt">总记录条数</param>
        /// <param name="pageSize">每页页数</param>
        /// <returns>分页总页数</returns>
        public static int GetPageTotalCount(int recordCnt, int pageSize)
        {
            if (recordCnt < 0)
                throw new ArgumentException("recordCnt");
            if (pageSize < 0)
                throw new ArgumentException("pageSize");
            int pageTotalCount = recordCnt / pageSize;
            if (recordCnt % pageSize != 0)
                pageTotalCount++;
            return pageTotalCount;
        }

        #region desc加密
        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="strString"></param>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public static string DES3Encrypt(string strString, string strKey)
        {
            if (string.IsNullOrEmpty(strString) || string.IsNullOrEmpty(strKey))
            {
                return string.Empty;
            }
            var des = new TripleDESCryptoServiceProvider();
            var hashMd5 = new MD5CryptoServiceProvider();
            des.Key = hashMd5.ComputeHash(Encoding.Default.GetBytes(strKey));
            des.Mode = CipherMode.ECB;
            var desEncrypt = des.CreateEncryptor();
            var buffer = Encoding.Default.GetBytes(strString);
            return Convert.ToBase64String(desEncrypt.TransformFinalBlock(buffer, 0, buffer.Length));
        }

        /// <summary>
        /// DES加密JSON UTF8
        /// </summary>
        /// <param name="strString"></param>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public static string DES3EncryptUtf8(string strString, string strKey)
        {
            if (string.IsNullOrEmpty(strString) || string.IsNullOrEmpty(strKey))
            {
                return string.Empty;
            }
            var des = new TripleDESCryptoServiceProvider();
            var hashMd5 = new MD5CryptoServiceProvider();
            des.Key = hashMd5.ComputeHash(Encoding.UTF8.GetBytes(strKey));
            des.Mode = CipherMode.ECB;
            var desEncrypt = des.CreateEncryptor();
            var buffer = Encoding.UTF8.GetBytes(strString);
            return Convert.ToBase64String(desEncrypt.TransformFinalBlock(buffer, 0, buffer.Length));
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="strString"></param>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public static string DES3DecryptUtf8(string strString, string strKey)
        {
            var des = new TripleDESCryptoServiceProvider();
            var hashMd5 = new MD5CryptoServiceProvider();
            des.Key = hashMd5.ComputeHash(Encoding.UTF8.GetBytes(strKey));
            des.Mode = CipherMode.ECB;
            var desDecrypt = des.CreateDecryptor();
            string result;
            try
            {
                var buffer = Convert.FromBase64String(strString);
                result = Encoding.UTF8.GetString(desDecrypt.TransformFinalBlock(buffer, 0, buffer.Length));
            }
            catch
            {
                return "";
            }
            return result;
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="strString"></param>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public static string DES3Decrypt(string strString, string strKey)
        {
            var des = new TripleDESCryptoServiceProvider();
            var hashMd5 = new MD5CryptoServiceProvider();
            des.Key = hashMd5.ComputeHash(Encoding.Default.GetBytes(strKey));
            des.Mode = CipherMode.ECB;
            var desDecrypt = des.CreateDecryptor();
            string result;
            try
            {
                var buffer = Convert.FromBase64String(strString);
                result = Encoding.Default.GetString(desDecrypt.TransformFinalBlock(buffer, 0, buffer.Length));
            }
            catch
            {
                return "";
            }
            return result;
        }
        #endregion
        #region MD5加密
        /// <summary>
        /// 获取MD5加密后字符串
        /// </summary>
        /// <param name="sourceString"></param>
        /// <returns></returns>
        public static string Md5Encode(string sourceString)
        {
            return Md5Encode(sourceString, 32);
        }

        public static string Md5Encode(string sourceString, int digits)
        {
            return Md5Encode(sourceString, digits, true);
        }

        /// <summary>
        /// 获取获取MD5加密后字符串
        /// </summary>
        /// <param name="sourceString">源字符串</param>
        /// <param name="digits">加密位数(16或32)</param>
        /// <param name="isToUpper">是否大写</param>
        /// <returns></returns>
        public static string Md5Encode(string sourceString, int digits, bool isToUpper)
        {
            if (string.IsNullOrEmpty(sourceString))
                return string.Empty;
            var targetString = digits == 16
                                   ? System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(
                                       sourceString, "MD5").Substring(8, 16)
                                   : System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(
                                       sourceString, "MD5");
            if (targetString != null) return isToUpper ? targetString.ToUpper() : targetString.ToLower();
            return null;
        }
        #endregion



        #region 检售票系统
        //添加人: 周陈
        //添加日期: 2014/10/16

        /// <summary>
        /// 发送GET形式的HTTP请求
        /// </summary>
        /// <param name="m_QuestURL">url</param>
        /// <returns></returns>
        public static string GetVersionRequest(string m_QuestURL)
        {
            string result = string.Empty;
            HttpWebRequest request = null;
            try
            {
                request = (HttpWebRequest)HttpWebRequest.Create(m_QuestURL);
                request.Timeout = 10000;
                request.Credentials = CredentialCache.DefaultCredentials;
                request.ServicePoint.Expect100Continue = false;
                request.ServicePoint.UseNagleAlgorithm = false;
                using (StreamReader avReader = new StreamReader(request.GetResponse().GetResponseStream()))
                {
                    result = avReader.ReadToEnd();
                    avReader.Close();
                }
            }
            catch (Exception err)
            {
                result = "Error:" + err.Message;
            }
            finally
            {
                request.Abort();
            }

            return result;
        }

        private const string StrKey = "yzjspxt1";
        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="strText">需要解密字符串</param>
        /// <param name="strKey">密钥</param>
        /// <returns></returns>
        public static string DESDecrypt(string strText)
        {
            if (string.IsNullOrEmpty(strText)) return string.Empty;
            var des = new DESCryptoServiceProvider();
            var inputByteArray = new byte[strText.Length / 2];
            for (int x = 0; x < strText.Length / 2; x++)
            {
                var i = (Convert.ToInt32(strText.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }

            des.Key = Encoding.ASCII.GetBytes(StrKey);
            des.IV = Encoding.ASCII.GetBytes(StrKey);
            var ms = new MemoryStream();
            var cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.Default.GetString(ms.ToArray());
        }

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="strText">需要加密字符串</param>
        /// <returns></returns>
        public static string DESEncrypt(string strText)
        {
            if (string.IsNullOrEmpty(strText)) return string.Empty;
            var des = new DESCryptoServiceProvider();
            var inputByteArray = Encoding.Default.GetBytes(strText);
            des.Key = Encoding.ASCII.GetBytes(StrKey);
            des.IV = Encoding.ASCII.GetBytes(StrKey);
            var ms = new MemoryStream();
            var cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            var ret = new StringBuilder();
            foreach (var b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            ret.ToString();
            return ret.ToString();
        }

        /// <summary>
        /// 设置app.config值
        /// </summary>
        /// <param name="xDoc">xml</param>
        /// <param name="appKey">key</param>
        /// <param name="value">value</param>
        public static void SetConfigValue(XmlNode xDoc, string appKey, string value)
        {
            var xElem = (XmlElement)xDoc.SelectSingleNode("//appSettings").SelectSingleNode("//add[@key='" + appKey + "']");
            if (xElem != null) xElem.SetAttribute("value", value);
        }

      

        

        /// <summary>
        /// 分割字符串
        /// </summary>
        public static string[] SplitString(string strContent, string strSplit)
        {
            if (!string.IsNullOrEmpty(strContent))
            {
                if (strContent.IndexOf(strSplit) < 0)
                    return new string[] { strContent };

                return Regex.Split(strContent, Regex.Escape(strSplit), RegexOptions.IgnoreCase);
            }
            else
                return new string[0] { };
        }
        #endregion

        /// <summary>
        /// 获取公网ip
        /// </summary>
        /// <returns></returns>
        public static string GetPublishIp()
        {
            string tempip = "";
            WebRequest request = WebRequest.Create("http://ip.qq.com/");
            request.Timeout = 10000;
  
            Stream resStream = null;
            StreamReader sr = null;
            try
            {
                WebResponse response = request.GetResponse();
                resStream = response.GetResponseStream();
                sr = new StreamReader(resStream, System.Text.Encoding.Default);
                string htmlinfo = sr.ReadToEnd();
                //匹配IP的正则表达式
                Regex r =
                    new Regex(
                        "((25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]\\d|\\d)\\.){3}(25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]\\d|[1-9])",
                        RegexOptions.None);
                Match mc = r.Match(htmlinfo);
                //获取匹配到的IP
                tempip = mc.Groups[0].Value;
            }
            catch(Exception ex)
            {

            }
            finally
            {
                if (resStream != null) 
                    resStream.Close();
                if (sr != null) 
                    sr.Close();
            }

            return tempip;
        }

        

    }

    /// <summary>
    ///  转换
    /// </summary>
    public static class ConversionHelper
    {
        #region 数据格式转换
        /// <summary>
        ///  转换成Int
        /// </summary>
        /// <param name="inputValue">输入值</param>
        /// <returns></returns>
        public static int ToInt(this object inputValue)
        {
            return inputValue.IsInt() ? int.Parse(inputValue.ToStringValue()) : 0;
        }

        /// <summary>
        ///  转换成Int32
        /// </summary>
        /// <param name="inputValue">输入值</param>
        /// <returns></returns>
        public static int ToInt32(this object inputValue)
        {
            return inputValue.IsInt() ? Convert.ToInt32(inputValue) : 0;
        }

        /// <summary>
        ///  转换成Int64
        /// </summary>
        /// <param name="inputValue">输入值</param>
        /// <returns></returns>
        public static long ToInt64(this object inputValue)
        {
            return inputValue.IsInt() ? Convert.ToInt64(inputValue) : 0;
        }

        /// <summary>
        ///  转换成Double
        /// </summary>
        /// <param name="inputValue">输入值</param>
        /// <returns></returns>
        public static Double ToDouble(this object inputValue)
        {
            return !inputValue.IsNullOrEmpty() ? Convert.ToDouble(inputValue) : 0;
        }
        /// <summary>
        ///  转换成Decimal
        /// </summary>
        /// <param name="inputValue">输入值</param>
        /// <returns></returns>
        public static Decimal ToDecimal(this object inputValue)
        {
            return !inputValue.IsNullOrEmpty() ? Convert.ToDecimal(inputValue) : 0;
        }
        /// <summary>
        ///  转换成Int64
        /// </summary>
        /// <param name="inputValue">输入值</param>
        /// <returns></returns>
        public static bool? ToBool(this object inputValue)
        {
            return inputValue.IsNullOrEmpty() ? (bool?)null : bool.Parse(inputValue.ToStringValue());
        }

        /// <summary>
        ///  转换obj 成string
        /// </summary>
        /// <param name="inputValue">object</param>
        /// <returns></returns>
        public static string ToStringValue(this object inputValue)
        {
            return inputValue == null ? "" : inputValue.ToString();
        }

        /// <summary>
        ///  转换成数据库字符串
        /// </summary>
        /// <param name="inputValue">输入值</param>
        /// <returns></returns>
        public static string ToDbString(this object inputValue)
        {
            return !inputValue.IsNullOrEmpty() ? "'" + inputValue.ToStringValue().Trim().Replace("'", "''") + "'" : "''";
        }

        /// <summary>
        ///  转换成时间类型yyyy-MM-dd
        /// </summary>
        /// <param name="inputValue">输入值</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this object inputValue)
        {
            return inputValue.IsNullOrEmpty() ? DateTime.MinValue : Convert.ToDateTime(inputValue);
        }

        /// <summary>
        ///  转换成时间类型
        /// </summary>
        /// <param name="inputValue">输入值</param>
        /// <param name="format">时间格式</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this object inputValue, string format)
        {
            return DateTime.ParseExact(inputValue.ToStringValue(), format, null);
        }
        #endregion
    }
    /// <summary>
    ///  验证
    /// </summary>
    public static class VerificationHelper
    {
        #region 正则表达式
        //邮政编码
        private static readonly Regex RegPostCode = new Regex("^\\d{6}$");
        //中国身份证验证
        private static readonly Regex RegCardId = new Regex("^\\d{17}[\\d|X]|\\d{15}|\\d{18}$");
        //数字
        private static readonly Regex RegNumber = new Regex("^\\d+$");
        //固定电话
        private static readonly Regex RegTel = new Regex("^\\d{3,4}-\\d{7,8}|\\d{7,8}$");
        //手机号
        private static readonly Regex RegPhone = new Regex("^[1][3-8]\\d{9}$");
        //电话号码（包括固定电话和手机号）
        private static readonly Regex RegTelePhone = new Regex("^(\\d{3,4}-\\d{7,8}|\\d{7,8})|([1][3-8]\\d{9})$");
        //邮箱
        private static readonly Regex RegEmail = new Regex("^[\\w-]+@[\\w-]+\\.(com|net|org|edu|mil|tv|biz|info)$");
        //中文
        private static readonly Regex RegChzn = new Regex("[\u4e00-\u9fa5]");
        //IP地址
        private static readonly Regex RegIp = new Regex("((25[0-5]|2[0-4]\\d|1?\\d?\\d)\\.){3}(25[0-5]|2[0-4]\\d|1?\\d?\\d)");
        #endregion

        #region 验证方法
        /// <summary>
        ///  判断是否是数字
        /// </summary>
        /// <param name="inputValue">输入值</param>
        /// <returns></returns>
        public static bool IsInt(this object inputValue)
        {
            int num;
            return int.TryParse(inputValue.ToStringValue(), out num);
        }
        /// <summary>
        ///  判断是否是小数
        /// </summary>
        /// <param name="inputValue">输入值</param>
        /// <returns></returns>
        public static bool IsDouble(this object inputValue)
        {
            Double dValue;
            return Double.TryParse(inputValue.ToStringValue(), out dValue);
        }
        /// <summary>
        ///  判断是否是小数
        /// </summary>
        /// <param name="inputValue">输入值</param>
        /// <returns></returns>
        public static bool IsFloat(this object inputValue)
        {
            float fValue;
            return float.TryParse(inputValue.ToStringValue(), out fValue);
        }
        /// <summary>
        ///  判断字符串是否为空
        ///  空：true，不为空：false
        /// </summary>
        /// <param name="inputValue">输入值</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this object inputValue)
        {
            return string.IsNullOrEmpty(inputValue.ToStringValue());
        }

        /// <summary>
        ///  判断字符串是否为Email
        /// </summary>
        /// <param name="inputValue">输入值</param>
        /// <returns></returns>
        public static bool IsEmail(this object inputValue)
        {
            var match = RegEmail.Match(inputValue.ToStringValue());
            return match.Success;
        }
        /// <summary>
        ///  判断字符串是否为固话
        /// </summary>
        /// <param name="inputValue">输入值</param>
        /// <returns></returns>
        public static bool IsTel(this object inputValue)
        {
            var match = RegTel.Match(inputValue.ToStringValue());
            return match.Success;
        }
        /// <summary>
        ///  判断字符串是否为手机号
        /// </summary>
        /// <param name="inputValue">输入值</param>
        /// <returns></returns>
        public static bool IsPhone(this object inputValue)
        {
            var match = RegPhone.Match(inputValue.ToStringValue());
            return match.Success;
        }
        /// <summary>
        ///  判断字符串是否为电话号码
        ///  （包含固定电话和手机号）
        /// </summary>
        /// <param name="inputValue">输入值</param>
        /// <returns></returns>
        public static bool IsTelePhone(this object inputValue)
        {
            var match = RegTelePhone.Match(inputValue.ToStringValue());
            return match.Success;
        }
        /// <summary>
        ///  判断字符串是否为IP地址
        /// </summary>
        /// <param name="inputValue">输入值</param>
        /// <returns></returns>
        public static bool IsIp(this object inputValue)
        {
            var match = RegIp.Match(inputValue.ToStringValue());
            return match.Success;
        }
        /// <summary>
        ///  判断字符串是否为邮编
        /// </summary>
        /// <param name="inputValue">输入值</param>
        /// <returns></returns>
        public static bool IsPostCode(this object inputValue)
        {
            var match = RegPostCode.Match(inputValue.ToStringValue());
            return match.Success;
        }
        /// <summary>
        ///  判断字符串是否为身份证
        /// </summary>
        /// <param name="inputValue">输入值</param>
        /// <returns></returns>
        public static bool IsCardId(this object inputValue)
        {
            var match = RegCardId.Match(inputValue.ToStringValue());
            return match.Success;
        }
        /// <summary>
        ///  判断字符串是否为中文
        /// </summary>
        /// <param name="inputValue">输入值</param>
        /// <returns></returns>
        public static bool IsChzn(this object inputValue)
        {
            var match = RegChzn.Match(inputValue.ToStringValue());
            return match.Success;
        }
        /// <summary>
        ///  判断字符串是否为数字
        /// </summary>
        /// <param name="inputValue">输入值</param>
        /// <returns></returns>
        public static bool IsNumber(this object inputValue)
        {
            var match = RegNumber.Match(inputValue.ToStringValue());
            return match.Success;
        }
        #endregion
    }
}