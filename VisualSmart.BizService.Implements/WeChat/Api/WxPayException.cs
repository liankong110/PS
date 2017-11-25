using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VisualSmart.BizService.Implements.WeChat.Api
{
    public class WxPayException : Exception
    {
        public WxPayException(string msg)
            : base(msg)
        {

        }
    }
}
