using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using VisualSmart.Dao.DataQuickStart.WeChat;
using VisualSmart.Domain.Alipay;
using VisualSmart.Domain.WeChat;
using VisualSmart.Util;

namespace VisualSmart.BizService.Implements.WeChat.Api
{
    /// <summary>
    /// 支付结果通知回调处理类
    /// 负责接收微信支付后台发送的支付结果并对订单有效性进行验证，将验证结果反馈给微信支付后台
    /// </summary>
    public class ResultNotify : Notify
    {
        public ResultNotify(HttpResponseMessage responseMessage, HttpRequest request)
            : base(responseMessage, request)
        {
        }
        public void ProcessNotify()
        {
            SceneryDomain sceneryDomain=new SceneryDomain();
           
            try
            {
                WxPayData notifyData = null;
                WxPayData res = null;
                try
                {
                    notifyData = GetNotifyData(sceneryDomain);
                }
                catch (WxPayException ex)
                {
                    //若签名错误，则立即返回结果给微信支付后台
                    res = new WxPayData();
                    res.SetValue("return_code", "FAIL");
                    res.SetValue("return_msg", ex.Message);
                    LogHelper.WeChatLog("Sign check error : " + res.ToXml());
                    ResponseMessage.Content = new StringContent(res.ToXml());
                    return;
                }
                //检查支付结果中transaction_id是否存在
                if (!notifyData.IsSet("transaction_id"))
                {
                    //若transaction_id不存在，则立即返回结果给微信支付后台
                    res = new WxPayData();
                    res.SetValue("return_code", "FAIL");
                    res.SetValue("return_msg", "支付结果中微信订单号不存在");
                    LogHelper.WeChatLog("The Pay result is error : " + res.ToXml());

                    ResponseMessage.Content = new StringContent(res.ToXml());

                    return;
                }

                string transaction_id = notifyData.GetValue("transaction_id").ToString();

                //查询订单，判断订单真实性
                var queryResult = QueryOrder(transaction_id, sceneryDomain.WeChat);
               
                if (queryResult == null)
                {
                    //若订单查询失败，则立即返回结果给微信支付后台
                    res = new WxPayData();
                    res.SetValue("return_code", "FAIL");
                    res.SetValue("return_msg", "订单查询失败");
                    LogHelper.WeChatLog("Order query failure : " + res.ToXml());
                    ResponseMessage.Content = new StringContent(res.ToXml());
                    return;
                }
                //查询订单成功
                else
                {
                    //业务处理
                    var detailDao = new WeChatDetailDao();
                    var qrCodeDao = new WeChatQRCodeDao();
                    //支付成功 查询交易信息
                    WeChatDetailDomain detail = new WeChatDetailDomain();
                    detail.Out_trade_no = queryResult.GetValue("out_trade_no").ToString();
                    detail.Total_fee = Convert.ToInt32(queryResult.GetValue("total_fee"));
                   

                    //需要检查数据库中是否有改数据 有的话 直接返回
                    var QRCodeDetail = qrCodeDao.IsExistOut_trade_no(detail.Out_trade_no, detail.Total_fee, sceneryDomain.WeChat.APPID);
                  
                    if (QRCodeDetail == null || QRCodeDetail.SceneryName == "-1")
                    {
                        LogHelper.WeChatLog("论寻：无通知参数");
                        res = new WxPayData();
                        res.SetValue("return_code", "FAIL");
                        res.SetValue("return_msg", "无通知参数");
                        LogHelper.WeChatLog("Order query failure : " + res.ToXml());
                        ResponseMessage.Content = new StringContent(res.ToXml());
                        return;
                    }

                    if (QRCodeDetail.SceneryName != "-2")
                    {
                        //保存微信信息
                        detail.SceneryName = QRCodeDetail.SceneryName;
                        detail.Creater = sceneryDomain.WeChat.APPID;
                        detail.Updater = "";
                        detail.AppId = sceneryDomain.WeChat.APPID;
                        detail.BatchNumber = QRCodeDetail.BatchNumber;
                        detail.SceneryRate = QRCodeDetail.SceneryRate;
                        detail.Time_end = DateTime.ParseExact(queryResult.GetValue("time_end").ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                        detail.WeChatPlayDate = Convert.ToDateTime(detail.Time_end.ToString("yyyy-MM-dd"));

                        detail.Err_code = "Success";
                        detail.Err_code_des = "异步";
                        detail.Openid = queryResult.GetValue("openid").ToString();
                        detail.Trade_type = queryResult.GetValue("trade_type").ToString();
                        detail.Bank_type = queryResult.GetValue("bank_type").ToString();
                        detail.Settlement_total_fee = Convert.ToInt32(queryResult.GetValue("settlement_total_fee"));
                        detail.Transaction_id = queryResult.GetValue("transaction_id").ToString();
                        detailDao.Add(detail); 
                    }
                    else
                    {
                        //论寻:数据已经存在，不需要插入
                        LogHelper.WeChatLog("论寻:数据已经存在，不需要插入");
                    }

                    res = new WxPayData();
                    res.SetValue("return_code", "SUCCESS");
                    res.SetValue("return_msg", "OK");
                    LogHelper.WeChatLog("order query success : " + res.ToXml());
                    ResponseMessage.Content = new StringContent(res.ToXml());
                }
            }
            catch (Exception ex)
            {
                LogHelper.WeChatLog("Notify 异常："+ex.Message);

                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "系统异常");
                LogHelper.WeChatLog("Sign check error : " + res.ToXml());
                ResponseMessage.Content = new StringContent(res.ToXml());
            }
        }


        //查询订单
        private WxPayData QueryOrder(string transaction_id, WxPayConfigDomain WxPayConfig)
        {
            WxPayData req = new WxPayData();
            req.SetValue("transaction_id", transaction_id);
            WxPayData res = WxPayApi.OrderQuery(req, WxPayConfig);
            if (res.GetValue("return_code").ToString() == "SUCCESS" &&
                res.GetValue("result_code").ToString() == "SUCCESS" &&
                res.GetValue("trade_state").ToString() == "SUCCESS")//支付成功
            {
                 
                return res;
            }
            else
            {
                return null;
            }
        }
    }
}
