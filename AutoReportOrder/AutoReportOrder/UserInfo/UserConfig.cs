using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoReportOrder.UserInfo
{
    public class UserConfig
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Content { get; set; }

        public int Cycles { get; set; }

        public int Interval { get; set; }
    }
}
