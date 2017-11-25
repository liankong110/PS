using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.BizService.Alipay;
using VisualSmart.Domain.Alipay.Precreate;
using VisualSmart.Domain.Alipay.Query;
using VisualSmart.Domain.Alipay;
using Aop.Api;
using VisualSmart.Util;
using Newtonsoft.Json;
using Aop.Api.Response;
using Aop.Api.Request;
using System.Threading;
using VisualSmart.Dao.DataQuickStart.Alipay;
using VisualSmart.Domain.SetUp;
using VisualSmart.Domain.Alipay.Refund;
using VisualSmart.Dao.DataQuickStart.SceneryOrder;

namespace VisualSmart.BizService.Implements.Alipay
{
    public class AlipayTradeBizService : IAlipayTradeBizService
    {
        public IAopClient client;

        #region 支付宝基本参数信息
        public string charset = "utf-8";
        public string sign_type = "RSA";
        public string version = "1.0";

        //测试环境
        //public string serverUrl = "https://openapi.alipaydev.com/gateway.do";

        //正式环境
        public string serverUrl = "https://openapi.alipay.com/gateway.do";

        public string mapiUrl = "https://mapi.alipay.com/gateway.do";
        #endregion

        #region 预创建交易
        /// <summary>
        /// 预创建交易
        /// </summary>
        /// <param name="request"></param>
        /// <param name="sceneryDomain"></param>
        /// <returns></returns>
        public PrecreateResponse Precreate(PrecreateRequest request, SceneryDomain sceneryDomain)
        {
            AlipayQRCodeDomain qrCode = new AlipayQRCodeDomain();
            qrCode.BatchNumber = request.out_trade_no;
            qrCode.SceneryName = sceneryDomain.SceneryName;
            qrCode.Creater = request.terminal_id;
            qrCode.Updater = request.terminal_id;
            qrCode.Phone = request.Phone;

            request.extend_params.sys_service_provid = sceneryDomain.Apipay.PID;
            PrecreateResponse responseAlipay = new PrecreateResponse();
            try
            {
                //创建淘宝支付信息
                client = new DefaultAopClient(serverUrl, sceneryDomain.Apipay.APP_ID, sceneryDomain.Apipay.APP_PRIVATE_KEY, "", version,
     sign_type, sceneryDomain.Apipay.ALIPAY_PUBLIC_KEY, charset);
                //生成商户订单号
                request.out_trade_no = GenerateSerialIdService.Get(FixedPrefix.Alipay, CustomPrefix.A, sceneryDomain.SceneryTCId.ToString()).SerialId;
                string content = JsonConvert.SerializeObject(request);
                qrCode.RequestJson = content;

                qrCode.Out_trade_no = request.out_trade_no;
                LogHelper.AlipayLog(string.Format("商户订单号：{0} 请求josn：{1}", request.out_trade_no, content));

                string QrCode = "";
                AlipayTradePrecreateResponse payResponse = Prepay(content);
                //payResponse.QrCode即二维码对于的链接
                //将链接用二维码工具生成二维码打印出来，顾客可以用支付宝钱包扫码支付。
                if (payResponse != null)
                {
                    string result = payResponse.Body;
                    if (payResponse.Code != ResultCode.SUCCESS)
                    {
                        qrCode.Remark = result;
                    }
                    else
                    {
                        qrCode.Remark = payResponse.Code;
                    }
                    switch (payResponse.Code)
                    {
                        case ResultCode.SUCCESS:
                            //预下单成功                         
                            QrCode = payResponse.QrCode;
                            qrCode.QrCode = QrCode;
                            LogHelper.AlipayLog(string.Format("请求成功，二维码地址：{0}", QrCode));
                            break;

                        case ResultCode.FAIL:
                            StringBuilder sb2 = new StringBuilder();
                            sb2.Append("{\"out_trade_no\":\"" + request.out_trade_no + "\"}");
                            LogHelper.AlipayLog(string.Format("请求失败，保存信息：{0}", result));
                            Cancel(sb2.ToString());
                            break;
                    }
                }

                responseAlipay.QRCode = QrCode;
                responseAlipay.Out_trade_no = request.out_trade_no;
                qrCode.AppId = sceneryDomain.Apipay.APP_ID;
            }
            catch (Exception ex)
            {
                LogHelper.AlipayLog(string.Format("获取支付宝URL:请求失败，保存信息：{0}", ex.Message));
            }
            finally
            {
                try
                {
                    new AlipayQRCodeDao().Add(qrCode);
                }
                catch (Exception ex)
                {
                    LogHelper.AlipayLog(string.Format("获取支付宝URL:新增操作日志失败：{0}", ex.Message));
                }
            }
            return responseAlipay;
        }


        /// <summary>
        /// 请求支付宝创建订单接口
        /// </summary>
        /// <param name="biz_content"></param>
        /// <returns></returns>
        private AlipayTradePrecreateResponse Prepay(string biz_content)
        {
            AlipayTradePrecreateRequest payRequst = new AlipayTradePrecreateRequest();
            payRequst.BizContent = biz_content;
            //需要异步通知的时候，需要是指接收异步通知的地址。
            payRequst.SetNotifyUrl("http://alipayapi.zhilvtx.com/API/ALIPAY/Notify");
            Dictionary<string, string> paramsDict = (Dictionary<string, string>)payRequst.GetParameters();
            AlipayTradePrecreateResponse payResponse = client.Execute(payRequst);
            return payResponse;
        }

        /// <summary>
        /// 取消订单接口
        /// </summary>
        /// <param name="biz_content"></param>
        /// <returns></returns>
        private AlipayTradeCancelResponse Cancel(string biz_content)
        {
            LogHelper.AlipayLog(string.Format("正在取消订单"));

            AlipayTradeCancelRequest cancelRequest = new AlipayTradeCancelRequest();
            cancelRequest.BizContent = biz_content;
            AlipayTradeCancelResponse cancelResponse = client.Execute(cancelRequest);

            if (null != cancelResponse)
            {

                if (cancelResponse.Code == ResultCode.FAIL && cancelResponse.RetryFlag == "Y")
                {
                    //if (cancelResponse.Body.Contains("\"retry_flag\":\"Y\""))		
                    //cancelOrderRetry(biz_content);
                    // 新开一个线程重试撤销
                    ParameterizedThreadStart ParStart = new ParameterizedThreadStart(cancelOrderRetry);
                    Thread myThread = new Thread(ParStart);
                    object o = biz_content;
                    myThread.Start(o);
                    LogHelper.AlipayLog(string.Format("取消-结果：{0}-{1}", cancelResponse.Code, cancelResponse.Body));
                }
                if (cancelResponse.Code == ResultCode.SUCCESS)
                {
                    //lblMessage.Text = cancelResponse.Body;
                    LogHelper.AlipayLog(string.Format("取消-结果：{0}", cancelResponse.Code));
                }

            }

            return cancelResponse;

        }
        /// <summary>
        /// 取消订单方法
        /// </summary>
        /// <param name="o"></param>

        public void cancelOrderRetry(object o)
        {
            int retryCount = 50;

            for (int i = 0; i < retryCount; ++i)
            {

                Thread.Sleep(10000);
                AlipayTradeCancelRequest cancelRequest = new AlipayTradeCancelRequest();
                cancelRequest.BizContent = o.ToString();
                // Dictionary<string, string> paramsDict = (Dictionary<string, string>)cancelRequest.GetParameters();
                AlipayTradeCancelResponse cancelResponse = client.Execute(cancelRequest);

                if (null != cancelResponse)
                {
                    if (cancelResponse.Code == ResultCode.FAIL)
                    {
                        LogHelper.AlipayLog(string.Format("取消-结果：{0},请求内容：{1}", cancelResponse.Code, o));
                        if (cancelResponse.RetryFlag == "N")
                        {
                            break;
                        }
                    }
                    if ((cancelResponse.Code == ResultCode.SUCCESS))
                    {
                        LogHelper.AlipayLog(string.Format("取消-结果：{0}", cancelResponse.Code));

                        break;
                    }
                }

                if (i == retryCount - 1)
                {
                    // 处理到最后一次，还是未撤销成功，需要在商户数据库中对此单最标记，人工介入处理
                    //lblMessage.Text = cancelResponse.Body;
                    LogHelper.AlipayLog(string.Format("处理到最后一次，还是未撤销成功，需要在商户数据库中对此单最标记，人工介入处理,请求内容：{0}", o));
                }

            }
        }
        #endregion

        #region 查询订单状态
        /// <summary>
        /// 查询订单状态
        /// </summary>
        /// <param name="request"></param>
        /// <param name="sceneryDomain"></param>
        /// <returns></returns>
        public QueryResponse Query(QueryRequest request, SceneryDomain sceneryDomain)
        {
            var result = false;
            //创建淘宝支付信息
            client = new DefaultAopClient(serverUrl, sceneryDomain.Apipay.APP_ID, sceneryDomain.Apipay.APP_PRIVATE_KEY, "", version,
 sign_type, sceneryDomain.Apipay.ALIPAY_PUBLIC_KEY, charset);

            QueryResponse queryResponse = new QueryResponse();
            StringBuilder biz_content = new StringBuilder();
            biz_content.Append("{\"out_trade_no\":\"" + request.Out_trade_no + "\"}");


            AlipayTradeQueryRequest payRequst = new AlipayTradeQueryRequest();
            payRequst.BizContent = biz_content.ToString();

            Dictionary<string, string> paramsDict = (Dictionary<string, string>)payRequst.GetParameters();
            AlipayTradeQueryResponse payResponse = null;


            LogHelper.AlipayLog(string.Format("论寻-请求josn：{0}", biz_content));
            payResponse = client.Execute(payRequst);

            LogHelper.AlipayLog(string.Format("论寻-结果：{0}-{1}-{2}", payResponse.Code, payResponse.TradeStatus, payResponse.Body));
            if (string.Compare(payResponse.Code, ResultCode.SUCCESS, false) == 0)
            {
                if (payResponse.TradeStatus == "TRADE_FINISHED"
                    || payResponse.TradeStatus == "TRADE_SUCCESS"
                    || payResponse.TradeStatus == "TRADE_CLOSED")
                {
                    if (payResponse.TradeStatus == "TRADE_SUCCESS")
                    {
                        var detailDao = new AlipayDetailDao();
                        var qrCodeDao = new AlipayQRCodeDao();
                        Alipay_trade_query_response alipayDetail = JsonConvert.DeserializeObject<Alipay_trade_query_response>(payResponse.Body);
                        //需要检查数据库中是否有改数据 有的话 直接返回
                        var QRCodeDetail = qrCodeDao.IsExistOut_trade_no(alipayDetail.alipay_trade_query_response.Out_trade_no, alipayDetail.alipay_trade_query_response.Total_amount
                            , sceneryDomain.Apipay.APP_ID);

                        if (result == null || QRCodeDetail.SceneryName == "-1")
                        {
                            LogHelper.AlipayLog("论寻：无通知参数");
                        }
                        //论寻:数据已经存在，不需要插入
                        if (QRCodeDetail.SceneryName != "-2")
                        {
                            //保存支付宝信息
                            string body = payResponse.Body;
                            alipayDetail.alipay_trade_query_response.SceneryName = sceneryDomain.SceneryName;
                            alipayDetail.alipay_trade_query_response.Creater = sceneryDomain.Apipay.APP_ID;
                            alipayDetail.alipay_trade_query_response.Updater = "";
                            alipayDetail.alipay_trade_query_response.AppId = sceneryDomain.Apipay.APP_ID;
                            alipayDetail.alipay_trade_query_response.BatchNumber = QRCodeDetail.BatchNumber;
                            alipayDetail.alipay_trade_query_response.SceneryRate = sceneryDomain.Rate;
                            alipayDetail.alipay_trade_query_response.AlipayPlayDate =Convert.ToDateTime(alipayDetail.alipay_trade_query_response.Send_pay_date.ToString("yyyy-MM-dd"));
                            detailDao.Add(alipayDetail.alipay_trade_query_response);
                        }
                        else
                        {
                            LogHelper.AlipayLog("论寻:数据已经存在，不需要插入");
                        }
                        queryResponse.buyer_logon_id = alipayDetail.alipay_trade_query_response.Buyer_logon_id;
                        queryResponse.gmt_payment = alipayDetail.alipay_trade_query_response.Send_pay_date;
                        queryResponse.trade_no = alipayDetail.alipay_trade_query_response.Trade_no;
                        result = true;
                    }
                }
            }

            if (result == false && request.IsLastRequest == 1)
            {
                var content = new StringBuilder();
                content.Append("{\"out_trade_no\":\"" + payResponse.OutTradeNo + "\"}");
                biz_content = content;
                Cancel(content.ToString());
            }
            return queryResponse;
        }
        #endregion

        #region 退款
        /// <summary>
        /// 交易信息全部退款
        /// </summary>
        /// <param name="alipayId"></param>
        /// <param name="sceneryDomain"></param>
        /// <returns>0 成功 1系统已经存在信息 2退款异常</returns>
        public int Refund(int alipayId, UserDomain CurrentUser)
        {
            LogHelper.AlipayLog(string.Format("退款开始-》操作人：{0}，退款ID:{1}", CurrentUser.Name, alipayId));
            var alipayDetailDao = new AlipayDetailDao();
            var list = alipayDetailDao.GetAllDomain(QueryCondition.Instance.AddEqual("Id", alipayId.ToString()));
            if (list.Count == 1)
            {
                var model = list[0];
                var tempTotal = -model.Total_amount;

                if (!alipayDetailDao.IsExistsOut_trade_no(model.Out_trade_no, tempTotal, model.AppId))
                {
                    ////////////////////////////////////////////请求参数////////////////////////////////////////////

                    //支付宝交易号
                    string trade_no = model.Trade_no;
                    //支付宝交易号与商户网站订单号不能同时为空
                    string out_request_no = model.Out_trade_no;
                    //退款金额
                    string refund_amount = model.Total_amount.ToString();
                    //退款金额不能大于订单金额

                    //商户网站订单系统中唯一订单号，必填
                    StringBuilder request = new StringBuilder();
                    request.Append("{\"out_request_no\":\"" + out_request_no + "\",");
                    request.Append("\"trade_no\":\"" + trade_no + "\",");
                    request.Append("\"refund_amount\":\"" + refund_amount + "\"}");

                    var query = QueryCondition.Instance.AddEqual("APP_ID", model.AppId).AddEqual("RowState", "1");

                    var alipay = new AlipayDao().GetAllDomain(query);
                    if (alipay.Count != 1)
                    {
                        return 2;
                    }
                    LogHelper.AlipayLog(string.Format("退款JSON：{0},APP_ID:{1}", request.ToString(), alipay[0].APP_ID));
                    AlipayTradeRefundResponse refundResponse = DoRefund(request.ToString(), alipay[0]);

                    //请在这里加上商户的业务逻辑程序代码
                    //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
                    string result = refundResponse.Body;

                    if (refundResponse.Code == "10000")//退款成功
                    {
                        Alipay_trade_refund_response alipayRefund = JsonConvert.DeserializeObject<Alipay_trade_refund_response>(refundResponse.Body);
                        model.Code = alipayRefund.alipay_trade_refund_response.code;
                        model.Trade_no = alipayRefund.alipay_trade_refund_response.trade_no;
                        model.Out_trade_no = alipayRefund.alipay_trade_refund_response.out_trade_no;
                        model.Total_amount = alipayRefund.alipay_trade_refund_response.refund_fee;
                        model.Send_pay_date = alipayRefund.alipay_trade_refund_response.gmt_refund_pay;
                        model.Msg = alipayRefund.alipay_trade_refund_response.msg;

                        model.Creater = CurrentUser.Name;
                        model.Updater = CurrentUser.Name;
                        model.CreateTime = DateTime.Now;
                        model.UpdateTime = DateTime.Now;

                        model.Total_amount = -model.Total_amount;
                        //新增一个负数的支付单信息。
                        alipayDetailDao.Add(model);
                        //新增一个退款申请的信息。
                        //判断退款信息是否存在。
                    }
                    else
                    {
                        LogHelper.AlipayLog(string.Format("退款失败：{0}，body：{1}", refundResponse.Code, result));
                        return 2;
                    }
                }
                else
                {
                    return 1;
                }
            }
            return 0;
        }

        private AlipayTradeRefundResponse DoRefund(string biz_content, AlipayDomain alipay)
        {
            AlipayTradeRefundRequest payRequst = new AlipayTradeRefundRequest();
            payRequst.BizContent = biz_content;
            payRequst.SetNotifyUrl("");
            Dictionary<string, string> paramsDict = (Dictionary<string, string>)payRequst.GetParameters();
            IAopClient client = new DefaultAopClient(serverUrl, alipay.APP_ID, alipay.APP_PRIVATE_KEY, "", version,
                sign_type, alipay.ALIPAY_PUBLIC_KEY, charset);

            AlipayTradeRefundResponse payResponse = client.Execute(payRequst);

            return payResponse;

        }


        /// <summary>
        /// 分销系统订单申请退款
        /// </summary>
        /// <param name="alipayId"></param>
        /// <param name="sceneryDomain"></param>
        /// <returns>0 成功 1系统已经存在信息 2退款异常</returns>
        public string SceneryOrderRefund(int alipayId, UserDomain CurrentUser)
        {

            LogHelper.AlipayLog(string.Format("退款开始-》操作人：{0}，退款ID:{1}", CurrentUser.Name,alipayId));

            var sceneryOrderRefund = new SceneryOrderRefundDao();

            var models = sceneryOrderRefund.GetAllDomain(QueryCondition.Instance.AddEqual("Id", alipayId.ToString()).AddEqual("ApprovalStatus", "0").AddEqual("RowState", "1"));

            var alipayDetailDao = new AlipayDetailDao();
            if (models.Count == 1)
            {
                //订单实体
                var sceneryOrder = models[0];
                //在交易表中查询是否已经退款过
                if (alipayDetailDao.IsExistsSceneryOrderRefund(sceneryOrder.BatchNumber, sceneryOrder.SerialId, -sceneryOrder.Total) == false)
                {
                    //查询交易表中支付时的记录
                    var list = alipayDetailDao.GetAllDomain(QueryCondition.Instance.AddEqual("SceneryName", sceneryOrder.SceneryName)
                        .AddEqual("BatchNumber", sceneryOrder.BatchNumber).AddEqualLarger("Total_amount", "0").AddEqual("SerialId", ""));
                    if (list.Count == 1)
                    {
                        //退款实体
                        var model = list[0];
                       // var tempTotal = -sceneryOrder.Total;


                        ////////////////////////////////////////////请求参数////////////////////////////////////////////

                        //支付宝交易号
                        string trade_no = model.Trade_no;
                        //退款金额
                        string refund_amount = sceneryOrder.Total.ToString();
                        //退款金额不能大于订单金额

                        //商户网站订单系统中唯一订单号，必填
                        StringBuilder request = new StringBuilder();
                        request.Append("{\"out_request_no\":\"" + sceneryOrder.SerialId + "\",");//本次退款请求流水号，部分退款时必传
                        request.Append("\"trade_no\":\"" + trade_no + "\",");//支付时返回的支付宝交易号，与out_trade_no必填一个
                        request.Append("\"refund_amount\":\"" + refund_amount + "\"}");

                        var query = QueryCondition.Instance.AddEqual("APP_ID", model.AppId).AddEqual("RowState", "1");


                        var alipay = new AlipayDao().GetAllDomain(query);
                        if (alipay.Count != 1)
                        {
                            return "没有找到支付宝配置信息";
                        }

                        LogHelper.AlipayLog(string.Format("退款JSON：{0},APP_ID:{1}", request.ToString(), alipay[0].APP_ID));
                        AlipayTradeRefundResponse refundResponse = DoRefund(request.ToString(), alipay[0]);

                        //请在这里加上商户的业务逻辑程序代码
                        //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
                        string result = refundResponse.Body;

                        if (refundResponse.Code == "10000")//退款成功
                        {
                            Alipay_trade_refund_response alipayRefund = JsonConvert.DeserializeObject<Alipay_trade_refund_response>(refundResponse.Body);
                            model.Code = alipayRefund.alipay_trade_refund_response.code;
                            model.Trade_no = alipayRefund.alipay_trade_refund_response.trade_no;
                            model.Out_trade_no = alipayRefund.alipay_trade_refund_response.out_trade_no;
                            model.Total_amount = alipayRefund.alipay_trade_refund_response.refund_fee;
                            model.Send_pay_date = alipayRefund.alipay_trade_refund_response.gmt_refund_pay;
                            model.Msg = alipayRefund.alipay_trade_refund_response.msg;
                            model.SerialId = sceneryOrder.SerialId;
                            model.Creater = CurrentUser.Name;
                            model.Updater = CurrentUser.Name;
                            model.CreateTime = DateTime.Now;
                            model.UpdateTime = DateTime.Now;
                            model.Total_amount = -sceneryOrder.Total;
                            model.AlipayPlayDate = sceneryOrder.PlayDate;
                            if (alipayDetailDao.Add(model))
                            {
                                //更新订单申请表状态
                                sceneryOrderRefund.ComfirmRefund(sceneryOrder.Id, CurrentUser);
                            }

                            return "";
                        }
                        else
                        {
                            LogHelper.AlipayLog(string.Format("退款失败：{0}，body：{1}", refundResponse.Code, result));
                            return "支付宝退款失败，网关返回码：" + refundResponse.Code + " ,业务返回码：" + refundResponse.SubCode + ",业务返回码描述:"+refundResponse.SubMsg ;
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

    public class ResultCode
    {
        public const string SUCCESS = "10000";
        public const string INRROCESS = "10003";
        public const string FAIL = "40004";
    }
}
