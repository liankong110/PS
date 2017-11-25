using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Web;

namespace VisualSmart.Util
{
    /// <summary>
    /// 订单生成结果信息
    /// </summary>
    public class ResultData
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public ResultCode ResultCode { get; set; }

        /// <summary>
        /// 错误详细信息
        /// </summary>
        public string ErrorDetails { get; set; }
    }

    /// <summary>
    /// 产生订单号结果
    /// </summary>
    public class SerialIdResultData : ResultData
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string SerialId { get; set; }
    }

    /// <summary>
    /// 反向解析订单号
    /// </summary>
    public class SerialIdDetailResultData : ResultData
    {
        /// <summary>
        /// 默认构造
        /// </summary>
        public SerialIdDetailResultData()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public SerialIdDetailResultData(FixedPrefix fixedPrefix, CustomPrefix customPrefix, DateTime createDateTime, string ip, int random)
        {
            FixedPrefix = fixedPrefix;
            CustomPrefix = customPrefix;
            CreateDateTime = createDateTime;
            Ip = ip;
            Random = random;
        }

        /// <summary>
        /// 固定前缀
        /// </summary>
        public FixedPrefix FixedPrefix { get; private set; }

        /// <summary>
        /// 自定义前缀
        /// </summary>
        public CustomPrefix CustomPrefix { get; private set; }

        /// <summary>
        /// 订单号产生的日期
        /// </summary>
        public DateTime CreateDateTime { get; private set; }

        /// <summary>
        /// 订单号产生的机器的IP地址
        /// </summary>
        public string Ip { get; private set; }

        /// <summary>
        /// 订单号产生的随机的3位数字
        /// </summary>
        public int Random { get; private set; }
    }

    /// <summary>
    /// 固定前缀
    /// </summary>
    public enum FixedPrefix
    {
        /// <summary>
        /// 支付宝
        /// </summary>
        [Description("A")]
        Alipay,
        /// <summary>
        /// 微信
        /// </summary>
        [Description("W")]
        WeChat
         
    }

    /// <summary>
    /// 各个项目扩展前缀，默认A
    /// </summary>
    public enum CustomPrefix
    {
        /// <summary>
        /// A
        /// </summary>
        [Description("A")]
        A,

        /// <summary>
        /// B
        /// </summary>
        [Description("B")]
        B,

        /// <summary>
        /// C
        /// </summary>
        [Description("C")]
        C,

        /// <summary>
        /// D
        /// </summary>
        [Description("D")]
        D,

        /// <summary>
        /// E
        /// </summary>
        [Description("E")]
        E,

        /// <summary>
        /// F
        /// </summary>
        [Description("F")]
        F,

        /// <summary>
        /// G
        /// </summary>
        [Description("G")]
        G,

        /// <summary>
        /// H
        /// </summary>
        [Description("H")]
        H,

        /// <summary>
        /// I
        /// </summary>
        [Description("I")]
        I,

        /// <summary>
        /// J
        /// </summary>
        [Description("J")]
        J,

        /// <summary>
        /// K
        /// </summary>
        [Description("K")]
        K,

        /// <summary>
        /// L
        /// </summary>
        [Description("L")]
        L,

        /// <summary>
        /// M
        /// </summary>
        [Description("M")]
        M,

        /// <summary>
        /// N
        /// </summary>
        [Description("N")]
        N,

        /// <summary>
        /// O
        /// </summary>
        [Description("O")]
        O,

        /// <summary>
        /// P
        /// </summary>
        [Description("P")]
        P,

        /// <summary>
        /// Q
        /// </summary>
        [Description("Q")]
        Q,

        /// <summary>
        /// R
        /// </summary>
        [Description("R")]
        R,

        /// <summary>
        /// S
        /// </summary>
        [Description("S")]
        S,

        /// <summary>
        /// T
        /// </summary>
        [Description("T")]
        T,

        /// <summary>
        /// U
        /// </summary>
        [Description("U")]
        U,

        /// <summary>
        /// V
        /// </summary>
        [Description("V")]
        V,

        /// <summary>
        /// W
        /// </summary>
        [Description("W")]
        W,

        /// <summary>
        /// X
        /// </summary>
        [Description("X")]
        X,

        /// <summary>
        /// Y
        /// </summary>
        [Description("Y")]
        Y,

        /// <summary>
        /// Z
        /// </summary>
        [Description("Z")]
        Z
    }

    /// <summary>
    /// 结果状态码
    /// </summary>
    public enum ResultCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        Success,

        /// <summary>
        /// 失败
        /// </summary>
        [Description("失败")]
        Error 
        
    }

    /// <summary>
    /// 生产订单号
    /// </summary>
    public static class GenerateSerialIdService
    {
        /// <summary>
        /// 对象锁
        /// </summary>
        private readonly static object LockObj = new object();

        /// <summary>
        /// 本机IP地址
        /// </summary>
        private static string _localIp = string.Empty;

        /// <summary>
        /// 首次开始使用的时间戳
        /// </summary>
        private static long? _beginTimespan;

        /// <summary>
        ///  获取订单号 规则： 前缀 + 时间戳 + 16进制IP + 3位随机数字
        ///  前缀： 长度为2位，第1位代表项目(酒店：H，国内机票：F，国际机票：I，租车：Z，景区：S，度假：L，投诉：T，自助游：A，保险：B)。
        ///     第1位由公共控制。
        ///     第2位由各个项目根据实际情况自定义。范围是字母(A-Z)，一共26个应该够用，如果不传，默认为A
        ///  时间戳：长度10位， 表示1970-1-1到当前时间的总的秒数。 长度为10位。到 2286年会升级到11位。
        ///  16进制IP 
        ///     需要注意的是，在一台物理机器上，如果存在多个虚拟机，则会造成相同的MAC地址。
        ///  随机数字：长度3位， 表示1秒之内，可以产生不同的订单号的数量，也就是1秒内1台机器最多可以产生999个订单号
        /// </summary>
        /// <param name="fixedPrefix">固定前缀</param>
        /// <param name="customPrefix">自定义后缀，默认值：A</param>
        /// <returns>订单号</returns>
        public static SerialIdResultData Get(FixedPrefix fixedPrefix, CustomPrefix? customPrefix, string SceneryTCId)
        {
            const string zero = "0";
            const string defaultLocalIp = "00000000";
            lock (LockObj)
            {
                if (!_beginTimespan.HasValue)//IIS被回收或者首次使用
                {
                    _beginTimespan = Helper.ConvertDateTimeInt(DateTime.Now);
                }
                var successSerialId = string.Empty;
                while (string.IsNullOrEmpty(successSerialId))
                {
                    //获取当前时间戳
                    var timestamp = Helper.ConvertDateTimeInt(DateTime.Now);
                    //判断首次使用的时间与当前时间
                    if ((timestamp - _beginTimespan.Value).Equals(0))
                    {
                        Thread.Sleep(10);
                        continue;
                    }
                    //获取固定前缀
                    var fixPrefix = Helper.GetEnumDesc(fixedPrefix);
                    //用户自定义前缀为空，则为A
                    customPrefix = customPrefix ?? CustomPrefix.A;
                    //获取用户自定义前缀
                    var cpx = Helper.GetEnumDesc(customPrefix);
                    //判断本机IP是否为空，为空则获取
                    if (string.IsNullOrEmpty(_localIp))
                    {
                        _localIp = GetIpAddress();
                    }
                    //获取不到本机IP，则用默认值代替
                    if (string.IsNullOrEmpty(_localIp))
                    {
                        _localIp = defaultLocalIp;
                    }
                    //构造随机种子
                    var seed = Guid.NewGuid().GetHashCode();
                    var random = new Random(seed);
                    //随机订单号的最后3位
                    var timefix = random.Next(0, 999).ToString(CultureInfo.InvariantCulture);
                    while (timefix.Length < 3)//不足3位前面补0
                    {
                        timefix = zero + timefix;
                    }
                    //构建订单号
                    //转换时间戳为16进制，并替换E为U
                    var time = Convert.ToString(timestamp, 16).ToUpper().Replace('E', 'U');
                    //IP替换E为U
                    var localIp = _localIp.ToUpper().Replace('E', 'U');
                    var serialId = fixPrefix + SceneryTCId + cpx + time + localIp + timefix;
                    serialId = serialId.ToLower();
                    //判断是否重复
                    var isDone = SystemCacheHelper.Store(serialId);
                    successSerialId = isDone ? serialId : string.Empty;
                }
                //转为小写
                successSerialId = successSerialId.ToLower();
                var resultData = new SerialIdResultData
                                     {
                                         ResultCode = string.IsNullOrEmpty(_localIp) ? ResultCode.Error : ResultCode.Success,
                                         SerialId = successSerialId
                                     };
                return resultData;
            }
        }

        /// <summary>
        /// 检售票系统订单号获取
        /// </summary>
        /// <param name="fixedPrefix"></param>
        /// <param name="customPrefix"></param>
        /// <returns></returns>
        public static SerialIdResultData CheckTicketGet(FixedPrefix fixedPrefix, CustomPrefix? customPrefix)
        {
            const string zero = "0";
            const string defaultLocalIp = "00000000";
            lock (LockObj)
            {
                if (!_beginTimespan.HasValue)//IIS被回收或者首次使用
                {
                    _beginTimespan = Helper.ConvertDateTimeInt(DateTime.Now);
                }
                var successSerialId = string.Empty;
                while (string.IsNullOrEmpty(successSerialId))
                {
                    //获取当前时间戳
                    var timestamp = Helper.ConvertDateTimeInt(DateTime.Now);
                    //判断首次使用的时间与当前时间
                    if ((timestamp - _beginTimespan.Value).Equals(0))
                    {
                        Thread.Sleep(10);
                        continue;
                    }
                    //获取固定前缀
                    var fixPrefix = Helper.GetEnumDesc(fixedPrefix);
                    //用户自定义前缀为空，则为A
                    customPrefix = customPrefix ?? CustomPrefix.A;
                    //获取用户自定义前缀
                    var cpx = Helper.GetEnumDesc(customPrefix);
                    //判断本机IP是否为空，为空则获取
                    if (string.IsNullOrEmpty(_localIp))
                    {
                        _localIp = GetIpAddress();
                    }
                    //获取不到本机IP，则用默认值代替
                    if (string.IsNullOrEmpty(_localIp))
                    {
                        _localIp = defaultLocalIp;
                    }
                    //构造随机种子
                    var seed = Guid.NewGuid().GetHashCode();
                    var random = new Random(seed);
                    //随机订单号的最后3位
                    var timefix = random.Next(0, 999).ToString(CultureInfo.InvariantCulture);
                    while (timefix.Length < 3)//不足3位前面补0
                    {
                        timefix = zero + timefix;
                    }
                    //构建订单号
                    //转换时间戳为16进制，并替换E为U
                    var time = Convert.ToString(timestamp, 16).ToUpper().Replace('E', 'U');
                    //IP替换E为U
                    var localIp = _localIp.ToUpper().Replace('E', 'U');
                    var serialId = fixPrefix + cpx + time + localIp + timefix;
                    serialId = serialId.ToLower();
                    //判断是否重复
                    var isDone = SystemCacheHelper.CheckTicketStore(serialId);
                    successSerialId = isDone ? serialId : string.Empty;
                }
                //转为小写
                successSerialId = successSerialId.ToLower();
                var resultData = new SerialIdResultData
                {
                    ResultCode = string.IsNullOrEmpty(_localIp) ? ResultCode.Error : ResultCode.Success,
                    SerialId = successSerialId
                };
                return resultData;
            }
        }

        /// <summary>
        /// 会员卡生成规则
        /// </summary>
        /// <param name="fixedPrefix"></param>
        /// <param name="customPrefix"></param>
        /// <returns></returns>
        public static SerialIdResultData GetCard(FixedPrefix fixedPrefix, CustomPrefix customPrefix)
        {
            const string zero = "0";
            const string defaultLocalIp = "00000000";
            lock (LockObj)
            {
                if (!_beginTimespan.HasValue)//IIS被回收或者首次使用
                {
                    _beginTimespan = Helper.ConvertDateTimeInt(DateTime.Now);
                }
                var successSerialId = string.Empty;
                while (string.IsNullOrEmpty(successSerialId))
                {
                    //获取当前时间戳
                    var timestamp = Helper.ConvertDateTimeInt(DateTime.Now);
                    //判断首次使用的时间与当前时间
                    if ((timestamp - _beginTimespan.Value).Equals(0))
                    {
                        Thread.Sleep(10);
                        continue;
                    }
                    //获取固定前缀
                    var fixPrefix = Helper.GetEnumDesc(fixedPrefix);
                    //用户自定义前缀为空，则为A
                    //获取用户自定义前缀
                    var cpx =Helper.GetEnumDesc(customPrefix);
                    //判断本机IP是否为空，为空则获取
                    if (string.IsNullOrEmpty(_localIp))
                    {
                        _localIp = GetIpAddress();
                    }
                    //获取不到本机IP，则用默认值代替
                    if (string.IsNullOrEmpty(_localIp))
                    {
                        _localIp = defaultLocalIp;
                    }
                    //构造随机种子
                    var seed = Guid.NewGuid().GetHashCode();
                    var random = new Random(seed);
                    //随机订单号的最后3位
                    var timefix = random.Next(0, 99).ToString(CultureInfo.InvariantCulture);
                    while (timefix.Length < 2)//不足3位前面补0
                    {
                        timefix = zero + timefix;
                    }
                    //构建订单号
                    //转换时间戳为16进制，并替换E为U
                    var time = Convert.ToString(timestamp, 16).ToUpper().Replace('E', 'U');
                    //IP替换E为U
                    var localIp = _localIp.ToUpper().Replace('E', 'U').Substring(0, 4);
                    var serialId = fixPrefix + cpx + time + localIp + timefix;
                    serialId = serialId.ToLower();
                    //判断是否重复
                    var isDone = SystemCacheHelper.TicketStore(serialId);
                    successSerialId = isDone ? serialId : string.Empty;
                }
                //转为小写
                successSerialId = successSerialId.ToLower();
                var resultData = new SerialIdResultData
                {
                    ResultCode = string.IsNullOrEmpty(_localIp) ? ResultCode.Error : ResultCode.Success,
                    SerialId = successSerialId
                };
                return resultData;
            }
        }

        /// <summary>
        /// 解析订单号
        /// </summary>
        /// <param name="serialId">订单号</param>
        /// <returns>订单号结构</returns>
        public static SerialIdDetailResultData Resolve(string serialId)
        {
            try
            {
                if (string.IsNullOrEmpty(serialId))
                    return new SerialIdDetailResultData
                    {
                        ResultCode = ResultCode.Error,
                        ErrorDetails = "订单号不允许为空"
                    };
                if (serialId.Length != 20)
                    return new SerialIdDetailResultData
                    {
                        ResultCode = ResultCode.Error,
                        ErrorDetails = "订单号长度不正确"
                    };
                //第一位：固定前缀
                var firstFix = serialId.Substring(0, 1);
                var fixPrefix = GetFixedPrefix(firstFix);
                if (!fixPrefix.HasValue)
                    return new SerialIdDetailResultData
                    {
                        ResultCode = ResultCode.Error,
                        ErrorDetails = "订单号固定前缀错误"
                    };
                //第二位：自定义前缀
                var secondFix = serialId.Substring(1, 1);
                var diyPrefix = GetDiyPrefix(secondFix);
                if (!diyPrefix.HasValue)
                    return new SerialIdDetailResultData
                    {
                        ResultCode = ResultCode.Error,
                        ErrorDetails = "订单号自定义前缀错误"
                    };
                //第三位：16进制的8位时间戳
                var thridFix = serialId.Substring(2, 8).ToLower().Replace('u', 'e');
                //转换为10进制时间戳
                int d;
                try
                {
                    d = int.Parse(thridFix, NumberStyles.AllowHexSpecifier);//16转10
                }
                catch (Exception)
                {
                    return new SerialIdDetailResultData
                    {
                        ResultCode = ResultCode.Error,
                        ErrorDetails = "时间戳转换失败"
                    };
                }
                var startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                var time = startTime.AddSeconds(d);
                //第四位：内网IP段标识 ha51affb552027940047
                var forthFix = serialId.Substring(10, 1);
                //第五位：内网IP后3段的16进制数
                var fivthFix = serialId.Substring(11, 6).ToLower().Replace('u', 'e');
                var ipFix = string.Empty;
                switch (forthFix)
                {
                    case "1":
                        ipFix = "10";
                        break;
                    case "2":
                        ipFix = "172";
                        break;
                    case "3":
                        ipFix = "192";
                        break;
                }
                var ip2 = int.Parse(fivthFix.Substring(0, 2), NumberStyles.AllowHexSpecifier);
                var ip3 = int.Parse(fivthFix.Substring(2, 2), NumberStyles.AllowHexSpecifier);
                var ip4 = int.Parse(fivthFix.Substring(4, 2), NumberStyles.AllowHexSpecifier);
                var ip = ipFix + "." + ip2 + "." + ip3 + "." + ip4;
                //最后一位：3位随机数
                var lastFix = int.Parse(serialId.Substring(17, 3));
                return new SerialIdDetailResultData(fixPrefix.Value, diyPrefix.Value, time, ip, lastFix);
            }
            catch (Exception ex)
            {
                return new SerialIdDetailResultData
                {
                    ResultCode = ResultCode.Error,
                    ErrorDetails = "订单号转换异常： " + ex.Message
                };
            }
        }

        /// <summary>
        /// 固定前缀
        /// </summary>
        /// <param name="key">描述</param>
        /// <returns>前缀枚举</returns>
        private static FixedPrefix? GetFixedPrefix(string key)
        {
            foreach (Enum item in Enum.GetValues(typeof(FixedPrefix)))
            {
                var fixPrefix = Helper.GetEnumDesc(item);
                if (!fixPrefix.ToLower().Equals(key.ToLower())) continue;
                return (FixedPrefix)item;
            }
            return null;
        }

        /// <summary>
        /// 自定义前缀
        /// </summary>
        /// <param name="key">描述</param>
        /// <returns>前缀枚举</returns>
        private static CustomPrefix? GetDiyPrefix(string key)
        {
            foreach (Enum item in Enum.GetValues(typeof(CustomPrefix)))
            {
                var fixPrefix = Helper.GetEnumDesc(item);
                if (!fixPrefix.ToLower().Equals(key.ToLower())) continue;
                return (CustomPrefix)item;
            }
            return null;
        }

        /// <summary>
        /// 获取本机IP地址
        /// </summary>
        /// <returns>IP地址</returns>
        private static string GetIpAddress()
        {
            try
            {
                const string local = "127.0.0.1";
                var values = string.Empty;
                var adapters = NetworkInterface.GetAllNetworkInterfaces();
                foreach (var adapter in adapters)
                {
                    var adapterProperties = adapter.GetIPProperties();
                    var allAddress = adapterProperties.UnicastAddresses;
                    if (allAddress.Count <= 0) continue;
                    foreach (var addr in allAddress)
                    {
                        if (addr.Address.AddressFamily != AddressFamily.InterNetwork) continue;
                        var addrIp = addr.Address.ToString();
                        if (addrIp.Equals(local)) continue;
                        if (!IsInnerIp(addrIp)) continue;
                        values = addrIp;
                        break;
                    }
                }
                if (!string.IsNullOrEmpty(values))
                {
                    var items = values.Split('.');
                    var ipType = "1";
                    switch (items[0]) //判断内网所属的IP端
                    {
                        case "10":
                            ipType = "1";
                            break;
                        case "172":
                            ipType = "2";
                            break;
                        case "192":
                            ipType = "3";
                            break;
                    }
                    values = ipType + FixLastIp(items[1]) + FixLastIp(items[2]) + FixLastIp(items[3]);
                }
                return values;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// IP段不足3位补0
        /// </summary>
        /// <param name="ip">IP段</param>
        /// <returns>补足0的IP段</returns>
        private static string FixLastIp(string ip)
        {
            var intIp = int.Parse(ip);
            var strIp = Convert.ToString(intIp, 16);
            while (strIp.Length < 2)
            {
                strIp = "0" + strIp;
            }
            return strIp;
        }

        /// <summary>
        /// 判断一个IP是否为内网Ip
        /// </summary>
        /// <param name="ipAddress">Ip</param>
        /// <returns>判断一个IP是否为内网Ip</returns>
        private static bool IsInnerIp(string ipAddress)
        {
            //私有IP： A类:10.0.0.0-10.255.255.255 B类:172.16.0.0-172.31.255.255 C类:192.168.0.0-192.168.255.255
            var ipNum = GetLongIp(ipAddress);
            var aBegin = GetLongIp("10.0.0.0");
            var aEnd = GetLongIp("10.255.255.255");
            var bBegin = GetLongIp("172.16.0.0");
            var bEnd = GetLongIp("172.31.255.255");
            var cBegin = GetLongIp("192.168.0.0");
            var cEnd = GetLongIp("192.168.255.255");
            var isInnerIp = IsBetween(ipNum, aBegin, aEnd) || IsBetween(ipNum, bBegin, bEnd) || IsBetween(ipNum, cBegin, cEnd);
            return isInnerIp;
        }

        /// <summary>
        /// 获取数字IP
        /// </summary>
        /// <param name="ip">字符串IP</param>
        /// <returns>数字IP</returns>
        private static long GetLongIp(string ip)
        {
            var ips = ip.Split('.');
            var a = long.Parse(ips[0]);
            var b = long.Parse(ips[1]);
            var c = long.Parse(ips[2]);
            var d = long.Parse(ips[3]);
            var ipNum = a * 256 * 256 * 256 + b * 256 * 256 + c * 256 + d;
            return ipNum;
        }

        /// <summary>
        /// 判断IP范围
        /// </summary>
        /// <param name="userIp">用户IP</param>
        /// <param name="begin">开始IP</param>
        /// <param name="end">结束IP</param>
        /// <returns>判断IP是否在其区间</returns>
        private static bool IsBetween(long userIp, long begin, long end)
        {
            return (userIp >= begin) && (userIp <= end);
        }
    }

    /// <summary>
    /// 帮助类
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// 将c# DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>时间戳</returns>
        public static long ConvertDateTimeInt(DateTime time)
        {
            var startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            var intResult = (time - startTime).TotalSeconds;
            return (long)intResult;
        }

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
    /// 缓存操作
    /// </summary>
    public sealed class SystemCacheHelper
    {
        #region 用户自定义变量


        /// <summary>
        /// 字典中的订单最大数量
        /// </summary>
        private const int MaxCount = 1000;

        /// <summary>
        /// 字典超时时间，单位 秒
        /// </summary>
        private const int Timeout = 3;

        #endregion


        #region 缓存实例插入

        /// <summary>
        /// 将订单号放入缓存字典
        /// </summary>
        /// <param name="key">订单号</param>
        /// <returns>放入缓存是否成功,成功 - true 失败 - false</returns>
        public static bool Store(string key)
        {
            // 缓存实例,Key：订单号  Value:时间戳
            HttpContext.Current.Application.Lock();
            var history = new ConcurrentDictionary<string, long>();
            var lastGenerateSerialIds = HttpContext.Current.Application["LastGenerateSerialIds"] as ConcurrentDictionary<string, long>;
            if (lastGenerateSerialIds != null)
            {
                history = lastGenerateSerialIds;
            }
            //先判断订单号是否重复
            if (history.ContainsKey(key))
            {
                return false;
            }
            //保留重复订单集合
            var repeats = new ConcurrentDictionary<string, long>();
            try
            {
                var nowTimespan = Helper.ConvertDateTimeInt(DateTime.Now);
                if (history.Count > MaxCount)
                {
                    //判断是否到达超时时间
                    foreach (var item in history)
                    {
                        if (nowTimespan - item.Value > Timeout)
                        {
                            repeats.TryAdd(item.Key, item.Value);
                        }
                    }
                    if (repeats.Count > 0)
                    {
                        foreach (var item in repeats)
                        {
                            long outValue;
                            history.TryRemove(item.Key, out outValue);
                        }
                    }
                }
                history.TryAdd(key, nowTimespan);
                HttpContext.Current.Application["LastGenerateSerialIds"] = history;
                return true;
            }
            catch (Exception ex)
            {
                var message = string.Format("缓存订单号发生错误：{0}", ex.Message);
                return false;
            }
            finally
            {
                repeats.Clear();
                HttpContext.Current.Application.UnLock();
            }
        }

        /// <summary>
        /// 将订单号放入缓存字典
        /// </summary>
        /// <param name="key">订单号</param>
        /// <returns>放入缓存是否成功,成功 - true 失败 - false</returns>
        public static bool TicketStore(string key)
        {
            // 缓存实例,Key：订单号  Value:时间戳
            var history = new ConcurrentDictionary<string, long>();
            var lastGenerateSerialIds = CacheHelper.GetCache("LastGenerateSerialIds") as ConcurrentDictionary<string, long>;
            if (lastGenerateSerialIds != null)
            {
                history = lastGenerateSerialIds;
            }
            //先判断订单号是否重复
            if (history.ContainsKey(key))
            {
                return false;
            }
            //保留重复订单集合
            var repeats = new ConcurrentDictionary<string, long>();
            try
            {
                var nowTimespan = Helper.ConvertDateTimeInt(DateTime.Now);
                if (history.Count > MaxCount)
                {
                    //判断是否到达超时时间
                    foreach (var item in history)
                    {
                        if (nowTimespan - item.Value > Timeout)
                        {
                            repeats.TryAdd(item.Key, item.Value);
                        }
                    }
                    if (repeats.Count > 0)
                    {
                        foreach (var item in repeats)
                        {
                            long outValue;
                            history.TryRemove(item.Key, out outValue);
                        }
                    }
                }
                history.TryAdd(key, nowTimespan);
                CacheHelper.SetCacheValue("LastGenerateSerialIds", history);
                return true;
            }
            catch (Exception ex)
            {
                var message = string.Format("缓存订单号发生错误：{0}", ex.Message);
                return false;
            }
            finally
            {
                repeats.Clear();
            }
        }
        #endregion

        /// <summary>
        /// 将订单号放入缓存字典(检售票)
        /// </summary>
        /// <param name="key">订单号</param>
        /// <returns>放入缓存是否成功,成功 - true 失败 - false</returns>
        public static bool CheckTicketStore(string key)
        {
            // 缓存实例,Key：订单号  Value:时间戳
            var history = new ConcurrentDictionary<string, long>();
            var lastGenerateSerialIds = CacheHelper.GetCache("LastGenerateSerialIds") as ConcurrentDictionary<string, long>;
            if (lastGenerateSerialIds != null)
            {
                history = lastGenerateSerialIds;
            }
            //先判断订单号是否重复
            if (history.ContainsKey(key))
            {
                return false;
            }
            //保留重复订单集合
            var repeats = new ConcurrentDictionary<string, long>();
            try
            {
                var nowTimespan = Helper.ConvertDateTimeInt(DateTime.Now);
                if (history.Count > MaxCount)
                {
                    //判断是否到达超时时间
                    foreach (var item in history)
                    {
                        if (nowTimespan - item.Value > Timeout)
                        {
                            repeats.TryAdd(item.Key, item.Value);
                        }
                    }
                    if (repeats.Count > 0)
                    {
                        foreach (var item in repeats)
                        {
                            long outValue;
                            history.TryRemove(item.Key, out outValue);
                        }
                    }
                }
                history.TryAdd(key, nowTimespan);
                CacheHelper.SetCacheValue("LastGenerateSerialIds", history);
                return true;
            }
            catch (Exception ex)
            {
                var message = string.Format("缓存订单号发生错误：{0}", ex.Message);
                return false;
            }
            finally
            {
                repeats.Clear();
            }
        }
    }
}
