
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.Domain.Base;

namespace VisualSmart.Domain.SetUp
{ 	 
    [Serializable]
	public class AlipayDomain1: Entity
	{ 
		
		/// <summary>
		/// -
		/// </summary>		
		public int Id {get;set;}		
									
		/// <summary>
		/// 支付宝公钥
		/// </summary>		
		public string Alipay_public_key {get;set;}
															
		/// <summary>
		/// 私钥
		/// </summary>		
		public string Merchant_private_key {get;set;}
															
		/// <summary>
		/// 公钥
		/// </summary>		
		public string Merchant_public_key {get;set;}
															
		/// <summary>
		/// 创建时间
		/// </summary>		
		public DateTime CreateTime {get;set;}
															
		/// <summary>
		/// 创建人
		/// </summary>		
		public string Creater {get;set;}
															
		/// <summary>
		/// 修改时间
		/// </summary>		
		public DateTime UpdateTime {get;set;}
															
		/// <summary>
		/// 修改人
		/// </summary>		
		public string Updater {get;set;}
															
		/// <summary>
		/// 行状态 1 正常 0 删除
		/// </summary>		
		public Byte RowState {get;set;}
														}
}
	
 
  