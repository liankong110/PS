using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace VisualSmart.Domain.API
{
    public enum ErrorCode
    {
          /// <summary>
        /// 报文错误，未明确具体类型
        /// </summary>
        [Description("报文错误")]
        MessageError = 9000,
        /// <summary>
        /// 账号不存在
        /// </summary>
        [Description("账号不存在")]
        AccountNotExist = 8002,
        /// <summary>
        /// 缺少节点
        /// </summary>
        [Description("缺少节点")]
        MessageNodeMissing = 9002,
        /// <summary>
        /// 数字签名认证失败
        /// </summary>
        [Description("数字签名认证失败")]
        SignatureAuthenticateFail = 8003,
        /// <summary>
        /// 缺少参数
        /// </summary>
        [Description("缺少参数")]
        ParameterMissing = 1001,

        /// <summary>
        /// 参数值错误，如超出取值范围等
        /// </summary>
        [Description("参数值错误")]
        ParameterValueError = 1003,
        /// <summary>
        /// 执行成功(查询，新增，更新，删除等)
        /// 返回（0000）
        /// </summary>
        [Description("执行成功")]
        Success = 0000, 
        /// <summary>
        /// 报文格式错误
        /// </summary>
        [Description("报文格式错误")]
        MessageFormatError = 9001,
        /// <summary>
        /// 服务名错误
        /// </summary>
        [Description("服务名错误")]
        ServiceNameError = 9004,
        /// <summary>
        /// 未知异常
        /// </summary>
        [Description("未知异常")]
        UnknownError = 5000,
        /// <summary>
        /// 数据查询失败
        /// </summary>
        [Description("数据查询失败")]
        SearchFail = 2002,     
        /// <summary>
        /// 没有找到原始支付数据
        /// </summary>
        [Description("没有找到原始支付数据")]
        NoAlipayDetail = 2003,
        /// <summary>
        /// 已经存在申请退款信息
        /// </summary>
        [Description("已经存在申请退款信息")]
        HadRequestReFund = 2004,
    }
}