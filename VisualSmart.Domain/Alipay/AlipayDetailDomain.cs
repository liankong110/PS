using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.Domain.Base;

namespace VisualSmart.Domain.Alipay
{
    public class AlipayDetailDomain : Entity
    {
        	 
		#region Model
	 
		private string _buyer_logon_id="";
        //private decimal _buyer_pay_amount=0M;
        //private string _buyer_user_id="";
        //private decimal _invoice_amount=0M;
        //private string _open_id="";
		private string _out_trade_no="";
        //private decimal _receipt_amount=0M;
		private DateTime _send_pay_date= Convert.ToDateTime("1900-1-1");
		private decimal _total_amount=0M;
		private string _trade_no="";
		private string _trade_status;
		private string _code="";
		private string _msg="";
	 
		 
		/// <summary>
		/// 买家支付宝账号
		/// </summary>
		public string Buyer_logon_id
		{
			set{ _buyer_logon_id=value;}
			get{return _buyer_logon_id;}
		}
		/// <summary>
		/// 付款金额
		/// </summary>
        //public decimal Buyer_pay_amount
        //{
        //    set{ _buyer_pay_amount=value;}
        //    get{return _buyer_pay_amount;}
        //}
		/// <summary>
		/// 买家在支付宝的用户id 
		/// </summary>
        //public string Buyer_user_id
        //{
        //    set{ _buyer_user_id=value;}
        //    get{return _buyer_user_id;}
        //}
		/// <summary>
		/// 交易中用户支付的可开具发票的金额，单位为元，两位小数。
		/// </summary>
        //public decimal Invoice_amount
        //{
        //    set{ _invoice_amount=value;}
        //    get{return _invoice_amount;}
        //}
		/// <summary>
		/// 买家支付宝用户号
		/// </summary>
        //public string Open_id
        //{
        //    set{ _open_id=value;}
        //    get{return _open_id;}
        //}
		/// <summary>
		/// 商家订单号
		/// </summary>
		public string Out_trade_no
		{
			set{ _out_trade_no=value;}
			get{return _out_trade_no;}
		}
		/// <summary>
		/// 实收金额，单位为元，两位小数
		/// </summary>
        //public decimal Receipt_amount
        //{
        //    set{ _receipt_amount=value;}
        //    get{return _receipt_amount;}
        //}
		/// <summary>
		/// 本次交易打款给卖家的时间
		/// </summary>
		public DateTime Send_pay_date
		{
			set{ _send_pay_date=value;}
			get{return _send_pay_date;}
		}
		/// <summary>
		/// 交易的订单金额，单位为元，两位小数。
		/// </summary>
		public decimal Total_amount
		{
			set{ _total_amount=value;}
			get{return _total_amount;}
		}
		/// <summary>
		/// 支付宝交易号
		/// </summary>
		public string Trade_no
		{
			set{ _trade_no=value;}
			get{return _trade_no;}
		}
		/// <summary>
		/// 交易状态：WAIT_BUYER_PAY（交易创建，等待买家付款）、TRADE_CLOSED（未付款交易超时关闭，或支付完成后全额退款）、TRADE_SUCCESS（交易支付成功）、TRADE_FINISHED（交易结束，不可退款） 
		/// </summary>
		public string Trade_status
		{
			set{ _trade_status=value;}
			get{return _trade_status;}
		}
		/// <summary>
		/// 网关返回码
		/// </summary>
		public string Code
		{
			set{ _code=value;}
			get{return _code;}
		}
		/// <summary>
		/// 网关返回码描述
		/// </summary>
		public string Msg
		{
			set{ _msg=value;}
			get{return _msg;}
		}
        /// <summary>
        /// 景区名称
        /// </summary>
        public string SceneryName { get; set; }

        /// <summary>
        /// 支付宝APPID
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 分销系统批次号-用于对账使用
        /// </summary>
        public string BatchNumber { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string SerialId { get; set; }

        /// <summary>
        /// 费率
        /// </summary>
        public decimal SceneryRate { get; set; }

        /// <summary>
        /// 支付游玩时间
        /// </summary>
        public DateTime AlipayPlayDate { get; set; }
		#endregion Model

    }
}
