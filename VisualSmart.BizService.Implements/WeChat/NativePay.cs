using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using VisualSmart.BizService.Implements.WeChat.Api;
using VisualSmart.BizService.WeChat;
using VisualSmart.Dao.DataQuickStart.SceneryOrder;
using VisualSmart.Dao.DataQuickStart.WeChat;
using VisualSmart.Domain.Alipay;
using VisualSmart.Domain.SetUp;
using VisualSmart.Domain.WeChat;
using VisualSmart.Domain.WeChat.Query;
using VisualSmart.Domain.WeChat.Unifiedorder;
using VisualSmart.Util;

namespace VisualSmart.BizService.Implements.WeChat
{
    public class NativePay : INativePay
    {
        /**
         * 生成直接支付url，支付url有效期为2小时,模式二
         * @param productId 商品ID
         * @return 模式二URL
         */
        public UnifiedorderResponse GetPayUrl(UnifiedorderRequest orderRequest, SceneryDomain sceneryDomain)
        {

            WeChatQRCodeDomain qrCode = new WeChatQRCodeDomain();
            qrCode.BatchNumber = orderRequest.out_trade_no;
            qrCode.SceneryName = sceneryDomain.SceneryName;
            qrCode.Creater = orderRequest.attach;
            qrCode.Updater = orderRequest.attach;
            qrCode.Phone = orderRequest.Phone;

            UnifiedorderResponse response = new UnifiedorderResponse();
            string out_trade_no = GenerateSerialIdService.Get(FixedPrefix.WeChat, CustomPrefix.A, sceneryDomain.SceneryTCId.ToString()).SerialId;
            orderRequest.out_trade_no = out_trade_no;
            WxPayData data = new WxPayData();
            data.SetValue("body", orderRequest.body);//商品描述         
            data.SetValue("out_trade_no", out_trade_no);//随机字符串
            data.SetValue("total_fee", Convert.ToInt32(orderRequest.total_fee));//总金额
            //10分钟 时间间隔
            data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));//交易起始时间
            data.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));//交易结束时间            
            data.SetValue("trade_type", "NATIVE");//交易类型
            data.SetValue("product_id", orderRequest.productId);//商品ID
            data.SetValue("attach", orderRequest.attach);//机器号

            
            string url = "";
            try
            {             
                qrCode.Out_trade_no = orderRequest.out_trade_no;
                qrCode.RequestJson = data.ToXml();
                WxPayData result = WxPayApi.UnifiedOrder(data, sceneryDomain.WeChat);//调用统一下单接口
                if (result.GetValue("return_code").ToString() == "SUCCESS")
                {
                    url = result.GetValue("code_url").ToString();//获得统一下单接口返回的二维码链接
                    LogHelper.WeChatLog(string.Format("请求成功，二维码地址：{0}", url));
                }
                else
                {
                    qrCode.Remark = result.GetValue("return_msg").ToString();
                    LogHelper.WeChatLog(string.Format("二维码地址请求失败：{0}", result.GetValue("return_msg")));
                }
               
                response.Out_trade_no = out_trade_no;
                response.QRCode = url;
               
                qrCode.AppId = sceneryDomain.WeChat.APPID;
                qrCode.QrCode = url;
            }
            catch (Exception ex)
            {
                qrCode.Remark = ex.Message;
                LogHelper.WeChatLog(string.Format("获取微信URL:请求失败，保存信息：{0}", ex.Message));
            }
            finally
            {
                try
                {
                    new WeChatQRCodeDao().Add(qrCode);
                }
                catch (Exception ex)
                {
                    LogHelper.WeChatLog(string.Format("获取微信URL:新增操作日志失败：{0}", ex.Message));
                }
            }
            return response;
        }

        /// <summary>
        /// 查询订单支付状态
        /// </summary>
        /// <param name="request"></param>
        /// <param name="sceneryDomain"></param>
        /// <returns></returns>
        public QueryResponse OrderQuery(QueryRequest request, SceneryDomain sceneryDomain)
        {
            QueryResponse queryResponse = new QueryResponse();
            var result = false;
            try
            {
                WxPayData data = new WxPayData();
                data.SetValue("out_trade_no", request.Out_trade_no);//随机字符串
                LogHelper.WeChatLog("论寻-请求：out_trade_no:" + request.Out_trade_no);
                WxPayData queryResult = WxPayApi.OrderQuery(data, sceneryDomain.WeChat);//调用统一下单接口

                LogHelper.WeChatLog(string.Format("论寻-结果：return_code:{0},result_code:{1},trade_state:{2}",
                    queryResult.GetValue("return_code"),queryResult.GetValue("result_code"),queryResult.GetValue("trade_state")));

                if (queryResult.GetValue("return_code").ToString() == "SUCCESS" &&
                queryResult.GetValue("result_code").ToString() == "SUCCESS"
                    && queryResult.GetValue("trade_state").ToString() == "SUCCESS")
                {
                    var detailDao = new WeChatDetailDao();
                    var qrCodeDao = new WeChatQRCodeDao();
                    //支付成功 查询交易信息
                    WeChatDetailDomain detail = new WeChatDetailDomain();
                    detail.Out_trade_no = queryResult.GetValue("out_trade_no").ToString();
                    detail.Total_fee = Convert.ToInt32(queryResult.GetValue("total_fee"));
                    //测试使用
                    //detail.Total_fee = 1;
                    //需要检查数据库中是否有改数据 有的话 直接返回
                    var QRCodeDetail = qrCodeDao.IsExistOut_trade_no(detail.Out_trade_no, detail.Total_fee, sceneryDomain.WeChat.APPID);

                    if (QRCodeDetail == null || QRCodeDetail.SceneryName == "-1")
                    {
                        LogHelper.WeChatLog("论寻：无通知参数");
                    }
                    //论寻:数据已经存在，不需要插入
                    if (QRCodeDetail.SceneryName != "-2")
                    {
                        //保存微信信息

                        detail.SceneryName = sceneryDomain.SceneryName;
                        detail.Creater = sceneryDomain.WeChat.APPID;
                        detail.Updater = "";
                        detail.AppId = sceneryDomain.WeChat.APPID;
                        detail.BatchNumber = QRCodeDetail.BatchNumber;
                        detail.SceneryRate = sceneryDomain.Rate;
                        DateTime time_end = DateTime.ParseExact(queryResult.GetValue("time_end").ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                        detail.Time_end = time_end;//20170727172015
                        detail.WeChatPlayDate = Convert.ToDateTime(detail.Time_end.ToString("yyyy-MM-dd"));

                        detail.Err_code = "Success";
                        detail.Err_code_des = "论寻"; 
                        detail.Openid = queryResult.GetValue("openid").ToString();
                        detail.Trade_type = queryResult.GetValue("trade_type").ToString();
                        detail.Bank_type = queryResult.GetValue("bank_type").ToString();
                        detail.Settlement_total_fee = Convert.ToInt32(queryResult.GetValue("settlement_total_fee"));
                        detail.Transaction_id = queryResult.GetValue("transaction_id").ToString();
                        detailDao.Add(detail);
                    }
                    else
                    {
                        if (queryResult.GetValue("transaction_id") != null)
                        {
                            detail.Transaction_id = queryResult.GetValue("transaction_id").ToString();
                        }
                        if (queryResult.GetValue("time_end") != null)
                        {
                            detail.Time_end = DateTime.ParseExact(queryResult.GetValue("time_end").ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                        }
                        LogHelper.WeChatLog("论寻:数据已经存在，不需要插入");
                    }
                    queryResponse.transaction_id = detail.Transaction_id;
                    queryResponse.time_end = detail.Time_end;
                    result = true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WeChatLog(string.Format("微信 查询订单时 异常：{0}", ex.Message));
            }

            if (result == false && request.IsLastRequest == 1)
            {
                CloseOrder(request.Out_trade_no, sceneryDomain);
            }
            return queryResponse;
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="out_trade_no"></param>
        /// <returns></returns>
        public string CloseOrder(string out_trade_no, SceneryDomain sceneryDomain)
        {
            string return_code = "";
            try
            {
                LogHelper.WeChatLog(string.Format("wx:正在取消订单：{0}", out_trade_no));
                WxPayData data = new WxPayData();
                data.SetValue("out_trade_no", out_trade_no);//随机字符串
                WxPayData result = WxPayApi.CloseOrder(data, sceneryDomain.WeChat);//调用统一下单接口
                return_code = result.GetValue("return_code").ToString();//获得统一下单接口返回的二维码链接
                LogHelper.WeChatLog(string.Format("wx:取消-结果：{0}-{1}", return_code, result.GetValue("return_msg")));
                //失败 或者 成功时但err_code =SYSTEMERROR 时， 系统异常，请重新调用该API
                if (result.GetValue("return_code").ToString() == "SUCCESS" &&
              result.GetValue("result_code").ToString() == "SUCCESS")
                {
                    LogHelper.WeChatLog("wx:开启线程");
                    ParameterizedThreadStart ParStart = new ParameterizedThreadStart(CloselOrderRetry);
                    Thread myThread = new Thread(ParStart);
                    List<object> objs = new List<object>();
                    objs.Add(out_trade_no);
                    objs.Add(sceneryDomain);
                    myThread.Start(objs);
                }

            }
            catch (Exception ex)
            {

                LogHelper.WeChatLog(string.Format("wx:正在取消订单", ex.Message));
            }
            return return_code;
        }


        /// <summary>
        /// 线程执行关闭订单
        /// </summary>
        /// <param name="o"></param>

        public void CloselOrderRetry(object o)
        {
            List<object> param = o as List<object>;
            string out_trade_no = param[0].ToString();
            var sceneryDomain = param[1] as SceneryDomain;
            int retryCount = 50;

            for (int i = 0; i < retryCount; ++i)
            {

                Thread.Sleep(10000);
                WxPayData data = new WxPayData();
                data.SetValue("out_trade_no", out_trade_no);//随机字符串
                WxPayData result = WxPayApi.CloseOrder(data, sceneryDomain.WeChat);//调用统一下单接口
                string return_code = result.GetValue("return_code").ToString();//获得统一下单接口返回的二维码链接

                //|| (return_code == "SUCCESS" && result.GetValue("err_code").ToString() == "SYSTEMERROR")
                if (return_code == "FAIL" )
                {
                    LogHelper.WeChatLog(string.Format("取消-结果：{0},请求内容：{1}", return_code, result.GetValue("return_msg")));

                }
                if (return_code == "SUCCESS")
                {
                    LogHelper.WeChatLog(string.Format("wx:取消-结果：{0}", "SUCCESS"));

                    break;
                }

                if (i == retryCount - 1)
                {
                    // 处理到最后一次，还是未撤销成功，需要在商户数据库中对此单最标记，人工介入处理
                    //lblMessage.Text = cancelResponse.Body;
                    LogHelper.WeChatLog(string.Format("处理到最后一次，还是未撤销成功，需要在商户数据库中对此单最标记，人工介入处理,请求内容：{0}", out_trade_no));
                }

            }
        }
        /**
        * 参数数组转换为url格式
        * @param map 参数名与参数值的映射表
        * @return URL字符串
        */
        private string ToUrlParams(SortedDictionary<string, object> map)
        {
            string buff = "";
            foreach (KeyValuePair<string, object> pair in map)
            {
                buff += pair.Key + "=" + pair.Value + "&";
            }
            buff = buff.Trim('&');
            return buff;
        }

        /// <summary>
        /// 回调处理基类
        /// 主要负责接收微信支付后台发送过来的数据，对数据进行签名验证
        /// </summary>
        /// <param name="responseMessage"></param>
        /// <param name="request"></param>
        /// <param name="sceneryDomain"></param>
        public void Notify(HttpResponseMessage responseMessage, HttpRequest request)
        {
            ResultNotify notify = new ResultNotify(responseMessage, request);
            notify.ProcessNotify();
        }



        #region 退款
        ///// <summary>
        ///// 交易信息全部退款
        ///// </summary>
        ///// <param name="alipayId"></param>
        ///// <param name="sceneryDomain"></param>
        ///// <returns>0 成功 1系统已经存在信息 2退款异常</returns>
        //public int Refund(int alipayId, UserDomain CurrentUser)
        //{
        //    LogHelper.WeChatLog(string.Format("退款开始-》操作人：{0}，退款ID:{1}", CurrentUser.Name, alipayId));
        //    var alipayDetailDao = new AlipayDetailDao();
        //    var list = alipayDetailDao.GetAllDomain(QueryCondition.Instance.AddEqual("Id", alipayId.ToString()));
        //    if (list.Count == 1)
        //    {
        //        var model = list[0];
        //        var tempTotal = -model.Total_amount;

        //        if (!alipayDetailDao.IsExistsOut_trade_no(model.Out_trade_no, tempTotal, model.AppId))
        //        {
        //            ////////////////////////////////////////////请求参数////////////////////////////////////////////

        //            //支付宝交易号
        //            string trade_no = model.Trade_no;
        //            //支付宝交易号与商户网站订单号不能同时为空
        //            string out_request_no = model.Out_trade_no;
        //            //退款金额
        //            string refund_amount = model.Total_amount.ToString();
        //            //退款金额不能大于订单金额

        //            //商户网站订单系统中唯一订单号，必填
        //            StringBuilder request = new StringBuilder();
        //            request.Append("{\"out_request_no\":\"" + out_request_no + "\",");
        //            request.Append("\"trade_no\":\"" + trade_no + "\",");
        //            request.Append("\"refund_amount\":\"" + refund_amount + "\"}");

        //            var query = QueryCondition.Instance.AddEqual("APP_ID", model.AppId).AddEqual("RowState", "1");

        //            var alipay = new AlipayDao().GetAllDomain(query);
        //            if (alipay.Count != 1)
        //            {
        //                return 2;
        //            }
        //            LogHelper.WeChatLog(string.Format("退款JSON：{0},APP_ID:{1}", request.ToString(), alipay[0].APP_ID));
        //            AlipayTradeRefundResponse refundResponse = DoRefund(request.ToString(), alipay[0]);

        //            //请在这里加上商户的业务逻辑程序代码
        //            //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
        //            string result = refundResponse.Body;

        //            if (refundResponse.Code == "10000")//退款成功
        //            {
        //                Alipay_trade_refund_response alipayRefund = JsonConvert.DeserializeObject<Alipay_trade_refund_response>(refundResponse.Body);
        //                model.Code = alipayRefund.alipay_trade_refund_response.code;
        //                model.Trade_no = alipayRefund.alipay_trade_refund_response.trade_no;
        //                model.Out_trade_no = alipayRefund.alipay_trade_refund_response.out_trade_no;
        //                model.Total_amount = alipayRefund.alipay_trade_refund_response.refund_fee;
        //                model.Send_pay_date = alipayRefund.alipay_trade_refund_response.gmt_refund_pay;
        //                model.Msg = alipayRefund.alipay_trade_refund_response.msg;

        //                model.Creater = CurrentUser.Name;
        //                model.Updater = CurrentUser.Name;
        //                model.CreateTime = DateTime.Now;
        //                model.UpdateTime = DateTime.Now;

        //                model.Total_amount = -model.Total_amount;
        //                //新增一个负数的支付单信息。
        //                alipayDetailDao.Add(model);
        //                //新增一个退款申请的信息。
        //                //判断退款信息是否存在。
        //            }
        //            else
        //            {
        //                LogHelper.WeChatLog(string.Format("退款失败：{0}，body：{1}", refundResponse.Code, result));
        //                return 2;
        //            }
        //        }
        //        else
        //        {
        //            return 1;
        //        }
        //    }
        //    return 0;
        //}

        //private AlipayTradeRefundResponse DoRefund(string biz_content, AlipayDomain alipay)
        //{
        //    AlipayTradeRefundRequest payRequst = new AlipayTradeRefundRequest();
        //    payRequst.BizContent = biz_content;
        //    payRequst.SetNotifyUrl("");
        //    Dictionary<string, string> paramsDict = (Dictionary<string, string>)payRequst.GetParameters();
        //    IAopClient client = new DefaultAopClient(serverUrl, alipay.APP_ID, alipay.APP_PRIVATE_KEY, "", version,
        //        sign_type, alipay.ALIPAY_PUBLIC_KEY, charset);

        //    AlipayTradeRefundResponse payResponse = client.Execute(payRequst);

        //    return payResponse;

        //}


        /// <summary>
        /// 微信 
        /// 分销系统订单申请退款
        /// </summary>
        /// <param name="WeChatId"></param>
        /// <param name="sceneryDomain"></param>
        /// <returns>0 成功 1系统已经存在信息 2退款异常</returns>
        public string SceneryOrderRefund(int WeChatId, UserDomain CurrentUser)
        {

            LogHelper.WeChatLog(string.Format("退款开始-》操作人：{0}，退款ID:{1}", CurrentUser.Name, WeChatId));

            var sceneryOrderRefund = new SceneryOrderRefundDao();

            var models = sceneryOrderRefund.GetAllDomain(QueryCondition.Instance.AddEqual("Id", WeChatId.ToString()).AddEqual("ApprovalStatus", "0").AddEqual("RowState", "1"));

            var weChatDetailDao = new WeChatDetailDao();
            if (models.Count == 1)
            {
                //订单实体
                var sceneryOrder = models[0];
                //在交易表中查询是否已经退款过
                if (weChatDetailDao.IsExistsSceneryOrderRefund(sceneryOrder.BatchNumber, sceneryOrder.SerialId, -Convert.ToInt32(sceneryOrder.Total*100)) == false)
                {
                    //查询交易表中支付时的记录
                    var list = weChatDetailDao.GetAllDomain(QueryCondition.Instance.AddEqual("SceneryName", sceneryOrder.SceneryName)
                        .AddEqual("BatchNumber", sceneryOrder.BatchNumber).AddEqualLarger("Total_fee", "0").AddEqual("SerialId", ""));
                    if (list.Count == 1)
                    {
                        //退款实体
                        var model = list[0];                        


                        ////////////////////////////////////////////请求参数////////////////////////////////////////////

                        //微信交易号
                        string Transaction_id = model.Transaction_id;
                        //退款金额
                        string refund_amount =Convert.ToInt32(sceneryOrder.Total*100).ToString();                         

                        var query = QueryCondition.Instance.AddEqual("APPID", model.AppId).AddEqual("RowState", "1");
                        var wechat = new WxPayConfigDao().GetAllDomain(query);
                        if (wechat.Count != 1)
                        {
                            return "没有找到微信配置信息";
                        }


                        try
                        {
                            var  result = VisualSmart.BizService.Implements.WeChat.Api.Refund.Run(
                                             model.Transaction_id, model.Out_trade_no, model.Total_fee.ToString(), refund_amount, wechat[0]);                           

                            if (result.GetValue("return_code").ToString() == "SUCCESS" &&
                                result.GetValue("result_code").ToString() == "SUCCESS")//退款申请成功
                            {                             

                                var out_refund_no = result.GetValue("out_refund_no");//商户退款单号
                                var refund_id = result.GetValue("refund_id");//微信退款单号
                                var refund_fee = result.GetValue("refund_fee");//金额 分

                                LogHelper.WeChatLog(string.Format("退款成功-》微信退款单号：{0}", refund_id));

                                model.Out_refund_no = out_refund_no.ToString();
                                model.Refund_id = refund_id.ToString();
                                model.Total_fee = -Convert.ToInt32(refund_fee);
                                model.WeChatPlayDate = sceneryOrder.PlayDate;
                                model.SerialId = sceneryOrder.SerialId;
                                model.Creater = CurrentUser.Name;
                                model.Updater = CurrentUser.Name;
                                model.CreateTime = DateTime.Now;
                                model.UpdateTime = DateTime.Now;
                                if (weChatDetailDao.Add(model))
                                {
                                    //更新订单申请表状态
                                    sceneryOrderRefund.ComfirmRefund(sceneryOrder.Id, CurrentUser);
                                }
                                return "";
                            }
                            else
                            {
                                LogHelper.WriteLog(string.Format("退款失败：{0}，业务代码：{1}", result.GetValue("return_msg"), result.GetValue("err_code_des")));
                                return "微信退款失败，网关返回码：" + result.GetValue("return_msg") + " ,业务返回码：" + result.GetValue("err_code") + ",业务返回码描述:" + result.GetValue("err_code_des");
                            }
                        }
                        catch (WxPayException ex)
                        {
                            return ex.Message; 
                        }
                        catch (Exception ex)
                        {
                            return ex.Message; 
                        }     
                    }
                    return "交易信息中没有找到原始交易记录";
                }
                return "交易表中已经存在退款信息";
            }
            return "退款订单信息不存在";
        }
        #endregion
    }
}
