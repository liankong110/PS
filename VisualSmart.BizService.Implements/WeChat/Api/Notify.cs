using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Xml;
using VisualSmart.Domain.Alipay;
using VisualSmart.Domain.WeChat;
using VisualSmart.Util;

namespace VisualSmart.BizService.Implements.WeChat.Api
{
    /// <summary>
    /// 回调处理基类
    /// 主要负责接收微信支付后台发送过来的数据，对数据进行签名验证
    /// 子类在此类基础上进行派生并重写自己的回调处理过程
    /// </summary>
    public class Notify
    {

       

        public HttpRequest Request { get; set; }

        public HttpResponseMessage ResponseMessage { get; set; }
        public Notify(HttpResponseMessage responseMessage, HttpRequest request)
        {
            this.Request = request;
            this.ResponseMessage = responseMessage;
        }

        /// <summary>
        /// 接收从微信支付后台发送过来的数据并验证签名
        /// </summary>
        /// <returns>微信支付后台返回的数据</returns>
        public WxPayData GetNotifyData(SceneryDomain sceneryDomain)
        {
             
            //接收从微信后台POST过来的数据
            System.IO.Stream s = Request.InputStream;
            int count = 0;
            byte[] buffer = new byte[1024];
            StringBuilder builder = new StringBuilder();
            while ((count = s.Read(buffer, 0, 1024)) > 0)
            {
                builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
            }
            s.Flush();
            s.Close();
            s.Dispose();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(builder.ToString());
            XmlNode xmlNode = xmlDoc.FirstChild;//获取到根节点<xml>
            XmlNodeList nodes = xmlNode.ChildNodes;
            foreach (XmlNode xn in nodes)
            {
                XmlElement xe = (XmlElement)xn;
                if (xe.Name == "appid")
                {
                    var appid = xe.InnerText;
                    LogHelper.WeChatLog("appid : " + appid);
                 
                    if (WxPayConfigBizService.wxPayConfigList == null)
                    {
                        new WxPayConfigBizService().ReLoadWxPayList(appid);
                    }
                    sceneryDomain.WeChat = WxPayConfigBizService.wxPayConfigList.First(t => t.APPID == appid);
                    if (sceneryDomain.WeChat == null)
                    {
                        throw new WxPayException("没有找到微信账号!");
                    }
                    break;
                }
              
            }



            LogHelper.WeChatLog("Receive data from WeChat : " + builder.ToString());

            //转换数据格式并验证签名
            WxPayData data = new WxPayData();
            data.FromXml(builder.ToString(), sceneryDomain.WeChat.PAYKEY);
            LogHelper.WeChatLog("Check sign success");
            return data;
        }

    }
}
