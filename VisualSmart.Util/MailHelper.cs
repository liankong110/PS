using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Xml; 


namespace VisualSmart.Util
{
    public class MailMessage
    {
        /// <summary>
        /// 接收人邮件地址
        /// </summary>
        public string[] To { get; set; }
        /// <summary>
        /// 邮件主题
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// 邮件内容
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// 邮件的优先级
        /// </summary>
        public MailPriority Priority { get; set; }
        /// <summary>
        /// 密送地址
        /// 密送就是你群发邮件时收邮件的人无法看到你发给了多少人以及他们的邮件地址
        /// </summary>
        public string[] Bcc { get; set; }
        /// <summary>
        /// 抄送地址
        /// 抄送就是群发邮件时收邮件的人则可以看到你发给了多少人以及他们的邮件地址。 
        /// </summary>
        public string[] Cc { get; set; }
        /// <summary>
        /// 获取或是设置MailFormat是否是HTML格式 
        /// </summary>
        public bool IsBodyHtml { get; set; }
        /// <summary>
        /// 指定消息的编码方式编码
        /// </summary>
        public Encoding BodyEncoding { get; set; }
        /// <summary>
        /// 邮件附件列表
        /// 数组中为 附件的 绝对路径
        /// </summary>
        public string[] Attachments { get; set; }
        /// <summary>
        /// 邮件内容
        /// </summary>
        public EmailBody EmailBody { get; set; }
    }
    /// <summary>
    /// 邮件优先级
    /// 创建人：姜启伟
    /// 创建时间：2014-09-28
    /// </summary>
    public class MailPriorityConst
    {
        /// <summary>
        ///  高
        /// </summary>
        public static MailPriority Hign
        {
            get
            {
                return MailPriority.High;
            }
        }
        /// <summary>
        ///  低
        /// </summary>
        public static MailPriority Low
        {
            get
            {
                return MailPriority.Low;
            }
        }
        /// <summary>
        ///  正常
        /// </summary>
        public static MailPriority Normal
        {
            get
            {
                return MailPriority.Normal;
            }
        }
    }

    /// <summary>
    /// 邮件内容实体
    /// 创建人：姜启伟
    /// 创建时间：2014-09-28
    /// </summary>
    public class EmailBody
    {
        /// <summary>
        /// 景区名称
        /// </summary>
        public string SceneryName { get; set; }
        /// <summary>
        /// 请求内容
        /// </summary>
        public string RequsetContent { get; set; }
        /// <summary>
        /// 错误地址
        /// </summary>
        public string ExceptionStackTrace { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ExceptionMessage { get; set; }
        /// <summary>
        /// 请求地址
        /// </summary>
        public string RequestUrl { get; set; }
        /// <summary>
        /// 接口返回码
        /// </summary>
        public string RespCode { get; set; }
        /// <summary>
        /// 接口返回信息
        /// </summary>
        public string RespMsg { get; set; }
        /// <summary>
        /// 接口请求地址
        /// </summary>
        public string RespTime { get; set; }
        //--------------------webjob票型推送----------------------
        /// <summary>
        /// 操作类型
        /// </summary>
        public string OperateType { get; set; }
        /// <summary>
        /// 订单信息
        /// </summary>
        public string OrderInfo { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderSerialId { get; set; }
        /// <summary>
        /// 票型编号
        /// </summary>
        public string TicketId { get; set; }

        //--------------------webjob同程订单反推异常邮件----------------------
        /// <summary>
        /// 实取数量
        /// </summary>
        public string RealPickCount { get; set; }
    }
    /// <summary>
    /// 发送邮件接口
    /// </summary>
    public interface ISendMail
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="message">邮件信息</param>
        /// <returns>发送邮件是否成功</returns>
        bool Send(MailMessage message);
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="message">邮件信息</param>
        /// <param name="templateType">发送邮件的模板</param>
        /// <returns>发送邮件是否成功</returns>
        bool Send(MailMessage message, int templateType);
    }
    /// <summary>
    /// 使用Smtp发送邮件
    /// </summary>
    public class MailHelper : ISendMail
    {
        /// <summary>
        /// 使用SMTP方式发送邮件
        /// </summary>
        /// <param name="message">邮件信息</param>
        /// <returns>是否发送成功</returns>
        public bool Send(MailMessage message)
        {
            try
            {               
                var fromMail = ConfigurationManager.AppSettings["MailFrom"];
                var smtpServer = ConfigurationManager.AppSettings["SmtpServer"];
                var smtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"] ?? "25");
                var userName = ConfigurationManager.AppSettings["UserName"];
                var userPwd = ConfigurationManager.AppSettings["UserPasswrod"];
                var isUseDefaultCredentials = bool.Parse(ConfigurationManager.AppSettings["UseDefaultCredentials"]);
                var mailMessage = new System.Net.Mail.MailMessage
                {
                    Body = message.Body,
                    BodyEncoding = message.BodyEncoding,
                    From = new MailAddress(fromMail),
                    IsBodyHtml = message.IsBodyHtml,
                    Priority = message.Priority,
                    Subject = message.Subject
                };
                if (message.To != null)
                    foreach (var to in message.To)
                    {
                        if (string.IsNullOrEmpty(to))
                        {
                            continue;
                        }
                        mailMessage.To.Add(new MailAddress(to));
                    }
                if (message.Bcc != null)
                    foreach (var cc in message.Bcc)
                    {
                        mailMessage.Bcc.Add(new MailAddress(cc));
                    }
                if (message.Cc != null)
                    foreach (var cc in message.Cc)
                    {
                        mailMessage.CC.Add(new MailAddress(cc));
                    }
                if (message.Attachments != null)
                    foreach (var attachment in message.Attachments)
                    {
                        mailMessage.Attachments.Add(new Attachment(attachment));
                    }

                var client = new SmtpClient
                {
                    Host = smtpServer,
                    Port = smtpPort
                };
                if (isUseDefaultCredentials)
                {
                    client.UseDefaultCredentials = true;
                    client.Credentials = new NetworkCredential(userName, userPwd);
                }
                client.Send(mailMessage);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 使用SMTP方式发送邮件
        /// 创建人：姜启伟
        /// 创建时间：2014-09-28
        /// </summary>
        /// <param name="message">邮件信息</param>
        /// <param name="templateType">模板类型</param>
        /// <returns>是否发送成功</returns>
        public bool Send(MailMessage message, int templateType)
        {
            try
            {
                var fromMail = ConfigurationManager.AppSettings["MailFrom"];
                var smtpServer = ConfigurationManager.AppSettings["SmtpServer"];
                var smtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"] ?? "25");
                var userName = ConfigurationManager.AppSettings["UserName"];
                var userPwd = ConfigurationManager.AppSettings["UserPasswrod"];
                var isUseDefaultCredentials = bool.Parse(ConfigurationManager.AppSettings["UseDefaultCredentials"]);

                //定义邮件内容,获取模板
                var mailBody = GetTemplates(message, templateType);

                var mailMessage = new System.Net.Mail.MailMessage
                {
                    Body = mailBody,
                    BodyEncoding = message.BodyEncoding,
                    From = new MailAddress(fromMail),
                    IsBodyHtml = message.IsBodyHtml,
                    Priority = message.Priority,
                    Subject = message.Subject,
                };
                if (message.To != null)
                    foreach (var to in message.To)
                    {
                        if (string.IsNullOrEmpty(to))
                        {
                            continue;
                        }
                        mailMessage.To.Add(new MailAddress(to));
                    }
                if (message.Bcc != null)
                    foreach (var cc in message.Bcc)
                    {
                        mailMessage.Bcc.Add(new MailAddress(cc));
                    }
                if (message.Cc != null)
                    foreach (var cc in message.Cc)
                    {
                        mailMessage.CC.Add(new MailAddress(cc));
                    }
                if (message.Attachments != null)
                    foreach (var attachment in message.Attachments)
                    {
                        mailMessage.Attachments.Add(new Attachment(attachment));
                    }

                var client = new SmtpClient
                {
                    Host = smtpServer,
                    Port = smtpPort
                };
                if (isUseDefaultCredentials)
                {
                    client.UseDefaultCredentials = true;
                    client.Credentials = new NetworkCredential(userName, userPwd);
                }
                client.Send(mailMessage);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 填充邮件模板
        /// 创建人：姜启伟
        /// 创建时间：2014-09-28
        /// </summary>
        /// <param name="message">邮件body实体</param>
        /// <param name="templateType">邮件模板</param>
        /// <returns></returns>
        public string GetTemplates(MailMessage message, int templateType = 1)
        {
            try
            {
                var sbMsg = new StringBuilder("");
                sbMsg.Append(GetTemplatesStyle());
                sbMsg.Append("<table class='template-table' cellspacing='0' cellpadding='0'>");
                sbMsg.Append(GetTemplatesTrHtml("所属景区", message.EmailBody.SceneryName));
                sbMsg.Append(GetTemplatesTrHtml("HTTP地址", message.EmailBody.RequestUrl));
                switch (templateType)
                {                 
                    default:
                        sbMsg.Append(GetTemplatesTrHtml("错误描述", message.EmailBody.ExceptionMessage));
                        sbMsg.Append(GetTemplatesTrHtml("错误地点", message.EmailBody.ExceptionStackTrace));
                        break;
                }
                sbMsg.Append(GetTemplatesTrHtml("当前时间", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff")));
                sbMsg.Append("</table>");
                return sbMsg.ToString();
            }
            catch (Exception ex)
            {
                return string.Format("发送邮件异常：{0}", ex.Message);
            }
        }

        /// <summary>
        ///  获取样式
        /// </summary>
        /// <returns></returns>
        public string GetTemplatesStyle()
        {
            var sbStyle = new StringBuilder("");
            sbStyle.Append("<style  type='text/css'>");
            sbStyle.Append("*{ margin:0; padding:0;}");
            sbStyle.Append(".template-table{ border-collapse:collapse; border: 2px solid #000;}");
            sbStyle.Append(".com-tr{ }");
            sbStyle.Append(".com-td{ padding:8px;border: 1px solid #000; }");
            sbStyle.Append(".title-td{ width:150px;height:30px; }");
            sbStyle.Append(".content-td{ width:600px;height:30px; }");
            sbStyle.Append("</style>");
            return sbStyle.ToString();
        }

        /// <summary>
        ///  根据标题和对应的值生成html
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public string GetTemplatesTrHtml(string title, string value)
        {
            var sbHtml = new StringBuilder("");
            sbHtml.AppendFormat("<tr class='com-tr'><td class='com-td title-td'>{0}:</td><td class='com-td content-td'>{1}</td></tr>", title, value);
            return sbHtml.ToString();
        }
    }

}
