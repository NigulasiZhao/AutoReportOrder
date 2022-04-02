using System;
using System.Collections.Generic;
using System.Text;

namespace AutoReportOrder.Common
{
    public class ResponseResult<T>
    {
        public string code { get; set; }

        public T data { get; set; }

        public string message { get; set; }

        public string version { get; set; }
    }
}
