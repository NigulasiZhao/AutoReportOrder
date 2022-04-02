using System;
using System.Collections.Generic;
using System.Text;

namespace AutoReportOrder.UserInfo
{
    public class UserModel
    {
        public string id { get; set; }
        public string userCode { get; set; }
        public string token { get; set; }

        public string mobile { get; set; }

        public HouseInfo house { get; set; }
    }
    public class HouseInfo
    {
        public string HouseId { get; set; }
    }
}
