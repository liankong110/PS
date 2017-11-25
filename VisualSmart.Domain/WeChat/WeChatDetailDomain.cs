using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSmart.Domain.WeChat
{
    public class WeChatDetailDomain:Base.Entity
    {
        private string _appid = "";
        private string _sceneryname = "";
        private string _batchnumber = "";
        private string _serialid = "";
        private string _result_code;
        private string _err_code = "";
        private string _err_code_des = "";
        private string _openid;
        private string _trade_type = "";
        private string _bank_type = "";
        private int _total_fee = 0;
        private int _settlement_total_fee = 0;
        private string _transaction_id = "";
        private string _out_trade_no = "";
        private DateTime _time_end = DateTime.Now;
        /// <summary>
        /// 
        /// </summary>
        public string AppId
        {
            set { _appid = value; }
            get { return _appid; }
        }
        /// <summary>
        /// 景区名称
        /// </summary>
        public string SceneryName
        {
            set { _sceneryname = value; }
            get { return _sceneryname; }
        }
        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNumber
        {
            set { _batchnumber = value; }
            get { return _batchnumber; }
        }
        /// <summary>
        /// 订单号
        /// </summary>
        public string SerialId
        {
            set { _serialid = value; }
            get { return _serialid; }
        }
        /// <summary>
        /// 业务结果
        /// </summary>
        public string Result_code
        {
            set { _result_code = value; }
            get { return _result_code; }
        }
        /// <summary>
        /// 错误代码
        /// </summary>
        public string Err_code
        {
            set { _err_code = value; }
            get { return _err_code; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Err_code_des
        {
            set { _err_code_des = value; }
            get { return _err_code_des; }
        }
        /// <summary>
        /// 用户标识
        /// </summary>
        public string Openid
        {
            set { _openid = value; }
            get { return _openid; }
        }
        /// <summary>
        /// 交易类型 JSAPI、NATIVE、APP
        /// </summary>
        public string Trade_type
        {
            set { _trade_type = value; }
            get { return _trade_type; }
        }
        /// <summary>
        /// 付款银行
        /// </summary>
        public string Bank_type
        {
            set { _bank_type = value; }
            get { return _bank_type; }
        }
        /// <summary>
        /// 订单金额
        /// </summary>
        public int Total_fee
        {
            set { _total_fee = value; }
            get { return _total_fee; }//分转元
        }
        /// <summary>
        /// 应结订单金额
        /// </summary>
        public int Settlement_total_fee
        {
            set { _settlement_total_fee = value; }
            get { return _settlement_total_fee; }
        }
        /// <summary>
        /// 微信支付订单号
        /// </summary>
        public string Transaction_id
        {
            set { _transaction_id = value; }
            get { return _transaction_id; }
        }
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string Out_trade_no
        {
            set { _out_trade_no = value; }
            get { return _out_trade_no; }
        }
        /// <summary>
        /// 支付完成时间
        /// </summary>
        public DateTime Time_end
        {
            set { _time_end = value; }
            get { return _time_end; }
        }

        /// <summary>
        /// 费率
        /// </summary>
        public decimal SceneryRate { get; set; }
        /// <summary>
        /// 游玩时间
        /// </summary>
        public DateTime WeChatPlayDate { get; set; }

        /// <summary>
        /// 商户退款单号
        /// </summary>
        public string Out_refund_no { get; set; }

        /// <summary>
        /// 微信退款单号
        /// </summary>
        public string Refund_id { get; set; }
    }
}
